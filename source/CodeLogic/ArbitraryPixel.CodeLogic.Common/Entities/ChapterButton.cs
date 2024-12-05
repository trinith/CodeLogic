using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Engine;
using Microsoft.Xna.Framework;
using System;

namespace ArbitraryPixel.CodeLogic.Common.Entities
{
    public interface IChapterButton : ISimpleButton
    {
        string Description { get; set; }
    }

    public class ChapterButton : SimpleButton, IChapterButton
    {
        protected ISpriteFont _descriptionFont;
        private string _description;

        public string Description
        {
            get { return _description; }
            set
            {
                if (value != _description)
                {
                    _description = value;
                    OnTextPropertyUpdated();
                }
            }
        }

        public ChapterButton(IEngine host, RectangleF bounds, ISpriteBatch spriteBatch, ISpriteFont titleFont, ISpriteFont descriptionFont)
            : base(host, bounds, spriteBatch, titleFont)
        {
            _descriptionFont = descriptionFont ?? throw new ArgumentNullException();
        }

        protected override void OnDrawBackground(bool isPressed, Rectangle bounds)
        {
            Color bgColour = isPressed ? this.NormalColour : this.BackColour;
            _spriteBatch.Draw(_pixel, bounds, bgColour);
        }

        protected override void OnDrawText(bool isPressed, Rectangle bounds)
        {
            bounds.Inflate(-CodeLogicEngine.Constants.TextWindowPadding.Width, -CodeLogicEngine.Constants.TextWindowPadding.Height);

            float textY = bounds.Top + (bounds.Height / 2f) - (_font.LineSpacing + _descriptionFont.LineSpacing) / 2f;
            Color textColour = isPressed ? this.PressedColour : this.NormalColour;

            _spriteBatch.DrawString(_font, this.Text, new Vector2(bounds.Left, textY), textColour);
            _spriteBatch.DrawString(_descriptionFont, this.Description, new Vector2(bounds.Left, textY + _font.LineSpacing), textColour);
        }
    }
}
