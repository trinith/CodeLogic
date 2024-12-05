using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArbitraryPixel.CodeLogic.Common.Scenes;
using ArbitraryPixel.Platform2D.Engine;
using NSubstitute;
using ArbitraryPixel.CodeLogic.Common.Layers;
using ArbitraryPixel.CodeLogic.Common;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Layer;
using Microsoft.Xna.Framework;
using ArbitraryPixel.CodeLogic.Common.Entities;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Platform2D.Text;
using System.Collections.Generic;
using ArbitraryPixel.Platform2D.UI;
using ArbitraryPixel.Platform2D.Scene;
using ArbitraryPixel.CodeLogic.Common.Model;
using System.Linq;

namespace ArbitraryPixel.CodeLogic.Common_Tests.Scenes
{
    [TestClass]
    public class NoAdMessageScene_Tests
    {
        private NoAdMessageScene _sut;
        private IEngine _mockEngine;

        private GameObjectFactory _mockGameObjectFactory;
        private ILayer _mockLayer;
        private IProgressLayer _mockProgressLayer;
        private ISimpleButton _mockButton;
        private ITextObjectBuilder _mockBuilder;
        private ISpriteBatch _mockSpriteBatch;
        private ITexture2D _mockTexture;
        private IScene _mockNextScene;
        private IScene _mockTransitionScene;

        [TestInitialize]
        public void Initialize()
        {
            _mockEngine = Substitute.For<IEngine>();

            _mockEngine.ScreenManager.World.Returns(new Point(1000, 1000));
            _mockEngine.GrfxFactory.SpriteBatchFactory.Create(Arg.Any<IGrfxDevice>()).Returns(_mockSpriteBatch = Substitute.For<ISpriteBatch>());

            _mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.AssetBank.Get<ITexture2D>("Pixel").Returns(_mockTexture);

            Dictionary<string, IScene> scenes = new Dictionary<string, IScene>();
            _mockEngine.Scenes.Returns(scenes);

            _mockNextScene = Substitute.For<IScene>();
            scenes.Add("DeviceBoot", _mockNextScene);

            _mockGameObjectFactory = Substitute.For<GameObjectFactory>();
            GameObjectFactory.SetInstance(_mockGameObjectFactory);

            _mockLayer = Substitute.For<ILayer>();
            _mockLayer.MainSpriteBatch.Returns(_mockSpriteBatch);
            _mockGameObjectFactory.CreateGenericLayer(Arg.Any<IEngine>(), Arg.Any<ISpriteBatch>()).Returns(_mockLayer);

            _mockProgressLayer = Substitute.For<IProgressLayer>();
            _mockGameObjectFactory.CreateProgressLayer(Arg.Any<IEngine>(), Arg.Any<ISpriteBatch>(), Arg.Any<ISpriteFont>(), Arg.Any<string>()).Returns(_mockProgressLayer);

            _mockButton = Substitute.For<ISimpleButton>();
            _mockGameObjectFactory.CreateSimpleButton(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ISpriteFont>()).Returns(_mockButton);

            _mockBuilder = Substitute.For<ITextObjectBuilder>();
            _mockGameObjectFactory.CreateTextObjectBuilder(Arg.Any<ITextFormatProcessor>(), Arg.Any<ITextObjectFactory>()).Returns(_mockBuilder);

            _mockTransitionScene = Substitute.For<IScene>();
            _mockGameObjectFactory.CreateFadeSceneTransition(Arg.Any<IEngine>(), Arg.Any<IScene>(), Arg.Any<IScene>(), Arg.Any<FadeSceneTransitionMode>(), Arg.Any<float>()).Returns(_mockTransitionScene);

            _sut = new NoAdMessageScene(_mockEngine);
        }

        #region Initialize Tests - Misc
        [TestMethod]
        public void InitializeShouldRequestScreenManagerWorld()
        {
            _sut.Initialize();

            var obj = (SizeF)_mockEngine.ScreenManager.Received(1).World;
        }
        #endregion

        #region Initialize Tests - Main Layer
        [TestMethod]
        public void InitializeShouldGetPixelTextureFromAssetBank()
        {
            _sut.Initialize();

            _mockEngine.AssetBank.Received(1).Get<ITexture2D>("Pixel");
        }

        [TestMethod]
        public void InitializeShouldCreateGenericLayer()
        {
            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockEngine.GrfxFactory.SpriteBatchFactory.Create(_mockEngine.Graphics.GraphicsDevice);
                    _mockGameObjectFactory.CreateGenericLayer(_mockEngine, _mockSpriteBatch);
                }
            );
        }

        [TestMethod]
        public void InitializeShouldAddMainLayerToEntities()
        {
            _sut.Initialize();

            Assert.IsTrue(_sut.Entities.Contains(_mockLayer));
        }
        #endregion

        #region Initialize Tests - Background Entity
        [TestMethod]
        public void InitializeShouldCreateTextureEntityForBackground()
        {
            ITextureEntity mockEntity = Substitute.For<ITextureEntity>();
            _mockGameObjectFactory.CreateTextureEntity(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ITexture2D>(), Arg.Any<Color>()).Returns(mockEntity);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockGameObjectFactory.CreateTextureEntity(
                        _mockEngine,
                        new RectangleF(0, 0, 1000, 1000),
                        _mockSpriteBatch,
                        _mockTexture,
                        Color.Black
                    );

                    _mockLayer.AddEntity(mockEntity);
                }
            );
        }
        #endregion

        #region Initialize Tests - Progress Bar Entity
        [TestMethod]
        public void InitializeShouldCreateProgressLayer()
        {
            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockGameObjectFactory.CreateProgressLayer(_mockEngine, _mockSpriteBatch, null, "");
                    _mockLayer.AddEntity(_mockProgressLayer);
                    _mockProgressLayer.Maximum = 15f;
                    _mockProgressLayer.Value = 15f;
                }
            );
        }
        #endregion

        #region Initialize Tests - Text
        [TestMethod]
        public void InitializeShouldCreateTextBuilder()
        {
            ISpriteFont mockFont = Substitute.For<ISpriteFont>();
            _mockEngine.AssetBank.Get<ISpriteFont>("AdLoadNormalFont").Returns(mockFont);
            _mockBuilder.GetRegisteredFont("Normal").Returns(mockFont);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockGameObjectFactory.CreateTextFormatValueHandlerManager();
                    _mockGameObjectFactory.CreateTextFormatProcessor(Arg.Any<ITextFormatValueHandlerManager>());
                    _mockGameObjectFactory.CreateTextObjectFactory();
                    _mockGameObjectFactory.CreateTextObjectBuilder(Arg.Any<ITextFormatProcessor>(), Arg.Any<ITextObjectFactory>());

                    _mockEngine.AssetBank.Get<ISpriteFont>("AdLoadNormalFont");
                    _mockBuilder.RegisterFont("Normal", mockFont);
                    _mockBuilder.GetRegisteredFont("Normal");
                    _mockBuilder.DefaultFont = mockFont;
                }
            );
        }

        [TestMethod]
        public void InitializeShouldCallBuildOnTextObjectBuilder()
        {
            _sut.Initialize();

            _mockBuilder.Received(1).Build(Arg.Any<string>(), new RectangleF(0, 0, 1000, 500f - 25f / 2f));
        }

        [TestMethod]
        public void InitializeShouldCreateLabelsForTextObjectsAtExpectedOffset()
        {
            ITextLabel mockLabelA, mockLabelB;
            _mockGameObjectFactory.CreateGenericTextLabel(Arg.Any<IEngine>(), Arg.Any<Vector2>(), Arg.Any<ISpriteBatch>(), Arg.Any<ISpriteFont>(), Arg.Any<string>(), Arg.Any<Color>()).Returns(
                mockLabelA = Substitute.For<ITextLabel>(),
                mockLabelB = Substitute.For<ITextLabel>(),
                Substitute.For<ITextLabel>()
            );

            List<ITextObject> mockObjects = new List<ITextObject>();
            _mockBuilder.Build(Arg.Any<string>(), Arg.Any<RectangleF>()).Returns(mockObjects);

            ISpriteFont mockFontA = Substitute.For<ISpriteFont>();
            ISpriteFont mockFontB = Substitute.For<ISpriteFont>();

            ITextObject objA = Substitute.For<ITextObject>();
            objA.Location.Returns(new Vector2(100, 100));
            objA.TextDefinition.Font.Returns(mockFontA);
            objA.TextDefinition.Text.Returns("ObjectA");
            objA.TextDefinition.Colour.Returns(Color.Pink);
            mockObjects.Add(objA);

            ITextObject objB = Substitute.For<ITextObject>();
            objB.Location.Returns(new Vector2(100, 300));
            objB.TextDefinition.Font.Returns(mockFontB);
            objB.TextDefinition.Text.Returns("ObjectB");
            objB.TextDefinition.Colour.Returns(Color.Purple);
            mockObjects.Add(objB);

            Vector2 expectedOffset = new Vector2(0, 143.75f);

            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockGameObjectFactory.CreateGenericTextLabel(
                        _mockEngine,
                        new Vector2(100, 100) + expectedOffset,
                        _mockSpriteBatch,
                        mockFontA,
                        "ObjectA",
                        Color.Pink
                    );
                    _mockLayer.AddEntity(mockLabelA);

                    _mockGameObjectFactory.CreateGenericTextLabel(
                        _mockEngine,
                        new Vector2(100, 300) + expectedOffset,
                        _mockSpriteBatch,
                        mockFontB,
                        "ObjectB",
                        Color.Purple
                    );
                    _mockLayer.AddEntity(mockLabelB);
                }
            );
        }
        #endregion

        #region Initialize Tests - Skip Button
        [TestMethod]
        public void InitializeShouldCreateButtonToSkipWait()
        {
            ISpriteFont mockFont = Substitute.For<ISpriteFont>();
            _mockEngine.AssetBank.Get<ISpriteFont>("MainMenuContentFont").Returns(mockFont);

            _mockProgressLayer.ProgressBarBounds.Returns(new RectangleF(0, 0, 400, 200));
            _sut.Initialize();

            Received.InOrder(
                () =>
                {
                    _mockEngine.AssetBank.Get<ISpriteFont>("MainMenuContentFont");
                    _mockGameObjectFactory.CreateSimpleButton(
                        _mockEngine,
                        new RectangleF(400f / 2f - 200f / 2f, 200 + (1000 - 200) / 2f, 200, 75),
                        _mockSpriteBatch,
                        mockFont
                    );
                    _mockButton.Text = "Skip";
                    _mockButton.Tapped += Arg.Any<EventHandler<ButtonEventArgs>>();
                    _mockLayer.AddEntity(_mockButton);
                }
            );
        }

        [TestMethod]
        public void SkipButtonTappedShouldSetProgressLayerValueToZero()
        {
            _sut.Initialize();

            _mockButton.Tapped += Raise.Event<EventHandler<ButtonEventArgs>>(_mockButton, new ButtonEventArgs(Vector2.Zero));

            _mockProgressLayer.Received(1).Value = 0;
        }
        #endregion

        #region Reset Tests
        [TestMethod]
        public void ResetShouldSetSceneCompleteFalse()
        {
            _sut.Initialize();
            _sut.SceneComplete = true;

            _sut.Reset();

            Assert.IsFalse(_sut.SceneComplete);
        }

        [TestMethod]
        public void ResetShouldSetProgressLayerValueToMaximum()
        {
            _sut.Initialize();
            _mockProgressLayer.Maximum.Returns(1234);

            _sut.Reset();

            _mockProgressLayer.Received(1).Value = 1234;
        }
        #endregion

        #region Update Tests
        [TestMethod]
        public void UpdateWithProgressLayerValueGreaterThanZeroShouldDecrementByElapsedSeconds()
        {
            _sut.Initialize();
            _mockProgressLayer.Value.Returns(100);

            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(5)));

            _mockProgressLayer.Received(1).Value = 95;
        }

        [TestMethod]
        public void UpdateWithProgressLayerDroppingBelowZeroShouldSetProgressLayerValueToZero()
        {
            _sut.Initialize();
            _mockProgressLayer.Value.Returns(3);

            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(5)));

            _mockProgressLayer.Received(1).Value = 0;
        }

        [TestMethod]
        public void UpdateWithProgressLayerDroppingBelowZeroShouldSetSceneCompleteTrue()
        {
            _sut.Initialize();
            _mockProgressLayer.Value.Returns(3);

            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(5)));

            Assert.IsTrue(_sut.SceneComplete);
        }

        [TestMethod]
        public void UpdateWithProgressLayerDroppingBelowZeroShouldCreateTransitionScene()
        {
            _sut.Initialize();
            _mockProgressLayer.Value.Returns(3);

            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(5)));

            _mockGameObjectFactory.Received(1).CreateFadeSceneTransition(_mockEngine, _sut, _mockNextScene, FadeSceneTransitionMode.In, CodeLogicEngine.Constants.FadeSceneTransitionTime);
        }

        [TestMethod]
        public void UpdateWithProgressLayerDroppingBelowZeroShouldSetNextScene()
        {
            _sut.Initialize();
            _mockProgressLayer.Value.Returns(3);

            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(5)));

            Assert.AreSame(_mockTransitionScene, _sut.NextScene);
        }
        #endregion
    }
}
