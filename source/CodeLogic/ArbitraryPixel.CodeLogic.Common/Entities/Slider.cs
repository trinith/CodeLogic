using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.UI;
using Microsoft.Xna.Framework;
using System;

namespace ArbitraryPixel.CodeLogic.Common.Entities
{
    public interface ISlider : IProgressBar
    {
        Color HotColour { get; set; }
    }

    public class Slider : ProgressBar, ISlider
    {
        private ITexture2D _pixel;
        private ISpriteBatch _spriteBatch;
        private bool _touched = false;

        public Slider(IEngine host, RectangleF bounds, ISpriteBatch spriteBatch)
            : base(host, bounds, spriteBatch)
        {
            _spriteBatch = spriteBatch ?? throw new ArgumentNullException();
            _pixel = this.Host.AssetBank.Get<ITexture2D>("Pixel");
        }

        #region ISlider Implementation
        public Color HotColour { get; set; } = Color.White;
        #endregion

        protected override void OnTouched(ButtonEventArgs e)
        {
            base.OnTouched(e);

            _touched = true;
        }

        protected override void OnTapped(ButtonEventArgs e)
        {
            base.OnTapped(e);

            _touched = false;
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            base.OnUpdate(gameTime);

            if (_touched)
            {
                var surfaceState = this.Host.InputManager.GetSurfaceState();

                if (surfaceState.IsTouched)
                {
                    Vector2 worldLocation = this.Host.ScreenManager.PointToWorld(surfaceState.SurfaceLocation);
                    float curPos = MathHelper.Clamp(worldLocation.X, this.Bounds.Left, this.Bounds.Right);
                    float curVal = ((curPos - this.Bounds.Left) / this.Bounds.Width) * (this.Maximum - this.Minimum);
                    this.Value = curVal;
                }
                else
                {
                    _touched = false;
                }
            }
        }

        protected override void OnDraw(GameTime gameTime)
        {
            // We will handle drawing ourselves.
            //base.OnDraw(gameTime);

            Color foreColour = (_touched) ? this.HotColour : this.ForegroundColour;

            // This is minor, we can just calculate values on draw.
            float value = MathHelper.Clamp(this.Value, this.Minimum, this.Maximum);
            float percentage = (value - this.Minimum) / (this.Maximum - this.Minimum);

            RectangleF bounds = this.Bounds;

            _spriteBatch.Draw(_pixel, bounds, foreColour);

            bounds.Inflate(-2, -2);
            _spriteBatch.Draw(_pixel, bounds, this.BackgroundColour);

            bounds.Inflate(-5, -5);
            _spriteBatch.Draw(
                _pixel,
                new RectangleF(
                    bounds.Left,
                    bounds.Top,
                    bounds.Width * percentage,
                    bounds.Height
                ),
                foreColour
            );
        }
    }
}
