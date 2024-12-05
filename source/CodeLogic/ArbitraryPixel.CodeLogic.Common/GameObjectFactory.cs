using ArbitraryPixel.CodeLogic.Common.BriefingContent;
using ArbitraryPixel.CodeLogic.Common.Config;
using ArbitraryPixel.CodeLogic.Common.Controllers;
using ArbitraryPixel.CodeLogic.Common.Credits;
using ArbitraryPixel.CodeLogic.Common.Entities;
using ArbitraryPixel.CodeLogic.Common.EntityGenerators;
using ArbitraryPixel.CodeLogic.Common.Layers;
using ArbitraryPixel.CodeLogic.Common.Model;
using ArbitraryPixel.CodeLogic.Common.Scenes;
using ArbitraryPixel.CodeLogic.Common.Theme;
using ArbitraryPixel.CodeLogic.Common.Theme.Device;
using ArbitraryPixel.CodeLogic.Common.UI;
using ArbitraryPixel.Common;
using ArbitraryPixel.Common.CodeLogic.BriefingContent;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Common.Graphics.Factory;
using ArbitraryPixel.Platform2D.Animation;
using ArbitraryPixel.Platform2D.Assets;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.Entity;
using ArbitraryPixel.Platform2D.Layer;
using ArbitraryPixel.Platform2D.Scene;
using ArbitraryPixel.Platform2D.Text;
using ArbitraryPixel.Platform2D.Text.ValueHandlers;
using ArbitraryPixel.Platform2D.Theme;
using ArbitraryPixel.Platform2D.Time;
using ArbitraryPixel.Platform2D.UI;
using ArbitraryPixel.Platform2D.UI.Factory;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;

namespace ArbitraryPixel.CodeLogic.Common
{
    /// <summary>
    /// An exception that is thrown when an instance of GameObjectFactory is expected, but it is null.
    /// </summary>
    public class GameObjectFactoryInstanceNullException : Exception { }

    /// <summary>
    /// An object responsible for creating game objects for use in CodeLogic.
    /// </summary>
    public class GameObjectFactory
    {
        #region Singleton Stuff
        private static GameObjectFactory _instance = null;

        public static void SetInstance(GameObjectFactory instance)
        {
            _instance = instance;
        }

        public static GameObjectFactory Instance
        {
            get { return _instance; }
        }
        #endregion

        #region Misc
        public virtual IRandom CreateRandom()
        {
            return new DotNetRandom();
        }

        public virtual IRandom CreateRandom(int seed)
        {
            return new DotNetRandom(seed);
        }

        public virtual IMenuFactory CreateMenuFactory()
        {
            return new MenuFactory();
        }

        public virtual IObjectSearcher CreateObjectSearcher()
        {
            return new ObjectSearcher();
        }
        #endregion

        #region Text
        public virtual ITextFormatValueHandlerManager CreateTextFormatValueHandlerManager()
        {
            ITextFormatValueHandlerManager manager = new TextFormatValueHandlerManager();
            manager.RegisterValueHandler("colour:color:c", SupportedFormat.Colour, new ColourValueHandler());
            manager.RegisterValueHandler("timepercharacter:tpc", SupportedFormat.TimePerCharacter, new DecimalValueHandler());
            manager.RegisterValueHandler("font:f", SupportedFormat.FontName, new StringValueHandler());
            manager.RegisterValueHandler("alignment:a", SupportedFormat.LineAlignment, new TextLineAlignmentValueHandler());
            return manager;
        }

        public virtual ITextFormatProcessor CreateTextFormatProcessor(ITextFormatValueHandlerManager handlerManager)
        {
            return new TextFormatProcessor(handlerManager);
        }

        public virtual ITextObjectFactory CreateTextObjectFactory()
        {
            return new TextObjectFactory();
        }

        public virtual ITextObjectBuilder CreateTextObjectBuilder(ITextFormatProcessor formatProcessor, ITextObjectFactory textObjectFactory)
        {
            return new TextObjectBuilder(formatProcessor, textObjectFactory);
        }

        public virtual ITextObjectBuilder CreateTextObjectBuilderWithConsoleFonts(ITextFormatProcessor formatProcessor, ITextObjectFactory textObjectFactory, IAssetBank assetBank)
        {
            ITextObjectBuilder textBuilder = CreateTextObjectBuilder(formatProcessor, textObjectFactory);

            textBuilder.RegisterFont("Normal", assetBank.Get<ISpriteFont>("ConsoleNormalFont"));
            textBuilder.RegisterFont("Heading", assetBank.Get<ISpriteFont>("ConsoleHeadingFont"));
            textBuilder.DefaultFont = textBuilder.GetRegisteredFont("Normal");

            return textBuilder;
        }

        public virtual ITextObjectRenderer CreateTextObjectRenderer(IRenderTargetFactory renderTargetFactory, IGrfxDevice device, ISpriteBatch spriteBatch, Rectangle bounds)
        {
            return new TextObjectRenderer(renderTargetFactory, device, spriteBatch, bounds);
        }

        public virtual ITextObjectRendererFactory CreateTextObjectRendererFactory()
        {
            return new TextObjectRendererFactory();
        }
        #endregion

        #region UI
        public virtual IButtonObjectDefinitionFactory CreateButtonObjectDefinitionFactory()
        {
            return new ButtonObjectDefinitionFactory();
        }

        public virtual IGenericButton CreateGenericButton(IEngine host, RectangleF bounds, ISpriteBatch spriteBatch)
        {
            return new GenericButton(host, bounds, spriteBatch);
        }

        public virtual IUIObjectFactory CreateUIObjectFactory()
        {
            return new UIObjectFactory();
        }
        #endregion

        #region Theme
        public virtual IThemeManagerCollection CreateThemeManagerCollection()
        {
            return new CodeLogicThemeCollection();
        }
        #endregion

        #region Scenes
        public virtual IScene CreateDeviceAssetLoadScene(IEngine host)
        {
            return new DeviceAssetLoadScene(host);
        }

        public virtual IScene CreateDeviceBootScene(IEngine host, IDeviceModel deviceModel, ILogPanelModel logPanelModel)
        {
            return new DeviceBootScene(host, deviceModel, logPanelModel);
        }

        public virtual IScene CreateDeviceMainScene(IEngine host, IDeviceModel deviceModel, ILogPanelModel logPanelModel)
        {
            return new DeviceMainScene(host, deviceModel, logPanelModel);
        }

        public virtual IScene CreateDeviceMenuScene(IEngine host, IDeviceModel model)
        {
            return new DeviceMenuScene(host, model);
        }

        public virtual IScene CreateMainMenuScene(IEngine host, IMainMenuModel model, IMenuFactory menuFactory, IGameStatsData gameStatsData)
        {
            return new MainMenuScene(host, model, menuFactory, gameStatsData);
        }

        public virtual IScene CreateGameStartupScene(IEngine host, IScene startScene)
        {
            return new GameStartupScene(host, startScene);
        }

        public virtual IScene CreateMissionDebriefingScene(IEngine host, IDeviceModel model, IGameStatsController gameStatsController)
        {
            return new MissionDebriefingScene(host, model, gameStatsController);
        }

        public virtual IScene CreatePreGameAdScene(IEngine host)
        {
            return new PreGameAdScene(host);
        }

        public virtual IScene CreateNoAdMessageScene(IEngine host)
        {
            return new NoAdMessageScene(host);
        }

        public virtual IScene CreateSplashScreenScene(IEngine host, IUIObjectFactory uiObjectFactory)
        {
            return new SplashScreenScene(host, uiObjectFactory);
        }
        #endregion

        #region Scene Transition
        public virtual IScene CreatePanSceneTransition(IEngine host, IScene startScene, IScene endScene, PanSceneTransitionMode transitionMode, double transitionTime)
        {
            return PanSceneTransition.Create(host, startScene, endScene, transitionMode, transitionTime);
        }

        public virtual IScene CreateFadeSceneTransition(IEngine host, IScene startScene, IScene endScene, FadeSceneTransitionMode mode, float transitionTime)
        {
            return FadeSceneTransition.Create(host, startScene, endScene, mode, transitionTime);
        }
        #endregion

        #region Layers
        public virtual ILayer CreateGenericLayer(IEngine host, ISpriteBatch mainSpriteBatch, SpriteSortMode sortMode = SpriteSortMode.Deferred, BlendState blendState = null, SamplerState samplerState = null, DepthStencilState depthStencilState = null, RasterizerState rasterizerState = null, IEffect effect = null, Matrix? matrix = null)
        {
            ILayer newLayer = new LayerBase(host, mainSpriteBatch)
            {
                SpriteSortMode = sortMode,
                BlendState = blendState,
                SamplerState = samplerState,
                DepthStencilState = depthStencilState,
                RasterizerState = rasterizerState,
                Effect = effect,
            };

            if (matrix != null)
                newLayer.Matrix = matrix.Value;

            return newLayer;
        }

        public virtual ICodeInputLayer CreateCodeInputLayer(IEngine host, ISpriteBatch mainSpriteBatch, IDeviceModel model)
        {
            return new CodeInputLayer(host, mainSpriteBatch, model);
        }

        public virtual IDeviceMainUILayer CreateDeviceMainUILayer(IEngine host, ISpriteBatch mainSpriteBatch, IDeviceModel deviceModel, ILogPanelModel logPanelModel)
        {
            return new DeviceMainUILayer(host, mainSpriteBatch, deviceModel, logPanelModel);
        }

        public virtual IDeviceMenuUILayer CreateDeviceMenuUILayer(IEngine host, ISpriteBatch mainSpriteBatch)
        {
            return new DeviceMenuUILayer(host, mainSpriteBatch);
        }

        public virtual IBuildInfoOverlayLayer CreateBuildInfoOverlayLayer(IEngine host, ISpriteBatch mainSpriteBatch, IBuildInfoOverlayLayerModel model, IRandom random)
        {
            return new BuildInfoOverlayLayer(host, mainSpriteBatch, model, random);
        }

        public virtual IMenuBackgroundLayer CreateMenuBackgroundLayer(IEngine host, ISpriteBatch mainSpriteBatch, ISoundPlaybackController soundController)
        {
            return new MenuBackgroundLayer(host, mainSpriteBatch, soundController);
        }

        public virtual IProgressLayer CreateProgressLayer(IEngine host, ISpriteBatch mainSpriteBatch, ISpriteFont titleFont, string titleText)
        {
            return new ProgressLayer(host, mainSpriteBatch, titleFont, titleText);
        }

        public virtual ILogPanelLayer CreateLogPanelLayer(IEngine host, ISpriteBatch spriteBatch, IDeviceModel deviceModel, ILogPanelModel panelModel)
        {
            return new LogPanelLayer(host, spriteBatch, deviceModel, panelModel);
        }

        public virtual ILayer CreateLogPanelContentLayer(IEngine host, ISpriteBatch mainSpriteBatch, IDeviceModel deviceModel, ILogPanelModel panelModel)
        {
            return new LogPanelContentLayer(host, mainSpriteBatch, deviceModel, panelModel);
        }

        public virtual ICloudCoverLayer CreateCloudCoverLayer(IEngine host, ISpriteBatch mainSpriteBatch, IEntityGenerator<ICloud> cloudGenerator, ICloudControllerFactory cloudControllerFactory, IRandom random, IObjectSearcher objectSearcher)
        {
            return new CloudCoverLayer(host, mainSpriteBatch, cloudGenerator, cloudControllerFactory, random, objectSearcher);
        }

        public virtual ILayer CreateMenuBriefingUILayer(IEngine host, ISpriteBatch mainSpriteBatch, RectangleF contentBounds, IMenuBriefingContentLayer hostLayer)
        {
            return new MenuBriefingUILayer(host, mainSpriteBatch, contentBounds, hostLayer);
        }
        #endregion

        #region Entities
        public virtual IButton CreateButtonBase(IEngine host, RectangleF bounds)
        {
            return new ButtonBase(host, bounds);
        }

        public virtual IDeviceBackground CreateDeviceBackground(IEngine host, RectangleF bounds, ISpriteBatch spriteBatch)
        {
            return new DeviceBackground(host, bounds, spriteBatch);
        }

        public virtual ICodeInputButton CreateCodeInputButton(IEngine host, RectangleF bounds, ISpriteBatch spriteBatch, ICodeInputButtonModel model, int index)
        {
            return new CodeInputButton(host, bounds, spriteBatch, model, index);
        }

        public virtual IStatusIndicator CreateStatusIndicator(IEngine host, RectangleF bounds, ISpriteBatch spriteBatch, IDeviceModel model)
        {
            return new StatusIndicator(host, bounds, spriteBatch, model);
        }

        public virtual IStatusIndicator CreateStatusBar(IEngine host, RectangleF bounds, ISpriteBatch spriteBatch, IDeviceModel model)
        {
            return new StatusBar(host, bounds, spriteBatch, model);
        }

        public virtual ITextLabel CreateGenericTextLabel(IEngine host, Vector2 location, ISpriteBatch spriteBatch, ISpriteFont font, string text, Color colour)
        {
            return new TextLabel(host, location, spriteBatch, font, text, colour);
        }

        public virtual ISequenceSubmitButton CreateSequenceSubmitButton(IEngine host, RectangleF bounds, ISpriteBatch spriteBatch)
        {
            return new SequenceSubmitButton(host, bounds, spriteBatch);
        }

        public virtual IConsoleWindow CreateConsoleWindow(IEngine host, RectangleF windowBounds, ISpriteBatch spriteBatch, ITextObjectBuilder textBuilder, ITextObjectRenderer textRenderer, bool animateOpen = true)
        {
            IDeviceTheme currentTheme = host.GetComponent<IThemeManagerCollection>()[ThemeObjectType.Device].GetCurrentTheme<IDeviceTheme>();

            return new ConsoleWindow(host, windowBounds, spriteBatch, textBuilder, textRenderer, animateOpen)
            {
                BorderColour = currentTheme.NormalColourMask,
                BackgroundColour = currentTheme.BackgroundColourMask
            };
        }

        public virtual ISequenceAttemptRecordView CreateSequenceAttemptRecordView(IEngine host, ISpriteBatch spriteBatch, Vector2 location, IDeviceModel model, int targetIndex)
        {
            return new SequenceAttemptRecordView(host, spriteBatch, location, model, targetIndex);
        }

        public virtual ITextureEntityFactory CreateTextureEntityFactory()
        {
            return new TextureEntityFactory();
        }

        public virtual ITextureEntity CreateTextureEntity(IEngine host, RectangleF bounds, ISpriteBatch spriteBatch, ITexture2D texture, Color mask, Rectangle? sourceRectangle = null)
        {
            return new TextureEntity(host, bounds, spriteBatch, texture, mask, sourceRectangle);
        }

        public virtual ICloud CreateCloud(IEngine host, RectangleF bounds, ISpriteBatch spriteBatch, ITexture2D texture, Color mask, Rectangle? sourceRectangle = null)
        {
            return new Cloud(host, bounds, spriteBatch, texture, mask, sourceRectangle);
        }

        public virtual ISimpleButton CreateSimpleButton(IEngine host, RectangleF bounds, ISpriteBatch spriteBatch, ISpriteFont font)
        {
            return new SimpleButton(host, bounds, spriteBatch, font);
        }

        public virtual IChapterButton CreateChapterButton(IEngine host, RectangleF bounds, ISpriteBatch spriteBatch, ISpriteFont titleFont, ISpriteFont descriptionFont)
        {
            return new ChapterButton(host, bounds, spriteBatch, titleFont, descriptionFont);
        }

        public virtual IButton CreateDeviceMenuButton(IEngine host, RectangleF bounds, ISpriteBatch spriteBatch, ITexture2D foregroundTexture)
        {
            return new DeviceMenuButton(host, bounds, spriteBatch, foregroundTexture);
        }

        public virtual IProgressBar CreateProgressBar(IEngine host, RectangleF bounds, ISpriteBatch spriteBatch)
        {
            return new ProgressBar(host, bounds, spriteBatch);
        }

        public virtual ICheckButton CreateCheckButton(IEngine host, RectangleF bounds, ISpriteBatch spriteBatch)
        {
            return new CheckButton(host, bounds, spriteBatch);
        }

        public virtual ISlider CreateSlider(IEngine host, RectangleF bounds, ISpriteBatch spriteBatch)
        {
            return new Slider(host, bounds, spriteBatch);
        }

        public virtual ICreditLineItem CreateCreditLineItem(IEngine host, RectangleF bounds, ISpriteBatch spriteBatch, ITextObject textObject)
        {
            return new CreditLineItem(host, bounds, spriteBatch, textObject);
        }

        public virtual IDialog CreateDialog(IEngine host, RectangleF windowBounds, ITextObjectBuilder textBuilder, string textFormat)
        {
            return new Dialog(host, windowBounds, textBuilder, textFormat);
        }

        public virtual IDialog CreateOkCancelDialog(IEngine host, RectangleF windowBounds, ITextObjectBuilder textBuilder, string textFormat)
        {
            return new OkCancelDialog(host, windowBounds, textBuilder, textFormat);
        }

        public virtual IScreenFadeOverlay CreateScreenFadeOverlay(IEngine host, ISpriteBatch spriteBatch, ITexture2D overlayTexture, IAnimationFactory<float> animationFactory)
        {
            return new ScreenFadeOverlay(host, spriteBatch, overlayTexture, animationFactory);
        }

        public virtual IGameEntity CreateTapScreenText(IEngine host, ISpriteBatch spriteBatch, ISpriteFont font, IAnimationFactory<float> animationFactory)
        {
            return new TapScreenText(host, spriteBatch, font, animationFactory);
        }

        public virtual IFormattedTextLabel CreateFormattedTextLabel(IEngine host, RectangleF bounds, ISpriteBatch spriteBatch, ITextObjectBuilder builder, ITextObjectRenderer renderer, string textFormat)
        {
            return new FormattedTextLabel(host, bounds, spriteBatch, builder, renderer, textFormat);
        }
        #endregion

        #region Entity Generators
        public virtual IEntityGenerator<ICloud> CreateCloudGenerator(SizeF screenSize, ITexture2D[] cloudTextures, ISpriteBatch spriteBatch, IRandom random)
        {
            return new CloudCoverGenerator(screenSize, cloudTextures, spriteBatch, random);
        }
        #endregion

        #region Model
        public virtual IDeviceModel CreateDeviceModel(IRandom random, IStopwatchManager stopwatchManager)
        {
            return new DeviceModel(random, new CodeValueColourMap(), stopwatchManager);
        }

        public virtual ICodeSequence CreateCodeSequence(int codeLength)
        {
            return new CodeSequence(codeLength);
        }

        public virtual ISequenceComparer CreateSequenceComparer()
        {
            return new SequenceComparer();
        }

        public virtual ISequenceAttemptCollection CreateSequenceAttemptCollection()
        {
            return new SequenceAttemptCollection();
        }

        public virtual ISequenceAttemptRecord CreateSequenceAttemptRecord(CodeValue[] code, SequenceIndexCompareResult[] result)
        {
            return new SequenceAttemptRecord(code, result);
        }

        public virtual ICodeInputButtonModel CreateCodeInputButtonModel(IDeviceModel deviceModel)
        {
            return new CodeInputButtonModel(deviceModel);
        }

        public virtual IBuildInfoOverlayLayerModel CreateBuildInfoOverlayModel(IBuildInfoStore buildInfoStore, ITextObjectBuilder textBuilder)
        {
            return new BuildInfoOverlayLayerModel(buildInfoStore, textBuilder);
        }

        public virtual IMainMenuModel CreateMainMenuModel(IEngine host, IMenuFactory menuFactory, IMenuContentMap contentMap, IMainMenuContentLayerFactory contentFactory)
        {
            return new MainMenuModel(host, menuFactory, contentMap, contentFactory);
        }

        public virtual IMenuContentMap CreateMenuContentMap()
        {
            return new MenuContentMap();
        }

        public virtual IMainMenuContentLayerFactory CreateMainMenuContentLayerFactory()
        {
            return new MainMenuContentLayerFactory();
        }

        public virtual IMissionDebriefAttemptRecordMarksBuilder CreateMissionDebriefAttemptRecordMarksBuilder(IEngine host, IRandom random, ITextureEntityFactory factory)
        {
            return new MissionDebriefAttemptRecordMarksBuilder(host, random, factory);
        }

        public virtual IMissionDebriefHistoryMarksBuilder CreateMissionDebriefHistoryMarksBuilder(IEngine host, IRandom random, ITextureEntityFactory factory, IMissionDebriefAttemptRecordMarksBuilder recordAttemptBuilder)
        {
            return new MissionDebriefHistoryMarksBuilder(host, random, factory, recordAttemptBuilder);
        }

        public virtual ILogPanelModel CreateLogPanelModel(ICodeLogicSettings settings, SizeF worldBounds)
        {
            return new LogPanelModel(settings, worldBounds);
        }
        #endregion

        #region Transition Model
        public virtual IPanSceneTransitionModel CreatePanSceneTransitionModel(IScene startScene, IScene endScene, IRenderTarget2D startTarget, IRenderTarget2D endTarget, PanSceneTransitionMode transitionMode, double transitionTime, SizeF screenSize)
        {
            return new PanSceneTransitionModel(startScene, endScene, startTarget, endTarget, transitionMode, transitionTime, screenSize);
        }

        public virtual IFadeSceneTransitionModel CreateFadeSceneTransitionModel(IScene startScene, IScene endScene, IRenderTarget2D startTarget, IRenderTarget2D endTarget, FadeSceneTransitionMode mode, float transitionTime)
        {
            return new FadeSceneTransitionModel(startScene, endScene, startTarget, endTarget, mode, transitionTime);
        }
        #endregion

        #region Controllers
        public virtual ICloudControllerFactory CreateCloudControllerFactory()
        {
            return new CloudControllerFactory();
        }

        public virtual ILayerFadeController CreateLayerFadeController(IAnimationFactory<float> animationFactory)
        {
            return new LayerFadeController(animationFactory);
        }

        public virtual ILayerFadeController CreateLayerFadeController(IAnimationFactory<float> animationFactory, float fadeTransitionTime)
        {
            return new LayerFadeController(animationFactory, fadeTransitionTime);
        }

        public virtual IController CreatePlanePositionController(IGameEntity planeEntity)
        {
            return new PlanePositionController(planeEntity);
        }

        public virtual IPlaneLandingController CreatePlaneLandingController(IGameEntity planeEntity, SizeF screenSize)
        {
            return new PlaneLandingController(planeEntity, screenSize);
        }

        public virtual ISoundPlaybackController CreateSoundPlaybackController()
        {
            return new SoundPlaybackController();
        }

        public virtual IGameStatsController CreateGameStatsController(IGameStatsModel gameStatsModel)
        {
            return new GameStatsController(gameStatsModel);
        }

        public virtual IEntity CreateDelayedDialogController(IEngine host, IDialog dialog, double delayTime)
        {
            return new DelayedDialogController(host, dialog, delayTime);
        }
        #endregion

        #region Animation
        public virtual IAnimationFactory<float> CreateFloatAnimationFactory()
        {
            return new FloatAnimationFactory();
        }
        #endregion

        #region Briefing Content
        public enum BriefingPage
        {
            Cover = 0,
            Contents,
            Chapter1,
            Objective,
            Bootup,
            Chapter2,
            InterfaceOverview,
            CurrentAttemptIndicator,
            CodeInput,
            AttemptHistory,
            SubmitButton,
            DeviceMenu,
            Chapter3,
            CodeOverview,
            ResultsOverview,
            Examples1,
            Examples2,
        }

        public virtual ILayer CreateBriefingPage(BriefingPage page, IEngine host, ISpriteBatch spriteBatch, RectangleF bounds, ITextObjectBuilder builder, ITextObjectRendererFactory rendererFactory, IUIObjectFactory uiFactory)
        {
            ILayer newPage = null;

            switch (page)
            {
                case BriefingPage.Cover:
                    newPage = new BriefingPage_Cover(host, spriteBatch, bounds, builder, rendererFactory, uiFactory);
                    break;
                case BriefingPage.Contents:
                    {
                        ContentsPageInfo[] pageListing =
                        {
                            new ContentsPageInfo(BriefingPage.Chapter1, "Chapter 1", "Introduction"),
                            new ContentsPageInfo(BriefingPage.Chapter2, "Chapter 2", "Interface Overview"),
                            new ContentsPageInfo(BriefingPage.Chapter3, "Chapter 3", "Field Guide"),
                        };
                        newPage = new BriefingContentsPage(host, spriteBatch, bounds, pageListing);
                    }
                    break;
                case BriefingPage.Chapter1:
                    newPage = new BriefingPage_Chapter1(host, spriteBatch, bounds, builder, rendererFactory, uiFactory);
                    break;
                case BriefingPage.Objective:
                    newPage = new BriefingPage_Objective(host, spriteBatch, bounds, builder, rendererFactory, uiFactory);
                    break;
                case BriefingPage.Bootup:
                    newPage = new BriefingPage_Bootup(host, spriteBatch, bounds, builder, rendererFactory, uiFactory);
                    break;
                case BriefingPage.Chapter2:
                    newPage = new BriefingPage_Chapter2(host, spriteBatch, bounds, builder, rendererFactory, uiFactory);
                    break;
                case BriefingPage.InterfaceOverview:
                    newPage = new BriefingPage_Overview(host, spriteBatch, bounds, builder, rendererFactory, uiFactory);
                    break;
                case BriefingPage.CurrentAttemptIndicator:
                    newPage = new BriefingPage_CurrentAttemptIndicator(host, spriteBatch, bounds, builder, rendererFactory, uiFactory);
                    break;
                case BriefingPage.CodeInput:
                    newPage = new BriefingPage_CodeInput(host, spriteBatch, bounds, builder, rendererFactory, uiFactory);
                    break;
                case BriefingPage.AttemptHistory:
                    newPage = new BriefingPage_AttemptHistory(host, spriteBatch, bounds, builder, rendererFactory, uiFactory);
                    break;
                case BriefingPage.SubmitButton:
                    newPage = new BriefingPage_SubmitButton(host, spriteBatch, bounds, builder, rendererFactory, uiFactory);
                    break;
                case BriefingPage.DeviceMenu:
                    newPage = new BriefingPage_DeviceMenu(host, spriteBatch, bounds, builder, rendererFactory, uiFactory);
                    break;
                case BriefingPage.Chapter3:
                    newPage = new BriefingPage_Chapter3(host, spriteBatch, bounds, builder, rendererFactory, uiFactory);
                    break;
                case BriefingPage.CodeOverview:
                    newPage = new BriefingPage_CodeOverview(host, spriteBatch, bounds, builder, rendererFactory, uiFactory);
                    break;
                case BriefingPage.ResultsOverview:
                    newPage = new BriefingPage_ResultsOverview(host, spriteBatch, bounds, builder, rendererFactory, uiFactory);
                    break;
                case BriefingPage.Examples1:
                    newPage = new BriefingPage_Examples1(host, spriteBatch, bounds, builder, rendererFactory, uiFactory);
                    break;
                case BriefingPage.Examples2:
                    newPage = new BriefingPage_Examples2(host, spriteBatch, bounds, builder, rendererFactory, uiFactory);
                    break;
            }

            return newPage;
        }
        #endregion
    }
}
