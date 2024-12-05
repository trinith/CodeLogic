using ArbitraryPixel.CodeLogic.Common.Entities;
using ArbitraryPixel.CodeLogic.Common.Model;
using ArbitraryPixel.Common.Audio;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.Text;
using ArbitraryPixel.Platform2D.UI;
using Microsoft.Xna.Framework;
using System;
using System.Text;

namespace ArbitraryPixel.CodeLogic.Common.Layers
{
    public interface IMenuSinglePlayerContentLayer : IMenuContentLayer
    {
        event EventHandler<EventArgs> StartButtonTapped;
    }

    public class MenuSinglePlayerContentLayer : MenuContentLayerBase, IMenuSinglePlayerContentLayer
    {
        private Vector2 _padding = new Vector2(10);

        public event EventHandler<EventArgs> StartButtonTapped;

        public MenuSinglePlayerContentLayer(IEngine host, ISpriteBatch mainSpriteBatch, RectangleF contentBounds)
            : base(host, mainSpriteBatch, contentBounds)
        {
            StringBuilder msg = new StringBuilder();
            msg.AppendLine("Agent, Director Pennywell here. Your mission, should you choose");
            msg.AppendLine("to accept it, is to retrieve the plans for a new experimental");
            msg.AppendLine("rocket EvilCorp is working on so we can find a way to");
            msg.AppendLine("neutralize it! Break into their head office and use the");
            msg.AppendLine("CodeCracker software on your mobile device to get into the safe");
            msg.AppendLine("where the plans are stored, retrieve them, and then get out!");
            msg.AppendLine();
            msg.AppendLine("Good luck!");

            ISpriteFont font = this.Host.AssetBank.Get<ISpriteFont>("MainMenuContentFont");
            ITextLabel tempLabel = GameObjectFactory.Instance.CreateGenericTextLabel(this.Host, _padding + contentBounds.Location, mainSpriteBatch, font, msg.ToString(), Color.White);
            this.AddEntity(tempLabel);

            ISpriteFont buttonFont = this.Host.AssetBank.Get<ISpriteFont>("MainButtonFont");
            SizeF buttonSize = new SizeF(CodeLogicEngine.Constants.MenuButtonSize.Width, CodeLogicEngine.Constants.MenuButtonSize.Height);
            ISimpleButton startButton = GameObjectFactory.Instance.CreateSimpleButton(
                this.Host,
                new RectangleF(
                    contentBounds.Right - buttonSize.Width - _padding.X,
                    contentBounds.Bottom - buttonSize.Height - _padding.Y,
                    buttonSize.Width,
                    buttonSize.Height
                ),
                mainSpriteBatch,
                buttonFont
            );
            startButton.Text = "Start";
            startButton.Tapped += (sender, e) => OnStartButtonTapped(new EventArgs());
            this.AddEntity(startButton);
        }

        protected virtual void OnStartButtonTapped(EventArgs e)
        {
            this.Host.AssetBank.Get<ISoundResource>("ButtonPress").Play();

            if (this.StartButtonTapped != null)
                this.StartButtonTapped(this, e);
        }
    }
}
