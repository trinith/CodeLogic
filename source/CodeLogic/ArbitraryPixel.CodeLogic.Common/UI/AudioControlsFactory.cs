using ArbitraryPixel.CodeLogic.Common.Entities;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.Entity;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ArbitraryPixel.CodeLogic.Common.UI
{
    public interface IAudioControlsFactory
    {
        AudioControlsFactoryResult CreateControls(Vector2 anchor, SizeF padding, ISpriteBatch spriteBatch, string fontAsset, string enableLabel, string volumeLabel);
    }

    public class AudioControlsFactory : IAudioControlsFactory
    {
        public IEngine Host { get; private set; }
        public RectangleF ContentBounds { get; private set; }

        public AudioControlsFactory(IEngine host, RectangleF contentBounds)
        {
            this.Host = host ?? throw new ArgumentNullException();
            this.ContentBounds = contentBounds;
        }

        public AudioControlsFactoryResult CreateControls(Vector2 anchor, SizeF padding, ISpriteBatch spriteBatch, string fontAsset, string enableLabel, string volumeLabel)
        {
            List<IEntity> entities = new List<IEntity>();
            Vector2 origAnchor = anchor;

            RectangleF buttonBounds = new RectangleF(anchor, new SizeF(100, 100));
            ICheckButton button = GameObjectFactory.Instance.CreateCheckButton(this.Host, buttonBounds, spriteBatch);
            entities.Add(button);

            ISpriteFont font = this.Host.AssetBank.Get<ISpriteFont>(fontAsset);

            entities.Add(
                GameObjectFactory.Instance.CreateGenericTextLabel(
                    this.Host,
                    new Vector2(
                        button.Bounds.Right + padding.Width,
                        button.Bounds.Centre.Y - font.LineSpacing / 2f
                    ),
                    spriteBatch,
                    font,
                    enableLabel,
                    CodeLogicEngine.Constants.ClrMenuFGHigh
                )
            );

            anchor.X = origAnchor.X + 2 * padding.Width;
            anchor.Y += button.Bounds.Height + padding.Height;

            entities.Add(
                GameObjectFactory.Instance.CreateGenericTextLabel(
                    this.Host,
                    anchor,
                    spriteBatch,
                    font,
                    volumeLabel,
                    CodeLogicEngine.Constants.ClrMenuFGHigh
                )
            );
            anchor.Y += font.LineSpacing + padding.Height;

            RectangleF sliderBounds = new RectangleF(
                anchor.X,
                anchor.Y,
                this.ContentBounds.Width - (anchor.X - this.ContentBounds.X) - padding.Width,
                50f
            );
            ISlider slider = GameObjectFactory.Instance.CreateSlider(this.Host, sliderBounds, spriteBatch);
            slider.BackgroundColour = CodeLogicEngine.Constants.ClrMenuFGLow;
            slider.ForegroundColour = CodeLogicEngine.Constants.ClrMenuFGMid;
            slider.HotColour = CodeLogicEngine.Constants.ClrMenuFGHigh;
            slider.Minimum = 0f;
            slider.Maximum = 1f;
            entities.Add(slider);

            anchor = new Vector2(origAnchor.X, slider.Bounds.Bottom + padding.Height);

            return new AudioControlsFactoryResult(entities.ToArray(), button, slider, anchor);
        }
    }
}
