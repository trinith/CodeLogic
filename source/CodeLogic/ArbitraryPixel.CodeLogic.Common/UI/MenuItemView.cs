using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.Text;
using Microsoft.Xna.Framework;
using System;

namespace ArbitraryPixel.CodeLogic.Common.UI
{
    public class MenuItemView : MenuViewBase, IMenuItemView
    {
        private const float XOFFSET = 10f;
        private const float YOFFSET = 6f;

        private ISpriteBatch _spriteBatch;
        private ISpriteFont _font;
        private Vector2 _textPos;
        private TextLineAlignment _lineAlignment = TextLineAlignment.Left;

        public bool IsSelected { get; set; } = false;
        public TextLineAlignment LineAlignment
        {
            get { return _lineAlignment; }
            set
            {
                if (_lineAlignment != value)
                {
                    _lineAlignment = value;
                    UpdateRelativePosition();
                }
            }
        }

        public MenuItemView(IEngine host, RectangleF bounds, IMenuItem viewOf, ISpriteBatch spriteBatch, ISpriteFont font, TextLineAlignment lineAlignment)
            : base(host, bounds, viewOf)
        {
            _spriteBatch = spriteBatch ?? throw new ArgumentNullException();
            _font = font ?? throw new ArgumentNullException();
            _lineAlignment = lineAlignment;

            UpdateRelativePosition();
        }

        private void UpdateRelativePosition()
        {
            SizeF textSize = _font.MeasureString(this.ViewOf.Text);

            switch (_lineAlignment)
            {
                case TextLineAlignment.Right:
                    _textPos = new Vector2(this.Bounds.Right - textSize.Width - XOFFSET, this.Bounds.Top);
                    break;
                case TextLineAlignment.Centre:
                    _textPos = new Vector2(this.Bounds.Left + this.Bounds.Width / 2f - textSize.Width / 2f, this.Bounds.Top);
                    break;
                case TextLineAlignment.Left:
                default:
                    _textPos = new Vector2(this.Bounds.Left + XOFFSET, this.Bounds.Top);
                    break;
            }

            // Centre item in vertical space.
            _textPos.Y = (int)(this.Bounds.Top + this.Bounds.Height / 2f - textSize.Height / 2f + YOFFSET);
        }

        protected override void OnViewOfSet()
        {
            base.OnViewOfSet();
            UpdateRelativePosition();
        }

        protected override void OnDraw(GameTime gameTime)
        {
            base.OnDraw(gameTime);

            Color textColour = (this.IsSelected) ? CodeLogicEngine.Constants.ClrMenuTextSelected : CodeLogicEngine.Constants.ClrMenuTextNormal;
            _spriteBatch.DrawString(_font, this.ViewOf.Text, _textPos, textColour);
        }
    }
}
