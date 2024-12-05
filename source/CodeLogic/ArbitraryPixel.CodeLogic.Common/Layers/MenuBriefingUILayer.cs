using ArbitraryPixel.CodeLogic.Common.Entities;
using ArbitraryPixel.Common.Audio;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.Layer;
using ArbitraryPixel.Platform2D.UI;
using Microsoft.Xna.Framework;
using System;

namespace ArbitraryPixel.CodeLogic.Common.Layers
{
    public class MenuBriefingUILayer : LayerBase
    {
        private IMenuBriefingContentLayer _hostLayer;

        private ISimpleButton _nextButton;
        private ISimpleButton _previousButton;
        private ISimpleButton _contentsButton;

        private ITextLabel _pageLabel;
        private RectangleF _contentBounds;

        public MenuBriefingUILayer(IEngine host, ISpriteBatch mainSpriteBatch, RectangleF contentBounds, IMenuBriefingContentLayer hostLayer)
            : base(host, mainSpriteBatch)
        {
            _hostLayer = hostLayer ?? throw new ArgumentNullException();

            _hostLayer.PageChanged += Handle_PageChanged;

            _contentBounds = contentBounds;

            SizeF buttonSize = CodeLogicEngine.Constants.MenuButtonSize * new SizeF(0.5f, 1);
            SizeF padding = CodeLogicEngine.Constants.TextWindowPadding;

            ISpriteFont font = this.Host.AssetBank.Get<ISpriteFont>("MainButtonFont");
            RectangleF buttonBounds = new RectangleF(
                contentBounds.Right - buttonSize.Width,
                contentBounds.Bottom - buttonSize.Height,
                buttonSize.Width,
                buttonSize.Height
            );

            _nextButton = GameObjectFactory.Instance.CreateSimpleButton(this.Host, buttonBounds, this.MainSpriteBatch, font);
            _nextButton.Text = ">";
            _nextButton.Tapped += Handle_NextButtonTapped;
            this.AddEntity(_nextButton);

            buttonBounds = new RectangleF(buttonBounds.Location.X - padding.Width - buttonSize.Width, buttonBounds.Location.Y, buttonSize.Width, buttonSize.Height);
            _previousButton = GameObjectFactory.Instance.CreateSimpleButton(this.Host, buttonBounds, this.MainSpriteBatch, font);
            _previousButton.Text = "<";
            _previousButton.Tapped += Handle_PreviousButtonTapped;
            this.AddEntity(_previousButton);

            buttonBounds.Location = new Vector2(contentBounds.Left, buttonBounds.Y);
            _contentsButton = GameObjectFactory.Instance.CreateSimpleButton(this.Host, buttonBounds, this.MainSpriteBatch, font);
            _contentsButton.Text = "^";
            _contentsButton.Tapped += Handle_ContentsButtonTapped;
            this.AddEntity(_contentsButton);

            _pageLabel = GameObjectFactory.Instance.CreateGenericTextLabel(this.Host, Vector2.Zero, this.MainSpriteBatch, this.Host.AssetBank.Get<ISpriteFont>("MainMenuContentFont"), "", CodeLogicEngine.Constants.ClrMenuFGHigh);
            this.AddEntity(_pageLabel);

            RefreshTextLabel();
        }

        private void RefreshTextLabel()
        {
            string text = string.Format("Page {0} of {1}", _hostLayer.CurrentPage, _hostLayer.TotalPages);
            SizeF textSize = _pageLabel.Font.MeasureString(text);

            _pageLabel.Text = text;
            _pageLabel.Bounds = new RectangleF(
                new Vector2(
                    (int)(_contentsButton.Bounds.Right + (_previousButton.Bounds.Left - _contentsButton.Bounds.Right) / 2f - textSize.Width / 2f),
                    (int)(_previousButton.Bounds.Centre.Y - textSize.Height / 2f)
                ),
                _pageLabel.Bounds.Size
            );
        }

        private void Handle_PageChanged(object sender, EventArgs e)
        {
            RefreshTextLabel();
        }

        private void Handle_NextButtonTapped(object sender, ButtonEventArgs e)
        {
            this.Host.AssetBank.Get<ISoundResource>("ButtonPress").Play();
            _hostLayer.NextPage();
        }

        private void Handle_PreviousButtonTapped(object sender, ButtonEventArgs e)
        {
            this.Host.AssetBank.Get<ISoundResource>("ButtonPress").Play();
            _hostLayer.PreviousPage();
        }

        private void Handle_ContentsButtonTapped(object sender, ButtonEventArgs e)
        {
            this.Host.AssetBank.Get<ISoundResource>("ButtonPress").Play();
            _hostLayer.SetPage(GameObjectFactory.BriefingPage.Contents);
        }
    }
}
