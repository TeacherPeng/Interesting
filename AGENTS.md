# AGENTS.md — ElevenAssistant

.NET 10 Android app (`net10.0-android`) using `AccessibilityService` for automated swipe, tap, and scheduled actions. Target SDK 24+.

## Build & Run

```powershell
dotnet build
dotnet publish -c Release   # publishes android-arm64 APK/AAB
```

No test, lint, or format commands exist. No CI.

## Architecture

- **`MainActivity.cs`** — Launcher UI with delay presets, checkboxes (Swipe / ClockIn / AdverOnly), Start/Stop buttons. Sends **broadcasts** to the service.
- **`ElevenAssistantService.cs`** — `AccessibilityService` that performs swipe gestures, timed "clock-in" tap sequences, and ad clicking. Runs a `Handler`-based loop.
- **`PackageInfo`** (in MainActivity.cs:8-19) — Constant definitions for broadcast actions and intent extras.
- **`Resources/layout/activity_main.xml`** — UI layout.
- **`Resources/xml/accessibility_service_config.xml`** — Service config (event types, gesture capability).
- **`AndroidManifest.xml`** — Declares `INTERNET`, `FOREGROUND_SERVICE` permissions; queries `com.ss.android.ugc.aweme.lite` (target app).

## IPC: Activity → Service

Communication uses Android broadcasts (not bound services):

| Action | Extra Keys | Effect |
|--------|-----------|--------|
| `com.pengsw.elevenassistant.START_ACTION` | `min_delay`, `max_delay`, `enable_swipe`, `enable_schedule`, `adver_only` | Start the automation loop |
| `com.pengsw.elevenassistant.STOP_ACTION` | *(none used beyond action)* | Stop the automation loop |

`ElevenAssistantService` registers an internal `BroadcastReceiver` (`ActionControlReceiver`) that reads extras from the intent.

## Behavior Modes (set via checkboxes)

| Mode | Flag | Behavior |
|------|------|----------|
| Swipe | `enableSwipe` | Random vertical swipe between 450-550x, 800-1600y with sine-wave tremor. Delay 4-10s default. |
| ClockIn | `enableSchedule` | At scheduled times (10:00, 12:00, 14:00, 16:00, 18:00), performs a 5-step tap sequence then 5x back. |
| AdverOnly | `adverOnly` | Click ad button → wait 35s → close ad → repeat every 11 min. Overrides all other modes. |

If both Swipe and ClockIn are on, ClockIn takes priority at scheduled times.

## Hardcoded Coordinates

Screen taps use hardcoded pixel coordinates from `UI坐标.txt`. These are specific to the target app (抖音/TikTok Lite) on a particular device/resolution. Always verify against actual device layout before changing.

## Key Conventions

- The solution uses the new `.slnx` XML format (not `.sln`).
- `ElevenAssistantService` uses `Handler` + `Runnable` for scheduling (no coroutines/async).
- Accessibility node traversal must `Recycle()` child nodes to avoid memory leaks (see `ElevenAssistantService.cs:286`).
- The target app package (`com.ss.android.ugc.aweme.lite`) is queried in the manifest under `<queries>`.
