using ArbitraryPixel.CodeLogic.Common.Entities;
using ArbitraryPixel.CodeLogic.Common.Model;
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
    public class LogPanelContentLayer : LayerBase
    {
        private IDeviceModel _deviceModel;
        private ILogPanelModel _panelModel;

        public List<ITexture2D> EntityTextures { get; private set; } = new List<ITexture2D>();
        public int LastIndexDrawn { get; private set; } = 0;

        public LogPanelContentLayer(IEngine host, ISpriteBatch mainSpriteBatch, IDeviceModel deviceModel, ILogPanelModel panelModel)
            : base(host, mainSpriteBatch)
        {
            _deviceModel = deviceModel ?? throw new ArgumentNullException();
            _panelModel = panelModel ?? throw new ArgumentNullException();

            _panelModel.ModelReset +=
                (sender, e) =>
                {
                    this.LastIndexDrawn = 0;
                    this.EntityTextures.Clear();
                };
        }

        private ITexture2D RenderAttemptViewToTexture(GameTime gameTime, int attemptIndex)
        {
            // Create a view that will render our attempt record.
            ISequenceAttemptRecordView tempView = GameObjectFactory.Instance.CreateSequenceAttemptRecordView(this.Host, this.MainSpriteBatch, Vector2.Zero, _deviceModel, attemptIndex);
            tempView.DrawBegin += (sender, e) => this.MainSpriteBatch.Begin();
            tempView.DrawEnd += (sender, e) => this.MainSpriteBatch.End();

            // Create a render target that we can render the view to.
            IRenderTarget2D target = this.Host.GrfxFactory.RenderTargetFactory.Create(this.Host.Graphics.GraphicsDevice, (int)tempView.Bounds.Width, (int)tempView.Bounds.Height, RenderTargetUsage.DiscardContents);
            target.SetData<Color>(Color.Transparent);

            // Render the view to the target.
            tempView.Draw(gameTime, target, Color.Transparent);

            return target;
        }

        private void DrawEntityTextures(ISpriteBatch spriteBatch)
        {
            if (EntityTextures.Count > 0)
            {
                Vector2 scale = new Vector2(1);

                SizeF padding = new SizeF(5);
                float borderHeight = 2;

                Vector2 position = Vector2.Zero;
                Vector2 origin = Vector2.Zero;

                ITexture2D texture;
                for (int i = 0; i < EntityTextures.Count; i++)
                {
                    texture = EntityTextures[i];

                    if (i % 3 == 0)
                    {
                        // When at the start of a row, calculate our start position.
                        int colCount = MathHelper.Clamp(EntityTextures.Count - i, 1, 3);

                        SizeF blockSize = new SizeF(
                            colCount * EntityTextures[0].Width + (colCount - 1) * padding.Width,
                            EntityTextures[0].Height
                        );
                        blockSize *= (SizeF)scale;

                        position = new Vector2(
                            _panelModel.WorldBounds.Width / 2f - blockSize.Width / 2f - _panelModel.CurrentOffset.X,
                            _panelModel.WorldBounds.Height - _panelModel.CurrentOffset.Y + padding.Height + borderHeight + ((i / 3) * (padding.Height + texture.Height * scale.Y))
                        );
                    }

                    position = position.ToPoint().ToVector2();

                    spriteBatch.Draw(texture, position, null, Color.White, 0f, origin, scale, SpriteEffects.None, 0f);
                    position.X += padding.Width + texture.Width * scale.X;
                }
            }
        }

        protected override void OnPreDraw(GameTime gameTime)
        {
            base.OnPreDraw(gameTime);

            while (this.LastIndexDrawn < _deviceModel.CurrentTrial - 1)
            {
                ITexture2D texture = RenderAttemptViewToTexture(gameTime, this.LastIndexDrawn);
                EntityTextures.Insert(0, texture);
                this.LastIndexDrawn++;
            }
        }

        protected override void OnDraw(GameTime gameTime)
        {
            base.OnDraw(gameTime);

            DrawEntityTextures(this.MainSpriteBatch);
        }
    }
}
