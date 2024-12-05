///////////////////////////////////////////////////////////////////////////////
//
// BriefingPage_CodeInput.cs
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
    public class BriefingPage_CodeInput : LayerBase
    {
        private ITextObjectBuilder _textBuilder;
        private ITextObjectRendererFactory _rendererFactory;
        private IUIObjectFactory _uiObjectFactory;

        private ISpriteBatch _rendererBatch;

        public BriefingPage_CodeInput(IEngine host, ISpriteBatch mainSpriteBatch, RectangleF contentBounds, ITextObjectBuilder textBuilder, ITextObjectRendererFactory rendererFactory, IUIObjectFactory uiObjectFactory)
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
            this.AddEntity(this.Create_IFormattedTextLabel(new RectangleF(0, 87, 630, 177), "{Alignment:Left}{Font:BriefingNormalFont}{C:White}\nUsing this part of CodeLogic allows you to change the code you\nwant to input. To do so, simply tap on the desired code index\nand the selection wheel pops out. Next, tap the colour you\nwant to change to and the code index will be updated.\n\nAlternatively, you can tap on the index and drag in the\ndirection of the colour you want. When you release, the\nhighlighted colour will be selected."));
            this.AddEntity(this.Create_IStaticTexture(new RectangleF(157, 270, 316, 127), this.Host.AssetBank.Get<ITexture2D>("CodeInput_Example")));
            this.AddEntity(this.Create_IStaticTexture(new RectangleF(165, 0, 299, 81), this.Host.AssetBank.Get<ITexture2D>("CodeInput")));
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
