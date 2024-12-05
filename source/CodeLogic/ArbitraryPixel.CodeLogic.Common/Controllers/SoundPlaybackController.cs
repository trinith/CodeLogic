using ArbitraryPixel.Common.Audio;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ArbitraryPixel.CodeLogic.Common.Controllers
{
    public interface ISoundPlaybackController : IController
    {
        ISound[] PlayingSounds { get; }

        void AddSound(ISound sound, float delay = 0);
    }

    public class SoundPlaybackController : ISoundPlaybackController
    {
        private Dictionary<ISound, float> _queuedSounds = new Dictionary<ISound, float>();
        private List<ISound> _playingSounds = new List<ISound>();

        public ISound[] PlayingSounds => _playingSounds.ToArray();

        public void AddSound(ISound sound, float delay = 0)
        {
            if (sound == null)
                throw new ArgumentNullException();

            _queuedSounds.Add(sound, delay);
        }

        public void Reset()
        {
            foreach (ISound sound in _playingSounds)
                sound.Stop();

            _playingSounds.Clear();
            _queuedSounds.Clear();
        }

        public void Update(GameTime gameTime)
        {
            ISound[] queuedSounds = _queuedSounds.Keys.ToArray();
            foreach (ISound sound in queuedSounds)
            {
                _queuedSounds[sound] -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (_queuedSounds[sound] <= 0)
                {
                    sound.Play();

                    _queuedSounds.Remove(sound);
                    _playingSounds.Add(sound);
                }
            }

            for (int i = 0; i < _playingSounds.Count; i++)
            {
                if (_playingSounds[i].State == SoundState.Stopped)
                {
                    _playingSounds.RemoveAt(i);
                    i--;
                }
            }
        }
    }
}
