///////////////////////////////////////////////////////////////////////////////
//
// BriefingPage_Objective.cs
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
    public class BriefingPage_Objective : LayerBase
    {
        private ITextObjectBuilder _textBuilder;
        private ITextObjectRendererFactory _rendererFactory;
        private IUIObjectFactory _uiObjectFactory;

        private ISpriteBatch _rendererBatch;

        public BriefingPage_Objective(IEngine host, ISpriteBatch mainSpriteBatch, RectangleF contentBounds, ITextObjectBuilder textBuilder, ITextObjectRendererFactory rendererFactory, IUIObjectFactory uiObjectFactory)
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
            this.AddEntity(this.Create_IFormattedTextLabel(new RectangleF(0, 0, 630, 444), "{Font:BriefingNormalFont}{Alignment:Left}{C:White}\nUsing CodeLogic, you can work your way towards solving a code.\nA code is made up of four digits and each digit can have one\nof five values. Within CodeLogic, these values are represented\nas colours... {C:Red}red{C:White}, {C:Green}green{C:White}, {C:Blue}blue{C:White}, {C:Yellow}yellow{C:White}, and {C:Orange}orange{C:White}.\n\nUsing the interface, you will make guesses as to what the code\nis and CodeLogic will try it out for you. If you get the right\ncode you will succeed in your mission; however, you will\nlikely not get it on your first attempt. When this happens,\nCodeLogic will give you some feedback about your guess. You\ncan use this feedback to guide subsequent guesses.\n\nWe will discuss this process in more detail future in the\nbriefing but first, lets go over the CodeLogic interface!"));
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
