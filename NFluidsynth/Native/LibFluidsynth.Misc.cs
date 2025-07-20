using System;
using System.Runtime.InteropServices;

namespace NFluidsynth.Native
{
    internal static partial class LibFluidsynth
    {
        [LibraryImport(LibraryName)]
        internal static partial int fluid_is_soundfont([MarshalAs(LP_Str)] string filename);

        [LibraryImport(LibraryName)]
        internal static partial int fluid_is_midifile([MarshalAs(LP_Str)] string filename);

        [LibraryImport(LibraryName)]
        internal static partial IntPtr fluid_set_log_function(int severity, Logger.LoggerDelegate func, IntPtr data);
    }
}
