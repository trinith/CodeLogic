///////////////////////////////////////////////////////////////////////////////
//
// BriefingPage_Overview.cs
//
// This file has been automatically generated via BriefingBuiilder. Please do
// not manually modify its contents.
//
///////////////////////////////////////////////////////////////////////////////

using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.Entity;
using ArbitraryPixel.Platform2D.Layer;
using ArbitraryPixel.Platform2D.Text;
using ArbitraryPixel.Platform2D.UI;
using ArbitraryPixel.Platform2D.UI.Factory;
using Microsoft.Xna.Framework;
using System;

namespace ArbitraryPixel.Common.CodeLogic.BriefingContent
{
    public class BriefingPage_Overview : LayerBase
    {
        private ITextObjectBuilder _textBuilder;
        private ITextObjectRendererFactory _rendererFactory;
        private IUIObjectFactory _uiObjectFactory;

        private ISpriteBatch _rendererBatch;

        public BriefingPage_Overview(IEngine host, ISpriteBatch mainSpriteBatch, RectangleF contentBounds, ITextObjectBuilder textBuilder, ITextObjectRendererFactory rendererFactory, IUIObjectFactory uiObjectFactory)
            : base(host, mainSpriteBatch)
        {
            _textBuilder = textBuilder ?? throw new ArgumentNullException();
            _rendererFactory = rendererFactory ?? throw new ArgumentNullException();
            _uiObjectFactory = uiObjectFactory ?? throw new ArgumentNullException();

            _rendererBatch = this.Host.GrfxFactory.SpriteBatchFactory.Create(this.Host.Graphics.GraphicsDevice);

            // Create the entities relative to (0, 0)
            CreateEntities();

            // Offset their positions, relative to the content bounds.
            AdjustEntityPositions(contentBounds.Location);
        }

        private void CreateEntities()
        {
            this.AddEntity(this.Create_IFormattedTextLabel(new RectangleF(0, 191, 630, 246), "{Alignment:Left}{C:White}{Font:BriefingNormalFont}\nThis is an overview of the CodeLogic main screen. Each\nelement of the interface is identified by the matching number\nbelow.\n\n1) Current Attempt Indicator\n2) Code Input\n3) Attempt History\n4) Submit Button\n5) Menu Button\n\nLets go through each of these elements..."));
            this.AddEntity(this.Create_IStaticTexture(new RectangleF(151, 0, 328, 185), this.Host.AssetBank.Get<ITexture2D>("BP3_Overview")));
        }

        private void AdjustEntityPositions(Vector2 offset)
        {
            foreach (IGameEntity entity in this.Entities)
            {
                entity.Bounds = new RectangleF(
                    offset.X + entity.Bounds.X,
                    offset.Y + entity.Bounds.Y,
                    entity.Bounds.Width,
                    entity.Bounds.Height
                );
            }
        }

        #region Entity Creation Helpers
        private IFormattedTextLabel Create_IFormattedTextLabel(RectangleF bounds, string textFormat)
        {
            // Create a renderer for the label to use.
            ITextObjectRenderer renderer = _rendererFactory.Create(
                this.Host.GrfxFactory.RenderTargetFactory,
                this.Host.Graphics.GraphicsDevice,
                _rendererBatch,
                new Rectangle(Point.Zero, new Point((int)bounds.Width, (int)bounds.Height))
            );

            IFormattedTextLabel label = _uiObjectFactory.CreateFormattedTextLabel(
                this.Host,
                bounds,
                this.MainSpriteBatch,
                _textBuilder,
                renderer,
                textFormat
            );

            return label;
        }

        private IStaticTexture Create_IStaticTexture(RectangleF bounds, ITexture2D texture)
        {
            IStaticTexture staticTexture = _uiObjectFactory.CreateStaticTexture(
                this.Host,
                bounds,
                this.MainSpriteBatch,
                texture
            );

            return staticTexture;
        }
        #endregion
    }
}
