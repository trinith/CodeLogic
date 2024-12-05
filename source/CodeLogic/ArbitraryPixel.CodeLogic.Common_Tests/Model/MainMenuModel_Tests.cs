using ArbitraryPixel.CodeLogic.Common;
using ArbitraryPixel.CodeLogic.Common.Config;
using ArbitraryPixel.CodeLogic.Common.Layers;
using ArbitraryPixel.CodeLogic.Common.Model;
using ArbitraryPixel.CodeLogic.Common.UI;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Engine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;

namespace ArbitraryPixel.CodeLogic.Common_Tests.Model
{
    [TestClass]
    public class MainMenuModel_Tests
    {
        private MainMenuModel _sut;
        private IMenuFactory _mockMenuFactory;
        private IEngine _mockEngine;
        private IMenuContentMap _mockContentMap;
        private IMainMenuContentLayerFactory _mockContentFactory;
        private ITargetPlatform _mockPlatform;

        private IMenuItem _mockRoot;

        private ISpriteBatch _mockSpriteBatch;
        private RectangleF _contentBounds = new RectangleF(200, 100, 400, 300);

        [TestInitialize]
        public void Initialize()
        {
            _mockEngine = Substitute.For<IEngine>();
            _mockMenuFactory = Substitute.For<IMenuFactory>();
            _mockContentMap = Substitute.For<IMenuContentMap>();
            _mockContentFactory = Substitute.For<IMainMenuContentLayerFactory>();

            _mockPlatform = Substitute.For<ITargetPlatform>();
            _mockEngine.GetComponent<ITargetPlatform>().Returns(_mockPlatform);

            _mockSpriteBatch = Substitute.For<ISpriteBatch>();

            _mockMenuFactory.CreateMenuItem("CodeLogic", CodeLogicEngine.Constants.MenuItemHeight).Returns(_mockRoot = Substitute.For<IMenuItem>());
        }

        private void Construct()
        {
            _sut = new MainMenuModel(_mockEngine, _mockMenuFactory, _mockContentMap, _mockContentFactory);
        }

        #region Constructor Parameter Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowException_Host()
        {
            _sut = new MainMenuModel(null, _mockMenuFactory, _mockContentMap, _mockContentFactory);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowException_MenuFactory()
        {
            _sut = new MainMenuModel(_mockEngine, null, _mockContentMap, _mockContentFactory);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowException_ContentMap()
        {
            _sut = new MainMenuModel(_mockEngine, _mockMenuFactory, null, _mockContentFactory);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowException_ContentFactory()
        {
            _sut = new MainMenuModel(_mockEngine, _mockMenuFactory, _mockContentMap, null);
        }
        #endregion

        #region Constructor Tests
        [TestMethod]
        public void ConstructShouldCallFactoryCreateMenuItem()
        {
            Construct();

            _mockMenuFactory.Received(1).CreateMenuItem("CodeLogic", CodeLogicEngine.Constants.MenuItemHeight);
        }

        [TestMethod]
        public void ConstructShouldSetRootMenuToExpectedValue()
        {
            Construct();

            Assert.AreSame(_mockRoot, _sut.RootMenu);
        }

        [TestMethod]
        public void ConstructShouldCreateSinglePlayerMenu()
        {
            Construct();

            _mockRoot.Received(1).CreateChild("Operation", CodeLogicEngine.Constants.MenuItemHeight);
        }

        [TestMethod]
        public void ConstructShouldSetSinglePlayerMenuToExpectedValue()
        {
            IMenuItem mockMenu = Substitute.For<IMenuItem>();
            _mockRoot.CreateChild("Operation", CodeLogicEngine.Constants.MenuItemHeight).Returns(mockMenu);

            Construct();

            Assert.AreSame(mockMenu, _sut.SinglePlayerMenu);
        }

        [TestMethod]
        public void ConstructShouldCreateExitMenuOnWindowsPlatform()
        {
            _mockPlatform.Platform.Returns(Platform.Windows);

            Construct();

            _mockRoot.Received(1).CreateChild("Exit", CodeLogicEngine.Constants.MenuItemHeight);
        }

        [TestMethod]
        public void ConstructShouldSetExitMenuToExpectedValueOnWindowsPlatform()
        {
            IMenuItem mockMenu = Substitute.For<IMenuItem>();
            _mockPlatform.Platform.Returns(Platform.Windows);
            _mockRoot.CreateChild("Exit", CodeLogicEngine.Constants.MenuItemHeight).Returns(mockMenu);

            Construct();

            Assert.AreSame(mockMenu, _sut.ExitMenu);
        }

        [TestMethod]
        public void ConstructShouldNotCreateExitMenuOnAndroidPlatform()
        {
            _mockPlatform.Platform.Returns(Platform.Android);

            Construct();

            _mockRoot.Received(0).CreateChild("Exit", CodeLogicEngine.Constants.MenuItemHeight);
        }

        [TestMethod]
        public void ConstructShouldSetExitMenuToNullOnAndroidPlatform()
        {
            _mockPlatform.Platform.Returns(Platform.Android);

            Construct();

            Assert.IsNull(_sut.ExitMenu);
        }

        [TestMethod]
        public void ConstructShouldCreateSettingsMenu()
        {
            Construct();

            _mockRoot.Received(1).CreateChild("Settings", CodeLogicEngine.Constants.MenuItemHeight);
        }

        [TestMethod]
        public void ConstructShouldSetSettingsMenuToExpectedValue()
        {
            IMenuItem mockSettings = Substitute.For<IMenuItem>();
            _mockRoot.CreateChild("Settings", CodeLogicEngine.Constants.MenuItemHeight).Returns(mockSettings);

            Construct();

            Assert.AreSame(mockSettings, _sut.SettingsMenu);
        }

        [TestMethod]
        public void ConstructShouldCreateCreditsMenu()
        {
            Construct();

            _mockRoot.Received(1).CreateChild("Credits", CodeLogicEngine.Constants.MenuItemHeight);
        }

        [TestMethod]
        public void ConstructShouldSetCreditsMenuToExpectedValue()
        {
            IMenuItem mockCredits = Substitute.For<IMenuItem>();
            _mockRoot.CreateChild("Credits", CodeLogicEngine.Constants.MenuItemHeight).Returns(mockCredits);

            Construct();

            Assert.AreEqual(mockCredits, _sut.CreditsMenu);
        }

        [TestMethod]
        public void ConstructShouldCreateBriefingMenu()
        {
            Construct();

            _mockRoot.Received(1).CreateChild("Briefing", CodeLogicEngine.Constants.MenuItemHeight);
        }

        [TestMethod]
        public void ConstructShouldSetBriefingMenuToExpectedValue()
        {
            IMenuItem mockBriefing = Substitute.For<IMenuItem>();
            _mockRoot.CreateChild("Briefing", CodeLogicEngine.Constants.MenuItemHeight).Returns(mockBriefing);

            Construct();

            Assert.AreSame(mockBriefing, _sut.BriefingMenu);
        }

        [TestMethod]
        public void ConstructShouldCreateStatisticsMenu()
        {
            Construct();

            _mockRoot.Received(1).CreateChild("Statistics", CodeLogicEngine.Constants.MenuItemHeight);
        }

        [TestMethod]
        public void ConstructShouldSetStatisticsMenuToExpectedValue()
        {
            IMenuItem mockStats = Substitute.For<IMenuItem>();
            _mockRoot.CreateChild("Statistics", CodeLogicEngine.Constants.MenuItemHeight).Returns(mockStats);

            Construct();

            Assert.AreSame(mockStats, _sut.StatsMenu);
        }
        #endregion

        #region BuildContentMap Tests
        [TestMethod]
        public void BuildContentMapShouldCallContentMapClear()
        {
            Construct();

            _sut.BuildContentMap(_mockEngine, _mockSpriteBatch, _contentBounds);

            _mockContentMap.Received(1).Clear();
        }

        [TestMethod]
        public void BuildContentMapShouldMapContentLayer_SinglePlayer()
        {
            IMenuItem mockItem = Substitute.For<IMenuItem>();
            _mockRoot.CreateChild("Operation", CodeLogicEngine.Constants.MenuItemHeight).Returns(mockItem);

            Construct();

            IMenuSinglePlayerContentLayer mockContent = Substitute.For<IMenuSinglePlayerContentLayer>();
            _mockContentFactory.CreateMenuSinglePlayerContentLayer(Arg.Any<IEngine>(), Arg.Any<ISpriteBatch>(), Arg.Any<RectangleF>()).Returns(mockContent);

            _sut.BuildContentMap(_mockEngine, _mockSpriteBatch, _contentBounds);

            Received.InOrder(
                () =>
                {
                    _mockContentFactory.CreateMenuSinglePlayerContentLayer(_mockEngine, _mockSpriteBatch, _contentBounds);
                    _mockContentMap.RegisterContentLayer(mockItem, mockContent);
                    mockContent.StartButtonTapped += Arg.Any<EventHandler<EventArgs>>();
                }
            );
        }

        public void BuildContentMapShouldMapContentLayer_Exit()
        {
            IMenuItem mockItem = Substitute.For<IMenuItem>();
            _mockRoot.CreateChild("Exit", CodeLogicEngine.Constants.MenuItemHeight).Returns(mockItem);

            Construct();

            IMenuExitContentLayer mockContent = Substitute.For<IMenuExitContentLayer>();
            _mockContentFactory.CreateMenuExitContentLayer(Arg.Any<IEngine>(), Arg.Any<ISpriteBatch>(), Arg.Any<RectangleF>()).Returns(mockContent);

            _sut.BuildContentMap(_mockEngine, _mockSpriteBatch, _contentBounds);

            Received.InOrder(
                () =>
                {
                    _mockContentFactory.CreateMenuExitContentLayer(_mockEngine, _mockSpriteBatch, _contentBounds);
                    _mockContentMap.RegisterContentLayer(mockItem, mockContent);
                    mockContent.ExitButtonTapped += Arg.Any<EventHandler<EventArgs>>();
                }
            );
        }

        [TestMethod]
        public void BuildContentMapWithNoExitMenuShouldNotCreateExitContentLayer()
        {
            IMenuItem mockItem = Substitute.For<IMenuItem>();
            _mockRoot.CreateChild("Exit", CodeLogicEngine.Constants.MenuItemHeight).Returns(mockItem);
            _mockPlatform.Platform.Returns(Platform.Android);
            Construct();

            _sut.BuildContentMap(_mockEngine, _mockSpriteBatch, _contentBounds);

            _mockContentFactory.DidNotReceive().CreateMenuExitContentLayer(Arg.Any<IEngine>(), Arg.Any<ISpriteBatch>(), Arg.Any<RectangleF>());
        }

        [TestMethod]
        public void BuildContentMapShouldMapContentLayer_Settings()
        {
            IMenuItem mockItem = Substitute.For<IMenuItem>();
            _mockRoot.CreateChild("Settings", CodeLogicEngine.Constants.MenuItemHeight).Returns(mockItem);

            Construct();

            IMenuSettingsContentLayer mockContent = Substitute.For<IMenuSettingsContentLayer>();
            _mockContentFactory.CreateMenuSettingsContentLayer(Arg.Any<IEngine>(), Arg.Any<ISpriteBatch>(), Arg.Any<RectangleF>()).Returns(mockContent);

            _sut.BuildContentMap(_mockEngine, _mockSpriteBatch, _contentBounds);

            Received.InOrder(
                () =>
                {
                    _mockContentFactory.CreateMenuSettingsContentLayer(_mockEngine, _mockSpriteBatch, _contentBounds);
                    _mockContentMap.RegisterContentLayer(mockItem, mockContent);
                }
            );
        }

        [TestMethod]
        public void BuildContentMapShouldCreateContentLayer_Credits()
        {
            IMenuItem mockItem = Substitute.For<IMenuItem>();
            _mockRoot.CreateChild("Credits", CodeLogicEngine.Constants.MenuItemHeight).Returns(mockItem);

            Construct();

            IMenuCreditsContentLayer mockContent = Substitute.For<IMenuCreditsContentLayer>();
            _mockContentFactory.CreateMenuCreditsContentLayer(Arg.Any<IEngine>(), Arg.Any<ISpriteBatch>(), Arg.Any<RectangleF>()).Returns(mockContent);

            _sut.BuildContentMap(_mockEngine, _mockSpriteBatch, _contentBounds);

            Received.InOrder(
                () =>
                {
                    _mockContentFactory.CreateMenuCreditsContentLayer(_mockEngine, _mockSpriteBatch, _contentBounds);
                    _mockContentMap.RegisterContentLayer(mockItem, mockContent);
                }
            );
        }

        [TestMethod]
        public void BuildContentMapShouldCreateContentLayer_Briefing()
        {
            IMenuItem mockItem = Substitute.For<IMenuItem>();
            _mockRoot.CreateChild("Briefing", CodeLogicEngine.Constants.MenuItemHeight).Returns(mockItem);

            Construct();

            IMenuCreditsContentLayer mockContent = Substitute.For<IMenuCreditsContentLayer>();
            _mockContentFactory.CreateMenuBriefingContentLayer(Arg.Any<IEngine>(), Arg.Any<ISpriteBatch>(), Arg.Any<RectangleF>()).Returns(mockContent);

            _sut.BuildContentMap(_mockEngine, _mockSpriteBatch, _contentBounds);

            Received.InOrder(
                () =>
                {
                    _mockContentFactory.CreateMenuBriefingContentLayer(_mockEngine, _mockSpriteBatch, _contentBounds);
                    _mockContentMap.RegisterContentLayer(mockItem, mockContent);
                }
            );
        }

        [TestMethod]
        public void BuildContentMapShouldCreateContentLayer_Statistics()
        {
            IMenuItem mockItem = Substitute.For<IMenuItem>();
            _mockRoot.CreateChild("Statistics", CodeLogicEngine.Constants.MenuItemHeight).Returns(mockItem);

            IGameStatsData mockGameStatsData = Substitute.For<IGameStatsData>();
            _mockEngine.GetComponent<IGameStatsData>().Returns(mockGameStatsData);

            Construct();

            IMenuStatsContentLayer mockContent = Substitute.For<IMenuStatsContentLayer>();
            _mockContentFactory.CreateMenuStatsContentLayer(Arg.Any<IEngine>(), Arg.Any<ISpriteBatch>(), Arg.Any<RectangleF>(), Arg.Any<IGameStatsData>()).Returns(mockContent);

            _sut.BuildContentMap(_mockEngine, _mockSpriteBatch, _contentBounds);

            Received.InOrder(
                () =>
                {
                    _mockContentFactory.CreateMenuStatsContentLayer(_mockEngine, _mockSpriteBatch, _contentBounds, mockGameStatsData);
                    _mockContentMap.RegisterContentLayer(mockItem, mockContent);
                    mockContent.ResetButtonTapped += Arg.Any<EventHandler<EventArgs>>();
                }
            );
        }
        #endregion

        #region Event Tests
        [TestMethod]
        public void SinglePlayerContentLayerStartButtonTappedShouldRaiseStartSinglePlayerGameEvent()
        {
            Construct();
            EventHandler<EventArgs> mockSubscriber = Substitute.For<EventHandler<EventArgs>>();
            _sut.StartSinglePlayerGame += mockSubscriber;
            IMenuSinglePlayerContentLayer mockContent = Substitute.For<IMenuSinglePlayerContentLayer>();
            _mockContentFactory.CreateMenuSinglePlayerContentLayer(Arg.Any<IEngine>(), Arg.Any<ISpriteBatch>(), Arg.Any<RectangleF>()).Returns(mockContent);
            _sut.BuildContentMap(_mockEngine, _mockSpriteBatch, _contentBounds);

            mockContent.StartButtonTapped += Raise.Event<EventHandler<EventArgs>>(mockContent, new EventArgs());

            mockSubscriber.Received(1)(_sut, Arg.Any<EventArgs>());
        }

        [TestMethod]
        public void ExitContentLayerExitButtonTappedShouldRaiseExitGameEvent()
        {
            Construct();
            EventHandler<EventArgs> mockSubscriber = Substitute.For<EventHandler<EventArgs>>();
            _sut.ExitGame += mockSubscriber;
            IMenuExitContentLayer mockContent = Substitute.For<IMenuExitContentLayer>();
            _mockContentFactory.CreateMenuExitContentLayer(Arg.Any<IEngine>(), Arg.Any<ISpriteBatch>(), Arg.Any<RectangleF>()).Returns(mockContent);
            _sut.BuildContentMap(_mockEngine, _mockSpriteBatch, _contentBounds);

            mockContent.ExitButtonTapped += Raise.Event<EventHandler<EventArgs>>(mockContent, new EventArgs());

            mockSubscriber.Received(1)(_sut, Arg.Any<EventArgs>());
        }

        [TestMethod]
        public void StatisticsContentLayerResetButtonTappedShouldRaiseResetStatisticsEvent()
        {
            Construct();
            var mockHandler = Substitute.For<EventHandler<EventArgs>>();
            _sut.ResetStatistics += mockHandler;
            IMenuStatsContentLayer mockContent = Substitute.For<IMenuStatsContentLayer>();
            _mockContentFactory.CreateMenuStatsContentLayer(Arg.Any<IEngine>(), Arg.Any<ISpriteBatch>(), Arg.Any<RectangleF>(), Arg.Any<IGameStatsData>()).Returns(mockContent);
            _sut.BuildContentMap(_mockEngine, _mockSpriteBatch, _contentBounds);

            mockContent.ResetButtonTapped += Raise.Event<EventHandler<EventArgs>>(mockContent, new EventArgs());

            mockHandler.Received(1)(_sut, Arg.Any<EventArgs>());
        }
        #endregion
    }
}
