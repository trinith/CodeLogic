using ArbitraryPixel.CodeLogic.Common;
using ArbitraryPixel.CodeLogic.Common.BriefingContent;
using ArbitraryPixel.CodeLogic.Common.Entities;
using ArbitraryPixel.CodeLogic.Common.Layers;
using ArbitraryPixel.Common.Audio;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.Entity;
using ArbitraryPixel.Platform2D.UI;
using ArbitraryPixel.Platform2D.UI.Factory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using NSubstitute;
using System;

namespace ArbitraryPixel.CodeLogic.Common_Tests.BriefingContent
{
    [TestClass]
    public class BriefingContentsPage_Tests
    {
        private BriefingContentsPage _sut;
        private IEngine _mockEngine;
        private ISpriteBatch _mockSpriteBatch;

        private GameObjectFactory _mockGOF;

        private RectangleF _bounds = new RectangleF(200, 100, 400, 300);
        private ContentsPageInfo[] _pageListing = null;

        [TestInitialize]
        public void Initialize()
        {
            _mockEngine = Substitute.For<IEngine>();
            _mockSpriteBatch = Substitute.For<ISpriteBatch>();

            _mockGOF = Substitute.For<GameObjectFactory>();
            GameObjectFactory.SetInstance(_mockGOF);

            _pageListing = new ContentsPageInfo[]
            {
                new ContentsPageInfo(GameObjectFactory.BriefingPage.Cover, "CoverTitle", "CoverDescription"),
                new ContentsPageInfo(GameObjectFactory.BriefingPage.Contents, "ContentsTitle", "ContentsDescription"),
            };
        }

        private void Construct()
        {
            _sut = new BriefingContentsPage(_mockEngine, _mockSpriteBatch, _bounds, _pageListing);
        }

        #region Constructor Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowExpectedException_PageListing()
        {
            _sut = new BriefingContentsPage(_mockEngine, _mockSpriteBatch, _bounds, (ContentsPageInfo[])null);
        }

        [TestMethod]
        public void ConstructShouldCreateHeaderLabel()
        {
            ISpriteFont mockFont = Substitute.For<ISpriteFont>();
            _mockEngine.AssetBank.Get<ISpriteFont>("BriefingTitleFont").Returns(mockFont);
            mockFont.MeasureString("Table of Contents").Returns(new SizeF(50, 20));

            Construct();

            _mockGOF.Received(1).CreateGenericTextLabel(
                _mockEngine,
                new Vector2(_bounds.Centre.X - 50 / 2f, _bounds.Top),
                _mockSpriteBatch,
                mockFont,
                "Table of Contents",
                Color.White
            );
        }

        [TestMethod]
        public void ConstructShouldAddHeaderLabelToEntities()
        {
            ITextLabel mockLabel = Substitute.For<ITextLabel>();
            _mockGOF.CreateGenericTextLabel(Arg.Any<IEngine>(), Arg.Any<Vector2>(), Arg.Any<ISpriteBatch>(), Arg.Any<ISpriteFont>(), Arg.Any<string>(), Arg.Any<Color>()).Returns(mockLabel);

            Construct();

            Assert.AreSame(mockLabel, _sut.Entities[0]);
        }

        [TestMethod]
        public void ConstructShouldExpectedButtonLabelForPageListing_Index0()
        {
            ISpriteFont mockTitleFont = Substitute.For<ISpriteFont>();
            mockTitleFont.LineSpacing.Returns(20);

            ISpriteFont mockDescriptionFont = Substitute.For<ISpriteFont>();
            mockDescriptionFont.LineSpacing.Returns(10);

            _mockEngine.AssetBank.Get<ISpriteFont>("BriefingTitleFont").Returns(mockTitleFont);
            _mockEngine.AssetBank.Get<ISpriteFont>("BriefingNormalFont").Returns(mockDescriptionFont);

            Vector2 expectedLocation = new Vector2(_bounds.Left, _bounds.Top + 20);
            SizeF expectedSize = new SizeF(_bounds.Width, 20 + 10 + CodeLogicEngine.Constants.TextWindowPadding.Width * 2 + 2);

            IChapterButton mockButton = Substitute.For<IChapterButton>();
            _mockGOF.CreateChapterButton(
                _mockEngine,
                new RectangleF(expectedLocation, expectedSize),
                _mockSpriteBatch,
                mockTitleFont,
                mockDescriptionFont
            ).Returns(mockButton);

            Construct();

            Received.InOrder(
                () =>
                {
                    mockButton.Text = "CoverTitle";
                    mockButton.Description = "CoverDescription";
                    mockButton.Tag = GameObjectFactory.BriefingPage.Cover;

                    mockButton.NormalColour = CodeLogicEngine.Constants.ClrMenuFGMid;
                    mockButton.PressedColour = CodeLogicEngine.Constants.ClrMenuFGHigh;
                    mockButton.BackColour = CodeLogicEngine.Constants.ClrMenuBGLow;
                }
            );
        }

        [TestMethod]
        public void ConstructShouldExpectedButtonLabelForPageListing_Index1()
        {
            ISpriteFont mockTitleFont = Substitute.For<ISpriteFont>();
            mockTitleFont.LineSpacing.Returns(20);

            ISpriteFont mockDescriptionFont = Substitute.For<ISpriteFont>();
            mockDescriptionFont.LineSpacing.Returns(10);

            _mockEngine.AssetBank.Get<ISpriteFont>("BriefingTitleFont").Returns(mockTitleFont);
            _mockEngine.AssetBank.Get<ISpriteFont>("BriefingNormalFont").Returns(mockDescriptionFont);

            Vector2 expectedLocation = new Vector2(_bounds.Left, _bounds.Top + 20);
            SizeF expectedSize = new SizeF(_bounds.Width, 20 + 10 + CodeLogicEngine.Constants.TextWindowPadding.Width * 2 + 2);

            expectedLocation.Y += expectedSize.Height + CodeLogicEngine.Constants.TextWindowPadding.Height;

            IChapterButton mockButton = Substitute.For<IChapterButton>();
            _mockGOF.CreateChapterButton(
                _mockEngine,
                new RectangleF(expectedLocation, expectedSize),
                _mockSpriteBatch,
                mockTitleFont,
                mockDescriptionFont
            ).Returns(mockButton);

            Construct();

            Received.InOrder(
                () =>
                {
                    mockButton.Text = "ContentsTitle";
                    mockButton.Description = "ContentsDescription";
                    mockButton.Tag = GameObjectFactory.BriefingPage.Contents;

                    mockButton.NormalColour = CodeLogicEngine.Constants.ClrMenuFGMid;
                    mockButton.PressedColour = CodeLogicEngine.Constants.ClrMenuFGHigh;
                    mockButton.BackColour = CodeLogicEngine.Constants.ClrMenuBGLow;
                }
            );
        }

        [TestMethod]
        public void ConstructShouldAddExpectedButtonsForPageListingToEntities_Index0()
        {
            IChapterButton mockIntroButton = Substitute.For<IChapterButton>();
            IChapterButton mockContentsButton = Substitute.For<IChapterButton>();
            _mockGOF.CreateChapterButton(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ISpriteFont>(), Arg.Any<ISpriteFont>()).Returns(mockIntroButton, mockContentsButton);

            Construct();

            Assert.AreEqual<IEntity>(mockIntroButton, _sut.Entities[1]);
        }

        [TestMethod]
        public void ConstructShouldAddExpectedButtonsForPageListingToEntities_Index1()
        {
            IChapterButton mockIntroButton = Substitute.For<IChapterButton>();
            IChapterButton mockContentsButton = Substitute.For<IChapterButton>();
            _mockGOF.CreateChapterButton(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ISpriteFont>(), Arg.Any<ISpriteFont>()).Returns(mockIntroButton, mockContentsButton);

            Construct();

            Assert.AreEqual<IEntity>(mockContentsButton, _sut.Entities[2]);
        }
        #endregion

        #region Property Tests
        [TestMethod]
        public void BriefingContentLayerShouldDefaultNull()
        {
            Construct();

            Assert.IsNull(_sut.BriefingContentLayer);
        }

        [TestMethod]
        public void BriefingContentLayerShouldReturnSetValue()
        {
            Construct();

            IMenuBriefingContentLayer mockBriefingContentLayer = Substitute.For<IMenuBriefingContentLayer>();
            _sut.BriefingContentLayer = mockBriefingContentLayer;

            Assert.AreSame(mockBriefingContentLayer, _sut.BriefingContentLayer);
        }
        #endregion

        #region ContentLinkButtonTapped Event Tests
        [TestMethod]
        public void ContentLinkButtonTappedShouldSetPageOnBriefingContentLayer_TestA()
        {
            GameObjectFactory.BriefingPage expectedPage = GameObjectFactory.BriefingPage.Cover;

            IChapterButton mockButton = Substitute.For<IChapterButton>();
            _mockGOF.CreateChapterButton(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ISpriteFont>(), Arg.Any<ISpriteFont>())
                .Returns(mockButton, Substitute.For<IChapterButton>());

            Construct();

            IMenuBriefingContentLayer mockBriefingContentLayer = Substitute.For<IMenuBriefingContentLayer>();
            _sut.BriefingContentLayer = mockBriefingContentLayer;

            mockButton.Tag.Returns(expectedPage);
            mockButton.Tapped += Raise.Event<EventHandler<ButtonEventArgs>>(mockButton, new ButtonEventArgs(Vector2.Zero));

            mockBriefingContentLayer.Received(1).SetPage(expectedPage);
        }

        [TestMethod]
        public void ContentLinkButtonTappedShouldSetPageOnBriefingContentLayer_TestB()
        {
            GameObjectFactory.BriefingPage expectedPage = GameObjectFactory.BriefingPage.Contents;

            IChapterButton mockButton = Substitute.For<IChapterButton>();
            _mockGOF.CreateChapterButton(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ISpriteFont>(), Arg.Any<ISpriteFont>())
                .Returns(Substitute.For<IChapterButton>(), mockButton, Substitute.For<IChapterButton>());

            Construct();

            IMenuBriefingContentLayer mockBriefingContentLayer = Substitute.For<IMenuBriefingContentLayer>();
            _sut.BriefingContentLayer = mockBriefingContentLayer;

            mockButton.Tag.Returns(expectedPage);
            mockButton.Tapped += Raise.Event<EventHandler<ButtonEventArgs>>(mockButton, new ButtonEventArgs(Vector2.Zero));

            mockBriefingContentLayer.Received(1).SetPage(expectedPage);
        }

        [TestMethod]
        public void ContentLinkButtonTappedShouldPlayExpectedSound()
        {
            IChapterButton mockButton = Substitute.For<IChapterButton>();
            _mockGOF.CreateChapterButton(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ISpriteFont>(), Arg.Any<ISpriteFont>())
                .Returns(Substitute.For<IChapterButton>(), mockButton, Substitute.For<IChapterButton>());

            Construct();

            mockButton.Tapped += Raise.Event<EventHandler<ButtonEventArgs>>(mockButton, new ButtonEventArgs(Vector2.Zero));

            _mockEngine.AssetBank.Get<ISoundResource>("ButtonPress").Received(1).Play();
        }
        #endregion
    }
}
