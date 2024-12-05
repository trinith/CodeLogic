using ArbitraryPixel.Common;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.UI;
using Microsoft.Xna.Framework;
using System;

namespace ArbitraryPixel.CodeLogic.Common.Entities
{
    public interface IProgressBar : IButton
    {
        event EventHandler<StateChangedEventArgs<float>> ValueChanged;

        float Minimum { get; set; }
        float Maximum { get; set; }
        float Value { get; set; }

        Color BackgroundColour { get; set; }
        Color ForegroundColour { get; set; }
    }

    public class ProgressBar : ButtonBase, IProgressBar
    {
        private ITexture2D _pixel;
        private ISpriteBatch _spriteBatch;

        public ProgressBar(IEngine host, RectangleF bounds, ISpriteBatch spriteBatch)
            : base(host, bounds)
        {
            _spriteBatch = spriteBatch ?? throw new ArgumentNullException();
            _pixel = this.Host.AssetBank.Get<ITexture2D>("Pixel");
        }

        #region IProgressBar Implementation
        public event EventHandler<StateChangedEventArgs<float>> ValueChanged;

        public float Maximum { get; set; } = 100;
        public float Minimum { get; set; } = 0;
        public Color BackgroundColour { get; set; } = new Color(32, 32, 32);
        public Color ForegroundColour { get; set; } = Color.LightGray;

        private float _value = 50;
        public float Value
        {
            get { return _value; }
            set
            {
                if (value != _value)
                {
                    float oldValue = _value;
                    _value = value;

                    OnValueChanged(new StateChangedEventArgs<float>(oldValue, _value));
                }
            }
        }
        #endregion

        protected void OnValueChanged(StateChangedEventArgs<float> e)
        {
            if (this.ValueChanged != null)
                this.ValueChanged(this, e);
        }

        protected override void OnDraw(GameTime gameTime)
        {
            base.OnDraw(gameTime);

            // This is minor, we can just calculate values on draw.
            float value = MathHelper.Clamp(this.Value, this.Minimum, this.Maximum);
            float percentage = (value - this.Minimum) / (this.Maximum - this.Minimum);

            _spriteBatch.Draw(_pixel, this.Bounds, this.BackgroundColour);
            _spriteBatch.Draw(
                _pixel,
                new RectangleF(
                    this.Bounds.Left,
                    this.Bounds.Top,
                    this.Bounds.Width * percentage,
                    this.Bounds.Height
                ),
                this.ForegroundColour
            );
        }
    }
}
