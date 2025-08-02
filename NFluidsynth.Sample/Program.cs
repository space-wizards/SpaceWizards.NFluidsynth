using System;
using System.Threading;
using System.Linq;
using System.IO;
using System.Runtime.InteropServices;

namespace NFluidsynth.Sample
{
    public static class Program
    {
        private const string LINUX_GS_DEFAULT_SOUNDFONT = "/usr/share/sounds/sf2/FluidR3_GS.sf2";
        private const string LINUX_GM_DEFAULT_SOUNDFONT = "/usr/share/sounds/sf2/FluidR3_GM.sf2";

        private const string WINDOWS_GM_DEFAULT_SOUNDFONT = @"C:\WINDOWS\SYSTEM32\DRIVERS\GM.DLS";

        private const string OSX_GS_DEFAULT_SOUNDFONT =
            "/System/Library/Components/CoreAudio.component/Contents/Resources/gs_instruments.dls";

        public static void Main(string[] args)
        {
            using (var settings = new Settings())
            {
                using (var syn = new Synth(settings))
                {
                    foreach (var arg in args)
                        if (SoundFont.IsSoundFont(arg))
                            syn.LoadSoundFont(arg, true);

                    if (syn.FontCount == 0 && !LoadDefaultSoundfont(syn))
                        return;

                    for (var i = 0; i < 16; i++)
                        syn.SoundFontSelect(i, 0);

                    var files = args.Where(SoundFont.IsMidiFile).ToList();

                    if (files.Count == 0)
                    {
                        using (new AudioDriver(syn.Settings, syn))
                        {
                            syn.ProgramChange(0, 1);
                            syn.NoteOn(0, 60, 120);
                            Thread.Sleep(500);
                            syn.NoteOff(0, 60);
                        }

                        return;
                    }

                    foreach (var arg in files)
                    {
                        using (var player = new Player(syn))
                        {
                            using (new AudioDriver(syn.Settings, syn))
                            {
                                player.Add(arg);
                                player.Play();
                                player.Join();
                            }
                        }
                    }
                }
            }
        }

        private static bool LoadDefaultSoundfont(Synth syn)
        {
            var soundfontPath = "";

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                if (File.Exists(LINUX_GS_DEFAULT_SOUNDFONT))
                    soundfontPath = LINUX_GS_DEFAULT_SOUNDFONT;
                else if (File.Exists(LINUX_GM_DEFAULT_SOUNDFONT))
                    soundfontPath = LINUX_GM_DEFAULT_SOUNDFONT;
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX) && File.Exists(OSX_GS_DEFAULT_SOUNDFONT))
                soundfontPath = OSX_GS_DEFAULT_SOUNDFONT;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && File.Exists(WINDOWS_GM_DEFAULT_SOUNDFONT))
                soundfontPath = WINDOWS_GM_DEFAULT_SOUNDFONT;

            if (soundfontPath == "")
            {
                Console.WriteLine("No system sound font file found.");

                return false;
            }

            syn.LoadSoundFont(soundfontPath, true);

            return true;
        }
    }
}
