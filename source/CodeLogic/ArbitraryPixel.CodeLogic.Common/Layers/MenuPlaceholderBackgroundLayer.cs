using ArbitraryPixel.CodeLogic.Common.Entities;
using ArbitraryPixel.Common;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.Layer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace ArbitraryPixel.CodeLogic.Common.Layers
{
    public class MenuPlaceholderBackgroundLayer : LayerBase
    {
        private class Star
        {
            public Vector2 Position { get; set; }
            public float Speed { get; set; }
            public Vector2 Direction { get; set; }
            public Color Colour { get; set; }

            public Vector2 Velocity
            {
                get { return this.Speed * this.Direction; }
            }
        }

        private ISpriteBatch _spriteBatch;
        private ISpriteFont _textFont;
        private ITexture2D _textTexture;

        private float _maxLength;
        private float _vMin = 50;
        private float _vMax = 1500;
        private float _textScale = 1f;
        private float _scaleMax = 1.05f;
        private float _scaleMin = 0.95f;
        private float _scaleInc = 0f;
        private Vector2 _textAnchor = Vector2.Zero;
        private RectangleF _screenRect;
        private List<Star> _stars = new List<Star>();
        private IRandom _random;
        private ITexture2D _starTexture;

        public MenuPlaceholderBackgroundLayer(IEngine host, ISpriteBatch mainSpriteBatch)
            : base(host, mainSpriteBatch)
        {
            _spriteBatch = mainSpriteBatch ?? throw new ArgumentNullException();

            _screenRect = new RectangleF(Vector2.Zero, (SizeF)this.Host.ScreenManager.World);

            ITextureEntity backDrop = GameObjectFactory.Instance.CreateTextureEntity(this.Host, _screenRect, mainSpriteBatch, this.Host.AssetBank.Get<ITexture2D>("Pixel"), Color.Black);
            this.AddEntity(backDrop);

            _random = GameObjectFactory.Instance.CreateRandom();
            _starTexture = this.Host.AssetBank.Get<ITexture2D>("Pixel");
            _textFont = this.Host.AssetBank.Get<ISpriteFont>("Debug");
            _scaleInc = (_scaleMax - _scaleMin) / 1f;

            GenerateStars(500);
        }

        private void GenerateStars(int numStars)
        {
            _maxLength = (Vector2.Zero - _screenRect.Centre).Length();
            for (int i = 0; i < numStars; i++)
            {
                Vector2 pos = new Vector2(_random.Next((int)_screenRect.Left, (int)_screenRect.Right), _random.Next((int)_screenRect.Top, (int)_screenRect.Bottom));
                Vector2 v = pos - _screenRect.Centre;
                Vector2 vDir = Vector2.Normalize(v);
                float distFactor = MathHelper.CatmullRom(1, 0, 1, 5, v.Length() / _maxLength);
                float speed = _vMin +  distFactor * (_vMax - _vMin);
                float cValue = 0.1f + 0.9f * MathHelper.Lerp(0, 1, distFactor);
                Color c = new Color(cValue, cValue, cValue);

                Star s = new Star()
                {
                    Position = pos,
                    Direction = vDir,
                    Speed = speed,
                    Colour = c
                };

                _stars.Add(s);
            }
        }

        private void CreateTextTexture()
        {
            string text = "I am a placeholder background! :D";
            SizeF textSize = _textFont.MeasureString(text);
            IRenderTarget2D target = this.Host.GrfxFactory.RenderTargetFactory.Create(this.Host.Graphics.GraphicsDevice, (int)textSize.Width, (int)textSize.Height, RenderTargetUsage.DiscardContents);
            this.Host.Graphics.GraphicsDevice.SetRenderTarget(target);
            _spriteBatch.Begin();
            _spriteBatch.DrawString(_textFont, text, Vector2.Zero, Color.White);
            _spriteBatch.End();
            this.Host.Graphics.GraphicsDevice.SetRenderTarget(null);
            _textTexture = target;

            _textAnchor = new Vector2(_screenRect.Centre.X, _screenRect.Bottom - 35 / 2f);
        }

        protected override void OnPreDraw(GameTime gameTime)
        {
            if (_textTexture == null)
                CreateTextTexture();
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            base.OnUpdate(gameTime);

            _textScale += _scaleInc * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_textScale > _scaleMax)
            {
                _textScale -= (_textScale - _scaleMax);
                _scaleInc *= -1;
            }
            else if (_textScale < _scaleMin)
            {
                _textScale += (_scaleMin - _textScale);
                _scaleInc *= -1;
            }

            // Update current stars
            for (int i = 0; i < _stars.Count; i++)
            {
                _stars[i].Position += _stars[i].Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (!_screenRect.Contains(_stars[i].Position))
                {
                    _stars.RemoveAt(i);
                    i--;
                }
                else
                {
                    float distFactor = (_stars[i].Position - _screenRect.Centre).Length() / _maxLength;
                    _stars[i].Speed = _vMin + MathHelper.CatmullRom(1, 0, 1, 5, distFactor) * (_vMax - _vMin);
                    float cValue = 0.1f + 0.9f * MathHelper.Lerp(0, 1, distFactor);
                    _stars[i].Colour = new Color(cValue, cValue, cValue);
                }
            }

            // Generate new ones
            GenerateStars(_random.Next(10, 30));
        }

        protected override void OnDraw(GameTime gameTime)
        {
            base.OnDraw(gameTime);

            _spriteBatch.Begin(this.SpriteSortMode, this.BlendState, this.SamplerState, this.DepthStencilState, this.RasterizerState, this.Effect, this.Host.ScreenManager.ScaleMatrix);

            foreach (Star s in _stars)
            {
                _spriteBatch.Draw(_starTexture, s.Position, s.Colour);
            }

            _spriteBatch.Draw(_textTexture, _textAnchor, null, Color.White, 0f, new Vector2(_textTexture.Width / 2f, _textTexture.Height / 2f), new Vector2(_textScale), SpriteEffects.None, 0f);
            _spriteBatch.End();
        }
    }
}
