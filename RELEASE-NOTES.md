# Release Notes

## Master

- Moved TFM back to support a minimum of .NET 7.

## 0.2.1

- Added v3 ABI support for macOS.

## 0.2.0

- Added `Synth.TryNoteOn` and `Synth.TryNoteOff`, which return the status code instead of throwing.
- Added v3 ABI support for Windows.
- Internal code cleanup & modernization.

## 0.1.1

- Added compatibility with Fluidsynth 3 ABI break.

## 0.1.0

First release. Changes over base NFluidsynth:
- **Dropped .NET Framework support.** Minimum required is now .NET Standard 2.1.
- Removed `NFluidsynth.MidiAccess` project.
- Removed `Generator` project.