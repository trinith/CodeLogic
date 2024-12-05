using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.UI;
using Microsoft.Xna.Framework;
using System;

namespace ArbitraryPixel.CodeLogic.Common.Entities
{
    public interface ICheckButton : IButton
    {
        event EventHandler CheckStateChanged;

        bool Checked { get; set; }
    }

    public class CheckButton : ButtonBase, ICheckButton
    {
        private ISpriteBatch _spriteBatch;

        public event EventHandler CheckStateChanged;
        public bool Checked
        {
            get { return _checked; }
            set
            {
                if (value != _checked)
                {
                    _checked = value;
                    OnCheckStateChanged(new EventArgs());
                }
            }
        }
        private bool _checked = false;

        public CheckButton(IEngine host, RectangleF bounds, ISpriteBatch spriteBatch)
            : base(host, bounds)
        {
            _spriteBatch = spriteBatch ?? throw new ArgumentNullException();
        }

        protected override void OnTapped(ButtonEventArgs e)
        {
            base.OnTapped(e);

            this.Checked = !this.Checked;
        }

        protected override void OnDraw(GameTime gameTime)
        {
            base.OnDraw(gameTime);

            Color foreColour = (this.State == ButtonState.Pressed) ? CodeLogicEngine.Constants.ClrMenuFGHigh : CodeLogicEngine.Constants.ClrMenuFGMid;

            Rectangle bounds = (Rectangle)this.Bounds;
            ITexture2D pixel = this.Host.AssetBank.Get<ITexture2D>("Pixel");

            _spriteBatch.Draw(pixel, bounds, foreColour);
            bounds.Inflate(-2, -2);
            _spriteBatch.Draw(pixel, bounds, CodeLogicEngine.Constants.ClrMenuFGLow);

            if (this.Checked)
            {
                bounds.Inflate(-5, -5);
                _spriteBatch.Draw(pixel, bounds, foreColour);
            }
        }

        protected void OnCheckStateChanged(EventArgs e)
        {
            if (this.CheckStateChanged != null)
                this.CheckStateChanged(this, e);
        }
    }
}
