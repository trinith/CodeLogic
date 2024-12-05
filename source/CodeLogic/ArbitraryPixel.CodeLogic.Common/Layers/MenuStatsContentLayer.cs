using ArbitraryPixel.CodeLogic.Common.Config;
using ArbitraryPixel.CodeLogic.Common.Entities;
using ArbitraryPixel.CodeLogic.Common.Model;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.Layer;
using ArbitraryPixel.Platform2D.Text;
using ArbitraryPixel.Platform2D.UI;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArbitraryPixel.CodeLogic.Common.Layers
{
    public interface IMenuStatsContentLayer : IMenuContentLayer
    {
        event EventHandler<EventArgs> ResetButtonTapped;
        void RefreshText();
    }

    public class MenuStatsContentLayer : MenuContentLayerBase, IMenuStatsContentLayer
    {
        private IGameStatsData _gameStatsData;
        private ITextureEntity _textTexture;

        private ITextObjectBuilder _textBuilder;
        private ITextObjectRenderer _textRenderer;

        public MenuStatsContentLayer(IEngine host, ISpriteBatch mainSpriteBatch, RectangleF contentBounds, IGameStatsData gameStatsData)
            : base(host, mainSpriteBatch, contentBounds)
        {
            this.Visible = false;

            _gameStatsData = gameStatsData ?? throw new ArgumentNullException();
            _gameStatsData.DataSaved += Handle_GameStatsDataSaved;

            contentBounds.Inflate(-CodeLogicEngine.Constants.TextWindowPadding);

            RectangleF buttonBounds = new RectangleF(new Vector2(contentBounds.Right, contentBounds.Bottom) - CodeLogicEngine.Constants.MenuButtonSize, CodeLogicEngine.Constants.MenuButtonSize);
            contentBounds.Height -= contentBounds.Bottom - (buttonBounds.Top - CodeLogicEngine.Constants.TextWindowPadding.Height);

            _textBuilder = GameObjectFactory.Instance.CreateTextObjectBuilder(
                GameObjectFactory.Instance.CreateTextFormatProcessor(GameObjectFactory.Instance.CreateTextFormatValueHandlerManager()),
                GameObjectFactory.Instance.CreateTextObjectFactory()
            );
            _textBuilder.RegisterFont("Normal", this.Host.AssetBank.Get<ISpriteFont>("StatsFont"));
            _textBuilder.DefaultFont = _textBuilder.GetRegisteredFont("Normal");

            _textRenderer = GameObjectFactory.Instance.CreateTextObjectRenderer(
                this.Host.GrfxFactory.RenderTargetFactory,
                this.Host.Graphics.GraphicsDevice,
                this.MainSpriteBatch,
                (Rectangle)contentBounds
            );

            // Reset button
            ISimpleButton resetButton = GameObjectFactory.Instance.CreateSimpleButton(this.Host, buttonBounds, this.MainSpriteBatch, this.Host.AssetBank.Get<ISpriteFont>("MainButtonFont"));
            resetButton.Text = "Reset";
            resetButton.Tapped += Handle_ResetButtonTapped;
            this.AddEntity(resetButton);

            // Text panel
            _textTexture = GameObjectFactory.Instance.CreateTextureEntity(this.Host, contentBounds, this.MainSpriteBatch, null, Color.White);
            this.AddEntity(_textTexture);
        }

        #region IMenuStatsContentLayer Implementation
        public event EventHandler<EventArgs> ResetButtonTapped;

        public void RefreshText()
        {
            _textRenderer.Clear();

            IGameStatsModel model = _gameStatsData.Model;

            Color clrTitle = CodeLogicEngine.Constants.ClrMenuTextNormal;
            Color clrValue = CodeLogicEngine.Constants.ClrMenuTextSelected;
            string cName = $"{{C:{clrTitle.R},{clrTitle.G},{clrTitle.B}}}";
            string cValue = $"{{C:{clrValue.R},{clrValue.G},{clrValue.B}}}";
            string timeFormat = "hh\\:mm\\:ss";
            double percentGamesWon = (model.GamesPlayed == 0) ? 0 : Math.Round(model.GamesWon / (double)model.GamesPlayed * 100);
            double averageGuesses = (model.GamesPlayed == 0) ? 0 : Math.Round(model.TotalGuesses / (double)model.GamesPlayed, 2);
            TimeSpan averageGameTime = (model.GamesPlayed == 0) ? TimeSpan.Zero : TimeSpan.FromSeconds(model.TotalGameTime.TotalSeconds / model.GamesPlayed);

            StringBuilder textFormat = new StringBuilder();
            textFormat.Append("{C:White}{Alignment:Left}");

            textFormat.AppendLine($"{cName}Games Played: {cValue}{model.GamesPlayed}");
            textFormat.AppendLine($"{cName}Games Won: {cValue}{model.GamesWon} {cName}({cValue}{percentGamesWon}%{cName})");

            textFormat.AppendLine();
            textFormat.AppendLine($"{cName}Total Guesses: {cValue}{model.TotalGuesses}");
            textFormat.AppendLine($"{cName}Average Guesses Per Game: {cValue}{averageGuesses}");
            textFormat.AppendLine($"{cName}Least Guesses To Win: {cValue}{model.LeastGuessesToWin}");

            textFormat.AppendLine();
            textFormat.AppendLine($"{cName}Total Time Played: {cValue}{model.TotalGameTime.ToString(timeFormat)}");
            textFormat.AppendLine($"{cName}Average Time Per Game: {cValue}{averageGameTime.ToString(timeFormat)}");

            textFormat.AppendLine();
            textFormat.AppendLine($"{cName}Current Win Streak: {cValue}{model.CurrentWinStreak}");
            textFormat.AppendLine($"{cName}Current Loss Streak: {cValue}{model.CurrentLossStreak}");

            textFormat.AppendLine();
            textFormat.AppendLine($"{cName}Fastest Win: {cValue}{model.FastestWin.ToString(timeFormat)}");
            textFormat.AppendLine($"{cName}Slowest Win: {cValue}{model.SlowestWin.ToString(timeFormat)}");

            List<ITextObject> textObjects = _textBuilder.Build(textFormat.ToString(), new RectangleF(Vector2.Zero, _textTexture.Bounds.Size));
            if (textObjects != null)
            {
                foreach (ITextObject textObject in textObjects)
                    _textRenderer.Enqueue(textObject);
            }

            _textRenderer.Flush();
            _textTexture.Texture = _textRenderer.Render();
        }
        #endregion

        #region Event Handlers
        private void Handle_ResetButtonTapped(object sender, ButtonEventArgs e)
        {
            if (this.ResetButtonTapped != null)
                this.ResetButtonTapped(this, new EventArgs());
        }

        private void Handle_GameStatsDataSaved(object sender, EventArgs e)
        {
            if (this.Visible)
                RefreshText();
        }
        #endregion

        #region Override Methods
        protected override void OnShow()
        {
            base.OnShow();

            this.Visible = true;
            RefreshText();
        }

        protected override void OnHide()
        {
            base.OnHide();
            this.Visible = false;
        }
        #endregion
    }
}
