using ArbitraryPixel.CodeLogic.Common.Scenes;
using ArbitraryPixel.CodeLogic.Common.Theme;
using ArbitraryPixel.CodeLogic.Common.Theme.Device;
using ArbitraryPixel.Common.Audio;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Assets;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.Scene;
using ArbitraryPixel.Platform2D.Theme;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Collections.Generic;

namespace ArbitraryPixel.CodeLogic.Common_Tests.Scenes
{
    [TestClass]
    public class DeviceAssetLoadScene_Tests
    {
        private DeviceAssetLoadScene _sut;

        private IEngine _mockEngine;
        private IThemeManagerCollection _mockThemeCollection;
        private IDeviceTheme _mockDeviceTheme;
        private IScene mockNextScene;

        [TestInitialize]
        public void Initialize()
        {
            _mockEngine = Substitute.For<IEngine>();

            _mockEngine.Scenes.Returns(new Dictionary<string, IScene>());
            _mockEngine.Scenes.Add("PreGameAd", mockNextScene = Substitute.For<IScene>());

            _mockDeviceTheme = Substitute.For<IDeviceTheme>();
            _mockDeviceTheme.GetFullAssetName(Arg.Any<string>()).Returns(x => x[0].ToString());

            _mockThemeCollection = Substitute.For<IThemeManagerCollection>();
            _mockEngine.GetComponent<IThemeManagerCollection>().Returns(_mockThemeCollection);
            _mockThemeCollection[ThemeObjectType.Device].GetCurrentTheme<IDeviceTheme>().Returns(_mockDeviceTheme);

            IAssetLoader mockLoader = Substitute.For<IAssetLoader>();
            AssetLoadMethod loadMethod = null;
            _mockEngine.AssetBank.CreateLoader().Returns(mockLoader);
            mockLoader.When(x => x.RegisterLoadMethod(Arg.Any<AssetLoadMethod>())).Do(x => loadMethod = x[0] as AssetLoadMethod);
            mockLoader.When(x => x.LoadBank()).Do(x => loadMethod(_mockEngine.AssetBank));

            _sut = new DeviceAssetLoadScene(_mockEngine);
        }

        #region Initialize Tests
        [TestMethod]
        public void InitializeShouldSetSceneCompleteToTrue()
        {
            _sut.Initialize();

            Assert.IsTrue(_sut.SceneComplete);
        }

        [TestMethod]
        public void InitializeShouldSetNextSceneToDeviceBootScene()
        {
            _sut.Initialize();

            Assert.AreSame(mockNextScene, _sut.NextScene);
        }
        #endregion

        #region Asset Load Tests - Textures
        [TestMethod]
        public void InitializeShouldLoadTexture_DeviceBackground()
        {
            string target = "DeviceBackground";
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, target).Returns(mockTexture);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockDeviceTheme.GetFullAssetName(target);
                    _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, target);
                    _mockEngine.AssetBank.Put<ITexture2D>(target, mockTexture);
                }
            );
        }

        [TestMethod]
        public void InitializeShouldLoadTexture_SequenceSubmitButtonForeground()
        {
            string target = "SequenceSubmitButtonForeground";
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, target).Returns(mockTexture);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockDeviceTheme.GetFullAssetName(target);
                    _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, target);
                    _mockEngine.AssetBank.Put<ITexture2D>(target, mockTexture);
                }
            );
        }

        [TestMethod]
        public void InitializeShouldLoadTexture_SequenceSubmitButtonBackground()
        {
            string target = "SequenceSubmitButtonBackground";
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, target).Returns(mockTexture);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockDeviceTheme.GetFullAssetName(target);
                    _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, target);
                    _mockEngine.AssetBank.Put<ITexture2D>(target, mockTexture);
                }
            );
        }

        [TestMethod]
        public void InitializeShouldLoadTexture_SequenceSubmitButtonHoldOverlay()
        {
            string target = "SequenceSubmitButtonHoldOverlay";
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, target).Returns(mockTexture);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockDeviceTheme.GetFullAssetName(target);
                    _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, target);
                    _mockEngine.AssetBank.Put<ITexture2D>(target, mockTexture);
                }
            );
        }

        [TestMethod]
        public void InitializeShouldLoadTexture_StatusBarBackgroundFill()
        {
            string target = "StatusBarBackgroundFill";
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, target).Returns(mockTexture);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockDeviceTheme.GetFullAssetName(target);
                    _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, target);
                    _mockEngine.AssetBank.Put<ITexture2D>(target, mockTexture);
                }
            );
        }

        [TestMethod]
        public void InitializeShouldLoadTexture_StatusBarProgressFrame()
        {
            string target = "StatusBarProgressFrame";
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, target).Returns(mockTexture);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockDeviceTheme.GetFullAssetName(target);
                    _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, target);
                    _mockEngine.AssetBank.Put<ITexture2D>(target, mockTexture);
                }
            );
        }

        [TestMethod]
        public void InitializeShouldLoadTexture_StatusBarProgressFill()
        {
            string target = "StatusBarProgressFill";
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, target).Returns(mockTexture);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockDeviceTheme.GetFullAssetName(target);
                    _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, target);
                    _mockEngine.AssetBank.Put<ITexture2D>(target, mockTexture);
                }
            );
        }

        [TestMethod]
        public void InitializeShouldLoadTexture_StatusBarProgressFillBackground()
        {
            string target = "StatusBarProgressFillBackground";
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, target).Returns(mockTexture);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockDeviceTheme.GetFullAssetName(target);
                    _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, target);
                    _mockEngine.AssetBank.Put<ITexture2D>(target, mockTexture);
                }
            );
        }

        [TestMethod]
        public void InitializeShouldLoadTexture_StatusBarCurrentTrialIndicator()
        {
            string target = "StatusBarCurrentTrialIndicator";
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, target).Returns(mockTexture);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockDeviceTheme.GetFullAssetName(target);
                    _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, target);
                    _mockEngine.AssetBank.Put<ITexture2D>(target, mockTexture);
                }
            );
        }

        [TestMethod]
        public void InitializeShouldLoadTexture_StatusBarBackgroundBorder()
        {
            string target = "StatusBarBackgroundBorder";
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, target).Returns(mockTexture);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockDeviceTheme.GetFullAssetName(target);
                    _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, target);
                    _mockEngine.AssetBank.Put<ITexture2D>(target, mockTexture);
                }
            );
        }

        [TestMethod]
        public void InitializeShouldLoadTexture_SceneChangeButtonFill()
        {
            string target = "SceneChangeButtonFill";
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, target).Returns(mockTexture);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockDeviceTheme.GetFullAssetName(target);
                    _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, target);
                    _mockEngine.AssetBank.Put<ITexture2D>(target, mockTexture);
                }
            );
        }

        [TestMethod]
        public void InitializeShouldLoadTexture_SceneChangeButtonBorder()
        {
            string target = "SceneChangeButtonBorder";
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, target).Returns(mockTexture);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockDeviceTheme.GetFullAssetName(target);
                    _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, target);
                    _mockEngine.AssetBank.Put<ITexture2D>(target, mockTexture);
                }
            );
        }

        [TestMethod]
        public void InitializeShouldLoadTexture_HexButtonBorder()
        {
            string target = "HexButtonBorder";
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, target).Returns(mockTexture);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockDeviceTheme.GetFullAssetName(target);
                    _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, target);
                    _mockEngine.AssetBank.Put<ITexture2D>(target, mockTexture);
                }
            );
        }

        [TestMethod]
        public void InitializeShouldLoadTexture_HexButtonFill()
        {
            string target = "HexButtonFill";
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, target).Returns(mockTexture);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockDeviceTheme.GetFullAssetName(target);
                    _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, target);
                    _mockEngine.AssetBank.Put<ITexture2D>(target, mockTexture);
                }
            );
        }

        [TestMethod]
        public void InitializeShouldLoadTexture_HexButtonIcon()
        {
            string target = "HexButtonIcon";
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, target).Returns(mockTexture);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockDeviceTheme.GetFullAssetName(target);
                    _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, target);
                    _mockEngine.AssetBank.Put<ITexture2D>(target, mockTexture);
                }
            );
        }

        [TestMethod]
        public void InitializeShouldLoadTexture_HexButtonSelectorFill()
        {
            string target = "HexButtonSelectorFill";
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, target).Returns(mockTexture);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockDeviceTheme.GetFullAssetName(target);
                    _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, target);
                    _mockEngine.AssetBank.Put<ITexture2D>(target, mockTexture);
                }
            );
        }

        [TestMethod]
        public void InitializeShouldLoadTexture_HexButtonSelectorColours()
        {
            string target = "HexButtonSelectorColours";
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, target).Returns(mockTexture);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockDeviceTheme.GetFullAssetName(target);
                    _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, target);
                    _mockEngine.AssetBank.Put<ITexture2D>(target, mockTexture);
                }
            );
        }

        [TestMethod]
        public void InitializeShouldLoadTexture_HexButtonSelectorBorder()
        {
            string target = "HexButtonSelectorBorder";
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, target).Returns(mockTexture);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockDeviceTheme.GetFullAssetName(target);
                    _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, target);
                    _mockEngine.AssetBank.Put<ITexture2D>(target, mockTexture);
                }
            );
        }

        [TestMethod]
        public void InitializeShouldLoadTexture_HexButtonSelectorHighlight()
        {
            string target = "HexButtonSelectorHighlight";
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, target).Returns(mockTexture);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockDeviceTheme.GetFullAssetName(target);
                    _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, target);
                    _mockEngine.AssetBank.Put<ITexture2D>(target, mockTexture);
                }
            );
        }

        [TestMethod]
        public void InitializeShouldLoadTexture_InputBackgroundBorder()
        {
            string target = "InputBackgroundBorder";
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, target).Returns(mockTexture);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockDeviceTheme.GetFullAssetName(target);
                    _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, target);
                    _mockEngine.AssetBank.Put<ITexture2D>(target, mockTexture);
                }
            );
        }

        [TestMethod]
        public void InitializeShouldLoadTexture_InputBackgroundFill()
        {
            string target = "InputBackgroundFill";
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, target).Returns(mockTexture);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockDeviceTheme.GetFullAssetName(target);
                    _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, target);
                    _mockEngine.AssetBank.Put<ITexture2D>(target, mockTexture);
                }
            );
        }

        [TestMethod]
        public void InitializeShouldLoadTexture_DeviceMenuButtonBackground()
        {
            string target = "DeviceMenuButtonBackground";
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, target).Returns(mockTexture);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockDeviceTheme.GetFullAssetName(target);
                    _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, target);
                    _mockEngine.AssetBank.Put<ITexture2D>(target, mockTexture);
                }
            );
        }

        [TestMethod]
        public void InitializeShouldLoadTexture_DeviceMenuButtonBorder()
        {
            string target = "DeviceMenuButtonBorder";
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, target).Returns(mockTexture);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockDeviceTheme.GetFullAssetName(target);
                    _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, target);
                    _mockEngine.AssetBank.Put<ITexture2D>(target, mockTexture);
                }
            );
        }

        [TestMethod]
        public void InitializeShouldLoadTexture_DeviceMenuButtonAbort()
        {
            string target = "DeviceMenuButtonAbort";
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, target).Returns(mockTexture);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockDeviceTheme.GetFullAssetName(target);
                    _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, target);
                    _mockEngine.AssetBank.Put<ITexture2D>(target, mockTexture);
                }
            );
        }

        [TestMethod]
        public void InitializeShouldLoadTexture_DeviceMenuButtonSettings()
        {
            string target = "DeviceMenuButtonSettings";
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, target).Returns(mockTexture);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockDeviceTheme.GetFullAssetName(target);
                    _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, target);
                    _mockEngine.AssetBank.Put<ITexture2D>(target, mockTexture);
                }
            );
        }

        [TestMethod]
        public void InitializeShouldLoadTexture_IconMenu()
        {
            string target = "IconMenu";
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, target).Returns(mockTexture);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockDeviceTheme.GetFullAssetName(target);
                    _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, target);
                    _mockEngine.AssetBank.Put<ITexture2D>(target, mockTexture);
                }
            );
        }

        [TestMethod]
        public void InitializeShouldLoadTexture_IconBack()
        {
            string target = "IconBack";
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, target).Returns(mockTexture);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockDeviceTheme.GetFullAssetName(target);
                    _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, target);
                    _mockEngine.AssetBank.Put<ITexture2D>(target, mockTexture);
                }
            );
        }

        [TestMethod]
        public void InitializeShouldLoadTexture_IconLog()
        {
            string target = "IconLog";
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, target).Returns(mockTexture);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockDeviceTheme.GetFullAssetName(target);
                    _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, target);
                    _mockEngine.AssetBank.Put<ITexture2D>(target, mockTexture);
                }
            );
        }

        [TestMethod]
        public void InitializeShouldLoadTexture_LogButtonFill()
        {
            string target = "LogButtonFill";
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, target).Returns(mockTexture);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockDeviceTheme.GetFullAssetName(target);
                    _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, target);
                    _mockEngine.AssetBank.Put<ITexture2D>(target, mockTexture);
                }
            );
        }

        [TestMethod]
        public void InitializeShouldLoadTexture_LogButtonBorder()
        {
            string target = "LogButtonBorder";
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, target).Returns(mockTexture);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockDeviceTheme.GetFullAssetName(target);
                    _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, target);
                    _mockEngine.AssetBank.Put<ITexture2D>(target, mockTexture);
                }
            );
        }

        #region Attempt Log Textures
        [TestMethod]
        public void InitializeShouldLoadTexture_HistoryChoiceIndex()
        {
            string target = "HistoryIndexChoice";
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, target).Returns(mockTexture);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockDeviceTheme.GetFullAssetName(target);
                    _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, target);
                    _mockEngine.AssetBank.Put<ITexture2D>(target, mockTexture);
                }
            );
        }

        [TestMethod]
        public void InitializeShouldLoadTexture_HistoryAttemptFrameBackground()
        {
            string target = "HistoryAttemptFrameBackground";
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, target).Returns(mockTexture);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockDeviceTheme.GetFullAssetName(target);
                    _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, target);
                    _mockEngine.AssetBank.Put<ITexture2D>(target, mockTexture);
                }
            );
        }

        [TestMethod]
        public void InitializeShouldLoadTexture_HistoryAttemptFrame()
        {
            string target = "HistoryAttemptFrame";
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, target).Returns(mockTexture);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockDeviceTheme.GetFullAssetName(target);
                    _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, target);
                    _mockEngine.AssetBank.Put<ITexture2D>(target, mockTexture);
                }
            );
        }

        [TestMethod]
        public void InitializeShouldLoadTexture_HistoryPartial()
        {
            string target = "HistoryPartial";
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, target).Returns(mockTexture);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockDeviceTheme.GetFullAssetName(target);
                    _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, target);
                    _mockEngine.AssetBank.Put<ITexture2D>(target, mockTexture);
                }
            );
        }

        [TestMethod]
        public void InitializeShouldLoadTexture_HistoryEqual()
        {
            string target = "HistoryEqual";
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, target).Returns(mockTexture);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockDeviceTheme.GetFullAssetName(target);
                    _mockEngine.GrfxFactory.Texture2DFactory.Create(_mockEngine.Content, target);
                    _mockEngine.AssetBank.Put<ITexture2D>(target, mockTexture);
                }
            );
        }
        #endregion
        #endregion

        #region Asset Load Tests - Fonts
        [TestMethod]
        public void InitializeShouldLoadFont_TitleFont()
        {
            string target = "TitleFont";
            ISpriteFont mockFont = Substitute.For<ISpriteFont>();
            _mockEngine.GrfxFactory.SpriteFontFactory.Create(_mockEngine.Content, target).Returns(mockFont);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockDeviceTheme.GetFullAssetName(target);
                    _mockEngine.GrfxFactory.SpriteFontFactory.Create(_mockEngine.Content, target);
                    _mockEngine.AssetBank.Put<ISpriteFont>(target, mockFont);
                }
            );
        }

        [TestMethod]
        public void InitializeShouldLoadFont_SceneChangeButtonFont()
        {
            string target = "SceneChangeButtonFont";
            ISpriteFont mockFont = Substitute.For<ISpriteFont>();
            _mockEngine.GrfxFactory.SpriteFontFactory.Create(_mockEngine.Content, target).Returns(mockFont);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockDeviceTheme.GetFullAssetName(target);
                    _mockEngine.GrfxFactory.SpriteFontFactory.Create(_mockEngine.Content, target);
                    _mockEngine.AssetBank.Put<ISpriteFont>(target, mockFont);
                }
            );
        }

        [TestMethod]
        public void InitializeShouldLoadFont_HistoryAttemptIndexFont()
        {
            string target = "HistoryAttemptIndexFont";
            ISpriteFont mockFont = Substitute.For<ISpriteFont>();
            _mockEngine.GrfxFactory.SpriteFontFactory.Create(_mockEngine.Content, target).Returns(mockFont);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockDeviceTheme.GetFullAssetName(target);
                    _mockEngine.GrfxFactory.SpriteFontFactory.Create(_mockEngine.Content, target);
                    _mockEngine.AssetBank.Put<ISpriteFont>(target, mockFont);
                }
            );
        }
        #endregion

        #region Asset Load Tests - Music
        [TestMethod]
        public void InitializeShouldLoadMusic_Gameplay()
        {
            string target = "Gameplay";
            string assetPath = "Music\\" + target;
            ISong mockSong = Substitute.For<ISong>();
            _mockEngine.AudioManager.MusicController.CreateSong(_mockEngine.Content, assetPath).Returns(mockSong);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockEngine.AudioManager.MusicController.CreateSong(_mockEngine.Content, assetPath);
                    _mockEngine.AssetBank.Put<ISong>(target, mockSong);
                }
            );
        }
        #endregion

        #region Asset Load Tests - Sounds
        [TestMethod]
        public void InitializeShouldLoadSounds_IndexValueChanged()
        {
            string target = "IndexValueChanged";
            ISoundResource mockResource = Substitute.For<ISoundResource>();
            _mockEngine.AudioManager.SoundController.CreateSoundResource(_mockEngine.Content, target).Returns(mockResource);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockDeviceTheme.GetFullAssetName(target);
                    _mockEngine.AudioManager.SoundController.CreateSoundResource(_mockEngine.Content, target);
                    _mockEngine.AssetBank.Put<ISoundResource>(target, mockResource);
                }
            );
        }

        [TestMethod]
        public void InitializeShouldLoadSounds_ButtonPress()
        {
            string target = "ButtonPress";
            ISoundResource mockResource = Substitute.For<ISoundResource>();
            _mockEngine.AudioManager.SoundController.CreateSoundResource(_mockEngine.Content, target).Returns(mockResource);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockDeviceTheme.GetFullAssetName(target);
                    _mockEngine.AudioManager.SoundController.CreateSoundResource(_mockEngine.Content, target);
                    _mockEngine.AssetBank.Put<ISoundResource>(target, mockResource);
                }
            );
        }

        [TestMethod]
        public void InitializeShouldLoadSounds_SubmitSequence()
        {
            string target = "SubmitSequence";
            ISoundResource mockResource = Substitute.For<ISoundResource>();
            _mockEngine.AudioManager.SoundController.CreateSoundResource(_mockEngine.Content, target).Returns(mockResource);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockDeviceTheme.GetFullAssetName(target);
                    _mockEngine.AudioManager.SoundController.CreateSoundResource(_mockEngine.Content, target);
                    _mockEngine.AssetBank.Put<ISoundResource>(target, mockResource);
                }
            );
        }
        #endregion
    }
}
