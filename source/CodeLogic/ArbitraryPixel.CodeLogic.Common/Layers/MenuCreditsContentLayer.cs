using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.Layer;
using ArbitraryPixel.Platform2D.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Text;

namespace ArbitraryPixel.CodeLogic.Common.Layers
{
    public interface IMenuCreditsContentLayer : IMenuContentLayer
    {
    }

    public class MenuCreditsContentLayer : MenuContentLayerBase, IMenuCreditsContentLayer
    {
        private ITextObjectBuilder _builder;
        private ILayer _textLayer;
        private IRenderTarget2D _textTarget;
        private string[] _creditLines;

        public MenuCreditsContentLayer(IEngine host, ISpriteBatch mainSpriteBatch, RectangleF contentBounds, string[] creditLines)
            : base(host, mainSpriteBatch, contentBounds)
        {
            _creditLines = creditLines ?? throw new ArgumentNullException();

            _builder = GameObjectFactory.Instance.CreateTextObjectBuilder(
                GameObjectFactory.Instance.CreateTextFormatProcessor(
                    GameObjectFactory.Instance.CreateTextFormatValueHandlerManager()
                ),
                GameObjectFactory.Instance.CreateTextObjectFactory()
            );
            _builder.RegisterFont("Title", this.Host.AssetBank.Get<ISpriteFont>("CreditsTitleFont"));
            _builder.RegisterFont("Credit", this.Host.AssetBank.Get<ISpriteFont>("CreditsCreditFont"));
            _builder.DefaultFont = _builder.GetRegisteredFont("Title");

            // Text layer needs to be unscaled because we'll draw it scaled later.
            _textLayer = GameObjectFactory.Instance.CreateGenericLayer(
                this.Host,
                this.Host.GrfxFactory.SpriteBatchFactory.Create(this.Host.Graphics.GraphicsDevice),
                SpriteSortMode.Deferred,
                null,
                null,
                null,
                null,
                null,
                Microsoft.Xna.Framework.Matrix.Identity
            );
        }

        protected override void OnShow()
        {
            StringBuilder credits = new StringBuilder();
            foreach (string line in _creditLines)
                credits.AppendLine(line);

            _textTarget = this.Host.GrfxFactory.RenderTargetFactory.Create(
                this.Host.Graphics.GraphicsDevice,
                (int)this.ContentBounds.Width,
                (int)this.ContentBounds.Height,
                RenderTargetUsage.DiscardContents
            );

            _textLayer.ClearEntities();
            var textObjects = _builder.Build(credits.ToString(), new RectangleF(this.ContentBounds.Left, this.ContentBounds.Bottom, this.ContentBounds.Width, this.ContentBounds.Height));
            if (textObjects != null && textObjects.Count > 0)
            {
                foreach (ITextObject textObject in textObjects)
                {
                    textObject.Location -= this.ContentBounds.Location;
                    _textLayer.AddEntity(
                        GameObjectFactory.Instance.CreateCreditLineItem(
                            this.Host,
                            new RectangleF(0, 0, this.ContentBounds.Width, this.ContentBounds.Height),
                            _textLayer.MainSpriteBatch,
                            textObject
                        )
                    );
                }
            }
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            base.OnUpdate(gameTime);

            _textLayer.Update(gameTime);
        }

        protected override void OnPreDraw(GameTime gameTime)
        {
            base.OnPreDraw(gameTime);

            if (_textTarget != null)
                _textLayer.Draw(gameTime, _textTarget, Color.Transparent);
        }

        protected override void OnDraw(GameTime gameTime)
        {
            base.OnDraw(gameTime);

            if (_textTarget != null)
                this.MainSpriteBatch.Draw(_textTarget, this.ContentBounds.Location, Color.White);
        }
    }
}
