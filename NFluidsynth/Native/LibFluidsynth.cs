using System;
using System.Runtime.InteropServices;

// ReSharper disable LoopCanBeConvertedToQuery

namespace NFluidsynth.Native;

public enum FluidSynthAbiVersion : uint {
    V2 = 2,
    V3 = 3
}

internal static partial class LibFluidsynth
{
    public const int FluidOk = 0;
    public const int FluidFailed = -1;
    public const string LibraryName = "fluidsynth";

    // Assumption here is that this binds against whatever API .3 is,
    //  but will try the general name anyway just in case.
    private static readonly string[] LINUX_FLUIDSYNTH_ABI_V3_DLL_NAMES = ["libfluidsynth.so.3"];
    private static readonly string[] LINUX_FLUIDSYNTH_ABI_V2_DLL_NAMES = ["libfluidsynth.so.2", "libfluidsynth.so"];
    private static readonly string[] OSX_FLUIDSYNTH_DLL_NAMES = ["libfluidsynth.dylib"];

    private static readonly string[] WINDOWS_FLUIDSYNTH_ABI_V3_DLL_NAMES = ["libfluidsynth-3"];
    private static readonly string[] WINDOWS_FLUIDSYNTH_ABI_V2_DLL_NAMES =
    [
        "libfluidsynth-2",
        "fluidsynth",
        "libfluidsynth"
    ];

    // Supports both ABI 2 and ABI 3 of FluidSynth
    // https://abi-laboratory.pro/index.php?view=timeline&l=fluidsynth
    public static FluidSynthAbiVersion LibraryVersion { get; private set; } = FluidSynthAbiVersion.V2;

#if NETCOREAPP
    static LibFluidsynth()
    {
        try
        {
            NativeLibrary.SetDllImportResolver(typeof(LibFluidsynth).Assembly, (name, assembly, path) =>
            {
                if (name != LibraryName)
                    return IntPtr.Zero;

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                    return TryLoadLinux(assembly, path);

                if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                    return TryLoadOsx(assembly, path);

                // ReSharper disable once ConvertIfStatementToReturnStatement
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    return TryLoadWindows(assembly, path);

                return IntPtr.Zero;
            });
        }
        catch (Exception)
        {
            // An exception can be thrown in the above call if someone has already set a DllImportResolver.
            // (Can occur if the application wants to override behaviour.)
            // This does not throw away failures to resolve.
        }
    }

    private static IntPtr TryLoadLinux(System.Reflection.Assembly assembly, DllImportSearchPath? path)
    {
        if (TryLoadDll(assembly, path, LINUX_FLUIDSYNTH_ABI_V3_DLL_NAMES, out var handle)) {
            LibraryVersion = FluidSynthAbiVersion.V3;

            return handle;
        }

        _ = TryLoadDll(assembly, path, LINUX_FLUIDSYNTH_ABI_V2_DLL_NAMES, out handle);

        return handle;
    }

    private static IntPtr TryLoadOsx(System.Reflection.Assembly assembly, DllImportSearchPath? path)
    {
        _ = TryLoadDll(assembly, path, OSX_FLUIDSYNTH_DLL_NAMES, out var handle);

        return handle;
    }


    private static IntPtr TryLoadWindows(System.Reflection.Assembly assembly, DllImportSearchPath? path)
    {
        if (TryLoadDll(assembly, path, WINDOWS_FLUIDSYNTH_ABI_V3_DLL_NAMES, out var handle)) {
            LibraryVersion = FluidSynthAbiVersion.V3;

            return handle;
        }

        _ = TryLoadDll(assembly, path, WINDOWS_FLUIDSYNTH_ABI_V2_DLL_NAMES, out handle);

        return handle;
    }

    private static bool TryLoadDll(
        System.Reflection.Assembly assembly,
        DllImportSearchPath? path,
        string[] names,
        out IntPtr handle
    )
    {
        foreach (var name in names)
        {
            if (!NativeLibrary.TryLoad(name, assembly, path, out handle))
                continue;

            return true;
        }

        handle = IntPtr.Zero;

        return false;
    }
#endif
}