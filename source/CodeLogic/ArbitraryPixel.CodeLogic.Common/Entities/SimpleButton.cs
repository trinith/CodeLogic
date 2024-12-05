using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.UI;
using Microsoft.Xna.Framework;
using System;

namespace ArbitraryPixel.CodeLogic.Common.Entities
{
    public interface ISimpleButton : IButton
    {
        Color BackColour { get; set; }
        Color NormalColour { get; set; }
        Color PressedColour { get; set; }
        string Text { get; set; }
    }

    public class SimpleButton : ButtonBase, ISimpleButton
    {
        protected ISpriteBatch _spriteBatch;
        protected ITexture2D _pixel;
        protected ISpriteFont _font;
        private Vector2 _textPos;
        private bool _isPressed = false;

        private string _text = "";
        public string Text
        {
            get { return _text; }
            set
            {
                if (_text != value)
                {
                    _text = value;
                    OnTextPropertyUpdated();
                }
            }
        }

        public override RectangleF Bounds
        {
            get { return base.Bounds; }
            set
            {
                base.Bounds = value;
                OnTextPropertyUpdated();
            }
        }

        public Color BackColour { get; set; } = new Color(32, 32, 32, 128);
        public Color NormalColour { get; set; } = Color.Gray;
        public Color PressedColour { get; set; } = Color.White;

        public SimpleButton(IEngine host, RectangleF bounds, ISpriteBatch spriteBatch, ISpriteFont font)
            : base(host, bounds)
        {
            _spriteBatch = spriteBatch ?? throw new ArgumentNullException();
            _font = font ?? throw new ArgumentNullException();

            _pixel = this.Host.AssetBank.Get<ITexture2D>("Pixel");
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            base.OnUpdate(gameTime);

            _isPressed = this.State == ButtonState.Pressed;
        }

        protected override void OnDraw(GameTime gameTime)
        {
            base.OnDraw(gameTime);

            int borderSize = 1;
            Rectangle bounds = (Rectangle)this.Bounds;

            OnDrawBackground(_isPressed, bounds);
            OnDrawBorder(_isPressed, bounds, borderSize);
            bounds.Inflate(-borderSize, -borderSize);
            OnDrawText(_isPressed, bounds);
        }

        protected virtual void OnDrawBackground(bool isPressed, Rectangle bounds)
        {
            _spriteBatch.Draw(_pixel, bounds, this.BackColour);
        }

        protected virtual void OnDrawBorder(bool isPressed, Rectangle bounds, int borderSize)
        {
            Color borderColour = isPressed ? this.PressedColour : this.NormalColour;

            _spriteBatch.Draw(_pixel, new Rectangle(bounds.Location, new Point(bounds.Width, borderSize)), borderColour);
            _spriteBatch.Draw(_pixel, new Rectangle(new Point(bounds.Left, bounds.Bottom - borderSize), new Point(bounds.Width, borderSize)), borderColour);

            _spriteBatch.Draw(_pixel, new Rectangle(bounds.Location, new Point(borderSize, bounds.Height)), borderColour);
            _spriteBatch.Draw(_pixel, new Rectangle(new Point(bounds.Right - borderSize, bounds.Top), new Point(borderSize, bounds.Height)), borderColour);
        }

        protected virtual void OnDrawText(bool isPressed, Rectangle bounds)
        {
            if (this.Text != "")
            {
                Color textColour = isPressed ? this.PressedColour : this.NormalColour;
                _spriteBatch.DrawString(_font, this.Text, _textPos, textColour);
            }
        }

        protected virtual void OnTextPropertyUpdated()
        {
            if (_font != null)
            {
                SizeF textSize = _font.MeasureString(this.Text);
                _textPos = this.Bounds.Centre - textSize.Centre + new Vector2(0, 2);
                _textPos = new Vector2((int)_textPos.X, (int)_textPos.Y);
            }
        }
    }
}
