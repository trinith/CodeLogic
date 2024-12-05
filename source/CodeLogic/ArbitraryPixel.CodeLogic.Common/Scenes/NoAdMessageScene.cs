using ArbitraryPixel.CodeLogic.Common.Layers;
using ArbitraryPixel.Common.ContentManagement;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Assets;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.Layer;
using ArbitraryPixel.Platform2D.Scene;
using Microsoft.Xna.Framework;
using ArbitraryPixel.Platform2D.Text;
using System.Text;
using System.Collections.Generic;
using ArbitraryPixel.CodeLogic.Common.Entities;
using ArbitraryPixel.CodeLogic.Common.Model;

namespace ArbitraryPixel.CodeLogic.Common.Scenes
{
    public class NoAdMessageScene : SceneBase
    {
        private const float MESSAGE_SHOW_DELAY = 15f;

        private ITexture2D _pixel;
        private IProgressLayer _progressLayer;

        public NoAdMessageScene(IEngine host)
            : base(host)
        {
        }

        protected override void OnLoadAssetBank(IContentManager content, IAssetBank bank)
        {
            base.OnLoadAssetBank(content, bank);

            bank.Put<ISpriteFont>("AdLoadNormalFont", this.Host.GrfxFactory.SpriteFontFactory.Create(content, @"Fonts\AdLoadNormalFont"));

            if (!bank.Exists<ISpriteFont>("MainMenuContentFont"))
                bank.Put<ISpriteFont>("MainMenuContentFont", this.Host.GrfxFactory.SpriteFontFactory.Create(content, @"Fonts\MainMenuContentFont"));
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            SizeF screenBounds = (SizeF)this.Host.ScreenManager.World;

            _pixel = this.Host.AssetBank.Get<ITexture2D>("Pixel");
            ILayer mainLayer = GameObjectFactory.Instance.CreateGenericLayer(this.Host, this.Host.GrfxFactory.SpriteBatchFactory.Create(this.Host.Graphics.GraphicsDevice));
            this.AddEntity(mainLayer);

            mainLayer.AddEntity(
                GameObjectFactory.Instance.CreateTextureEntity(
                    this.Host,
                    new RectangleF(Vector2.Zero, screenBounds),
                    mainLayer.MainSpriteBatch,
                    _pixel,
                    Color.Black
                )
            );

            mainLayer.AddEntity(
                _progressLayer = GameObjectFactory.Instance.CreateProgressLayer(
                    this.Host,
                    mainLayer.MainSpriteBatch,
                    null,
                    ""
                )
            );
            _progressLayer.Maximum = MESSAGE_SHOW_DELAY;
            _progressLayer.Value = MESSAGE_SHOW_DELAY;

            AddTextObjects(mainLayer, screenBounds);

            SizeF buttonSize = new SizeF(CodeLogicEngine.Constants.MenuButtonSize.Width, CodeLogicEngine.Constants.MenuButtonSize.Height);
            RectangleF buttonBounds = new RectangleF(
                new Vector2(
                    _progressLayer.ProgressBarBounds.Centre.X - buttonSize.Centre.X,
                    _progressLayer.ProgressBarBounds.Bottom + (screenBounds.Height - _progressLayer.ProgressBarBounds.Bottom) / 2f
                ),
                buttonSize
            );
            ISimpleButton skipButton = GameObjectFactory.Instance.CreateSimpleButton(
                this.Host,
                buttonBounds,
                mainLayer.MainSpriteBatch,
                this.Host.AssetBank.Get<ISpriteFont>("MainMenuContentFont")
            );

            skipButton.Text = "Skip";
            skipButton.Tapped +=
                (sender, e) =>
                {
                    _progressLayer.Value = 0;
                };

            mainLayer.AddEntity(skipButton);
        }

        protected override void OnReset()
        {
            base.OnReset();
            this.SceneComplete = false;
            _progressLayer.Value = _progressLayer.Maximum;
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            base.OnUpdate(gameTime);

            if (_progressLayer.Value > 0)
                _progressLayer.Value -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_progressLayer.Value <= 0)
            {
                _progressLayer.Value = 0;
                this.ChangeScene(GameObjectFactory.Instance.CreateFadeSceneTransition(this.Host, this, this.Host.Scenes["DeviceBoot"], FadeSceneTransitionMode.In, CodeLogicEngine.Constants.FadeSceneTransitionTime));
            }
        }

        private void AddTextObjects(ILayer layer, SizeF screenBounds)
        {
            ITextObjectBuilder builder = GameObjectFactory.Instance.CreateTextObjectBuilder(
                GameObjectFactory.Instance.CreateTextFormatProcessor(GameObjectFactory.Instance.CreateTextFormatValueHandlerManager()),
                GameObjectFactory.Instance.CreateTextObjectFactory()
            );

            builder.RegisterFont("Normal", this.Host.AssetBank.Get<ISpriteFont>("AdLoadNormalFont"));
            builder.DefaultFont = builder.GetRegisteredFont("Normal");

            RectangleF textBounds = new RectangleF(
                Vector2.Zero,
                new SizeF(
                    screenBounds.Width,
                    screenBounds.Height / 2f - CodeLogicEngine.Constants.AdProgressBarSize.Height / 2f
                )
            );

            List<ITextObject> textObjects = builder.Build(GetMessage(), textBounds);
            Vector2 offset = Vector2.Zero;

            if (textObjects != null && textObjects.Count > 0)
            {
                float minY = textObjects[0].Location.Y;

                ITextObject lastObject = textObjects[textObjects.Count - 1];
                SizeF lastObjectSize = lastObject.TextDefinition.Font.MeasureString(lastObject.TextDefinition.Text);
                float maxY = lastObject.Location.Y + lastObjectSize.Height;

                offset = new Vector2(0, textBounds.Height / 2f - (maxY - minY) / 2f);

                foreach (ITextObject textObject in textObjects)
                    layer.AddEntity(
                        GameObjectFactory.Instance.CreateGenericTextLabel(
                            this.Host,
                            textObject.Location + offset,
                            layer.MainSpriteBatch,
                            textObject.TextDefinition.Font,
                            textObject.TextDefinition.Text,
                            textObject.TextDefinition.Colour
                        )
                    );
            }
        }

        private string GetMessage()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("{Alignment:Centre}");
            sb.AppendLine("Unfortunately, we were unable to load an ad for you. That's cool,");
            sb.AppendLine("we're not too worried about it and are happy to let you play");
            sb.AppendLine("anyway! If you are enjoying the game though, please consider");
            sb.AppendLine("purchasing the ad-free version :)");
            sb.AppendLine();
            sb.AppendLine("We hope you have a great time!");

            return sb.ToString();
        }
    }
}
