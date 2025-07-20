# NFluidsynth (Space Wizards Edition)
![Build & Test](https://github.com/space-wizards/SpaceWizards.NFluidsynth/workflows/Build%20&%20Test/badge.svg) [![NuGet link](https://img.shields.io/nuget/v/SpaceWizards.NFluidsynth)](https://www.nuget.org/packages/SpaceWizards.NFluidsynth)

NFluidsynth is a C# binding for [libfluidsynth](https://github.com/Fluidsynth/fluidsynth/).

It is a P/Invoke wrapper, therefore you need native libfluidsynth.so / libfluidsynth.dylib / (lib)fluidsynth.dll.
NFluidsynth builds and packages don't come up with those native libraries, so you are supposed to prepare them by yourself (at least for now).

The target API is Fluidsynth 2.1.x. The API mappings may not be complete (contributions are welcome).

Used mainly in [RobustToolbox](https://github.com/space-wizards/RobustToolbox) for MIDI input and playback support.

## How to Run This Project

1. Get the supported version of FluidSynth (currently 2.1.0.0) [from the FluidSynth repo](https://github.com/FluidSynth/fluidsynth/releases). Get the one for your operating system.
2. Clone this Git repo.
3. Open the downloaded Fluidsynth release. Open the /bin/ folder.
4. Copy everything from the /bin/ folder into the root of the repo (other than the Fluidsynth executable)
5. Run `dotnet run --project NFluidsynth.Sample`

You should now have a working test-bench for NFluidsynth.

