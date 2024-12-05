using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Animation;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.Entity;
using Microsoft.Xna.Framework;
using System;

namespace ArbitraryPixel.CodeLogic.Common.Entities
{
    public class TapScreenText : GameEntityBase
    {
        public const double DelayBeforeVisible = 10f;

        private ISpriteBatch _spriteBatch;
        private ISpriteFont _font;
        private IValueAnimation<float> _opacityAnimation;

        private double _showDelay = DelayBeforeVisible;
        private string _text = "Tap the Screen";

        public TapScreenText(IEngine host, ISpriteBatch spriteBatch, ISpriteFont font, IAnimationFactory<float> animationFactory)
            : base(host, new RectangleF(0, 0, 1, 1))
        {
            _spriteBatch = spriteBatch ?? throw new ArgumentNullException();
            _font = font ?? throw new ArgumentNullException();
            var tAnimationFactory = animationFactory ?? throw new ArgumentNullException();

            _opacityAnimation = animationFactory.CreateValueAnimation(0f, new float[] { 1f, 1f, 0f, 1f }, true);

            SizeF textSize = _font.MeasureString(_text);
            Vector2 loc = ((SizeF)this.Host.ScreenManager.World).Centre - textSize.Centre;
            loc.Y = textSize.Height;

            this.Bounds = new RectangleF(loc, textSize);
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            base.OnUpdate(gameTime);

            if (_showDelay > 0)
            {
                _showDelay -= gameTime.ElapsedGameTime.TotalSeconds;
                if (_showDelay < 0)
                    _showDelay = 0;
            }
            else
            {
                _opacityAnimation.Update(gameTime);
            }
        }

        protected override void OnDraw(GameTime gameTime)
        {
            base.OnDraw(gameTime);

            if (_showDelay == 0)
                _spriteBatch.DrawString(_font, _text, this.Bounds.Location, Color.White * _opacityAnimation.Value);
        }
    }
}
