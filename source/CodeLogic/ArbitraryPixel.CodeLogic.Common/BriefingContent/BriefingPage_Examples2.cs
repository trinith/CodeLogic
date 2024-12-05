///////////////////////////////////////////////////////////////////////////////
//
// BriefingPage_Examples2.cs
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
    public class BriefingPage_Examples2 : LayerBase
    {
        private ITextObjectBuilder _textBuilder;
        private ITextObjectRendererFactory _rendererFactory;
        private IUIObjectFactory _uiObjectFactory;

        private ISpriteBatch _rendererBatch;

        public BriefingPage_Examples2(IEngine host, ISpriteBatch mainSpriteBatch, RectangleF contentBounds, ITextObjectBuilder textBuilder, ITextObjectRendererFactory rendererFactory, IUIObjectFactory uiObjectFactory)
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
            this.AddEntity(this.Create_IFormattedTextLabel(new RectangleF(0, 396, 630, 93), "{Font:BriefingNormalFont}{Alignment:Left}{C:White}\nHopefully that gives you a good idea of how things work, but\nthere's nothing like just diving right and giving things a try\nto help you learn!"));
            this.AddEntity(this.Create_IFormattedTextLabel(new RectangleF(39, 275, 591, 115), "{Font:BriefingNormalFont}{Alignment:Left}{C:White}\nIn this example, we get two {C:Green}Equal {C:White}results for the blue\nand green, which are in the correct spot, then two {C:Orange}Partial\n{C:White}results because there is a red and a green in the code,\nbut in our attempt they are in the wrong spot."));
            this.AddEntity(this.Create_IStaticTexture(new RectangleF(39, 237, 187, 32), this.Host.AssetBank.Get<ITexture2D>("Example_Results4")));
            this.AddEntity(this.Create_IFormattedTextLabel(new RectangleF(39, 116, 591, 115), "{Font:BriefingNormalFont}{Alignment:Left}{C:White}\nHere, we get an {C:Green}Equal {C:White}for the blue that's in the correct\nspot, then a {C:Orange}Partial {C:White}for the blue that's in an incorrect\nspot. Note that the results order does not go with the\nattempt order."));
            this.AddEntity(this.Create_IStaticTexture(new RectangleF(39, 78, 187, 32), this.Host.AssetBank.Get<ITexture2D>("Example_Results3")));
            this.AddEntity(this.Create_IFormattedTextLabel(new RectangleF(0, 0, 630, 72), "{Font:BriefingNormalFont}{Alignment:Left}{C:White}\nBuilding on the previous examples, let's look at two more that\nexplain how {C:Green}Equal {C:White}results work."));
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
