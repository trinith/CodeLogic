using System;
using System.Collections.Generic;
using System.Text;
using ArbitraryPixel.CodeLogic.Common;
using ArbitraryPixel.CodeLogic.Common.Credits;
using ArbitraryPixel.CodeLogic.Common.Layers;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.Layer;
using ArbitraryPixel.Platform2D.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NSubstitute;

namespace ArbitraryPixel.CodeLogic.Common_Tests.Layers
{
    [TestClass]
    public class MenuCreditsContentLayer_Tests
    {
        private MenuCreditsContentLayer _sut;
        private IEngine _mockEngine;
        private ISpriteBatch _mockSpriteBatch;
        private GameObjectFactory _mockGOF;

        private RectangleF _bounds = new RectangleF(200, 100, 400, 300);

        private string[] _testLines = new string[] { "Line 1", "Line 2", "Line 3" };

        [TestInitialize]
        public void Initialize()
        {
            _mockEngine = Substitute.For<IEngine>();
            _mockSpriteBatch = Substitute.For<ISpriteBatch>();

            _mockGOF = Substitute.For<GameObjectFactory>();
            GameObjectFactory.SetInstance(_mockGOF);
        }

        private void Construct()
        {
            _sut = new MenuCreditsContentLayer(_mockEngine, _mockSpriteBatch, _bounds, _testLines);
        }

        #region Constructor Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowException_CreditLines()
        {
            _sut = new MenuCreditsContentLayer(_mockEngine, _mockSpriteBatch, _bounds, null);
        }

        [TestMethod]
        public void ConstructShouldCreateTextObjectBuilder()
        {
            ITextFormatValueHandlerManager mockTextFormatValueHandlerManager = Substitute.For<ITextFormatValueHandlerManager>();
            _mockGOF.CreateTextFormatValueHandlerManager().Returns(mockTextFormatValueHandlerManager);

            ITextFormatProcessor mockTextFormatProcessor = Substitute.For<ITextFormatProcessor>();
            _mockGOF.CreateTextFormatProcessor(mockTextFormatValueHandlerManager).Returns(mockTextFormatProcessor);

            ITextObjectFactory mockTextObjectFactory = Substitute.For<ITextObjectFactory>();
            _mockGOF.CreateTextObjectFactory().Returns(mockTextObjectFactory);

            Construct();

            _mockGOF.Received(1).CreateTextObjectBuilder(mockTextFormatProcessor, mockTextObjectFactory);
        }

        [TestMethod]
        public void ConstructShouldRegisterTitleFont()
        {
            ITextObjectBuilder mockBuilder = Substitute.For<ITextObjectBuilder>();
            _mockGOF.CreateTextObjectBuilder(Arg.Any<ITextFormatProcessor>(), Arg.Any<ITextObjectFactory>()).Returns(mockBuilder);

            ISpriteFont mockFont = Substitute.For<ISpriteFont>();
            _mockEngine.AssetBank.Get<ISpriteFont>("CreditsTitleFont").Returns(mockFont);

            Construct();

            mockBuilder.Received(1).RegisterFont("Title", mockFont);
        }

        [TestMethod]
        public void ConstructShouldRegisterCreditFont()
        {
            ITextObjectBuilder mockBuilder = Substitute.For<ITextObjectBuilder>();
            _mockGOF.CreateTextObjectBuilder(Arg.Any<ITextFormatProcessor>(), Arg.Any<ITextObjectFactory>()).Returns(mockBuilder);

            ISpriteFont mockFont = Substitute.For<ISpriteFont>();
            _mockEngine.AssetBank.Get<ISpriteFont>("CreditsCreditFont").Returns(mockFont);

            Construct();

            mockBuilder.Received(1).RegisterFont("Credit", mockFont);
        }

        [TestMethod]
        public void ConstructShouldSetDefaultFontToTitleFont()
        {
            ITextObjectBuilder mockBuilder = Substitute.For<ITextObjectBuilder>();
            _mockGOF.CreateTextObjectBuilder(Arg.Any<ITextFormatProcessor>(), Arg.Any<ITextObjectFactory>()).Returns(mockBuilder);

            ISpriteFont mockFont = Substitute.For<ISpriteFont>();
            mockBuilder.GetRegisteredFont("Title").Returns(mockFont);

            Construct();

            mockBuilder.Received(1).DefaultFont = mockFont;
        }

        [TestMethod]
        public void ConstructShouldCreateGenericLayerForText()
        {
            ISpriteBatch textSpriteBatch = Substitute.For<ISpriteBatch>();
            _mockEngine.GrfxFactory.SpriteBatchFactory.Create(Arg.Any<IGrfxDevice>()).Returns(textSpriteBatch);

            Construct();

            _mockGOF.Received(1).CreateGenericLayer(
                _mockEngine,
                textSpriteBatch,
                SpriteSortMode.Deferred,
                null,
                null,
                null,
                null,
                null,
                Matrix.Identity
            );
        }
        #endregion

        #region Show Tests
        [TestMethod]
        public void ShowShouldCreateRenderTarget()
        {
            Construct();

            _sut.Show();

            _mockEngine.GrfxFactory.RenderTargetFactory.Received(1).Create(
                _mockEngine.Graphics.GraphicsDevice,
                400,
                300,
                RenderTargetUsage.DiscardContents
            );
        }

        [TestMethod]
        public void ShowShouldClearTextLayerEntities()
        {
            ILayer mockTextLayer = Substitute.For<ILayer>();
            _mockGOF.CreateGenericLayer(Arg.Any<IEngine>(), Arg.Any<ISpriteBatch>(), Arg.Any<SpriteSortMode>(), null, null, null, null, null, Arg.Any<Matrix?>()).Returns(mockTextLayer);
            Construct();

            _sut.Show();

            mockTextLayer.Received(1).ClearEntities();
        }

        [TestMethod]
        public void ShowShouldBuildTextObjects()
        {
            StringBuilder expectedCredits = new StringBuilder();
            foreach (string line in _testLines)
                expectedCredits.AppendLine(line);

            ITextObjectBuilder mockBuilder = Substitute.For<ITextObjectBuilder>();
            _mockGOF.CreateTextObjectBuilder(Arg.Any<ITextFormatProcessor>(), Arg.Any<ITextObjectFactory>()).Returns(mockBuilder);
            Construct();

            _sut.Show();

            mockBuilder.Received(1).Build(expectedCredits.ToString(), new RectangleF(200, 400, 400, 300));
        }

        [TestMethod]
        public void ShowShouldCreateLineItemsForCredits_TestA()
        {
            ILayer mockTextLayer = Substitute.For<ILayer>();
            _mockGOF.CreateGenericLayer(Arg.Any<IEngine>(), Arg.Any<ISpriteBatch>(), Arg.Any<SpriteSortMode>(), null, null, null, null, null, Arg.Any<Matrix?>()).Returns(mockTextLayer);

            StringBuilder expectedCredits = new StringBuilder();
            foreach (string line in _testLines)
                expectedCredits.AppendLine(line);

            ITextObjectBuilder mockBuilder = Substitute.For<ITextObjectBuilder>();
            _mockGOF.CreateTextObjectBuilder(Arg.Any<ITextFormatProcessor>(), Arg.Any<ITextObjectFactory>()).Returns(mockBuilder);
            Construct();

            List<ITextObject> fakeTextObjects = new List<ITextObject>();
            fakeTextObjects.AddRange(
                new ITextObject[]
                {
                    Substitute.For<ITextObject>(),
                    Substitute.For<ITextObject>(),
                }
            );
            mockBuilder.Build(Arg.Any<string>(), Arg.Any<RectangleF>()).Returns(fakeTextObjects);

            _sut.Show();

            _mockGOF.Received(1).CreateCreditLineItem(_mockEngine, new RectangleF(0, 0, 400, 300), mockTextLayer.MainSpriteBatch, fakeTextObjects[0]);
        }

        [TestMethod]
        public void ShowShouldCreateLineItemsForCredits_TestB()
        {
            ILayer mockTextLayer = Substitute.For<ILayer>();
            _mockGOF.CreateGenericLayer(Arg.Any<IEngine>(), Arg.Any<ISpriteBatch>(), Arg.Any<SpriteSortMode>(), null, null, null, null, null, Arg.Any<Matrix?>()).Returns(mockTextLayer);

            StringBuilder expectedCredits = new StringBuilder();
            foreach (string line in _testLines)
                expectedCredits.AppendLine(line);

            ITextObjectBuilder mockBuilder = Substitute.For<ITextObjectBuilder>();
            _mockGOF.CreateTextObjectBuilder(Arg.Any<ITextFormatProcessor>(), Arg.Any<ITextObjectFactory>()).Returns(mockBuilder);
            Construct();

            List<ITextObject> fakeTextObjects = new List<ITextObject>();
            fakeTextObjects.AddRange(
                new ITextObject[]
                {
                    Substitute.For<ITextObject>(),
                    Substitute.For<ITextObject>(),
                }
            );
            mockBuilder.Build(Arg.Any<string>(), Arg.Any<RectangleF>()).Returns(fakeTextObjects);

            _sut.Show();

            _mockGOF.Received(1).CreateCreditLineItem(_mockEngine, new RectangleF(0, 0, 400, 300), mockTextLayer.MainSpriteBatch, fakeTextObjects[1]);
        }

        [TestMethod]
        public void ShowShouldAddCreditLineItemsToTextLayerEntities_TestA()
        {
            ILayer mockTextLayer = Substitute.For<ILayer>();
            _mockGOF.CreateGenericLayer(Arg.Any<IEngine>(), Arg.Any<ISpriteBatch>(), Arg.Any<SpriteSortMode>(), null, null, null, null, null, Arg.Any<Matrix?>()).Returns(mockTextLayer);

            ICreditLineItem mockItemA = Substitute.For<ICreditLineItem>();
            ICreditLineItem mockItemB = Substitute.For<ICreditLineItem>();
            _mockGOF.CreateCreditLineItem(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ITextObject>()).Returns(mockItemA, mockItemB, Substitute.For<ICreditLineItem>());

            ITextObjectBuilder mockBuilder = Substitute.For<ITextObjectBuilder>();
            _mockGOF.CreateTextObjectBuilder(Arg.Any<ITextFormatProcessor>(), Arg.Any<ITextObjectFactory>()).Returns(mockBuilder);
            Construct();

            List<ITextObject> fakeTextObjects = new List<ITextObject>();
            fakeTextObjects.AddRange(
                new ITextObject[]
                {
                    Substitute.For<ITextObject>(),
                    Substitute.For<ITextObject>(),
                }
            );
            mockBuilder.Build(Arg.Any<string>(), Arg.Any<RectangleF>()).Returns(fakeTextObjects);

            _sut.Show();

            mockTextLayer.Received(1).AddEntity(mockItemA);
        }

        [TestMethod]
        public void ShowShouldAddCreditLineItemsToTextLayerEntities_TestB()
        {
            ILayer mockTextLayer = Substitute.For<ILayer>();
            _mockGOF.CreateGenericLayer(Arg.Any<IEngine>(), Arg.Any<ISpriteBatch>(), Arg.Any<SpriteSortMode>(), null, null, null, null, null, Arg.Any<Matrix?>()).Returns(mockTextLayer);

            ICreditLineItem mockItemA = Substitute.For<ICreditLineItem>();
            ICreditLineItem mockItemB = Substitute.For<ICreditLineItem>();
            _mockGOF.CreateCreditLineItem(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ITextObject>()).Returns(mockItemA, mockItemB, Substitute.For<ICreditLineItem>());

            ITextObjectBuilder mockBuilder = Substitute.For<ITextObjectBuilder>();
            _mockGOF.CreateTextObjectBuilder(Arg.Any<ITextFormatProcessor>(), Arg.Any<ITextObjectFactory>()).Returns(mockBuilder);
            Construct();

            List<ITextObject> fakeTextObjects = new List<ITextObject>();
            fakeTextObjects.AddRange(
                new ITextObject[]
                {
                    Substitute.For<ITextObject>(),
                    Substitute.For<ITextObject>(),
                }
            );
            mockBuilder.Build(Arg.Any<string>(), Arg.Any<RectangleF>()).Returns(fakeTextObjects);

            _sut.Show();

            mockTextLayer.Received(1).AddEntity(mockItemB);
        }
        #endregion

        #region Update Tests
        [TestMethod]
        public void UpdateShouldCallTextLayerUpdate()
        {
            ILayer mockTextLayer = Substitute.For<ILayer>();
            _mockGOF.CreateGenericLayer(Arg.Any<IEngine>(), Arg.Any<ISpriteBatch>(), Arg.Any<SpriteSortMode>(), null, null, null, null, null, Arg.Any<Matrix?>()).Returns(mockTextLayer);
            Construct();

            GameTime expectedGT = new GameTime();
            _sut.Update(expectedGT);

            mockTextLayer.Received(1).Update(expectedGT);
        }
        #endregion

        #region PreDraw Tests
        [TestMethod]
        public void PreDrawWithTextTargetShouldCallTextLayerDraw()
        {
            ILayer mockTextLayer = Substitute.For<ILayer>();
            _mockGOF.CreateGenericLayer(Arg.Any<IEngine>(), Arg.Any<ISpriteBatch>(), Arg.Any<SpriteSortMode>(), null, null, null, null, null, Arg.Any<Matrix?>()).Returns(mockTextLayer);

            IRenderTarget2D mockTarget = Substitute.For<IRenderTarget2D>();
            _mockEngine.GrfxFactory.RenderTargetFactory.Create(Arg.Any<IGrfxDevice>(), Arg.Any<int>(), Arg.Any<int>(), Arg.Any<RenderTargetUsage>()).Returns(mockTarget);

            Construct();
            _sut.Show();

            GameTime expected = new GameTime();
            _sut.PreDraw(expected);

            mockTextLayer.Received(1).Draw(expected, mockTarget, Color.Transparent);
        }

        [TestMethod]
        public void PreDrawWithoutTextTargetShouldNotCallTexLayerDraw()
        {
            ILayer mockTextLayer = Substitute.For<ILayer>();
            _mockGOF.CreateGenericLayer(Arg.Any<IEngine>(), Arg.Any<ISpriteBatch>(), Arg.Any<SpriteSortMode>(), null, null, null, null, null, Arg.Any<Matrix?>()).Returns(mockTextLayer);

            _mockEngine.GrfxFactory.RenderTargetFactory.Create(Arg.Any<IGrfxDevice>(), Arg.Any<int>(), Arg.Any<int>(), Arg.Any<RenderTargetUsage>()).Returns((IRenderTarget2D)null);

            Construct();
            _sut.Show();

            GameTime expected = new GameTime();
            _sut.PreDraw(expected);

            mockTextLayer.Received(0).Draw(Arg.Any<GameTime>(), Arg.Any<IRenderTarget2D>(), Arg.Any<Color>());
        }
        #endregion

        #region Draw Tests
        [TestMethod]
        public void DrawWithTextTargetShouldCallSpriteBatchDraw()
        {
            IRenderTarget2D mockTarget = Substitute.For<IRenderTarget2D>();
            _mockEngine.GrfxFactory.RenderTargetFactory.Create(Arg.Any<IGrfxDevice>(), Arg.Any<int>(), Arg.Any<int>(), Arg.Any<RenderTargetUsage>()).Returns(mockTarget);

            Construct();
            _sut.Show();

            _sut.Draw(new GameTime());

            _mockSpriteBatch.Received(1).Draw(mockTarget, _bounds.Location, Color.White);
        }

        [TestMethod]
        public void DrawWithoutTextTargetShouldNotcallSpriteBatchDraw()
        {
            _mockEngine.GrfxFactory.RenderTargetFactory.Create(Arg.Any<IGrfxDevice>(), Arg.Any<int>(), Arg.Any<int>(), Arg.Any<RenderTargetUsage>()).Returns((IRenderTarget2D)null);
            Construct();
            _sut.Show();

            _sut.Draw(new GameTime());

            _mockSpriteBatch.Received(0).Draw(Arg.Any<ITexture2D>(), Arg.Any<Vector2>(), Arg.Any<Color>());
        }
        #endregion
    }
}
