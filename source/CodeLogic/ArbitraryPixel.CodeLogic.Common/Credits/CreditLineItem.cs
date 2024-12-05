using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.Entity;
using ArbitraryPixel.Platform2D.Text;
using Microsoft.Xna.Framework;
using System;

namespace ArbitraryPixel.CodeLogic.Common.Credits
{
    public interface ICreditLineItem : IGameEntity
    {
    }

    public class CreditLineItem : GameEntityBase, ICreditLineItem
    {
        private ISpriteBatch _spriteBatch;
        private Vector2 _velocity = new Vector2(0, -25);

        public ITextObject TextObject { get; private set; }

        public CreditLineItem(IEngine host, RectangleF bounds, ISpriteBatch spriteBatch, ITextObject textObject)
            : base(host, bounds)
        {
            _spriteBatch = spriteBatch ?? throw new ArgumentNullException();
            this.TextObject = textObject ?? throw new ArgumentNullException();
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            base.OnUpdate(gameTime);

            this.TextObject.Location += _velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (this.TextObject.Location.Y < this.Bounds.Top - this.TextObject.TextDefinition.Font.LineSpacing)
            {
                this.Alive = false;
            }
        }

        protected override void OnDraw(GameTime gameTime)
        {
            base.OnDraw(gameTime);

            if (this.TextObject.Location.Y <= this.Bounds.Bottom)
            {
                _spriteBatch.DrawString(
                    this.TextObject.TextDefinition.Font,
                    this.TextObject.CurrentText,
                    this.TextObject.Location,
                    this.TextObject.TextDefinition.Colour
                );
            }
        }
    }
}
