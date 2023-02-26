﻿using System;
using NFluidsynth;
using System.Threading;
using System.Linq;
using System.IO;

namespace NFluidsynth.Sample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using (var settings = new Settings())
            {
                // Change this if you don't have pulseaudio or want to change to anything else.
                if (Environment.OSVersion.Platform == PlatformID.Unix)
                    settings[ConfigurationKeys.AudioDriver].StringValue = "pulseaudio";
                settings[ConfigurationKeys.SynthAudioChannels].IntValue = 2;
                using (var syn = new Synth(settings))
                {
                    foreach (var arg in args)
                        if (SoundFont.IsSoundFont(arg))
                            syn.LoadSoundFont(arg, true);
                    if (syn.FontCount == 0)
                    {                        
                        const string SOUND_FONT_UBUNTU_14_04 = "/usr/share/sounds/sf2/FluidR3_GS.sf2";
                        const string SOUND_FONT_UBUNTU_22_10 = "/usr/share/sounds/sf2/FluidR3_GM.sf2";
                        if (File.Exists(SOUND_FONT_UBUNTU_14_04))
                            syn.LoadSoundFont(SOUND_FONT_UBUNTU_14_04, true);
                        else if (File.Exists(SOUND_FONT_UBUNTU_22_10))
                            syn.LoadSoundFont(SOUND_FONT_UBUNTU_22_10, true);
                        else
                        {
                            System.Console.WriteLine("No system sound font file found.");
                            return;
                        }
                    }
                    for (int i = 0; i < 16; i++)
                        syn.SoundFontSelect(i, 0);
                    var files = args.Where(SoundFont.IsMidiFile);
                    if (files.Any())
                    {
                        foreach (var arg in files)
                        {
                            using (var player = new Player(syn))
                            {
                                using (var adriver = new AudioDriver(syn.Settings, syn))
                                {
                                    player.Add(arg);
                                    player.Play();
                                    player.Join();
                                }
                            }
                        }
                    }
                    else
                    {
                        using (var adriver = new AudioDriver(syn.Settings, syn))
                        {
                            syn.ProgramChange(0, 1);
                            syn.NoteOn(0, 60, 120);
                            Thread.Sleep(5000);
                            syn.NoteOff(0, 60);
                        }
                    }
                }
            }
        }
    }
}
