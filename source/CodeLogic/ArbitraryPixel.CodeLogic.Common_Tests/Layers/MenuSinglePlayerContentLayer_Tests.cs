using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArbitraryPixel.CodeLogic.Common.Layers;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Engine;
using NSubstitute;
using ArbitraryPixel.CodeLogic.Common;
using Microsoft.Xna.Framework;
using ArbitraryPixel.Platform2D.UI;
using ArbitraryPixel.CodeLogic.Common.Entities;
using System.Linq;
using ArbitraryPixel.Common.Audio;

namespace ArbitraryPixel.CodeLogic.Common_Tests.Layers
{
    [TestClass]
    public class MenuSinglePlayerContentLayer_Tests
    {
        private MenuSinglePlayerContentLayer _sut;
        private RectangleF _contentBounds;
        private ISpriteBatch _mockSpriteBatch;
        private IEngine _mockEngine;
        private GameObjectFactory _mockGameObjectFactory;

        [TestInitialize]
        public void Initialize()
        {
            _mockEngine = Substitute.For<IEngine>();
            _mockSpriteBatch = Substitute.For<ISpriteBatch>();
            _contentBounds = new RectangleF(200, 100, 400, 300);

            _mockGameObjectFactory = Substitute.For<GameObjectFactory>();
            GameObjectFactory.SetInstance(_mockGameObjectFactory);
        }

        private void Construct()
        {
            _sut = new MenuSinglePlayerContentLayer(_mockEngine, _mockSpriteBatch, _contentBounds);
        }

        #region Constructor Tests
        [TestMethod]
        public void ConstructShouldRequestContentFontFromAssetBank()
        {
            Construct();

            _mockEngine.AssetBank.Received(1).Get<ISpriteFont>("MainMenuContentFont");
        }

        [TestMethod]
        public void ConstructShouldCreateLabel()
        {
            ISpriteFont mockFont = Substitute.For<ISpriteFont>();
            _mockEngine.AssetBank.Get<ISpriteFont>("MainMenuContentFont").Returns(mockFont);

            Construct();

            _mockGameObjectFactory.Received(1).CreateGenericTextLabel(_mockEngine, new Vector2(210, 110), _mockSpriteBatch, mockFont, Arg.Any<string>(), Color.White);
        }

        [TestMethod]
        public void ConstructShouldAddTextLabelToEntities()
        {
            ITextLabel mockTextLabel = Substitute.For<ITextLabel>();
            _mockGameObjectFactory.CreateGenericTextLabel(Arg.Any<IEngine>(), Arg.Any<Vector2>(), Arg.Any<ISpriteBatch>(), Arg.Any<ISpriteFont>(), Arg.Any<string>(), Arg.Any<Color>()).Returns(mockTextLabel);

            Construct();

            Assert.IsTrue(_sut.Entities.Contains(mockTextLabel));
        }

        [TestMethod]
        public void ConstructShouldCreateStartButton()
        {
            ISpriteFont mockFont = Substitute.For<ISpriteFont>();
            _mockEngine.AssetBank.Get<ISpriteFont>("MainButtonFont").Returns(mockFont);

            Construct();

            _mockGameObjectFactory.Received(1).CreateSimpleButton(_mockEngine, new RectangleF(_contentBounds.Right - 10f - 200f, _contentBounds.Bottom - 10f - 75f, 200f, 75f), _mockSpriteBatch, mockFont);
        }

        [TestMethod]
        public void ConstructShouldSetStartButtonText()
        {
            ISimpleButton mockButton = Substitute.For<ISimpleButton>();
            _mockGameObjectFactory.CreateSimpleButton(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ISpriteFont>()).Returns(mockButton);

            Construct();

            mockButton.Received(1).Text = "Start";
        }

        [TestMethod]
        public void ConstructShouldAttachToStartButtonTappedEvent()
        {
            ISimpleButton mockButton = Substitute.For<ISimpleButton>();
            _mockGameObjectFactory.CreateSimpleButton(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ISpriteFont>()).Returns(mockButton);

            Construct();

            mockButton.Received(1).Tapped += Arg.Any<EventHandler<ButtonEventArgs>>();
        }

        [TestMethod]
        public void ConstructShouldAddStartButtonToEntitiesList()
        {
            ISimpleButton mockButton = Substitute.For<ISimpleButton>();
            _mockGameObjectFactory.CreateSimpleButton(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ISpriteFont>()).Returns(mockButton);

            Construct();

            Assert.IsTrue(_sut.Entities.Contains(mockButton));
        }
        #endregion

        #region Button Tapped Event Tests
        [TestMethod]
        public void StartButtonTappedShouldTriggerStartButtonTappedEvent()
        {
            ISimpleButton mockButton = Substitute.For<ISimpleButton>();
            _mockGameObjectFactory.CreateSimpleButton(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ISpriteFont>()).Returns(mockButton);
            Construct();
            var subscriber = Substitute.For<EventHandler<EventArgs>>();
            _sut.StartButtonTapped += subscriber;

            mockButton.Tapped += Raise.Event<EventHandler<ButtonEventArgs>>(mockButton, new ButtonEventArgs(Vector2.Zero));

            subscriber.Received(1)(_sut, Arg.Any<EventArgs>());
        }

        [TestMethod]
        public void StartButtonTappedShouldPlayExpectedSound()
        {
            ISoundResource mockSound = Substitute.For<ISoundResource>();
            _mockEngine.AssetBank.Get<ISoundResource>("ButtonPress").Returns(mockSound);

            ISimpleButton mockButton = Substitute.For<ISimpleButton>();
            _mockGameObjectFactory.CreateSimpleButton(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ISpriteFont>()).Returns(mockButton);
            Construct();

            mockButton.Tapped += Raise.Event<EventHandler<ButtonEventArgs>>(mockButton, new ButtonEventArgs(Vector2.Zero));

            mockSound.Received(1).Play();
        }
        #endregion
    }
}
