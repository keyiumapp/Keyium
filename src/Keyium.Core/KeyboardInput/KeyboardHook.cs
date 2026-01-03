/**
* Keyium.Core, KeyboardHook.cs.
* Copyright (c) 2025 DarkerMango. All rights reserved.
* SPDX-License-Identifier: GPL-3.0-or-later
*
* This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
* as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
*
* This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the
* implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
*
* You should have received a copy of the GNU General Public License along with this program. If not, see <https://www.gnu.org/licenses/>.
**/

using Keyium.Core.AudioEngine;
using Keyium.Core.KeyboardInput.KeyboardAdapter;
using SharpHook;

namespace Keyium.Core.KeyboardInput
{
    public class KeyboardHook
    {
        public string AudioFile { get; private set; }

        private readonly TaskPoolGlobalHook _hook = new();
        private readonly IAudioPlayer _audioPlayer;

        //public KeyboardHook(CachedSound audioClip, AudioPlayer audioPlayer)
        //{
        //    AudioClip = audioClip;
        //    AudioFile = AudioClip.ToString();

        //    _audioPlayer = audioPlayer;
        //}

        public KeyboardHook(string audioFile, IAudioPlayer audioPlayer)
        {
            AudioFile = audioFile;
            _audioPlayer = audioPlayer;
        }

        public void StartListening(IKeyboardAdapter adapter)
        {
            switch (adapter.TriggerMethod)
            {
                case KeyboardTriggerMethod.Pressed:
                    _hook.KeyPressed += adapter.OnKeyTrigger;
                    break;

                case KeyboardTriggerMethod.Released:
                    _hook.KeyReleased += adapter.OnKeyTrigger;
                    break;

                case KeyboardTriggerMethod.Typed:
                    _hook.KeyTyped += adapter.OnKeyTrigger;
                    break;
            }

            _hook.Run();
        }

        public void Start()
        {
            _hook.KeyPressed += OnKeyTrigger;
            _hook.Run();
        }

        public void OnKeyTrigger(object? sender, KeyboardHookEventArgs e) =>
            _audioPlayer.PlaySound(AudioFile);
    }
}
