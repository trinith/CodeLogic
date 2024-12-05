using System;
using ArbitraryPixel.CodeLogic.Common.Entities;
using ArbitraryPixel.CodeLogic.Common.Model;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.Layer;
using ArbitraryPixel.Platform2D.UI;
using Microsoft.Xna.Framework;
using ArbitraryPixel.CodeLogic.Common.Theme.Device;
using ArbitraryPixel.CodeLogic.Common.Theme;
using ArbitraryPixel.Platform2D.Theme;
using ArbitraryPixel.Common.Audio;

namespace ArbitraryPixel.CodeLogic.Common.Layers
{
    public interface ICodeInputLayer : ILayer
    {
    }

    public class CodeInputLayer : LayerBase, ICodeInputLayer
    {
        private ICodeInputButton _touchedButton = null;

        public CodeInputLayer(IEngine host, ISpriteBatch mainSpriteBatch, IDeviceModel model) 
            : base(host, mainSpriteBatch)
        {
            this.SamplerState = Microsoft.Xna.Framework.Graphics.SamplerState.PointClamp;

            IDeviceTheme theme = this.Host.GetComponent<IThemeManagerCollection>()[ThemeObjectType.Device].GetCurrentTheme<IDeviceTheme>();

            int numButtons = 4;
            SizeF buttonSize = new SizeF(200, 200);
            int buttonPadding = 25;

            // Calculate total size of button area.
            RectangleF bounds = new RectangleF(
                Vector2.Zero,
                new SizeF(
                    buttonSize.Width * numButtons + buttonPadding * (numButtons - 1),
                    buttonSize.Height
                )
            );

            // Centre button area over screen.
            bounds.Location = new Vector2(
                this.Host.ScreenManager.World.X / 2f - bounds.Width / 2f,
                this.Host.ScreenManager.World.Y / 2f - bounds.Height / 2f
            );

            // Create a background
            ITexture2D backgroundBorder = this.Host.AssetBank.Get<ITexture2D>("InputBackgroundBorder");
            ITexture2D backgroundFill = this.Host.AssetBank.Get<ITexture2D>("InputBackgroundFill");
            SizeF backgroundSize = new SizeF(backgroundBorder.Width, backgroundBorder.Height);
            RectangleF backgroundBounds = new RectangleF(
                bounds.Centre - backgroundSize.Centre,
                backgroundSize
            );
            backgroundBounds.Location = backgroundBounds.Location.ToPoint().ToVector2();

            this.AddEntity(
                GameObjectFactory.Instance.CreateTextureEntity(
                    this.Host,
                    backgroundBounds,
                    this.MainSpriteBatch,
                    backgroundFill,
                    theme.BackgroundImageMask
                )
            );

            this.AddEntity(
                GameObjectFactory.Instance.CreateTextureEntity(
                    this.Host,
                    backgroundBounds,
                    this.MainSpriteBatch,
                    backgroundBorder,
                    theme.NormalColourMask
                )
            );

            // Create the buttons.
            Vector2 buttonLocation = bounds.Location;
            for (int i = 0; i < numButtons; i++)
            {
                RectangleF buttonBounds = new RectangleF(buttonLocation, buttonSize);
                ICodeInputButtonModel buttonModel = GameObjectFactory.Instance.CreateCodeInputButtonModel(model);
                ICodeInputButton button = GameObjectFactory.Instance.CreateCodeInputButton(this.Host, buttonBounds, this.MainSpriteBatch, buttonModel, i);
                this.AddEntity(button);

                button.Touched += Handle_Touched;
                button.SelectorOpened += Handle_SelectorOpened;
                button.SelectorClosed += Handle_SelectorClosed;

                buttonLocation.X += buttonSize.Width + buttonPadding;
            }
        }

        private void Handle_SelectorOpened(object sender, EventArgs e)
        {
            this.Host.AssetBank.Get<ISoundResource>("ButtonPress").Play();
        }

        private void Handle_SelectorClosed(object sender, SelectorClosedEventArgs e)
        {
            if (e.ValueChanged)
                this.Host.AssetBank.Get<ISoundResource>("IndexValueChanged").Play();
            else
                this.Host.AssetBank.Get<ISoundResource>("ButtonPress").Play();
        }

        private void Handle_Touched(object sender, ButtonEventArgs e)
        {
            _touchedButton = sender as ICodeInputButton;
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            base.OnUpdate(gameTime);

            if (_touchedButton != null && _touchedButton.Model.SelectorState != CodeInputButtonSelectorState.Closed)
            {
                this.RemoveEntity(_touchedButton);
                this.AddEntity(_touchedButton);
                _touchedButton = null;
            }
        }
    }
}
