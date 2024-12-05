using System;
using System.Linq;
using ArbitraryPixel.CodeLogic.Common;
using ArbitraryPixel.CodeLogic.Common.Entities;
using ArbitraryPixel.CodeLogic.Common.Layers;
using ArbitraryPixel.Common.Audio;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.Entity;
using ArbitraryPixel.Platform2D.UI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using NSubstitute;

namespace ArbitraryPixel.CodeLogic.Common_Tests.Layers
{
    [TestClass]
    public class MenuBriefingUILayer_Tests
    {
        private MenuBriefingUILayer _sut;
        private IEngine _mockEngine;
        private ISpriteBatch _mockSpriteBatch;
        private IMenuBriefingContentLayer _mockHostLayer;

        private GameObjectFactory _mockGOF;

        private RectangleF _bounds = new RectangleF(200, 100, 400, 300);

        [TestInitialize]
        public void Initialize()
        {
            _mockEngine = Substitute.For<IEngine>();
            _mockSpriteBatch = Substitute.For<ISpriteBatch>();
            _mockHostLayer = Substitute.For<IMenuBriefingContentLayer>();

            _mockGOF = Substitute.For<GameObjectFactory>();
            GameObjectFactory.SetInstance(_mockGOF);
        }

        private void Construct()
        {
            _sut = new MenuBriefingUILayer(_mockEngine, _mockSpriteBatch, _bounds, _mockHostLayer);
        }

        #region Constructor Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowException_HostLayer()
        {
            _sut = new MenuBriefingUILayer(_mockEngine, _mockSpriteBatch, _bounds, null);
        }

        [TestMethod]
        public void ConstructShouldCreateNextButton()
        {
            ISpriteFont mockFont = Substitute.For<ISpriteFont>();
            _mockEngine.AssetBank.Get<ISpriteFont>("MainButtonFont").Returns(mockFont);

            SizeF buttonSize = CodeLogicEngine.Constants.MenuButtonSize * new SizeF(0.5f, 1);
            RectangleF expectedBounds = new RectangleF(
                new Vector2(
                    _bounds.Right - buttonSize.Width,
                    _bounds.Bottom - buttonSize.Height
                ),
                buttonSize
            );

            Construct();

            _mockGOF.Received(1).CreateSimpleButton(_mockEngine, expectedBounds, _mockSpriteBatch, mockFont);
        }

        [TestMethod]
        public void ConstructShouldSetNextButtonText()
        {
            ISimpleButton mockNextButton = Substitute.For<ISimpleButton>();
            _mockGOF.CreateSimpleButton(_mockEngine, Arg.Any<RectangleF>(), _mockSpriteBatch, Arg.Any<ISpriteFont>()).Returns(mockNextButton, Substitute.For<ISimpleButton>());

            Construct();

            mockNextButton.Received(1).Text = ">";
        }

        [TestMethod]
        public void ConstructShouldAttachToNextButtonTappedEvent()
        {
            ISimpleButton mockNextButton = Substitute.For<ISimpleButton>();
            _mockGOF.CreateSimpleButton(_mockEngine, Arg.Any<RectangleF>(), _mockSpriteBatch, Arg.Any<ISpriteFont>()).Returns(mockNextButton, Substitute.For<ISimpleButton>());

            Construct();

            mockNextButton.Received(1).Tapped += Arg.Any<EventHandler<ButtonEventArgs>>();
        }

        [TestMethod]
        public void ConstructShouldCreatePreviousButton()
        {
            ISpriteFont mockFont = Substitute.For<ISpriteFont>();
            _mockEngine.AssetBank.Get<ISpriteFont>("MainButtonFont").Returns(mockFont);

            SizeF buttonSize = CodeLogicEngine.Constants.MenuButtonSize * new SizeF(0.5f, 1);
            SizeF padding = CodeLogicEngine.Constants.TextWindowPadding;

            RectangleF expectedBounds = new RectangleF(
                new Vector2(
                    _bounds.Right - buttonSize.Width - padding.Width - buttonSize.Width,
                    _bounds.Bottom - buttonSize.Height
                ),
                buttonSize
            );

            Construct();

            _mockGOF.Received(1).CreateSimpleButton(_mockEngine, expectedBounds, _mockSpriteBatch, mockFont);
        }

        [TestMethod]
        public void ConstructShouldSetPreviousButtonText()
        {
            ISimpleButton mockNextButton = Substitute.For<ISimpleButton>();
            _mockGOF.CreateSimpleButton(_mockEngine, Arg.Any<RectangleF>(), _mockSpriteBatch, Arg.Any<ISpriteFont>()).Returns(Substitute.For<ISimpleButton>(), mockNextButton, Substitute.For<ISimpleButton>());

            Construct();

            mockNextButton.Received(1).Text = "<";
        }

        [TestMethod]
        public void ConstructShoudAttachToPreviousButtonTappedEvent()
        {
            ISimpleButton mockNextButton = Substitute.For<ISimpleButton>();
            _mockGOF.CreateSimpleButton(_mockEngine, Arg.Any<RectangleF>(), _mockSpriteBatch, Arg.Any<ISpriteFont>()).Returns(Substitute.For<ISimpleButton>(), mockNextButton, Substitute.For<ISimpleButton>());

            Construct();

            mockNextButton.Received(1).Tapped += Arg.Any<EventHandler<ButtonEventArgs>>();
        }

        [TestMethod]
        public void ConstructShouldCreateContentsButton()
        {
            ISpriteFont mockFont = Substitute.For<ISpriteFont>();
            _mockEngine.AssetBank.Get<ISpriteFont>("MainButtonFont").Returns(mockFont);

            SizeF buttonSize = CodeLogicEngine.Constants.MenuButtonSize * new SizeF(0.5f, 1);

            RectangleF expectedBounds = new RectangleF(
                new Vector2(
                    _bounds.Left,
                    _bounds.Bottom - buttonSize.Height
                ),
                buttonSize
            );

            Construct();

            _mockGOF.Received(1).CreateSimpleButton(_mockEngine, expectedBounds, _mockSpriteBatch, mockFont);
        }

        [TestMethod]
        public void ConstructShouldSetContentsButtonText()
        {
            ISimpleButton mockContentsButton = Substitute.For<ISimpleButton>();
            _mockGOF.CreateSimpleButton(_mockEngine, Arg.Any<RectangleF>(), _mockSpriteBatch, Arg.Any<ISpriteFont>()).Returns(Substitute.For<ISimpleButton>(), Substitute.For<ISimpleButton>(), mockContentsButton, Substitute.For<ISimpleButton>());

            Construct();

            mockContentsButton.Received(1).Text = "^";
        }

        [TestMethod]
        public void ConstructShouldAttachToContentsButtonTappedEvent()
        {
            ISimpleButton mockContentsButton = Substitute.For<ISimpleButton>();
            _mockGOF.CreateSimpleButton(_mockEngine, Arg.Any<RectangleF>(), _mockSpriteBatch, Arg.Any<ISpriteFont>()).Returns(Substitute.For<ISimpleButton>(), Substitute.For<ISimpleButton>(), mockContentsButton, Substitute.For<ISimpleButton>());

            Construct();

            mockContentsButton.Received(1).Tapped += Arg.Any<EventHandler<ButtonEventArgs>>();
        }

        [TestMethod]
        public void ConstructShouldCreatePageLabel()
        {
            ISpriteFont mockFont = Substitute.For<ISpriteFont>();
            _mockEngine.AssetBank.Get<ISpriteFont>("MainMenuContentFont").Returns(mockFont);

            Construct();

            _mockGOF.Received(1).CreateGenericTextLabel(_mockEngine, Vector2.Zero, _mockSpriteBatch, mockFont, "", CodeLogicEngine.Constants.ClrMenuFGHigh);
        }

        [TestMethod]
        public void ConstructShouldSetPageLabelText()
        {
            ITextLabel mockLabel = Substitute.For<ITextLabel>();
            _mockGOF.CreateGenericTextLabel(_mockEngine, Vector2.Zero, _mockSpriteBatch, Arg.Any<ISpriteFont>(), "", Arg.Any<Color>()).Returns(mockLabel);

            _mockHostLayer.CurrentPage.Returns(3);
            _mockHostLayer.TotalPages.Returns(5);

            Construct();

            mockLabel.Received(1).Text = "Page 3 of 5";
        }

        [TestMethod]
        public void ConstructShouldSetPageLabelBounds()
        {
            _mockHostLayer.CurrentPage.Returns(3);
            _mockHostLayer.TotalPages.Returns(5);

            ITextLabel mockLabel = Substitute.For<ITextLabel>();
            _mockGOF.CreateGenericTextLabel(_mockEngine, Vector2.Zero, _mockSpriteBatch, Arg.Any<ISpriteFont>(), "", Arg.Any<Color>()).Returns(mockLabel);
            mockLabel.Bounds.Returns(new RectangleF(Vector2.Zero, new SizeF(100, 50)));

            ISpriteFont mockFont = Substitute.For<ISpriteFont>();
            mockLabel.Font.Returns(mockFont);
            mockFont.MeasureString("Page 3 of 5").Returns(new SizeF(40, 10));

            ISimpleButton mockPreviousButton = Substitute.For<ISimpleButton>();
            mockPreviousButton.Bounds.Returns(new RectangleF(_bounds.Right - 100, _bounds.Bottom - 50, 100, 50));

            ISimpleButton mockContentsButton = Substitute.For<ISimpleButton>();
            mockContentsButton.Bounds.Returns(new RectangleF(_bounds.Left + 10, _bounds.Bottom - 50, 100, 50));

            _mockGOF.CreateSimpleButton(_mockEngine, Arg.Any<RectangleF>(), _mockSpriteBatch, Arg.Any<ISpriteFont>()).Returns(Substitute.For<ISimpleButton>(), mockPreviousButton, mockContentsButton, Substitute.For<ISimpleButton>());

            Construct();

            mockLabel.Received(1).Bounds = new RectangleF(
                //(int)(200 + (500 - 200) / 2f - 40 / 2f),
                (int)((210 + 100) + (500 - (210 + 100)) / 2f - 40 / 2f),
                (int)(375 - 10 / 2f),
                100,
                50
            );
        }

        [TestMethod]
        public void ConstructShouldAddEntitiesInExpectedOrder()
        {
            ISimpleButton mockNextButton = Substitute.For<ISimpleButton>();
            ISimpleButton mockPreviousButton = Substitute.For<ISimpleButton>();
            ISimpleButton mockContentsButton = Substitute.For<ISimpleButton>();
            _mockGOF.CreateSimpleButton(_mockEngine, Arg.Any<RectangleF>(), _mockSpriteBatch, Arg.Any<ISpriteFont>()).Returns(mockNextButton, mockPreviousButton, mockContentsButton, Substitute.For<ISimpleButton>());

            ITextLabel mockPageLabel = Substitute.For<ITextLabel>();
            _mockGOF.CreateGenericTextLabel(_mockEngine, Vector2.Zero, _mockSpriteBatch, Arg.Any<ISpriteFont>(), "", Arg.Any<Color>()).Returns(mockPageLabel);

            Construct();

            Assert.IsTrue(_sut.Entities.SequenceEqual(new IEntity[] { mockNextButton, mockPreviousButton, mockContentsButton, mockPageLabel }));
        }

        [TestMethod]
        public void ConstructShouldAttachEventHandlerToHostLayerPageChangedEvent()
        {
            Construct();

            _mockHostLayer.Received(1).PageChanged += Arg.Any<EventHandler<EventArgs>>();
        }
        #endregion

        #region HostLayer PageChanged Tests
        [TestMethod]
        public void PageChangedShouldUpdatePageLabelText()
        {
            _mockHostLayer.CurrentPage.Returns(2);
            _mockHostLayer.TotalPages.Returns(10);

            ISimpleButton mockNextButton = Substitute.For<ISimpleButton>();
            ISimpleButton mockPreviousButton = Substitute.For<ISimpleButton>();
            _mockGOF.CreateSimpleButton(_mockEngine, Arg.Any<RectangleF>(), _mockSpriteBatch, Arg.Any<ISpriteFont>()).Returns(mockNextButton, mockPreviousButton, Substitute.For<ISimpleButton>());

            ITextLabel mockPageLabel = Substitute.For<ITextLabel>();
            _mockGOF.CreateGenericTextLabel(_mockEngine, Vector2.Zero, _mockSpriteBatch, Arg.Any<ISpriteFont>(), "", CodeLogicEngine.Constants.ClrMenuFGHigh).Returns(mockPageLabel);

            Construct();
            mockPageLabel.ClearReceivedCalls();

            _mockHostLayer.PageChanged += Raise.Event<EventHandler<EventArgs>>(_sut, new EventArgs());

            mockPageLabel.Received(1).Text = "Page 2 of 10";
        }

        [TestMethod]
        public void PageChangedShouldUpdatePageLabelBounds()
        {
            _mockHostLayer.CurrentPage.Returns(2);
            _mockHostLayer.TotalPages.Returns(10);

            ISimpleButton mockNextButton = Substitute.For<ISimpleButton>();
            ISimpleButton mockPreviousButton = Substitute.For<ISimpleButton>();
            _mockGOF.CreateSimpleButton(_mockEngine, Arg.Any<RectangleF>(), _mockSpriteBatch, Arg.Any<ISpriteFont>()).Returns(mockNextButton, mockPreviousButton, Substitute.For<ISimpleButton>());

            ITextLabel mockPageLabel = Substitute.For<ITextLabel>();
            _mockGOF.CreateGenericTextLabel(_mockEngine, Vector2.Zero, _mockSpriteBatch, Arg.Any<ISpriteFont>(), "", CodeLogicEngine.Constants.ClrMenuFGHigh).Returns(mockPageLabel);

            Construct();
            mockPageLabel.ClearReceivedCalls();

            _mockHostLayer.PageChanged += Raise.Event<EventHandler<EventArgs>>(_sut, new EventArgs());

            // I don't want to re-math this... I don't give a crap what the value is so long as the bounds is updated.
            mockPageLabel.Received(1).Bounds = Arg.Any<RectangleF>();
        }
        #endregion

        #region Button Event Tests
        [TestMethod]
        public void NextButtonTappedShouldCallHostLayerNextPage()
        {
            ISimpleButton mockNextButton = Substitute.For<ISimpleButton>();
            ISimpleButton mockPreviousButton = Substitute.For<ISimpleButton>();
            _mockGOF.CreateSimpleButton(_mockEngine, Arg.Any<RectangleF>(), _mockSpriteBatch, Arg.Any<ISpriteFont>()).Returns(mockNextButton, mockPreviousButton, Substitute.For<ISimpleButton>());

            Construct();

            mockNextButton.Tapped += Raise.Event<EventHandler<ButtonEventArgs>>(mockNextButton, new ButtonEventArgs(Vector2.Zero));

            _mockHostLayer.Received(1).NextPage();
        }

        [TestMethod]
        public void NextButtonTappedShouldPlayExpectedSound()
        {
            ISimpleButton mockNextButton = Substitute.For<ISimpleButton>();
            ISimpleButton mockPreviousButton = Substitute.For<ISimpleButton>();
            _mockGOF.CreateSimpleButton(_mockEngine, Arg.Any<RectangleF>(), _mockSpriteBatch, Arg.Any<ISpriteFont>()).Returns(mockNextButton, mockPreviousButton, Substitute.For<ISimpleButton>());

            Construct();

            mockNextButton.Tapped += Raise.Event<EventHandler<ButtonEventArgs>>(mockNextButton, new ButtonEventArgs(Vector2.Zero));

            _mockEngine.AssetBank.Get<ISoundResource>("ButtonPress").Received(1).Play();
        }

        [TestMethod]
        public void PreviousButtonTappedShouldCallHostLayerPreviousPage()
        {
            ISimpleButton mockNextButton = Substitute.For<ISimpleButton>();
            ISimpleButton mockPreviousButton = Substitute.For<ISimpleButton>();
            _mockGOF.CreateSimpleButton(_mockEngine, Arg.Any<RectangleF>(), _mockSpriteBatch, Arg.Any<ISpriteFont>()).Returns(mockNextButton, mockPreviousButton, Substitute.For<ISimpleButton>());

            Construct();

            mockPreviousButton.Tapped += Raise.Event<EventHandler<ButtonEventArgs>>(mockPreviousButton, new ButtonEventArgs(Vector2.Zero));

            _mockHostLayer.Received(1).PreviousPage();
        }

        [TestMethod]
        public void PreviousButtonTappedShouldPlayExpectedSound()
        {
            ISimpleButton mockNextButton = Substitute.For<ISimpleButton>();
            ISimpleButton mockPreviousButton = Substitute.For<ISimpleButton>();
            _mockGOF.CreateSimpleButton(_mockEngine, Arg.Any<RectangleF>(), _mockSpriteBatch, Arg.Any<ISpriteFont>()).Returns(mockNextButton, mockPreviousButton, Substitute.For<ISimpleButton>());

            Construct();

            mockPreviousButton.Tapped += Raise.Event<EventHandler<ButtonEventArgs>>(mockPreviousButton, new ButtonEventArgs(Vector2.Zero));

            _mockEngine.AssetBank.Get<ISoundResource>("ButtonPress").Received(1).Play();
        }

        [TestMethod]
        public void ContentsButtonTappedShouldCallHostLayerSetPageWithExpectedPage()
        {
            ISimpleButton mockContentsButton = Substitute.For<ISimpleButton>();
            _mockGOF.CreateSimpleButton(_mockEngine, Arg.Any<RectangleF>(), _mockSpriteBatch, Arg.Any<ISpriteFont>()).Returns(Substitute.For<ISimpleButton>(), Substitute.For<ISimpleButton>(), mockContentsButton, Substitute.For<ISimpleButton>());

            Construct();

            mockContentsButton.Tapped += Raise.Event<EventHandler<ButtonEventArgs>>(mockContentsButton, new ButtonEventArgs(Vector2.Zero));

            _mockHostLayer.Received(1).SetPage(GameObjectFactory.BriefingPage.Contents);
        }

        [TestMethod]
        public void ContentsButtonTappedShouldPlayExpectedSound()
        {
            ISimpleButton mockContentsButton = Substitute.For<ISimpleButton>();
            _mockGOF.CreateSimpleButton(_mockEngine, Arg.Any<RectangleF>(), _mockSpriteBatch, Arg.Any<ISpriteFont>()).Returns(Substitute.For<ISimpleButton>(), Substitute.For<ISimpleButton>(), mockContentsButton, Substitute.For<ISimpleButton>());

            Construct();

            mockContentsButton.Tapped += Raise.Event<EventHandler<ButtonEventArgs>>(mockContentsButton, new ButtonEventArgs(Vector2.Zero));

            _mockEngine.AssetBank.Get<ISoundResource>("ButtonPress").Received(1).Play();
        }
        #endregion
    }
}
