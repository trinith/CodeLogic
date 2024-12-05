///////////////////////////////////////////////////////////////////////////////
//
// BriefingPage_Examples1.cs
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
    public class BriefingPage_Examples1 : LayerBase
    {
        private ITextObjectBuilder _textBuilder;
        private ITextObjectRendererFactory _rendererFactory;
        private IUIObjectFactory _uiObjectFactory;

        private ISpriteBatch _rendererBatch;

        public BriefingPage_Examples1(IEngine host, ISpriteBatch mainSpriteBatch, RectangleF contentBounds, ITextObjectBuilder textBuilder, ITextObjectRendererFactory rendererFactory, IUIObjectFactory uiObjectFactory)
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
            this.AddEntity(this.Create_IFormattedTextLabel(new RectangleF(39, 286, 591, 91), "{Font:BriefingNormalFont}{Alignment:Left}{C:White}\nAgain, we get one {C:Orange}Partial {C:White}because we have a green in the\nwrong place. Since the code only has one green and the\nattempt has two, we get a {C:Red}Not Equal {C:White}for the second green."));
            this.AddEntity(this.Create_IStaticTexture(new RectangleF(39, 248, 187, 32), this.Host.AssetBank.Get<ITexture2D>("Example_Results2")));
            this.AddEntity(this.Create_IFormattedTextLabel(new RectangleF(39, 151, 591, 91), "{Font:BriefingNormalFont}{Alignment:Left}{C:White}\nIn this example, we get one {C:Orange}Partial {C:White}match because we have\na green in the code, but in our attempt it's in the wrong\nplace."));
            this.AddEntity(this.Create_IStaticTexture(new RectangleF(39, 113, 187, 32), this.Host.AssetBank.Get<ITexture2D>("Example_Results1")));
            this.AddEntity(this.Create_IFormattedTextLabel(new RectangleF(0, 0, 630, 107), "{Font:BriefingNormalFont}{Alignment:Left}{C:White}\nUsing our example code of ({C:Red}Red{C:White}, {C:Blue}Blue{C:White}, {C:Blue}Blue{C:White}, {C:Green}Green{C:White}), lets look\nat some sample guesses and the results they generate.\n\nTo start, lets explore how the {C:Orange}Partial {C:White}results work."));
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
