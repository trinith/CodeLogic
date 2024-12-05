///////////////////////////////////////////////////////////////////////////////
// NOTE: This file is not auto generated... it dynamically builds a table of
// contents based on the GameObjectFactory.BriefingPage enum. It can be
// modified and *should* be tested!
///////////////////////////////////////////////////////////////////////////////

using ArbitraryPixel.CodeLogic.Common.Entities;
using ArbitraryPixel.CodeLogic.Common.Layers;
using ArbitraryPixel.Common.Audio;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.Layer;
using ArbitraryPixel.Platform2D.UI;
using Microsoft.Xna.Framework;
using System;

namespace ArbitraryPixel.CodeLogic.Common.BriefingContent
{
    public interface IBriefingContentsPage : ILayer
    {
        IMenuBriefingContentLayer BriefingContentLayer { get; set; }
    }

    public class ContentsPageInfo
    {
        public GameObjectFactory.BriefingPage TargetPage { get; }
        public string Title { get; }
        public string Description { get; }

        public ContentsPageInfo(GameObjectFactory.BriefingPage targetPage, string title, string description)
        {
            this.TargetPage = targetPage;
            this.Title = title;
            this.Description = description;
        }
    }

    public class BriefingContentsPage : LayerBase, IBriefingContentsPage
    {
        public BriefingContentsPage(IEngine host, ISpriteBatch mainSpriteBatch, RectangleF contentBounds, ContentsPageInfo[] pageListing)
            : base(host, mainSpriteBatch)
        {
            var chk_pageListing = pageListing ?? throw new ArgumentNullException();

            // Header
            string headerText = "Table of Contents";
            ISpriteFont headerFont = this.Host.AssetBank.Get<ISpriteFont>("BriefingTitleFont");
            SizeF textSize = headerFont.MeasureString(headerText);
            this.AddEntity(
                GameObjectFactory.Instance.CreateGenericTextLabel(
                    this.Host,
                    new Vector2(contentBounds.Centre.X - textSize.Centre.X, contentBounds.Top),
                    mainSpriteBatch,
                    headerFont,
                    headerText,
                    Color.White
                )
            );

            ISpriteFont descriptionFont = this.Host.AssetBank.Get<ISpriteFont>("BriefingNormalFont");
            Vector2 buttonLocation = new Vector2(contentBounds.Left, contentBounds.Top + headerFont.LineSpacing);
            float buttonHeight = headerFont.LineSpacing + descriptionFont.LineSpacing + CodeLogicEngine.Constants.TextWindowPadding.Height * 2 + 2;
            SizeF buttonSize = new SizeF(contentBounds.Width, buttonHeight);
            SizeF buttonPadding = CodeLogicEngine.Constants.TextWindowPadding;

            foreach (ContentsPageInfo page in pageListing)
            {
                // Create page button
                IChapterButton button = GameObjectFactory.Instance.CreateChapterButton(this.Host, new RectangleF(buttonLocation, buttonSize), this.MainSpriteBatch, headerFont, descriptionFont);
                button.Text = page.Title;
                button.Description = page.Description;
                button.Tag = page.TargetPage;

                button.NormalColour = CodeLogicEngine.Constants.ClrMenuFGMid;
                button.PressedColour = CodeLogicEngine.Constants.ClrMenuFGHigh;
                button.BackColour = CodeLogicEngine.Constants.ClrMenuBGLow;

                button.Tapped += Handle_ContentLinkButtonTapped;

                this.AddEntity(button);

                // Update buttonLocation
                buttonLocation.Y += buttonSize.Height + buttonPadding.Height;
            }
        }

        private void Handle_ContentLinkButtonTapped(object sender, ButtonEventArgs e)
        {
            this.Host.AssetBank.Get<ISoundResource>("ButtonPress").Play();

            IButton button = sender as IButton;
            if (button != null && button.Tag is GameObjectFactory.BriefingPage && this.BriefingContentLayer != null)
            {
                this.BriefingContentLayer.SetPage((GameObjectFactory.BriefingPage)button.Tag);
            }
        }

        #region IBriefingContentsPage Implementation
        public IMenuBriefingContentLayer BriefingContentLayer { get; set; } = null;
        #endregion
    }
}
