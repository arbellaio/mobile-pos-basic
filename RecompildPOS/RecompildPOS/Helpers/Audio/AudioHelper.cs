using System;
using System.Collections.Generic;
using System.Text;
using Plugin.SimpleAudioPlayer;

namespace RecompildPOS.Helpers.Audio
{
    public class AudioHelper
    {
        static ISimpleAudioPlayer _player;
        public static void PlayBeep()
        {
            InitializePlayer();
            _player.Play();
        }

        public static void InitializePlayer()
        {
            if (_player == null)
            {
                _player = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current;
                _player.Load("beep.mp3");
            }

        }
    }
}
