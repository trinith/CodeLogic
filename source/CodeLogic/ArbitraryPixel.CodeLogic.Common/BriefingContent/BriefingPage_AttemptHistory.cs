///////////////////////////////////////////////////////////////////////////////
//
// BriefingPage_AttemptHistory.cs
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
    public class BriefingPage_AttemptHistory : LayerBase
    {
        private ITextObjectBuilder _textBuilder;
        private ITextObjectRendererFactory _rendererFactory;
        private IUIObjectFactory _uiObjectFactory;

        private ISpriteBatch _rendererBatch;

        public BriefingPage_AttemptHistory(IEngine host, ISpriteBatch mainSpriteBatch, RectangleF contentBounds, ITextObjectBuilder textBuilder, ITextObjectRendererFactory rendererFactory, IUIObjectFactory uiObjectFactory)
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
            this.AddEntity(this.Create_IFormattedTextLabel(new RectangleF(0, 295, 630, 205), "{Alignment:Left}{Font:BriefingNormalFont}{C:White}\nEach attempt record shows you the attempt number, the code\nthat was tested, and the feedback for the attempt. A square\nshows that an index was the correct colour in the correct\nplace, while a triangle shows that an index was the correct\ncolour but in an incorrect place. An empty spot indicates\nthat an index was not present in the code.\n\nWe will go over what the feedback means in more detail once we\nhave finished going over the interface."));
            this.AddEntity(this.Create_IFormattedTextLabel(new RectangleF(0, 133, 630, 114), "{Alignment:Left}{Font:BriefingNormalFont}{C:White}\nAt the bottom of the interface you will find the Attempt\nHistory. This shows you a log of all the attempts you have\nmade so far, listed in order of most recent to least recent.\nTapping the icon at the top will change the size of the panel\nif you want to see less information, or hide it completely."));
            this.AddEntity(this.Create_IStaticTexture(new RectangleF(220, 253, 190, 36), this.Host.AssetBank.Get<ITexture2D>("HistoryZoom")));
            this.AddEntity(this.Create_IStaticTexture(new RectangleF(129, 0, 372, 127), this.Host.AssetBank.Get<ITexture2D>("HistoryFull")));
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
