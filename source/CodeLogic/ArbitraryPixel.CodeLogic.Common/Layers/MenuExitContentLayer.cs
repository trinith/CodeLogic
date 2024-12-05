using ArbitraryPixel.CodeLogic.Common.Entities;
using ArbitraryPixel.CodeLogic.Common.Model;
using ArbitraryPixel.Common.Audio;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.UI;
using Microsoft.Xna.Framework;
using System;
using System.Text;

namespace ArbitraryPixel.CodeLogic.Common.Layers
{
    public interface IMenuExitContentLayer : IMenuContentLayer
    {
        event EventHandler<EventArgs> ExitButtonTapped;
    }

    public class MenuExitContentLayer : MenuContentLayerBase, IMenuExitContentLayer
    {
        private Vector2 _padding = new Vector2(10);

        public event EventHandler<EventArgs> ExitButtonTapped;

        public MenuExitContentLayer(IEngine host, ISpriteBatch mainSpriteBatch, RectangleF contentBounds)
            : base(host, mainSpriteBatch, contentBounds)
        {
            StringBuilder msg = new StringBuilder();
            msg.AppendLine("We're sorry to see you go, Agent, but rest and relaxation is");
            msg.AppendLine("important to stay at peak performance! We look forward to");
            msg.AppendLine("working with you again in the future.");
            msg.AppendLine();
            msg.AppendLine("Tap the Exit button to confirm.");


            ISpriteFont font = this.Host.AssetBank.Get<ISpriteFont>("MainMenuContentFont");
            ITextLabel textLabel = GameObjectFactory.Instance.CreateGenericTextLabel(this.Host, _padding + contentBounds.Location, mainSpriteBatch, font, msg.ToString(), Color.White);
            this.AddEntity(textLabel);

            ISpriteFont buttonFont = this.Host.AssetBank.Get<ISpriteFont>("MainButtonFont");
            SizeF buttonSize = new SizeF(CodeLogicEngine.Constants.MenuButtonSize.Width, CodeLogicEngine.Constants.MenuButtonSize.Height);
            ISimpleButton exitButton = GameObjectFactory.Instance.CreateSimpleButton(
                this.Host,
                new RectangleF(
                    new Vector2(contentBounds.Right - _padding.X - buttonSize.Width, contentBounds.Bottom - _padding.Y - buttonSize.Height),
                    buttonSize
                ),
                mainSpriteBatch,
                buttonFont
            );
            exitButton.Text = "Exit";
            exitButton.Tapped += Handle_Tapped;
            this.AddEntity(exitButton);
        }

        private void Handle_Tapped(object sender, ButtonEventArgs e)
        {
            OnExitButtonTapped(new EventArgs());
        }

        protected virtual void OnExitButtonTapped(EventArgs e)
        {
            this.Host.AssetBank.Get<ISoundResource>("ButtonPress").Play();

            if (this.ExitButtonTapped != null)
                this.ExitButtonTapped(this, e);
        }
    }
}
