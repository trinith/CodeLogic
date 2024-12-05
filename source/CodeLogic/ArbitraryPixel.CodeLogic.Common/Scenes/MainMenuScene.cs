using ArbitraryPixel.CodeLogic.Common.Config;
using ArbitraryPixel.CodeLogic.Common.Controllers;
using ArbitraryPixel.CodeLogic.Common.Layers;
using ArbitraryPixel.CodeLogic.Common.Model;
using ArbitraryPixel.CodeLogic.Common.UI;
using ArbitraryPixel.Common.Audio;
using ArbitraryPixel.Common.ContentManagement;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Common.Graphics.Factory;
using ArbitraryPixel.Platform2D.Assets;
using ArbitraryPixel.Platform2D.Audio;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.Entity;
using ArbitraryPixel.Platform2D.Layer;
using ArbitraryPixel.Platform2D.Scene;
using ArbitraryPixel.Platform2D.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace ArbitraryPixel.CodeLogic.Common.Scenes
{
    public class MainMenuScene : SceneBase
    {
        private IMainMenuModel _model;
        private IMenuFactory _menuFactory;
        private IGameStatsData _gameStatsData;

        private IMenuListView _listView;
        private IMenuView _titleView;
        private IEntity _contentBackground;
        private IMenuContentLayer _currentMenuContent = null;
        private IDialog _exitDialog = null;
        private IDialog _resetStatsDialog = null;
        private IEntity _permissionsDialogController = null;

        private IEntity _inputBlocker;
        private ILayer _contentLayer;
        private IMenuBackgroundLayer _backgroundLayer;

        private ILayerFadeController _layerFadeController;
        private bool _dataLoaded = false;

        public MainMenuScene(IEngine host, IMainMenuModel model, IMenuFactory menuFactory, IGameStatsData gameStatsData)
            : base(host)
        {
            _model = model ?? throw new ArgumentNullException();
            _menuFactory = menuFactory ?? throw new ArgumentNullException();
            _gameStatsData = gameStatsData ?? throw new ArgumentNullException();

            _model.StartSinglePlayerGame += Handle_StartSinglePlayerGame;
            _model.ExitGame += Handle_ExitGame;
            _model.ResetStatistics += Handle_ResetStatistics;

            this.Host.ExternalActionOccurred += Handle_ExternalActionOccurred;

            _layerFadeController = GameObjectFactory.Instance.CreateLayerFadeController(GameObjectFactory.Instance.CreateFloatAnimationFactory());
        }

        protected override void OnLoadAssetBank(IContentManager content, IAssetBank bank)
        {
            base.OnLoadAssetBank(content, bank);

            #region Textures
            ITexture2DFactory textureFactory = this.Host.GrfxFactory.Texture2DFactory;

            // General
            bank.Put<ITexture2D>("Plane", textureFactory.Create(content, @"Textures\Plane"));
            bank.Put<ITexture2D>("MainMenuBackground", textureFactory.Create(content, @"Textures\MainMenuBackground"));
            bank.Put<ITexture2D>("cloud0", textureFactory.Create(content, @"Textures\cloud0"));
            bank.Put<ITexture2D>("cloud1", textureFactory.Create(content, @"Textures\cloud1"));

            // Briefing
            bank.Put<ITexture2D>("CX4Logo", textureFactory.Create(content, @"Textures\Briefing\CX4Logo"));
            bank.Put<ITexture2D>("BP2_BootupImage", textureFactory.Create(content, @"Textures\Briefing\BP2_BootupImage"));
            bank.Put<ITexture2D>("BP3_Overview", textureFactory.Create(content, @"Textures\Briefing\BP3_Overview"));
            bank.Put<ITexture2D>("BP4_CurrentAttempt", textureFactory.Create(content, @"Textures\Briefing\BP4_CurrentAttempt"));
            bank.Put<ITexture2D>("CodeInput", textureFactory.Create(content, @"Textures\Briefing\CodeInput"));
            bank.Put<ITexture2D>("CodeInput_Example", textureFactory.Create(content, @"Textures\Briefing\CodeInput_Example"));
            bank.Put<ITexture2D>("HistoryFull", textureFactory.Create(content, @"Textures\Briefing\HistoryFull"));
            bank.Put<ITexture2D>("HistoryZoom", textureFactory.Create(content, @"Textures\Briefing\HistoryZoom"));
            bank.Put<ITexture2D>("SubmitButton", textureFactory.Create(content, @"Textures\Briefing\SubmitButton"));
            bank.Put<ITexture2D>("MenuButton", textureFactory.Create(content, @"Textures\Briefing\MenuButton"));
            bank.Put<ITexture2D>("ReturnButton", textureFactory.Create(content, @"Textures\Briefing\ReturnButton"));
            bank.Put<ITexture2D>("MenuScreen", textureFactory.Create(content, @"Textures\Briefing\MenuScreen"));

            bank.Put<ITexture2D>("Example_Code", textureFactory.Create(content, @"Textures\Briefing\Example_Code"));
            bank.Put<ITexture2D>("Example_Results1", textureFactory.Create(content, @"Textures\Briefing\Example_Results1"));
            bank.Put<ITexture2D>("Example_Results2", textureFactory.Create(content, @"Textures\Briefing\Example_Results2"));
            bank.Put<ITexture2D>("Example_Results3", textureFactory.Create(content, @"Textures\Briefing\Example_Results3"));
            bank.Put<ITexture2D>("Example_Results4", textureFactory.Create(content, @"Textures\Briefing\Example_Results4"));

            bank.Put<ITexture2D>("Results_Equal", textureFactory.Create(content, @"Textures\Briefing\Results_Equal"));
            bank.Put<ITexture2D>("Results_Partial", textureFactory.Create(content, @"Textures\Briefing\Results_Partial"));
            bank.Put<ITexture2D>("Results_NotEqual", textureFactory.Create(content, @"Textures\Briefing\Results_NotEqual"));

            #endregion

            #region Fonts
            ISpriteFontFactory fontFactory = this.Host.GrfxFactory.SpriteFontFactory;

            bank.Put<ISpriteFont>("MainMenuFont", fontFactory.Create(content, @"Fonts\MainMenuFont"));
            bank.Put<ISpriteFont>("MainMenuContentFont", fontFactory.Create(content, @"Fonts\MainMenuContentFont"));
            bank.Put<ISpriteFont>("MainButtonFont", fontFactory.Create(content, @"Fonts\MainButtonFont"));
            bank.Put<ISpriteFont>("CreditsTitleFont", fontFactory.Create(content, @"Fonts\CreditsTitleFont"));
            bank.Put<ISpriteFont>("CreditsCreditFont", fontFactory.Create(content, @"Fonts\CreditsCreditFont"));

            bank.Put<ISpriteFont>("ConsoleNormalFont", fontFactory.Create(content, @"Fonts\ConsoleNormalFont"));
            bank.Put<ISpriteFont>("ConsoleHeadingFont", fontFactory.Create(content, @"Fonts\ConsoleHeadingFont"));

            bank.Put<ISpriteFont>("VersionFont", fontFactory.Create(content, @"Fonts\VersionFont"));
            bank.Put<ISpriteFont>("StatsFont", fontFactory.Create(content, @"Fonts\StatsFont"));

            bank.Put<ISpriteFont>("BriefingNormalFont", fontFactory.Create(content, @"Fonts\Briefing\BriefingNormalFont"));
            bank.Put<ISpriteFont>("BriefingTitleFont", fontFactory.Create(content, @"Fonts\Briefing\BriefingTitleFont"));
            #endregion

            #region Music
            bank.Put<ISong>("MainMenu", this.Host.AudioManager.MusicController.CreateSong(this.Host.Content, @"Music\MainMenu"));
            #endregion

            #region Sound Effects
            ISoundController soundController = this.Host.AudioManager.SoundController;
            bank.Put<ISoundResource>("WindowOpen", soundController.CreateSoundResource(content, @"Sounds\WindowOpen"));
            bank.Put<ISoundResource>("WindowClose", soundController.CreateSoundResource(content, @"Sounds\WindowClose"));
            bank.Put<ISoundResource>("Thunder1", soundController.CreateSoundResource(content, @"Sounds\Thunder1"));
            bank.Put<ISoundResource>("AirplaneNormal", soundController.CreateSoundResource(content, @"Sounds\AirplaneNormal"));
            bank.Put<ISoundResource>("AirplaneReducePowerFade", soundController.CreateSoundResource(content, @"Sounds\AirplaneReducePowerFade"));
            #endregion

            #region Shaders
            bank.Put<IEffect>("LightningFlash", this.Host.GrfxFactory.EffectFactory.Create(content, @"Shaders\LightningFlash"));
            #endregion
        }

        protected override void OnReset()
        {
            base.OnReset();

            this.NextScene = null;
            this.SceneComplete = false;

            _titleView.ViewOf = _model.RootMenu;

            // Clear ListView and force an update so it processes the clear.
            _listView.SelectedItem = null;
            _listView.ViewOf = _model.RootMenu;
            _listView.Update(new GameTime());

            _backgroundLayer.Reset();
            _inputBlocker.Enabled = false;
        }

        protected override void OnStarting()
        {
            base.OnStarting();

            _layerFadeController.Reset();

            this.Host.AudioManager.MusicController.VolumeAttenuation = 0f;
            this.Host.AudioManager.MusicController.Play(this.Host.AssetBank.Get<ISong>("MainMenu"));
            this.Host.AudioManager.MusicController.FadeVolumeAttenuation(1f, CodeLogicEngine.Constants.FadeSceneTransitionTime, 0.25);

            if (!_dataLoaded)
            {
                _gameStatsData.LoadData();
                _dataLoaded = true;
            }
        }

        protected override void OnEnding()
        {
            base.OnEnding();

            this.Host.AudioManager.MusicController.FadeVolumeAttenuation(0, CodeLogicEngine.Constants.FadeSceneTransitionTimeHalf);
            _backgroundLayer.StopSounds();
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            ITexture2D pixelTexture = this.Host.AssetBank.Get<ITexture2D>("Pixel");
            ISpriteFont menuFont = this.Host.AssetBank.Get<ISpriteFont>("MainMenuFont");

            // Create Background Layer
            _backgroundLayer = GameObjectFactory.Instance.CreateMenuBackgroundLayer(
                this.Host,
                this.Host.GrfxFactory.SpriteBatchFactory.Create(this.Host.Graphics.GraphicsDevice),
                GameObjectFactory.Instance.CreateSoundPlaybackController()
            );
            _backgroundLayer.SequenceEnded += Handle_BackgroundLayerSequenceEnded;
            this.AddEntity(_backgroundLayer);

            // Create the menu layer
            ILayer menuLayer = GameObjectFactory.Instance.CreateGenericLayer(this.Host, this.Host.GrfxFactory.SpriteBatchFactory.Create(this.Host.Graphics.GraphicsDevice), SpriteSortMode.Deferred, BlendState.NonPremultiplied);
            this.AddEntity(menuLayer);

            // Add the logo to the main menu layer
            ITexture2D logoTexture = this.Host.AssetBank.Get<ITexture2D>("APLogo");
            SizeF logoSize = new SizeF(logoTexture.Width / 2, logoTexture.Height / 2);
            Vector2 logoPos = new Vector2(this.Host.ScreenManager.World.X - logoSize.Width - CodeLogicEngine.Constants.TextWindowPadding.Width, this.Host.ScreenManager.World.Y - logoSize.Height - CodeLogicEngine.Constants.TextWindowPadding.Height);
            menuLayer.AddEntity(
                GameObjectFactory.Instance.CreateTextureEntity(
                    this.Host,
                    new RectangleF(logoPos, logoSize),
                    menuLayer.MainSpriteBatch,
                    logoTexture,
                    Color.White * 0.5f
                )
            );

            // Create a layer to hold the menu content.
            _contentLayer = GameObjectFactory.Instance.CreateGenericLayer(this.Host, this.Host.GrfxFactory.SpriteBatchFactory.Create(this.Host.Graphics.GraphicsDevice), SpriteSortMode.Deferred, BlendState.NonPremultiplied);
            this.AddEntity(_contentLayer);

            // Figure out where the content is going to go, then build it.
            RectangleF totalBounds = new RectangleF(Vector2.Zero, new SizeF(1000, 650));
            totalBounds.Location = ((SizeF)this.Host.ScreenManager.World).Centre - totalBounds.Centre;
            RectangleF titleBounds = new RectangleF(totalBounds.Location, new SizeF(totalBounds.Width, menuFont.LineSpacing));
            RectangleF menuBounds = new RectangleF(
                new Vector2(titleBounds.Left, titleBounds.Bottom),
                new SizeF(350, totalBounds.Height - titleBounds.Height)
            );

            RectangleF contentBounds = new RectangleF(
                new Vector2(menuBounds.Right, titleBounds.Bottom),
                new SizeF(totalBounds.Width - menuBounds.Width, totalBounds.Height - titleBounds.Height)
            );

            _model.BuildContentMap(this.Host, _contentLayer.MainSpriteBatch, contentBounds);

            // Create a background for the content.
            _contentBackground = GameObjectFactory.Instance.CreateTextureEntity(this.Host, contentBounds, _contentLayer.MainSpriteBatch, pixelTexture, CodeLogicEngine.Constants.ClrMenuBGLow);
            _contentLayer.AddEntity(_contentBackground);

            // Set up the menu's list view.
            _listView = _menuFactory.CreateMenuListView(this.Host, menuBounds, _model.RootMenu, menuLayer.MainSpriteBatch, menuFont, _menuFactory);
            _titleView = _menuFactory.CreateMenuTitleView(this.Host, titleBounds, _model.RootMenu, menuLayer.MainSpriteBatch, menuFont);

            _listView.MenuBackTapped += Handle_MenuBackTapped;
            _listView.MenuItemTapped += Handle_MenuItemTapped;
            _listView.SelectedItemChanged += Handle_SelectedItemChanged;

            // Add the views to the menu layer.
            menuLayer.AddEntity(GameObjectFactory.Instance.CreateTextureEntity(this.Host, totalBounds, menuLayer.MainSpriteBatch, pixelTexture, CodeLogicEngine.Constants.ClrMenuBG));
            menuLayer.AddEntity(_listView);
            menuLayer.AddEntity(_titleView);

            // Add the layers we need to fade to the fade controller.
            _layerFadeController.AddLayer(_contentLayer);
            _layerFadeController.AddLayer(menuLayer);
            _layerFadeController.Reset();

            // Create dialogs for exit and reset statistics prompts
            ITextObjectBuilder textBuilder = GameObjectFactory.Instance.CreateTextObjectBuilderWithConsoleFonts(
                GameObjectFactory.Instance.CreateTextFormatProcessor(GameObjectFactory.Instance.CreateTextFormatValueHandlerManager()),
                GameObjectFactory.Instance.CreateTextObjectFactory(),
                this.Host.AssetBank
            );

            this.AddEntity(_exitDialog = CreateDialog(textBuilder, CodeLogicEngine.Constants.DialogText_ExitConfirmation));
            _exitDialog.DialogClosed += Handle_ExitDialogClosed;

            this.AddEntity(_resetStatsDialog = CreateDialog(textBuilder, CodeLogicEngine.Constants.DialogText_ResetStatisticsConfirmation));
            _resetStatsDialog.DialogClosed += Handle_ResetStatsDialogClosed;

            if (this.Host.GetComponent<ICodeLogicSettings>().IsTransient)
            {
                // If our settings component is transient, display a message that we don't have storage permissions.
                this.AddEntity(
                    _permissionsDialogController = GameObjectFactory.Instance.CreateDelayedDialogController(
                        this.Host,
                        CreateInfoDialog(textBuilder,  CodeLogicEngine.Constants.DialogText_NoStoragePermissions),
                        1.0
                    )
                );
                _permissionsDialogController.Disposed += (sender, e) => _permissionsDialogController = null;
            }

            // Create a generic button to go over top of all the entities in this scene to intercept input. We can use this to prevent interaction during various animations.
            this.AddEntity(_inputBlocker = GameObjectFactory.Instance.CreateButtonBase(this.Host, new RectangleF(Vector2.Zero, (SizeF)this.Host.ScreenManager.World)));
            _inputBlocker.Visible = false;
            _inputBlocker.Enabled = false;
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            base.OnUpdate(gameTime);

            if (true
                && !_layerFadeController.IsAnimating
                && !_layerFadeController.IsAnimationComplete(FadeMode.FadeIn)
                && this.Host.InputManager.IsActive
                && this.Host.InputManager.GetSurfaceState().IsTouched
                && _permissionsDialogController == null)
            {
                _backgroundLayer.HideText();
                _layerFadeController.StartAnimation(FadeMode.FadeIn);
            }

            _layerFadeController.Update(gameTime);
        }

        private IDialog CreateDialog(ITextObjectBuilder textBuilder, string dialogText)
        {
            RectangleF windowBounds = new RectangleF(0, 0, 750, 200);
            windowBounds.Location = ((SizeF)this.Host.ScreenManager.World).Centre - windowBounds.Size.Centre;
            IDialog dialog = GameObjectFactory.Instance.CreateOkCancelDialog(
                this.Host,
                windowBounds,
                textBuilder,
                dialogText
            );

            return dialog;
        }

        private IDialog CreateInfoDialog(ITextObjectBuilder textBuilder, string dialogText)
        {
            RectangleF windowBounds = new RectangleF(0, 0, 750, 200);
            windowBounds.Location = ((SizeF)this.Host.ScreenManager.World).Centre - windowBounds.Size.Centre;
            IDialog dialog = GameObjectFactory.Instance.CreateDialog(
                this.Host,
                windowBounds,
                textBuilder,
                dialogText
            );

            return dialog;
        }

        private void Handle_ExitDialogClosed(object sender, DialogClosedEventArgs e)
        {
            if (e.Result == DialogResult.Ok)
            {
                this.Host.Exit();
            }
        }

        private void Handle_ResetStatsDialogClosed(object sender, DialogClosedEventArgs e)
        {
            if (e.Result == DialogResult.Ok)
            {
                _gameStatsData.Model.Reset();
                _gameStatsData.SaveData();
            }
        }

        private void Handle_ExternalActionOccurred(object sender, ExternalActionEventArgs e)
        {
            if (e.Data != null && e.Data.Equals(CodeLogicEngine.Constants.ExternalActions.BackPressed) && this.Host.CurrentScene == this)
            {
                if (_exitDialog.IsOpen == false)
                    _exitDialog.Show();
                else
                    _exitDialog.Close();
            }
        }

        private void Handle_MenuBackTapped(object sender, EventArgs e)
        {
            IMenuItem parent = _listView.ViewOf.Parent;
            if (parent != null)
            {
                _listView.ViewOf = parent;
            }
        }

        private void Handle_MenuItemTapped(object sender, MenuItemEventArgs e)
        {
            if (e.Item.Items.Length > 0)
            {
                _listView.ViewOf = e.Item;
            }
            else
            {
                _listView.SelectedItem = e.Item;
            }

            if (this.Host.GetComponent<ICodeLogicSettings>().CacheChanged)
                this.Host.GetComponent<ICodeLogicSettings>().PersistCache();
        }

        private void Handle_SelectedItemChanged(object sender, MenuItemEventArgs e)
        {
            if (e.Item != null)
                this.Host.AssetBank.Get<ISoundResource>("ButtonPress").Play();

            _titleView.ViewOf = (_listView.SelectedItem != null) ? _listView.SelectedItem : _listView.ViewOf;

            if (_currentMenuContent != null)
            {
                _currentMenuContent.Hide();
                _currentMenuContent = null;
            }

            _contentLayer.ClearEntities();
            _contentLayer.AddEntity(_contentBackground);

            if (e.Item != null && _model.ContentMap.HasMappedLayer(e.Item))
            {
                _contentLayer.AddEntity(_currentMenuContent = _model.ContentMap.GetLayer(e.Item));
                _currentMenuContent.Show();
            }
        }

        private void Handle_BackgroundLayerSequenceEnded(object sender, EventArgs e)
        {
            // Hard cut to this scene.
            this.ChangeScene(this.Host.Scenes["DeviceAssetLoad"]);
        }

        private void Handle_StartSinglePlayerGame(object sender, EventArgs e)
        {
            _inputBlocker.Enabled = true;
            _backgroundLayer.StartEndSequence();
            _layerFadeController.StartAnimation(FadeMode.FadeOut);
        }

        private void Handle_ExitGame(object sender, EventArgs e)
        {
            this.Host.Exit();
        }

        private void Handle_ResetStatistics(object sender, EventArgs e)
        {
            if (_resetStatsDialog.IsOpen == false)
                _resetStatsDialog.Show();
        }
    }
}
