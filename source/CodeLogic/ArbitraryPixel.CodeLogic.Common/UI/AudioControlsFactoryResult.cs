using ArbitraryPixel.CodeLogic.Common.Entities;
using ArbitraryPixel.Platform2D.Entity;
using Microsoft.Xna.Framework;
using System;

namespace ArbitraryPixel.CodeLogic.Common.UI
{
    public class AudioControlsFactoryResult
    {
        public IEntity[] Entities { get; private set; }

        public ICheckButton EnableControl { get; private set; }
        public ISlider VolumeControl { get; private set; }
        public Vector2 NextAnchor { get; private set; }

        public AudioControlsFactoryResult(IEntity[] entities, ICheckButton enableControl, ISlider volumeControl, Vector2 nextAnchor)
        {
            this.Entities = entities ?? throw new ArgumentNullException();
            this.EnableControl = enableControl ?? throw new ArgumentNullException();
            this.VolumeControl = volumeControl ?? throw new ArgumentNullException();
            this.NextAnchor = nextAnchor;
        }
    }
}
