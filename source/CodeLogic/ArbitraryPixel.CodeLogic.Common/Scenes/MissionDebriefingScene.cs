using ArbitraryPixel.CodeLogic.Common.Config;
using ArbitraryPixel.CodeLogic.Common.Controllers;
using ArbitraryPixel.CodeLogic.Common.Entities;
using ArbitraryPixel.CodeLogic.Common.Model;
using ArbitraryPixel.Common;
using ArbitraryPixel.Common.Audio;
using ArbitraryPixel.Common.ContentManagement;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Common.Graphics.Factory;
using ArbitraryPixel.Platform2D.Assets;
using ArbitraryPixel.Platform2D.Audio;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.Layer;
using ArbitraryPixel.Platform2D.Scene;
using ArbitraryPixel.Platform2D.Text;
using Microsoft.Xna.Framework;
using System;
using System.Text;

namespace ArbitraryPixel.CodeLogic.Common.Scenes
{
    public class MissionDebriefingScene : SceneBase
    {
        #region Private Members
        private IDeviceModel _model;
        private IGameStatsController _gameStatsController;

        private ILayer _rootLayer;
        private ILayer _textLayer;
        private ILayer _marksLayer;
        private ILayer _uiLayer;
        private RectangleF _paperBounds;

        private ITextObjectBuilder _textBuilder;
        private IConsoleWindow _consoleWindow;

        private IMissionDebriefHistoryMarksBuilder _marksBuilder;
        #endregion

        #region Constructor(s)
        public MissionDebriefingScene(IEngine host, IDeviceModel model, IGameStatsController gameStatsController)
            : base(host)
        {
            _model = model ?? throw new ArgumentNullException();
            _gameStatsController = gameStatsController ?? throw new ArgumentNullException();

            this.Host.ExternalActionOccurred += Handle_ExternalActionOccurred;
        }
        #endregion

        #region Override Methods
        protected override void OnLoadAssetBank(IContentManager content, IAssetBank bank)
        {
            base.OnLoadAssetBank(content, bank);

            ITexture2DFactory textureFactory = this.Host.GrfxFactory.Texture2DFactory;
            bank.Put<ITexture2D>("MissionDebriefingBackground", textureFactory.Create(content, @"Textures\MissionDebriefingBackground"));
            bank.Put<ITexture2D>("MissionDebriefingLightOverlay", textureFactory.Create(content, @"Textures\MissionDebriefingLightOverlay"));
            bank.Put<ITexture2D>("MissionDebriefMarks", textureFactory.Create(content, @"Textures\MissionDebriefMarks"));
            bank.Put<ITexture2D>("MissionDebriefEqualityMarks", textureFactory.Create(content, @"Textures\MissionDebriefEqualityMarks"));
            bank.Put<ITexture2D>("MissionDebriefSignature", textureFactory.Create(content, @"Textures\MissionDebriefSignature"));

            // Default debrief photos
            bank.Put<ITexture2D>("DebriefPhoto_Default_Device", textureFactory.Create(content, @"Textures\DebriefPhoto_Default_Device"));
            bank.Put<ITexture2D>("DebriefPhoto_Default_Objective", textureFactory.Create(content, @"Textures\DebriefPhoto_Default_Objective"));
            bank.Put<ITexture2D>("DebriefPhoto_Default_Setting", textureFactory.Create(content, @"Textures\DebriefPhoto_Default_Setting"));

            ISpriteFontFactory fontFactory = this.Host.GrfxFactory.SpriteFontFactory;
            bank.Put<ISpriteFont>("TypewriterNormalFont", fontFactory.Create(content, @"Fonts\TypewriterNormalFont"));
            bank.Put<ISpriteFont>("TypewriterTitleFont", fontFactory.Create(content, @"Fonts\TypewriterTitleFont"));
            bank.Put<ISpriteFont>("TypewriterSmallFont", fontFactory.Create(content, @"Fonts\TypewriterSmallFont"));

            IMusicController musicController = this.Host.AudioManager.MusicController;
            bank.Put<ISong>("Debriefing", musicController.CreateSong(content, @"Music\Debriefing"));
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            IRandom random = GameObjectFactory.Instance.CreateRandom();
            ITextureEntityFactory textureEntityFactory = GameObjectFactory.Instance.CreateTextureEntityFactory();

            _marksBuilder = GameObjectFactory.Instance.CreateMissionDebriefHistoryMarksBuilder(
                this.Host,
                random,
                textureEntityFactory,
                GameObjectFactory.Instance.CreateMissionDebriefAttemptRecordMarksBuilder(
                    this.Host,
                    random,
                    textureEntityFactory
                )
            );

            _rootLayer = GameObjectFactory.Instance.CreateGenericLayer(this.Host, this.Host.GrfxFactory.SpriteBatchFactory.Create(this.Host.Graphics.GraphicsDevice));
            RectangleF bounds = new RectangleF(Vector2.Zero, (SizeF)this.Host.ScreenManager.World);

            ILayer backgroundLayer = GameObjectFactory.Instance.CreateGenericLayer(this.Host, _rootLayer.MainSpriteBatch);
            backgroundLayer.AddEntity(GameObjectFactory.Instance.CreateTextureEntity(this.Host, bounds, _rootLayer.MainSpriteBatch, this.Host.AssetBank.Get<ITexture2D>("MissionDebriefingBackground"), Color.White));
            CreatePhotos(backgroundLayer);

            ILayer textLayer = CreateTextLayer(_rootLayer);

            _marksLayer = GameObjectFactory.Instance.CreateGenericLayer(this.Host, _rootLayer.MainSpriteBatch);

            ILayer lightOverlayLayer = GameObjectFactory.Instance.CreateGenericLayer(this.Host, _rootLayer.MainSpriteBatch);
            lightOverlayLayer.AddEntity(GameObjectFactory.Instance.CreateTextureEntity(this.Host, bounds, _rootLayer.MainSpriteBatch, this.Host.AssetBank.Get<ITexture2D>("MissionDebriefingLightOverlay"), Color.White));

            _uiLayer = GameObjectFactory.Instance.CreateGenericLayer(this.Host, _rootLayer.MainSpriteBatch);
            SizeF buttonSize = new SizeF(CodeLogicEngine.Constants.MenuButtonSize.Width, CodeLogicEngine.Constants.MenuButtonSize.Height);
            SizeF padding = new SizeF(10f);
            ISimpleButton okButton = GameObjectFactory.Instance.CreateSimpleButton(this.Host, new RectangleF(bounds.Right - buttonSize.Width - padding.Width, bounds.Bottom - buttonSize.Height - padding.Height, buttonSize.Width, buttonSize.Height), _rootLayer.MainSpriteBatch, this.Host.AssetBank.Get<ISpriteFont>("MainButtonFont"));
            okButton.Text = "Ok";
            okButton.BackColour = new Color(32, 32, 32, 225);

            // Fade out and hard cut.
            okButton.Tapped += Handle_OkButtonTapped;

            _uiLayer.AddEntity(okButton);
            _uiLayer.Enabled = false;
            _uiLayer.Visible = false;

            _rootLayer.AddEntity(backgroundLayer);
            _rootLayer.AddEntity(textLayer);
            _rootLayer.AddEntity(_marksLayer);
            _rootLayer.AddEntity(lightOverlayLayer);
            _rootLayer.AddEntity(_uiLayer);
            this.AddEntity(_rootLayer);
        }

        protected override void OnReset()
        {
            base.OnReset();

            this.SceneComplete = false;

            _gameStatsController.Update(_model);
            SetupEntities(_model.GameWon);
        }

        protected override void OnStarting()
        {
            base.OnStarting();

            this.Host.AudioManager.MusicController.VolumeAttenuation = 0f;
            this.Host.AudioManager.MusicController.Play(this.Host.AssetBank.Get<ISong>("Debriefing"));
            this.Host.AudioManager.MusicController.FadeVolumeAttenuation(this.Host.GetComponent<ICodeLogicSettings>().MusicVolume, CodeLogicEngine.Constants.FadeSceneTransitionTime);
        }

        protected override void OnEnding()
        {
            base.OnEnding();

            if (this.Host.GetComponent<ICodeLogicSettings>().CacheChanged)
                this.Host.GetComponent<ICodeLogicSettings>().PersistCache();

            this.Host.AudioManager.MusicController.FadeVolumeAttenuation(0, CodeLogicEngine.Constants.FadeSceneTransitionTime);
        }
        #endregion

        #region Private Methods
        private void CreatePhotos(ILayer hostLayer)
        {
            // Photos
            Vector2 anchor = Vector2.Zero;
            SizeF photoSize = new SizeF(171, 170);

            // Photo 1
            anchor = new Vector2(179, 91);
            hostLayer.AddEntity(
                GameObjectFactory.Instance.CreateTextureEntity(
                    this.Host,
                    new RectangleF(anchor, photoSize),
                    hostLayer.MainSpriteBatch,
                    this.Host.AssetBank.Get<ITexture2D>("DebriefPhoto_Default_Setting"),
                    Color.White
                )
            );

            // Photo 2
            anchor = new Vector2(409, 279);
            hostLayer.AddEntity(
                GameObjectFactory.Instance.CreateTextureEntity(
                    this.Host,
                    new RectangleF(anchor, photoSize),
                    hostLayer.MainSpriteBatch,
                    this.Host.AssetBank.Get<ITexture2D>("DebriefPhoto_Default_Device"),
                    Color.White
                )
            );

            // Photo 3
            anchor = new Vector2(182, 417);
            hostLayer.AddEntity(
                GameObjectFactory.Instance.CreateTextureEntity(
                    this.Host,
                    new RectangleF(anchor, photoSize),
                    hostLayer.MainSpriteBatch,
                    this.Host.AssetBank.Get<ITexture2D>("DebriefPhoto_Default_Objective"),
                    Color.White
                )
            );
        }

        private ILayer CreateTextLayer(ILayer rootLayer)
        {
            _paperBounds = new RectangleF(662, 62, 464, 600);
            _paperBounds.Inflate(-25, -25);

            _textLayer = GameObjectFactory.Instance.CreateGenericLayer(this.Host, rootLayer.MainSpriteBatch);

            _textBuilder = GameObjectFactory.Instance.CreateTextObjectBuilder(
                GameObjectFactory.Instance.CreateTextFormatProcessor(GameObjectFactory.Instance.CreateTextFormatValueHandlerManager()),
                GameObjectFactory.Instance.CreateTextObjectFactory()
            );

            _textBuilder.RegisterFont("Normal", this.Host.AssetBank.Get<ISpriteFont>("TypewriterNormalFont"));
            _textBuilder.RegisterFont("Small", this.Host.AssetBank.Get<ISpriteFont>("TypewriterSmallFont"));
            _textBuilder.RegisterFont("Title", this.Host.AssetBank.Get<ISpriteFont>("TypewriterTitleFont"));
            _textBuilder.DefaultFont = _textBuilder.GetRegisteredFont("Normal");

            return _textLayer;
        }

        private void SetupEntities(bool gameWon)
        {
            _uiLayer.Visible = false;
            _uiLayer.Enabled = false;

            SetupConsoleWindow(gameWon);
            SetupMarksLayer();
        }

        private void SetupConsoleWindow(bool gameWon)
        {
            _textLayer.ClearEntities();

            ITextObjectRenderer textRenderer = GameObjectFactory.Instance.CreateTextObjectRenderer(this.Host.GrfxFactory.RenderTargetFactory, this.Host.Graphics.GraphicsDevice, this.Host.GrfxFactory.SpriteBatchFactory.Create(this.Host.Graphics.GraphicsDevice), (Rectangle)_paperBounds);

            _consoleWindow = GameObjectFactory.Instance.CreateConsoleWindow(this.Host, _paperBounds, _rootLayer.MainSpriteBatch, _textBuilder, textRenderer, false);
            _consoleWindow.AutoAdvanceOnTap = false;
            _consoleWindow.ShowBackground = false;

            _consoleWindow.Tapped += (sender, e) => FinishTextTyping();
            _consoleWindow.WindowStateChanged += Handle_ConsoleWindowStateChanged;

            _consoleWindow.SetTextFormat(GetPaperText(gameWon));
            
            _textLayer.AddEntity(_consoleWindow);
        }

        private void FinishTextTyping()
        {
            _consoleWindow.FlushText();
        }

        private void ReturnToMenuScene()
        {
            this.ChangeScene(GameObjectFactory.Instance.CreateFadeSceneTransition(this.Host, this, this.Host.Scenes["MainMenu"], FadeSceneTransitionMode.Out, CodeLogicEngine.Constants.FadeSceneTransitionTime));
        }

        private void SetupMarksLayer()
        {
            _marksLayer.ClearEntities();
            _marksLayer.Visible = false;

            // Attempt marks
            foreach (ITextureEntity historyMarkEntity in _marksBuilder.CreateAttemptHistoryMarks(_model, new Vector2(717, 308), _marksLayer.MainSpriteBatch))
                _marksLayer.AddEntity(historyMarkEntity);

            // Code marks
            foreach (ITextureEntity finalCodeEntity in _marksBuilder.CreateFinalCodeMarks(_model, new Vector2(805, 444), _marksLayer.MainSpriteBatch))
                _marksLayer.AddEntity(finalCodeEntity);

            // Signature
            ITexture2D signatureTexture = this.Host.AssetBank.Get<ITexture2D>("MissionDebriefSignature");
            Vector2 anchorCentre = new Vector2(1014f, 540.5f);
            _marksLayer.AddEntity(
                GameObjectFactory.Instance.CreateTextureEntity(
                    this.Host,
                    new RectangleF(anchorCentre.X - signatureTexture.Width / 2, anchorCentre.Y - signatureTexture.Height / 2, signatureTexture.Width, signatureTexture.Height),
                    _marksLayer.MainSpriteBatch,
                    signatureTexture,
                    Color.White
                )
            );
        }

        private string GetPaperText(bool gameWon)
        {
            StringBuilder sb = new StringBuilder();
            string dateString = DateTime.Now.Date.ToString("yyyy/MM/dd").Replace('0', 'O').Replace('1', 'l');

            string missionID = DateTime.Now.Ticks.ToString("X").Replace('0', 'O').Replace('1', 'l');

            sb.AppendLine("{TPC:0.1}{C:Black}");
            sb.AppendLine("{Font:Title}{Alignment:Centre} ");
            sb.AppendLine("{Font:Normal}{Alignment:Left}");
            sb.AppendLine();
            sb.AppendLine("        " + dateString);
            sb.Append("          ");
            sb.Append((gameWon) ? "{C:Green}Success" : "{C:Red}Failed");
            sb.AppendLine("{C:Black}");
            sb.AppendLine();
            sb.AppendLine("       ");
            sb.AppendLine("       ");
            sb.AppendLine("\n\n\n\n\n\n\n\n\n\n\n");
            sb.AppendLine("           ");
            sb.AppendLine();
            sb.AppendLine("{Alignment:Right}Mission Director,");
            sb.AppendLine("\n\n");
            sb.AppendLine("Eldrid Pennywell");
            sb.AppendLine();
            sb.AppendLine("{Alignment: Left}{Font:Small}             " + missionID);

            return sb.ToString();
        }
        #endregion

        #region Event Handlers
        private void Handle_OkButtonTapped(object sender, Platform2D.UI.ButtonEventArgs e)
        {
            this.Host.AssetBank.Get<ISoundResource>("ButtonPress").Play();

            ReturnToMenuScene();
        }

        private void Handle_ConsoleWindowStateChanged(object sender, StateChangedEventArgs<WindowState> e)
        {
            if (e.PreviousState == WindowState.Processing && e.CurrentState == WindowState.Waiting)
            {
                _marksLayer.Visible = true;
                _uiLayer.Enabled = true;
                _uiLayer.Visible = true;
            }
        }

        private void Handle_ExternalActionOccurred(object sender, ExternalActionEventArgs e)
        {
            if (e.Data != null && e.Data.Equals(CodeLogicEngine.Constants.ExternalActions.BackPressed) && this.Host.CurrentScene == this)
            {
                if (_consoleWindow.WindowState != WindowState.Waiting)
                    FinishTextTyping();
                else
                    ReturnToMenuScene();
            }
        }
        #endregion
    }
}
