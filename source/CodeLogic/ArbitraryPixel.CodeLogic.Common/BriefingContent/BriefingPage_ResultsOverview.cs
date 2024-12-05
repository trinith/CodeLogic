///////////////////////////////////////////////////////////////////////////////
//
// BriefingPage_ResultsOverview.cs
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
    public class BriefingPage_ResultsOverview : LayerBase
    {
        private ITextObjectBuilder _textBuilder;
        private ITextObjectRendererFactory _rendererFactory;
        private IUIObjectFactory _uiObjectFactory;

        private ISpriteBatch _rendererBatch;

        public BriefingPage_ResultsOverview(IEngine host, ISpriteBatch mainSpriteBatch, RectangleF contentBounds, ITextObjectBuilder textBuilder, ITextObjectRendererFactory rendererFactory, IUIObjectFactory uiObjectFactory)
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
            this.AddEntity(this.Create_IFormattedTextLabel(new RectangleF(-3, 322, 630, 147), "{Font:BriefingNormalFont}{Alignment:Left}{C:White}\nYou will always get four pieces of feedback, one for each\ndigit in your attempt. However, they will always be listed in\nthe order of {C:Green}Equal {C:White}-> {C:Orange}Partial {C:White}-> {C:Red}Not Equal{C:White}.\n\nThis is important to remember. The order of the results does\nnot correspond to the order in your attempt."));
            this.AddEntity(this.Create_IStaticTexture(new RectangleF(37, 243, 26, 26), this.Host.AssetBank.Get<ITexture2D>("Results_NotEqual")));
            this.AddEntity(this.Create_IFormattedTextLabel(new RectangleF(179, 248, 451, 68), "{Font:BriefingNormalFont}{Alignment:Left}{C:White}\nOne of the digits in the attempt was not a\ncolour present in the code."));
            this.AddEntity(this.Create_IStaticTexture(new RectangleF(37, 147, 26, 26), this.Host.AssetBank.Get<ITexture2D>("Results_Partial")));
            this.AddEntity(this.Create_IFormattedTextLabel(new RectangleF(179, 152, 451, 90), "{Font:BriefingNormalFont}{Alignment:Left}{C:White}\nOne of the digits in the attempt was the\ncorrect colour, but it was not in the\ncorrect place."));
            this.AddEntity(this.Create_IFormattedTextLabel(new RectangleF(69, 248, 104, 26), "{Font:BriefingNormalFont}{Alignment:Left}{C:Red}\nNot Equal"));
            this.AddEntity(this.Create_IFormattedTextLabel(new RectangleF(69, 152, 104, 26), "{Font:BriefingNormalFont}{Alignment:Left}{C:Orange}\nPartial"));
            this.AddEntity(this.Create_IFormattedTextLabel(new RectangleF(69, 79, 104, 26), "{Font:BriefingNormalFont}{Alignment:Left}{C:Green}\nEqual"));
            this.AddEntity(this.Create_IFormattedTextLabel(new RectangleF(179, 79, 451, 67), "{Font:BriefingNormalFont}{Alignment:Left}{C:White}\nOne of the digits in the attempt was the\ncorrect colour and was in the correct place."));
            this.AddEntity(this.Create_IStaticTexture(new RectangleF(37, 74, 26, 26), this.Host.AssetBank.Get<ITexture2D>("Results_Equal")));
            this.AddEntity(this.Create_IFormattedTextLabel(new RectangleF(0, 0, 630, 54), "{Font:BriefingNormalFont}{Alignment:Left}{C:White}\nThe first thing we need to understand is how the results work.\nCodeLogic will give you three kinds of feedback on your guess."));
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
