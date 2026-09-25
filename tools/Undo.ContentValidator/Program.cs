using System.Text.Json;
using System.Text.RegularExpressions;
using Undo.Core.Events;
using Undo.Core.State;

namespace Undo.ContentValidator;

internal static partial class Program
{
    private const int SupportedSchemaVersion = 1;
    private static readonly HashSet<string> AllowedRoutineStepTypes =
        new(StringComparer.Ordinal) { "MOVE_TO", "WAIT", "INTENT" };

    public static int Main(string[] args)
    {
        var contentRoot = Path.GetFullPath(args.Length > 0 ? args[0] : "game/content");
        var errors = new List<string>();

        if (!Directory.Exists(contentRoot))
        {
            Console.Error.WriteLine($"[CONTENT] Content root does not exist: {contentRoot}");
            return 1;
        }

        var puzzlesRoot = Path.Combine(contentRoot, "puzzles");
        if (!Directory.Exists(puzzlesRoot))
        {
            Console.Error.WriteLine($"[CONTENT] Puzzle root does not exist: {puzzlesRoot}");
            return 1;
        }

        var manifestEntries = ValidateManifest(contentRoot, errors);

        var puzzleFiles = Directory
            .EnumerateFiles(puzzlesRoot, "puzzle.json", SearchOption.AllDirectories)
            .OrderBy(path => path, StringComparer.Ordinal)
            .ToArray();

        if (puzzleFiles.Length == 0)
        {
            errors.Add("No puzzle.json files were found under game/content/puzzles.");
        }

        var puzzleIdsOnDisk = new HashSet<string>(StringComparer.Ordinal);
        foreach (var puzzleFile in puzzleFiles)
        {
            var puzzleId = ValidatePuzzle(puzzleFile, contentRoot, errors);
            if (puzzleId is not null)
            {
                puzzleIdsOnDisk.Add(puzzleId);
            }
        }

        foreach (var manifestEntry in manifestEntries)
        {
            if (!puzzleIdsOnDisk.Contains(manifestEntry.PuzzleId))
            {
                errors.Add(
                    $"{manifestEntry.ManifestPath}: manifest puzzleId '{manifestEntry.PuzzleId}' has no matching validated puzzle file.");
            }

            if (File.Exists(manifestEntry.ResolvedPuzzlePath))
            {
                using var document = ParseDocument(manifestEntry.ResolvedPuzzlePath, errors);
                if (document is not null &&
                    document.RootElement.TryGetProperty("puzzleId", out var puzzleIdElement) &&
                    puzzleIdElement.ValueKind == JsonValueKind.String &&
                    !string.Equals(
                        puzzleIdElement.GetString(),
                        manifestEntry.PuzzleId,
                        StringComparison.Ordinal))
                {
                    errors.Add(
                        $"{manifestEntry.ManifestPath}: manifest puzzleId '{manifestEntry.PuzzleId}' does not match file puzzleId '{puzzleIdElement.GetString()}'.");
                }
            }
        }

        if (errors.Count > 0)
        {
            foreach (var error in errors)
            {
                Console.Error.WriteLine($"[CONTENT] ERROR {error}");
            }

            Console.Error.WriteLine($"[CONTENT] Validation failed with {errors.Count} error(s).");
            return 1;
        }

        Console.WriteLine($"[CONTENT] Validated {puzzleFiles.Length} puzzle file(s) successfully.");
        return 0;
    }

    private static IReadOnlyList<ManifestEntry> ValidateManifest(string contentRoot, List<string> errors)
    {
        var manifestPath = Path.Combine(contentRoot, "manifests", "puzzles.json");
        var entries = new List<ManifestEntry>();

        if (!File.Exists(manifestPath))
        {
            errors.Add($"{manifestPath}: puzzle manifest is missing.");
            return entries;
        }

        using var document = ParseDocument(manifestPath, errors);
        if (document is null)
        {
            return entries;
        }

        var root = document.RootElement;
        ValidateSchemaVersion(root, manifestPath, errors);

        if (!root.TryGetProperty("puzzles", out var puzzles) || puzzles.ValueKind != JsonValueKind.Array)
        {
            errors.Add($"{manifestPath}: $.puzzles must be an array.");
            return entries;
        }

        var seenPuzzleIds = new HashSet<string>(StringComparer.Ordinal);

        foreach (var entry in puzzles.EnumerateArray())
        {
            var puzzleId = ReadRequiredString(entry, "puzzleId", manifestPath, "$.puzzles[]", errors);
            var resourcePath = ReadRequiredString(entry, "path", manifestPath, "$.puzzles[]", errors);
            var scenePath = ReadRequiredString(entry, "scenePath", manifestPath, "$.puzzles[]", errors);

            if (puzzleId is not null && !seenPuzzleIds.Add(puzzleId))
            {
                errors.Add($"{manifestPath}: duplicate manifest puzzleId '{puzzleId}'.");
            }

            if (puzzleId is null || resourcePath is null || scenePath is null)
            {
                continue;
            }

            var resolvedPuzzlePath = ResolveResourcePath(contentRoot, resourcePath, manifestPath, errors);
            var resolvedScenePath = ResolveResourcePath(contentRoot, scenePath, manifestPath, errors);

            if (resolvedPuzzlePath is not null && !File.Exists(resolvedPuzzlePath))
            {
                errors.Add($"{manifestPath}: referenced puzzle file does not exist: {resourcePath}.");
            }

            if (resolvedScenePath is not null && !File.Exists(resolvedScenePath))
            {
                errors.Add($"{manifestPath}: referenced scene file does not exist: {scenePath}.");
            }

            if (resolvedPuzzlePath is not null)
            {
                entries.Add(new ManifestEntry(puzzleId, manifestPath, resolvedPuzzlePath));
            }
        }

        return entries;
    }

    private static string? ValidatePuzzle(string puzzlePath, string contentRoot, List<string> errors)
    {
        using var document = ParseDocument(puzzlePath, errors);
        if (document is null)
        {
            return null;
        }

        var root = document.RootElement;
        ValidateSchemaVersion(root, puzzlePath, errors);

        var puzzleId = ReadRequiredString(root, "puzzleId", puzzlePath, "$", errors);
        if (puzzleId is not null && !StableIdPattern().IsMatch(puzzleId))
        {
            errors.Add($"{puzzlePath}: $.puzzleId '{puzzleId}' is not a valid stable ID.");
        }

        var scenePath = ReadRequiredString(root, "scenePath", puzzlePath, "$", errors);
        if (scenePath is not null)
        {
            var resolvedScene = ResolveResourcePath(contentRoot, scenePath, puzzlePath, errors);
            if (resolvedScene is not null && !File.Exists(resolvedScene))
            {
                errors.Add($"{puzzlePath}: $.scenePath does not exist: {scenePath}.");
            }
        }

        if (!root.TryGetProperty("correctionCapacity", out var capacity) ||
            capacity.ValueKind != JsonValueKind.Number ||
            !capacity.TryGetInt32(out var capacityValue) ||
            capacityValue is < 1 or > 3)
        {
            errors.Add($"{puzzlePath}: $.correctionCapacity must be an integer from 1 to 3.");
        }

        var locations = ReadLocations(root, puzzlePath, errors);
        var entities = ValidateInitialState(root, locations, puzzlePath, errors);
        ValidateActors(root, locations, entities, puzzlePath, errors);
        ValidateCompletion(root, locations, puzzlePath, errors);

        return puzzleId;
    }

    private static HashSet<string> ReadLocations(JsonElement root, string puzzlePath, List<string> errors)
    {
        var locations = new HashSet<string>(StringComparer.Ordinal);

        if (!root.TryGetProperty("locations", out var locationArray) ||
            locationArray.ValueKind != JsonValueKind.Array)
        {
            errors.Add($"{puzzlePath}: $.locations must be an array.");
            return locations;
        }

        foreach (var location in locationArray.EnumerateArray())
        {
            if (location.ValueKind != JsonValueKind.String ||
                string.IsNullOrWhiteSpace(location.GetString()))
            {
                errors.Add($"{puzzlePath}: every $.locations[] value must be a non-empty string.");
                continue;
            }

            var locationId = location.GetString()!;
            if (!locationId.StartsWith("LOC_", StringComparison.Ordinal) ||
                !StableIdPattern().IsMatch(locationId))
            {
                errors.Add($"{puzzlePath}: location '{locationId}' must be a valid LOC_ stable ID.");
            }

            if (!locations.Add(locationId))
            {
                errors.Add($"{puzzlePath}: duplicate location ID '{locationId}'.");
            }
        }

        return locations;
    }

    private static HashSet<string> ValidateInitialState(
        JsonElement root,
        HashSet<string> locations,
        string puzzlePath,
        List<string> errors)
    {
        var entities = new HashSet<string>(StringComparer.Ordinal);

        if (!root.TryGetProperty("initialState", out var initialState) ||
            initialState.ValueKind != JsonValueKind.Object)
        {
            errors.Add($"{puzzlePath}: $.initialState must be an object.");
            return entities;
        }

        foreach (var entityProperty in initialState.EnumerateObject())
        {
            if (!entities.Add(entityProperty.Name))
            {
                errors.Add($"{puzzlePath}: duplicate initial-state entity '{entityProperty.Name}'.");
            }

            if (!StableIdPattern().IsMatch(entityProperty.Name))
            {
                errors.Add($"{puzzlePath}: entity ID '{entityProperty.Name}' is not a valid stable ID.");
            }

            if (entityProperty.Value.ValueKind != JsonValueKind.Object)
            {
                errors.Add($"{puzzlePath}: $.initialState.{entityProperty.Name} must be an object.");
                continue;
            }

            foreach (var channelProperty in entityProperty.Value.EnumerateObject())
            {
                if (!Enum.TryParse<StateChannel>(channelProperty.Name, ignoreCase: true, out var channel))
                {
                    errors.Add(
                        $"{puzzlePath}: unknown StateChannel '{channelProperty.Name}' on '{entityProperty.Name}'.");
                    continue;
                }

                if (!TryGetStateValueKind(channelProperty.Value, out var valueKind) ||
                    !StateValueRules.IsCompatible(channel, valueKind))
                {
                    errors.Add(
                        $"{puzzlePath}: invalid value type for {entityProperty.Name}.{channelProperty.Name}.");
                }

                if (channel == StateChannel.Position &&
                    channelProperty.Value.ValueKind == JsonValueKind.String)
                {
                    var locationId = channelProperty.Value.GetString();
                    if (locationId is not null && !locations.Contains(locationId))
                    {
                        errors.Add(
                            $"{puzzlePath}: {entityProperty.Name}.POSITION references unknown location '{locationId}'.");
                    }
                }
            }
        }

        return entities;
    }

    private static void ValidateActors(
        JsonElement root,
        HashSet<string> locations,
        HashSet<string> entities,
        string puzzlePath,
        List<string> errors)
    {
        if (!root.TryGetProperty("actors", out var actors) || actors.ValueKind != JsonValueKind.Array)
        {
            errors.Add($"{puzzlePath}: $.actors must be an array.");
            return;
        }

        var seenActors = new HashSet<string>(StringComparer.Ordinal);

        foreach (var actor in actors.EnumerateArray())
        {
            var actorId = ReadRequiredString(actor, "actorId", puzzlePath, "$.actors[]", errors);
            var initialLocation = ReadRequiredString(
                actor,
                "initialLocation",
                puzzlePath,
                "$.actors[]",
                errors);

            if (actorId is not null)
            {
                if (!StableIdPattern().IsMatch(actorId))
                {
                    errors.Add($"{puzzlePath}: actor ID '{actorId}' is not a valid stable ID.");
                }

                if (!seenActors.Add(actorId))
                {
                    errors.Add($"{puzzlePath}: duplicate actor ID '{actorId}'.");
                }

                entities.Add(actorId);
            }

            if (initialLocation is not null && !locations.Contains(initialLocation))
            {
                errors.Add($"{puzzlePath}: actor references unknown initialLocation '{initialLocation}'.");
            }

            ValidateRoutine(actor, locations, entities, puzzlePath, errors);
        }
    }

    private static void ValidateRoutine(
        JsonElement actor,
        HashSet<string> locations,
        HashSet<string> entities,
        string puzzlePath,
        List<string> errors)
    {
        if (!actor.TryGetProperty("routine", out var routine) || routine.ValueKind != JsonValueKind.Array)
        {
            errors.Add($"{puzzlePath}: $.actors[].routine must be an array.");
            return;
        }

        foreach (var step in routine.EnumerateArray())
        {
            var type = ReadRequiredString(step, "type", puzzlePath, "$.actors[].routine[]", errors);
            if (type is null)
            {
                continue;
            }

            if (!AllowedRoutineStepTypes.Contains(type))
            {
                errors.Add($"{puzzlePath}: unsupported routine step type '{type}'.");
                continue;
            }

            if (type == "MOVE_TO")
            {
                var location = ReadRequiredString(
                    step,
                    "location",
                    puzzlePath,
                    "$.actors[].routine[]",
                    errors);

                if (location is not null && !locations.Contains(location))
                {
                    errors.Add($"{puzzlePath}: MOVE_TO references unknown location '{location}'.");
                }

                continue;
            }

            if (type == "WAIT")
            {
                if (!step.TryGetProperty("durationSeconds", out var duration) ||
                    duration.ValueKind != JsonValueKind.Number ||
                    !duration.TryGetDouble(out var durationValue) ||
                    durationValue < 0)
                {
                    errors.Add($"{puzzlePath}: WAIT.durationSeconds must be a non-negative number.");
                }

                continue;
            }

            var verb = ReadRequiredString(step, "verb", puzzlePath, "$.actors[].routine[]", errors);
            var target = ReadRequiredString(step, "target", puzzlePath, "$.actors[].routine[]", errors);
            var source = ReadRequiredString(step, "source", puzzlePath, "$.actors[].routine[]", errors);

            if (verb is not null && !Enum.TryParse<EventVerb>(verb, ignoreCase: true, out _))
            {
                errors.Add($"{puzzlePath}: INTENT verb '{verb}' is not in the canonical EventVerb vocabulary.");
            }

            if (target is not null && !entities.Contains(target))
            {
                errors.Add($"{puzzlePath}: INTENT target '{target}' is not a declared entity.");
            }

            if (!step.TryGetProperty("reversible", out var reversible) ||
                reversible.ValueKind is not (JsonValueKind.True or JsonValueKind.False))
            {
                errors.Add($"{puzzlePath}: INTENT.reversible must be boolean.");
            }

            if (source is not null && !KnownRecordSources.Contains(source))
            {
                errors.Add($"{puzzlePath}: INTENT source '{source}' is not a supported record source.");
            }
        }
    }

    private static void ValidateCompletion(
        JsonElement root,
        HashSet<string> locations,
        string puzzlePath,
        List<string> errors)
    {
        if (!root.TryGetProperty("completion", out var completion) ||
            completion.ValueKind != JsonValueKind.Object ||
            !completion.TryGetProperty("all", out var all) ||
            all.ValueKind != JsonValueKind.Array)
        {
            errors.Add($"{puzzlePath}: $.completion.all must be an array.");
            return;
        }

        foreach (var predicate in all.EnumerateArray())
        {
            if (!predicate.TryGetProperty("playerAtLocation", out var playerAtLocation) ||
                playerAtLocation.ValueKind != JsonValueKind.Object)
            {
                errors.Add($"{puzzlePath}: Prototype A completion supports only playerAtLocation predicates.");
                continue;
            }

            var location = ReadRequiredString(
                playerAtLocation,
                "location",
                puzzlePath,
                "$.completion.all[].playerAtLocation",
                errors);

            if (location is not null && !locations.Contains(location))
            {
                errors.Add($"{puzzlePath}: completion references unknown location '{location}'.");
            }
        }
    }

    private static string? ResolveResourcePath(
        string contentRoot,
        string resourcePath,
        string sourcePath,
        List<string> errors)
    {
        const string prefix = "res://";
        if (!resourcePath.StartsWith(prefix, StringComparison.Ordinal))
        {
            errors.Add($"{sourcePath}: resource path '{resourcePath}' must begin with '{prefix}'.");
            return null;
        }

        var gameRoot = Directory.GetParent(contentRoot)?.FullName;
        if (gameRoot is null)
        {
            errors.Add($"{sourcePath}: cannot resolve game root from '{contentRoot}'.");
            return null;
        }

        return Path.Combine(
            gameRoot,
            resourcePath[prefix.Length..].Replace('/', Path.DirectorySeparatorChar));
    }

    private static bool TryGetStateValueKind(JsonElement value, out StateValueKind kind)
    {
        switch (value.ValueKind)
        {
            case JsonValueKind.True:
            case JsonValueKind.False:
                kind = StateValueKind.Boolean;
                return true;
            case JsonValueKind.Number when value.TryGetInt64(out _):
                kind = StateValueKind.Integer;
                return true;
            case JsonValueKind.String:
                kind = StateValueKind.Identifier;
                return true;
            default:
                kind = default;
                return false;
        }
    }

    private static void ValidateSchemaVersion(JsonElement root, string filePath, List<string> errors)
    {
        if (!root.TryGetProperty("schemaVersion", out var schemaVersion) ||
            schemaVersion.ValueKind != JsonValueKind.Number ||
            !schemaVersion.TryGetInt32(out var schemaValue) ||
            schemaValue != SupportedSchemaVersion)
        {
            errors.Add(
                $"{filePath}: $.schemaVersion must equal supported version {SupportedSchemaVersion}.");
        }
    }

    private static string? ReadRequiredString(
        JsonElement element,
        string propertyName,
        string filePath,
        string propertyPath,
        List<string> errors)
    {
        if (!element.TryGetProperty(propertyName, out var property) ||
            property.ValueKind != JsonValueKind.String ||
            string.IsNullOrWhiteSpace(property.GetString()))
        {
            errors.Add($"{filePath}: {propertyPath}.{propertyName} must be a non-empty string.");
            return null;
        }

        return property.GetString();
    }

    private static JsonDocument? ParseDocument(string path, List<string> errors)
    {
        try
        {
            return JsonDocument.Parse(
                File.ReadAllText(path),
                new JsonDocumentOptions
                {
                    AllowTrailingCommas = false,
                    CommentHandling = JsonCommentHandling.Disallow,
                });
        }
        catch (JsonException exception)
        {
            errors.Add($"{path}: invalid JSON: {exception.Message}");
            return null;
        }
    }

    private static readonly HashSet<string> KnownRecordSources =
        new(StringComparer.Ordinal)
        {
            "ACCESS_CONTROL",
            "MACHINERY",
            "SECURITY",
            "ARCHIVE_SYSTEM",
            "ENVIRONMENTAL_SENSOR",
            "CORRELATED_INSTITUTIONAL_RECORD",
        };

    private sealed record ManifestEntry(
        string PuzzleId,
        string ManifestPath,
        string ResolvedPuzzlePath);

    [GeneratedRegex("^[A-Z0-9_]+$", RegexOptions.CultureInvariant)]
    private static partial Regex StableIdPattern();
}
