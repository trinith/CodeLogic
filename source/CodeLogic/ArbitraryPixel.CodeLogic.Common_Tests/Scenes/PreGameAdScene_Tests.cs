using ArbitraryPixel.CodeLogic.Common;
using ArbitraryPixel.CodeLogic.Common.Entities;
using ArbitraryPixel.CodeLogic.Common.Layers;
using ArbitraryPixel.CodeLogic.Common.Model;
using ArbitraryPixel.CodeLogic.Common.Scenes;
using ArbitraryPixel.Common.Advertising;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.Layer;
using ArbitraryPixel.Platform2D.Scene;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ArbitraryPixel.CodeLogic.Common_Tests.Scenes
{
    [TestClass]
    public class PreGameAdScene_Tests
    {
        private PreGameAdScene _sut;
        private IEngine _mockEngine;
        private IAdProvider _mockAdProvider;
        private GameObjectFactory _mockGameObjectFactory;

        private IScene _mockTransitionScene;
        private IScene _mockDeviceBootScene;
        private IScene _mockNoAdMessageScene;
        private IProgressLayer _mockProgressLayer;

        [TestInitialize]
        public void Initialize()
        {
            _mockEngine = Substitute.For<IEngine>();

            _mockAdProvider = Substitute.For<IAdProvider>();
            _mockEngine.GetComponent<IAdProvider>().Returns(_mockAdProvider);

            _mockGameObjectFactory = Substitute.For<GameObjectFactory>();
            GameObjectFactory.SetInstance(_mockGameObjectFactory);

            _mockProgressLayer = Substitute.For<IProgressLayer>();
            _mockGameObjectFactory.CreateProgressLayer(Arg.Any<IEngine>(), Arg.Any<ISpriteBatch>(), Arg.Any<ISpriteFont>(), Arg.Any<string>()).Returns(_mockProgressLayer);

            _mockDeviceBootScene = Substitute.For<IScene>();
            _mockNoAdMessageScene = Substitute.For<IScene>();
            Dictionary<string, IScene> scenes = new Dictionary<string, IScene>();
            _mockEngine.Scenes.Returns(scenes);

            scenes.Add("DeviceBoot", _mockDeviceBootScene);
            scenes.Add("NoAdMessage", _mockNoAdMessageScene);

            _mockTransitionScene = Substitute.For<IScene>();
            _mockGameObjectFactory.CreateFadeSceneTransition(Arg.Any<IEngine>(), Arg.Any<IScene>(), Arg.Any<IScene>(), Arg.Any<FadeSceneTransitionMode>(), Arg.Any<float>()).Returns(_mockTransitionScene);
        }

        private void Construct(bool useDefault = true)
        {
            if (useDefault == false)
                _mockEngine.GetComponent<IAdProvider>().Returns((IAdProvider)null);

            _sut = new PreGameAdScene(_mockEngine);
        }

        #region Constructor Tests
        [TestMethod]
        public void ConstructShouldAttachToAdProviderAdClosedEvent()
        {
            Construct();

            _mockAdProvider.Received(1).AdClosed += Arg.Any<EventHandler<EventArgs>>();
        }

        [TestMethod]
        public void ConstructWhenAdProviderNullShouldSucceed()
        {
            Construct(false);
        }
        #endregion

        #region Initialize Tests
        [TestMethod]
        public void InitializeShouldRequestPixelTexture()
        {
            Construct();

            _sut.Initialize();

            _mockEngine.AssetBank.Received(1).Get<ITexture2D>("Pixel");
        }

        [TestMethod]
        public void InitializeShouldCreateLayer()
        {
            Construct();

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockEngine.GrfxFactory.SpriteBatchFactory.Create(_mockEngine.Graphics.GraphicsDevice);
                    _mockGameObjectFactory.Received(1).CreateGenericLayer(_mockEngine, Arg.Any<ISpriteBatch>());
                }
            );
        }

        [TestMethod]
        public void InitializeShouldCreateAndAddTExtureEntityForBackground()
        {
            Construct();

            ITexture2D mockPixel = Substitute.For<ITexture2D>();
            _mockEngine.AssetBank.Get<ITexture2D>("Pixel").Returns(mockPixel);

            _mockEngine.ScreenManager.World.Returns(new Point(200, 100));

            ILayer mockLayer = Substitute.For<ILayer>();
            _mockGameObjectFactory.CreateGenericLayer(Arg.Any<IEngine>(), Arg.Any<ISpriteBatch>()).Returns(mockLayer);

            ITextureEntity mockEntity = Substitute.For<ITextureEntity>();
            _mockGameObjectFactory.CreateTextureEntity(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ITexture2D>(), Arg.Any<Color>()).Returns(mockEntity);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockGameObjectFactory.CreateTextureEntity(
                        _mockEngine,
                        new RectangleF(0, 0, 200, 100),
                        Arg.Any<ISpriteBatch>(),
                        mockPixel,
                        Color.Black
                    );

                    mockLayer.AddEntity(mockEntity);
                }
            );
        }

        [TestMethod]
        public void InitializeShouldCallAddEntityWithCreatedLayer()
        {
            Construct();

            ILayer mockLayer = Substitute.For<ILayer>();
            _mockGameObjectFactory.CreateGenericLayer(Arg.Any<IEngine>(), Arg.Any<ISpriteBatch>()).Returns(mockLayer);

            _sut.Initialize();

            Assert.IsTrue(_sut.Entities.Contains(mockLayer));
        }
        #endregion

        #region Reset Tests
        [TestMethod]
        public void ResetShouldSetSceneCompleteFalse()
        {
            Construct();
            _sut.Initialize();
            _sut.SceneComplete = true;

            _sut.Reset();

            Assert.IsFalse(_sut.SceneComplete);
        }

        [TestMethod]
        public void ResetShouldSetProgressLayerVariables()
        {
            Construct();
            _sut.Initialize();
            _mockProgressLayer.Maximum.Returns(32);

            _sut.Reset();

            Received.InOrder(
                () =>
                {
                    _mockProgressLayer.Value = 32;
                    _mockProgressLayer.Visible = false;
                }
            );
        }

        [TestMethod]
        public void ResetWithNoAdReadyShouldRequestAd()
        {
            Construct();
            _sut.Initialize();
            _mockAdProvider.AdReady.Returns(false);

            _sut.Reset();

            _mockAdProvider.Received(1).RequestAd();
        }

        [TestMethod]
        public void ResetWithNoAdReadyShouldSetProgressLayerVisible()
        {
            Construct();
            _sut.Initialize();
            _mockAdProvider.AdReady.Returns(false);

            _sut.Reset();

            _mockProgressLayer.Received(1).Visible = true;
        }

        [TestMethod]
        public void ResetWithAdProviderShouldSetProgressLayerValueToZero()
        {
            Construct();
            _sut.Initialize();
            _mockAdProvider.AdReady.Returns(true);

            _sut.Reset();

            _mockProgressLayer.Received(1).Value = 0;
        }
        #endregion

        #region Update Tests
        [TestMethod]
        public void UpdateWhenAdProviderNullShouldSetNextSceneToExpecetedValue()
        {
            Construct(false);
            _sut.Initialize();
            _sut.Reset();

            _sut.Update(new GameTime());

            Assert.AreSame(_mockTransitionScene, _sut.NextScene);
        }

        [TestMethod]
        public void UpdateWhenAdProviderNullShouldSetSceneCompleteTrue()
        {
            Construct(false);
            _sut.Initialize();
            _sut.Reset();

            _sut.Update(new GameTime());

            Assert.IsTrue(_sut.SceneComplete);
        }

        [TestMethod]
        public void UpdateWhenAdProviderNullShouldCreateMoveToNextScene()
        {
            Construct(false);
            _sut.Initialize();
            _sut.Reset();

            _sut.Update(new GameTime());

            Assert.IsTrue(_sut.SceneComplete);
            Assert.AreSame(_mockTransitionScene, _sut.NextScene);
        }

        [TestMethod]
        public void UpdateWhenProgressLayerVisibleAndValueNotZeroShoudDecrementProgressLayerValue()
        {
            Construct();
            _sut.Initialize();
            _mockProgressLayer.Visible.Returns(true);
            _mockProgressLayer.Value.Returns(5);
            _mockProgressLayer.ClearReceivedCalls();

            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(0.25)));

            _mockProgressLayer.Received(1).Value = 4.75f;
        }

        [TestMethod]
        public void UpdateWhenProgressLayerVisibleAndValueLTEZeroShouldSetValueToZero()
        {
            Construct();
            _sut.Initialize();
            _mockProgressLayer.Visible.Returns(true);
            _mockProgressLayer.Value.Returns(5);
            _mockProgressLayer.ClearReceivedCalls();

            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(5.1)));

            _mockProgressLayer.Received(1).Value = 0;
        }

        [TestMethod]
        public void UpdateWhenProgressReachesZeroShouldCallAdProviderShowAd()
        {
            Construct();
            _sut.Initialize();
            _mockProgressLayer.Visible.Returns(true);
            _mockProgressLayer.Value.Returns(-1);
            _mockAdProvider.AdReady.Returns(true);

            _sut.Update(new GameTime());

            _mockAdProvider.Received(1).ShowAd();
        }

        [TestMethod]
        public void UpdateWhenProgressReachesZeroShouldChangeScene()
        {
            Construct();
            _sut.Initialize();
            _mockProgressLayer.Visible.Returns(true);
            _mockProgressLayer.Value.Returns(-1);
            _mockAdProvider.AdReady.Returns(false);
            _sut.Update(new GameTime()); // Push skip ad to true

            _sut.Update(new GameTime()); // Should change scene

            Assert.IsTrue(_sut.SceneComplete);
            Assert.AreSame(_mockNoAdMessageScene, _sut.NextScene);
        }
        #endregion

        #region AdClosed Event Handler Tests
        [TestMethod]
        public void AdClosedEventWhenAdShownShouldCreateTransitionScene()
        {
            Construct();

            _mockAdProvider.AdClosed += Raise.Event<EventHandler<EventArgs>>(_mockAdProvider, new EventArgs());

            _mockGameObjectFactory.Received(1).CreateFadeSceneTransition(_mockEngine, _sut, _mockDeviceBootScene, FadeSceneTransitionMode.In, CodeLogicEngine.Constants.FadeSceneTransitionTime);
        }

        [TestMethod]
        public void AdClosedEventShouldChangeToExpectedScene()
        {
            Construct();

            _mockAdProvider.AdClosed += Raise.Event<EventHandler<EventArgs>>(_mockAdProvider, new EventArgs());

            Assert.AreSame(_mockTransitionScene, _sut.NextScene);
        }

        [TestMethod]
        public void AdClosedEventShouldSetSceneCompleteTrue()
        {
            Construct();

            _mockAdProvider.AdClosed += Raise.Event<EventHandler<EventArgs>>(_mockAdProvider, new EventArgs());

            Assert.IsTrue(_sut.SceneComplete);
        }
        #endregion

        #region AdLoaded Event Handler Tests
        [TestMethod]
        public void AdLoadedEventShouldSetProgressLayerValues()
        {
            Construct();
            _sut.Initialize();
            _mockProgressLayer.ClearReceivedCalls();

            _mockAdProvider.AdLoaded += Raise.Event<EventHandler<EventArgs>>(_mockAdProvider, new EventArgs());

            Received.InOrder(
                () =>
                {
                    _mockProgressLayer.Visible = false;
                    _mockProgressLayer.Value = 0;
                }
            );
        }
        #endregion
    }
}
