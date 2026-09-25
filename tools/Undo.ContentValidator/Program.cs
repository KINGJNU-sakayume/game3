using System.Text.Json;
using System.Text.RegularExpressions;
using Undo.Core.State;

namespace Undo.ContentValidator;

internal static partial class Program
{
    private const int SupportedSchemaVersion = 1;

    public static int Main(string[] args)
    {
        var contentRoot = Path.GetFullPath(args.Length > 0 ? args[0] : "game/content");
        var errors = new List<string>();

        if (!Directory.Exists(contentRoot))
        {
            Console.Error.WriteLine($"[CONTENT] Content root does not exist: {contentRoot}");
            return 1;
        }

        ValidateManifest(contentRoot, errors);

        var puzzleFiles = Directory
            .EnumerateFiles(Path.Combine(contentRoot, "puzzles"), "puzzle.json", SearchOption.AllDirectories)
            .OrderBy(path => path, StringComparer.Ordinal)
            .ToArray();

        if (puzzleFiles.Length == 0)
        {
            errors.Add("No puzzle.json files were found under game/content/puzzles.");
        }

        foreach (var puzzleFile in puzzleFiles)
        {
            ValidatePuzzle(puzzleFile, errors);
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

    private static void ValidateManifest(string contentRoot, List<string> errors)
    {
        var manifestPath = Path.Combine(contentRoot, "manifests", "puzzles.json");
        if (!File.Exists(manifestPath))
        {
            errors.Add($"{manifestPath}: puzzle manifest is missing.");
            return;
        }

        using var document = ParseDocument(manifestPath, errors);
        if (document is null)
        {
            return;
        }

        var root = document.RootElement;
        ValidateSchemaVersion(root, manifestPath, errors);

        if (!root.TryGetProperty("puzzles", out var puzzles) || puzzles.ValueKind != JsonValueKind.Array)
        {
            errors.Add($"{manifestPath}: $.puzzles must be an array.");
            return;
        }

        var seenPuzzleIds = new HashSet<string>(StringComparer.Ordinal);

        foreach (var entry in puzzles.EnumerateArray())
        {
            var puzzleId = ReadRequiredString(entry, "puzzleId", manifestPath, "$.puzzles[]", errors);
            var resourcePath = ReadRequiredString(entry, "path", manifestPath, "$.puzzles[]", errors);

            if (puzzleId is not null && !seenPuzzleIds.Add(puzzleId))
            {
                errors.Add($"{manifestPath}: duplicate manifest puzzleId '{puzzleId}'.");
            }

            if (resourcePath is null)
            {
                continue;
            }

            const string contentPrefix = "res://content/";
            if (!resourcePath.StartsWith(contentPrefix, StringComparison.Ordinal))
            {
                errors.Add($"{manifestPath}: path '{resourcePath}' must begin with '{contentPrefix}'.");
                continue;
            }

            var relativePath = resourcePath[contentPrefix.Length..]
                .Replace('/', Path.DirectorySeparatorChar);
            var resolvedPath = Path.Combine(contentRoot, relativePath);

            if (!File.Exists(resolvedPath))
            {
                errors.Add($"{manifestPath}: referenced puzzle file does not exist: {resourcePath}.");
            }
        }
    }

    private static void ValidatePuzzle(string puzzlePath, List<string> errors)
    {
        using var document = ParseDocument(puzzlePath, errors);
        if (document is null)
        {
            return;
        }

        var root = document.RootElement;
        ValidateSchemaVersion(root, puzzlePath, errors);

        var puzzleId = ReadRequiredString(root, "puzzleId", puzzlePath, "$", errors);
        if (puzzleId is not null && !StableIdPattern().IsMatch(puzzleId))
        {
            errors.Add($"{puzzlePath}: $.puzzleId '{puzzleId}' is not a valid stable ID.");
        }

        if (!root.TryGetProperty("correctionCapacity", out var capacity) ||
            capacity.ValueKind != JsonValueKind.Number ||
            !capacity.TryGetInt32(out var capacityValue) ||
            capacityValue is < 1 or > 3)
        {
            errors.Add($"{puzzlePath}: $.correctionCapacity must be an integer from 1 to 3.");
        }

        var locations = ReadLocations(root, puzzlePath, errors);
        ValidateInitialState(root, locations, puzzlePath, errors);
        ValidateActors(root, locations, puzzlePath, errors);
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
            if (!locationId.StartsWith("LOC_", StringComparison.Ordinal))
            {
                errors.Add($"{puzzlePath}: location '{locationId}' must use the LOC_ prefix.");
            }

            if (!locations.Add(locationId))
            {
                errors.Add($"{puzzlePath}: duplicate location ID '{locationId}'.");
            }
        }

        return locations;
    }

    private static void ValidateInitialState(
        JsonElement root,
        HashSet<string> locations,
        string puzzlePath,
        List<string> errors)
    {
        if (!root.TryGetProperty("initialState", out var initialState) ||
            initialState.ValueKind != JsonValueKind.Object)
        {
            errors.Add($"{puzzlePath}: $.initialState must be an object.");
            return;
        }

        var seenEntities = new HashSet<string>(StringComparer.Ordinal);

        foreach (var entityProperty in initialState.EnumerateObject())
        {
            if (!seenEntities.Add(entityProperty.Name))
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

            var seenChannels = new HashSet<string>(StringComparer.Ordinal);

            foreach (var channelProperty in entityProperty.Value.EnumerateObject())
            {
                if (!seenChannels.Add(channelProperty.Name))
                {
                    errors.Add(
                        $"{puzzlePath}: duplicate state channel '{channelProperty.Name}' for '{entityProperty.Name}'.");
                }

                if (!Enum.TryParse<StateChannel>(channelProperty.Name, ignoreCase: true, out var channel))
                {
                    errors.Add(
                        $"{puzzlePath}: unknown StateChannel '{channelProperty.Name}' on '{entityProperty.Name}'.");
                    continue;
                }

                if (!IsChannelValueValid(channel, channelProperty.Value))
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
    }

    private static void ValidateActors(
        JsonElement root,
        HashSet<string> locations,
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
            }

            if (initialLocation is not null && !locations.Contains(initialLocation))
            {
                errors.Add($"{puzzlePath}: actor references unknown initialLocation '{initialLocation}'.");
            }
        }
    }

    private static bool IsChannelValueValid(StateChannel channel, JsonElement value) =>
        channel switch
        {
            StateChannel.Open => value.ValueKind is JsonValueKind.True or JsonValueKind.False,
            StateChannel.Lock => value.ValueKind == JsonValueKind.String,
            StateChannel.Position => value.ValueKind == JsonValueKind.String,
            StateChannel.Possession => value.ValueKind == JsonValueKind.String,
            StateChannel.Authorization => value.ValueKind == JsonValueKind.String,
            StateChannel.Power => value.ValueKind is JsonValueKind.String or JsonValueKind.True or JsonValueKind.False,
            _ => value.ValueKind is JsonValueKind.String
                or JsonValueKind.Number
                or JsonValueKind.True
                or JsonValueKind.False,
        };

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

    [GeneratedRegex("^[A-Z0-9_]+$", RegexOptions.CultureInvariant)]
    private static partial Regex StableIdPattern();
}
