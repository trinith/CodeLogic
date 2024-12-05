using ArbitraryPixel.CodeLogic.Common;
using ArbitraryPixel.CodeLogic.Common.Entities;
using ArbitraryPixel.CodeLogic.Common.Layers;
using ArbitraryPixel.Common.Audio;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.UI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using NSubstitute;
using System;
using System.Linq;

namespace ArbitraryPixel.CodeLogic.Common_Tests.Layers
{
    [TestClass]
    public class MenuExitContentLayer_Tests
    {
        private MenuExitContentLayer _sut;
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
            _sut = new MenuExitContentLayer(_mockEngine, _mockSpriteBatch, _contentBounds);
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
        public void ConstructShouldCreateSimpleButton()
        {
            ISpriteFont mockFont = Substitute.For<ISpriteFont>();
            _mockEngine.AssetBank.Get<ISpriteFont>("MainButtonFont").Returns(mockFont);

            Construct();

            _mockGameObjectFactory.Received(1).CreateSimpleButton(_mockEngine, new RectangleF(new Vector2(_contentBounds.Right - 10f - 200, _contentBounds.Bottom - 10f - 75), new SizeF(200, 75)), _mockSpriteBatch, mockFont);
        }

        [TestMethod]
        public void ConstructShouldSetExitButtonText()
        {
            ISimpleButton mockButton = Substitute.For<ISimpleButton>();
            _mockGameObjectFactory.CreateSimpleButton(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ISpriteFont>()).Returns(mockButton);

            Construct();

            mockButton.Received(1).Text = "Exit";
        }

        [TestMethod]
        public void ConstructShouldAddEventHandlerToTapped()
        {
            ISimpleButton mockButton = Substitute.For<ISimpleButton>();
            _mockGameObjectFactory.CreateSimpleButton(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ISpriteFont>()).Returns(mockButton);

            Construct();

            mockButton.Received(1).Tapped += Arg.Any<EventHandler<ButtonEventArgs>>();
        }
        #endregion

        #region Event Tests
        [TestMethod]
        public void ExitButtonTappedSHouldRaiseExitButtonTappedEvent()
        {
            EventHandler<EventArgs> mockSubscriber = Substitute.For<EventHandler<EventArgs>>();
            ISimpleButton mockButton = Substitute.For<ISimpleButton>();
            _mockGameObjectFactory.CreateSimpleButton(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ISpriteFont>()).Returns(mockButton);
            Construct();
            _sut.ExitButtonTapped += mockSubscriber;

            mockButton.Tapped += Raise.Event<EventHandler<ButtonEventArgs>>(mockButton, new ButtonEventArgs(Vector2.Zero));

            mockSubscriber.Received(1)(_sut, Arg.Any<EventArgs>());
        }

        [TestMethod]
        public void ExitButtontappedShouldPlayExpectedSound()
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
