using ArbitraryPixel.CodeLogic.Common;
using ArbitraryPixel.CodeLogic.Common.Config;
using ArbitraryPixel.CodeLogic.Common.Entities;
using ArbitraryPixel.CodeLogic.Common.Layers;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.Text;
using ArbitraryPixel.Platform2D.UI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ArbitraryPixel.CodeLogic.Common_Tests.Layers
{
    [TestClass]
    public class MenuStatsContentLayer_Tests
    {
        private MenuStatsContentLayer _sut;

        private IEngine _mockEngine;
        private ISpriteBatch _mockSpriteBatch;
        private RectangleF _contentBounds = new RectangleF(200, 100, 400, 300);
        private IGameStatsData _mockGameStatsData;

        private GameObjectFactory _mockGOF;

        [TestInitialize]
        public void Initialize()
        {
            _mockEngine = Substitute.For<IEngine>();
            _mockSpriteBatch = Substitute.For<ISpriteBatch>();
            _mockGameStatsData = Substitute.For<IGameStatsData>();

            _mockGOF = Substitute.For<GameObjectFactory>();
            GameObjectFactory.SetInstance(_mockGOF);
        }

        private void Construct()
        {
            _sut = new MenuStatsContentLayer(_mockEngine, _mockSpriteBatch, _contentBounds, _mockGameStatsData);
        }

        #region Constructor Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowException_GameStatsData()
        {
            _sut = new MenuStatsContentLayer(_mockEngine, _mockSpriteBatch, _contentBounds, null);
        }

        [TestMethod]
        public void ConstructShouldSetVisibleFalse()
        {
            Construct();

            Assert.IsFalse(_sut.Visible);
        }

        [TestMethod]
        public void ConstructShouldAttachToGameStatsDataDataSavedEvent()
        {
            Construct();

            _mockGameStatsData.Received(1).DataSaved += Arg.Any<EventHandler<EventArgs>>();
        }

        [TestMethod]
        public void ConstructShouldCreateTextObjectBuilder()
        {
            ITextFormatValueHandlerManager mockValueHandlerManager = Substitute.For<ITextFormatValueHandlerManager>();
            _mockGOF.CreateTextFormatValueHandlerManager().Returns(mockValueHandlerManager);

            ITextFormatProcessor mockFormatProcessor = Substitute.For<ITextFormatProcessor>();
            _mockGOF.CreateTextFormatProcessor(mockValueHandlerManager).Returns(mockFormatProcessor);

            ITextObjectFactory mockTextFactory = Substitute.For<ITextObjectFactory>();
            _mockGOF.CreateTextObjectFactory().Returns(mockTextFactory);

            Construct();

            _mockGOF.Received(1).CreateTextObjectBuilder(mockFormatProcessor, mockTextFactory);
        }

        [TestMethod]
        public void ConstructShouldRegisterStatsFontOnTextObjectBuilder()
        {
            ITextFormatValueHandlerManager mockValueHandlerManager = Substitute.For<ITextFormatValueHandlerManager>();
            _mockGOF.CreateTextFormatValueHandlerManager().Returns(mockValueHandlerManager);

            ITextFormatProcessor mockFormatProcessor = Substitute.For<ITextFormatProcessor>();
            _mockGOF.CreateTextFormatProcessor(mockValueHandlerManager).Returns(mockFormatProcessor);

            ITextObjectFactory mockTextFactory = Substitute.For<ITextObjectFactory>();
            _mockGOF.CreateTextObjectFactory().Returns(mockTextFactory);

            ITextObjectBuilder mockTextBuilder = Substitute.For<ITextObjectBuilder>();
            _mockGOF.CreateTextObjectBuilder(mockFormatProcessor, mockTextFactory).Returns(mockTextBuilder);

            ISpriteFont mockFont = Substitute.For<ISpriteFont>();
            _mockEngine.AssetBank.Get<ISpriteFont>("StatsFont").Returns(mockFont);

            Construct();

            mockTextBuilder.Received(1).RegisterFont("Normal", mockFont);
        }

        [TestMethod]
        public void ConstructShouldSetDefaultFontOnTextObjectBuilder()
        {
            ITextFormatValueHandlerManager mockValueHandlerManager = Substitute.For<ITextFormatValueHandlerManager>();
            _mockGOF.CreateTextFormatValueHandlerManager().Returns(mockValueHandlerManager);

            ITextFormatProcessor mockFormatProcessor = Substitute.For<ITextFormatProcessor>();
            _mockGOF.CreateTextFormatProcessor(mockValueHandlerManager).Returns(mockFormatProcessor);

            ITextObjectFactory mockTextFactory = Substitute.For<ITextObjectFactory>();
            _mockGOF.CreateTextObjectFactory().Returns(mockTextFactory);

            ITextObjectBuilder mockTextBuilder = Substitute.For<ITextObjectBuilder>();
            _mockGOF.CreateTextObjectBuilder(mockFormatProcessor, mockTextFactory).Returns(mockTextBuilder);

            ISpriteFont mockFont = Substitute.For<ISpriteFont>();
            mockTextBuilder.GetRegisteredFont("Normal").Returns(mockFont);

            Construct();

            mockTextBuilder.Received(1).DefaultFont = mockFont;
        }

        [TestMethod]
        public void ConstructShouldCreateTextObjectRenderer()
        {
            RectangleF expectedBounds = _contentBounds;
            expectedBounds.Inflate(-CodeLogicEngine.Constants.TextWindowPadding);

            RectangleF buttonBounds = new RectangleF(new Vector2(expectedBounds.Right, expectedBounds.Bottom) - CodeLogicEngine.Constants.MenuButtonSize, CodeLogicEngine.Constants.MenuButtonSize);
            expectedBounds.Height -= expectedBounds.Bottom - (buttonBounds.Top - CodeLogicEngine.Constants.TextWindowPadding.Height);

            Construct();

            _mockGOF.Received(1).CreateTextObjectRenderer(_mockEngine.GrfxFactory.RenderTargetFactory, _mockEngine.Graphics.GraphicsDevice, _mockSpriteBatch, (Rectangle)expectedBounds);
        }

        [TestMethod]
        public void ConstructShouldAddResetButtonToEntities()
        {
            RectangleF contentBounds = _contentBounds;
            contentBounds.Inflate(-CodeLogicEngine.Constants.TextWindowPadding);

            RectangleF buttonBounds = new RectangleF(new Vector2(contentBounds.Right, contentBounds.Bottom) - CodeLogicEngine.Constants.MenuButtonSize, CodeLogicEngine.Constants.MenuButtonSize);
            contentBounds.Height -= contentBounds.Bottom - (buttonBounds.Top - CodeLogicEngine.Constants.TextWindowPadding.Height);

            ISpriteFont mockFont = Substitute.For<ISpriteFont>();
            _mockEngine.AssetBank.Get<ISpriteFont>("MainButtonFont").Returns(mockFont);

            ISimpleButton mockButton = Substitute.For<ISimpleButton>();
            _mockGOF.CreateSimpleButton(_mockEngine, buttonBounds, _mockSpriteBatch, mockFont).Returns(mockButton);

            Construct();

            Assert.IsTrue(_sut.Entities.ToList().Contains(mockButton));
        }

        [TestMethod]
        public void ConstructShouldSetResetButtonText()
        {
            RectangleF contentBounds = _contentBounds;
            contentBounds.Inflate(-CodeLogicEngine.Constants.TextWindowPadding);

            RectangleF buttonBounds = new RectangleF(new Vector2(contentBounds.Right, contentBounds.Bottom) - CodeLogicEngine.Constants.MenuButtonSize, CodeLogicEngine.Constants.MenuButtonSize);
            contentBounds.Height -= contentBounds.Bottom - (buttonBounds.Top - CodeLogicEngine.Constants.TextWindowPadding.Height);

            ISpriteFont mockFont = Substitute.For<ISpriteFont>();
            _mockEngine.AssetBank.Get<ISpriteFont>("MainButtonFont").Returns(mockFont);

            ISimpleButton mockButton = Substitute.For<ISimpleButton>();
            _mockGOF.CreateSimpleButton(_mockEngine, buttonBounds, _mockSpriteBatch, mockFont).Returns(mockButton);

            Construct();

            mockButton.Received(1).Text = "Reset";
        }

        [TestMethod]
        public void ConstructShouldAttachToResetButtonTappedEvent()
        {
            RectangleF contentBounds = _contentBounds;
            contentBounds.Inflate(-CodeLogicEngine.Constants.TextWindowPadding);

            RectangleF buttonBounds = new RectangleF(new Vector2(contentBounds.Right, contentBounds.Bottom) - CodeLogicEngine.Constants.MenuButtonSize, CodeLogicEngine.Constants.MenuButtonSize);
            contentBounds.Height -= contentBounds.Bottom - (buttonBounds.Top - CodeLogicEngine.Constants.TextWindowPadding.Height);

            ISpriteFont mockFont = Substitute.For<ISpriteFont>();
            _mockEngine.AssetBank.Get<ISpriteFont>("MainButtonFont").Returns(mockFont);

            ISimpleButton mockButton = Substitute.For<ISimpleButton>();
            _mockGOF.CreateSimpleButton(_mockEngine, buttonBounds, _mockSpriteBatch, mockFont).Returns(mockButton);

            Construct();

            mockButton.Received(1).Tapped += Arg.Any<EventHandler<ButtonEventArgs>>();
        }

        [TestMethod]
        public void ConstructShouldAddTextTextureToEntities()
        {
            RectangleF contentBounds = _contentBounds;
            contentBounds.Inflate(-CodeLogicEngine.Constants.TextWindowPadding);

            RectangleF buttonBounds = new RectangleF(new Vector2(contentBounds.Right, contentBounds.Bottom) - CodeLogicEngine.Constants.MenuButtonSize, CodeLogicEngine.Constants.MenuButtonSize);
            contentBounds.Height -= contentBounds.Bottom - (buttonBounds.Top - CodeLogicEngine.Constants.TextWindowPadding.Height);

            ITextureEntity mockEntity = Substitute.For<ITextureEntity>();
            _mockGOF.CreateTextureEntity(_mockEngine, contentBounds, _mockSpriteBatch, null, Color.White).Returns(mockEntity);

            Construct();

            Assert.IsTrue(_sut.Entities.ToList().Contains(mockEntity));
        }
        #endregion

        #region RefreshText Tests
        [TestMethod]
        public void RefreshTextShouldClearTextRenderer()
        {
            ITextObjectRenderer mockRenderer = Substitute.For<ITextObjectRenderer>();
            _mockGOF.CreateTextObjectRenderer(_mockEngine.GrfxFactory.RenderTargetFactory, _mockEngine.Graphics.GraphicsDevice, _mockSpriteBatch, Arg.Any<Rectangle>()).Returns(mockRenderer);

            Construct();

            _sut.RefreshText();

            mockRenderer.Received(1).Clear();
        }

        [TestMethod]
        public void RefreshTextShouldCallTextBuilderBuild()
        {
            ITextObjectBuilder mockTextBuilder = Substitute.For<ITextObjectBuilder>();
            _mockGOF.CreateTextObjectBuilder(Arg.Any<ITextFormatProcessor>(), Arg.Any<ITextObjectFactory>()).Returns(mockTextBuilder);

            ITextureEntity mockEntity = Substitute.For<ITextureEntity>();
            mockEntity.Bounds.Returns(new RectangleF(123, 321, 300, 100));
            _mockGOF.CreateTextureEntity(_mockEngine, Arg.Any<RectangleF>(), _mockSpriteBatch, null, Color.White).Returns(mockEntity);

            Construct();

            _sut.RefreshText();

            mockTextBuilder.Received(1).Build(Arg.Any<string>(), new RectangleF(Vector2.Zero, new SizeF(300, 100)));
        }

        [TestMethod]
        public void RefreshTextShouldRenderExpectedTextObjects()
        {
            ITextObject mockTextObjectA = Substitute.For<ITextObject>();
            ITextObject mockTextObjectB = Substitute.For<ITextObject>();

            ITextObjectBuilder mockTextBuilder = Substitute.For<ITextObjectBuilder>();
            mockTextBuilder.Build(Arg.Any<string>(), Arg.Any<RectangleF>()).Returns(new List<ITextObject>(new ITextObject[] { mockTextObjectA, mockTextObjectB }));
            _mockGOF.CreateTextObjectBuilder(Arg.Any<ITextFormatProcessor>(), Arg.Any<ITextObjectFactory>()).Returns(mockTextBuilder);

            ITextObjectRenderer mockRenderer = Substitute.For<ITextObjectRenderer>();
            _mockGOF.CreateTextObjectRenderer(_mockEngine.GrfxFactory.RenderTargetFactory, _mockEngine.Graphics.GraphicsDevice, _mockSpriteBatch, Arg.Any<Rectangle>()).Returns(mockRenderer);

            Construct();

            _sut.RefreshText();

            Received.InOrder(
                () =>
                {
                    mockRenderer.Enqueue(mockTextObjectA);
                    mockRenderer.Enqueue(mockTextObjectB);
                }
            );
        }

        [TestMethod]
        public void RefreshTextShouldCallTextRendererFlush()
        {
            ITextObjectRenderer mockRenderer = Substitute.For<ITextObjectRenderer>();
            _mockGOF.CreateTextObjectRenderer(_mockEngine.GrfxFactory.RenderTargetFactory, _mockEngine.Graphics.GraphicsDevice, _mockSpriteBatch, Arg.Any<Rectangle>()).Returns(mockRenderer);

            Construct();

            _sut.RefreshText();

            mockRenderer.Received(1).Flush();
        }

        [TestMethod]
        public void RefreshTextShouldSetTextTextureTexture()
        {
            ITexture2D mockTexture = Substitute.For<ITexture2D>();

            ITextObjectRenderer mockRenderer = Substitute.For<ITextObjectRenderer>();
            mockRenderer.Render().Returns(mockTexture);
            _mockGOF.CreateTextObjectRenderer(_mockEngine.GrfxFactory.RenderTargetFactory, _mockEngine.Graphics.GraphicsDevice, _mockSpriteBatch, Arg.Any<Rectangle>()).Returns(mockRenderer);

            ITextureEntity mockEntity = Substitute.For<ITextureEntity>();
            _mockGOF.CreateTextureEntity(_mockEngine, Arg.Any<RectangleF>(), _mockSpriteBatch, null, Color.White).Returns(mockEntity);

            Construct();

            _sut.RefreshText();

            mockEntity.Received(1).Texture = mockTexture;
        }
        #endregion

        #region ResetButtonTapped Event Tests
        [TestMethod]
        public void ResetButtonTappedShouldFireResetButtonTappedEvent()
        {
            ISimpleButton mockButton = Substitute.For<ISimpleButton>();
            _mockGOF.CreateSimpleButton(_mockEngine, Arg.Any<RectangleF>(), _mockSpriteBatch, Arg.Any<ISpriteFont>()).Returns(mockButton);

            Construct();

            var eventHandler = Substitute.For<EventHandler<EventArgs>>();
            _sut.ResetButtonTapped += eventHandler;

            mockButton.Tapped += Raise.Event<EventHandler<ButtonEventArgs>>(mockButton, new ButtonEventArgs(Vector2.Zero));

            eventHandler.Received(1)(_sut, Arg.Any<EventArgs>());
        }
        #endregion

        #region GameStatsData DataSaved EventTests
        [TestMethod]
        public void GameStatsDataDataSavedWhenLayerVisibleShouldRefreshText()
        {
            ITextureEntity mockEntity = Substitute.For<ITextureEntity>();
            _mockGOF.CreateTextureEntity(_mockEngine, Arg.Any<RectangleF>(), _mockSpriteBatch, null, Color.White).Returns(mockEntity);

            Construct();
            _sut.Visible = true;

            _mockGameStatsData.DataSaved += Raise.Event<EventHandler<EventArgs>>(_mockGameStatsData, new EventArgs());

            mockEntity.Received(1).Texture = Arg.Any<ITexture2D>();
        }

        [TestMethod]
        public void GameStatsDataDataSavedWhenLayerNotVisibleShouldNotRefreshText()
        {
            ITextureEntity mockEntity = Substitute.For<ITextureEntity>();
            _mockGOF.CreateTextureEntity(_mockEngine, Arg.Any<RectangleF>(), _mockSpriteBatch, null, Color.White).Returns(mockEntity);

            Construct();
            _sut.Visible = false;

            _mockGameStatsData.DataSaved += Raise.Event<EventHandler<EventArgs>>(_mockGameStatsData, new EventArgs());

            mockEntity.Received(0).Texture = Arg.Any<ITexture2D>();
        }
        #endregion

        #region Show / Hide Tests
        [TestMethod]
        public void ShowShouldSetVisibleToTrue()
        {
            Construct();

            _sut.Show();

            Assert.IsTrue(_sut.Visible);
        }

        [TestMethod]
        public void ShowShouldRefreshText()
        {
            ITextureEntity mockEntity = Substitute.For<ITextureEntity>();
            _mockGOF.CreateTextureEntity(_mockEngine, Arg.Any<RectangleF>(), _mockSpriteBatch, null, Color.White).Returns(mockEntity);

            Construct();

            _sut.Show();

            mockEntity.Received(1).Texture = Arg.Any<ITexture2D>();
        }

        [TestMethod]
        public void HideShouldSetVisibleFalse()
        {
            Construct();

            _sut.Hide();

            Assert.IsFalse(_sut.Visible);
        }
        #endregion
    }
}
