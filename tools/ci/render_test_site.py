#!/usr/bin/env python3
"""Render the UNDO Prototype A GitHub Actions validation report.

This script is CI-only infrastructure. It does not load or modify gameplay data.
"""

from __future__ import annotations

import html
import os
from datetime import datetime, timezone
from pathlib import Path


CHECKS = [
    ("restore", "Restore", "dotnet restore UNDO.sln"),
    (
        "solution_build",
        "Solution build",
        "dotnet build UNDO.sln --configuration Release --no-restore",
    ),
    (
        "domain_tests",
        "Domain tests",
        "dotnet test tests/Undo.Core.Tests/Undo.Core.Tests.csproj --configuration Release --no-build",
    ),
    (
        "content_validation",
        "Content validation",
        "dotnet run --project tools/Undo.ContentValidator/Undo.ContentValidator.csproj --configuration Release --no-build -- game/content",
    ),
    (
        "godot_build",
        "Godot C# build",
        "dotnet build game/Undo.Game.csproj --configuration Debug --no-restore",
    ),
    (
        "headless_smoke",
        "Godot headless smoke",
        'godot --headless --path game --quit-after 3',
    ),
    (
        "prototype_a_integration",
        "Prototype A integration",
        "godot --headless --path game res://scenes/tests/prototype_a_integration.tscn",
    ),
]


def env(name: str, default: str = "") -> str:
    return os.environ.get(name, default)


def status_for(key: str) -> str:
    return env(f"UNDO_STATUS_{key.upper()}", "unknown").lower()


def status_label(status: str) -> str:
    return {
        "success": "PASS",
        "failure": "FAIL",
        "cancelled": "CANCELLED",
        "skipped": "SKIPPED",
    }.get(status, status.upper())


def css_class(status: str) -> str:
    return "pass" if status == "success" else "fail"


def main() -> int:
    output_dir = Path(env("UNDO_SITE_DIR", "artifacts/test-site"))
    output_dir.mkdir(parents=True, exist_ok=True)

    statuses = {key: status_for(key) for key, _, _ in CHECKS}
    overall = "success" if all(value == "success" for value in statuses.values()) else "failure"

    repository = env("GITHUB_REPOSITORY", "KINGJNU-sakayume/game3")
    sha = env("GITHUB_SHA", "unknown")
    ref_name = env("GITHUB_REF_NAME", "unknown")
    server_url = env("GITHUB_SERVER_URL", "https://github.com")
    run_id = env("GITHUB_RUN_ID", "")
    run_number = env("GITHUB_RUN_NUMBER", "")
    run_url = f"{server_url}/{repository}/actions/runs/{run_id}" if run_id else "#"
    commit_url = f"{server_url}/{repository}/commit/{sha}" if sha != "unknown" else "#"
    generated = datetime.now(timezone.utc).strftime("%Y-%m-%d %H:%M:%S UTC")

    rows = []
    for key, title, command in CHECKS:
        status = statuses[key]
        rows.append(
            f"""
            <tr>
              <td>{html.escape(title)}</td>
              <td><span class="status {css_class(status)}">{html.escape(status_label(status))}</span></td>
              <td><code>{html.escape(command)}</code></td>
            </tr>
            """
        )

    overall_label = "PASS" if overall == "success" else "FAIL"
    overall_class = "pass" if overall == "success" else "fail"

    page = f"""<!doctype html>
<html lang="en">
<head>
  <meta charset="utf-8">
  <meta name="viewport" content="width=device-width, initial-scale=1">
  <meta name="robots" content="noindex,nofollow">
  <title>UNDO Prototype A — CI Test Report</title>
  <style>
    :root {{
      color-scheme: light dark;
      font-family: ui-monospace, SFMono-Regular, Menlo, Monaco, Consolas, "Liberation Mono", monospace;
    }}
    body {{
      max-width: 1040px;
      margin: 0 auto;
      padding: 40px 24px 64px;
      line-height: 1.55;
    }}
    header {{
      border-bottom: 1px solid currentColor;
      padding-bottom: 20px;
      margin-bottom: 28px;
    }}
    h1 {{
      font-size: 1.7rem;
      margin: 0 0 8px;
    }}
    h2 {{
      margin-top: 34px;
      font-size: 1.15rem;
    }}
    .overall {{
      display: inline-block;
      padding: 5px 9px;
      border: 1px solid currentColor;
      font-weight: 700;
      letter-spacing: 0.05em;
    }}
    .pass {{ color: #16803a; }}
    .fail {{ color: #b42318; }}
    .meta {{
      display: grid;
      grid-template-columns: max-content 1fr;
      gap: 6px 18px;
      margin-top: 20px;
      font-size: 0.92rem;
    }}
    table {{
      width: 100%;
      border-collapse: collapse;
      margin-top: 14px;
    }}
    th, td {{
      text-align: left;
      vertical-align: top;
      padding: 10px 8px;
      border-bottom: 1px solid color-mix(in srgb, currentColor 25%, transparent);
    }}
    th {{ font-weight: 700; }}
    .status {{
      font-weight: 700;
      white-space: nowrap;
    }}
    code {{
      white-space: normal;
      overflow-wrap: anywhere;
      font-size: 0.88rem;
    }}
    .note {{
      border-left: 3px solid currentColor;
      padding-left: 14px;
      margin-top: 28px;
    }}
    a {{ color: inherit; }}
    @media (max-width: 720px) {{
      .meta {{ grid-template-columns: 1fr; }}
      table, thead, tbody, tr, th, td {{ display: block; }}
      thead {{ display: none; }}
      td {{ padding: 6px 0; border: 0; }}
      tr {{ padding: 12px 0; border-bottom: 1px solid color-mix(in srgb, currentColor 25%, transparent); }}
    }}
  </style>
</head>
<body>
  <header>
    <h1>UNDO — Prototype A CI Test Report</h1>
    <span class="overall {overall_class}">{overall_label}</span>
    <div class="meta">
      <strong>Repository</strong><span>{html.escape(repository)}</span>
      <strong>Ref</strong><span>{html.escape(ref_name)}</span>
      <strong>Commit</strong><span><a href="{html.escape(commit_url)}">{html.escape(sha[:12])}</a></span>
      <strong>Run</strong><span><a href="{html.escape(run_url)}">#{html.escape(run_number or run_id or "unknown")}</a></span>
      <strong>Generated</strong><span>{html.escape(generated)}</span>
    </div>
  </header>

  <main>
    <h2>Automated checks</h2>
    <table>
      <thead>
        <tr><th>Check</th><th>Result</th><th>Command</th></tr>
      </thead>
      <tbody>
        {''.join(rows)}
      </tbody>
    </table>

    <div class="note">
      <strong>Scope:</strong>
      This page reports automated validation of the existing Prototype A implementation.
      It does not alter or reimplement game rules, scenes, controls, art, or causal behavior.
    </div>

    <div class="note">
      <strong>Platform note:</strong>
      The game itself is not hosted in this page. The canonical prototype target remains
      Godot 4.7.2 .NET on desktop; this site is only a CI test report.
    </div>
  </main>
</body>
</html>
"""

    (output_dir / "index.html").write_text(page, encoding="utf-8")
    (output_dir / ".nojekyll").write_text("", encoding="utf-8")

    print(f"[TEST-SITE] Rendered {output_dir / 'index.html'} overall={overall}")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
