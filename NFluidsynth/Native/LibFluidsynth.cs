using System;
using System.Runtime.InteropServices;

namespace NFluidsynth.Native
{
    internal static partial class LibFluidsynth
    {
        public const string LibraryName = "fluidsynth";

        /**
        * Supports both ABI 2 and ABI 3 of Fluid Synth
        * https://abi-laboratory.pro/index.php?view=timeline&l=fluidsynth
        */
        public static int LibraryVersion 
        { 
            get
            {
                return _libraryVersion;
            }
            
            private set
            {
                _libraryVersion = value;
            }
        }
        private static int _libraryVersion = 2; // Assume using ABI 2 unless detecting otherwise

        public const int FluidOk = 0;
        public const int FluidFailed = -1;

#if NETCOREAPP
        static LibFluidsynth()
        {
            try
            {
                NativeLibrary.SetDllImportResolver(typeof(LibFluidsynth).Assembly, (name, assembly, path) =>
                {
                    IntPtr handle;
                    if (name == LibraryName)
                    {
                        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                        {
                            // Assumption here is that this binds against whatever API .3 is,
                            //  but will try the general name anyway just in case.
                            if (NativeLibrary.TryLoad("libfluidsynth.so.3", assembly, path, out handle))
                            {
                                LibFluidsynth.LibraryVersion = 3;
                                return handle;
                            }

                            if (NativeLibrary.TryLoad("libfluidsynth.so.2", assembly, path, out handle))
                                return handle;

                            if (NativeLibrary.TryLoad("libfluidsynth.so", assembly, path, out handle))
                                return handle;
                        }

                        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                        {
                            if (NativeLibrary.TryLoad("libfluidsynth.dylib", assembly, path, out handle))
                                return handle;
                        }
                    }

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
#endif
    }
}
