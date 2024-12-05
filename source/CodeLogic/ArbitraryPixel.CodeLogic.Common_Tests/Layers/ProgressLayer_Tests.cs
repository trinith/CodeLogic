using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArbitraryPixel.CodeLogic.Common.Layers;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Common.Graphics;
using NSubstitute;
using ArbitraryPixel.CodeLogic.Common.Entities;
using ArbitraryPixel.CodeLogic.Common;
using ArbitraryPixel.Common.Drawing;
using Microsoft.Xna.Framework;
using ArbitraryPixel.Platform2D.UI;
using System.Linq;

namespace ArbitraryPixel.CodeLogic.Common_Tests.Layers
{
    [TestClass]
    public class ProgressLayer_Tests
    {
        private ProgressLayer _sut;
        private IEngine _mockEngine;
        private ISpriteBatch _mockSpriteBatch;
        private ISpriteFont _mockFont;
        private GameObjectFactory _mockGameObjectFactory;
        private IProgressBar _mockProgressBar;

        [TestInitialize]
        public void Initialize()
        {
            _mockEngine = Substitute.For<IEngine>();
            _mockSpriteBatch = Substitute.For<ISpriteBatch>();
            _mockFont = Substitute.For<ISpriteFont>();

            _mockGameObjectFactory = Substitute.For<GameObjectFactory>();
            GameObjectFactory.SetInstance(_mockGameObjectFactory);

            _mockProgressBar = Substitute.For<IProgressBar>();
            _mockGameObjectFactory.CreateProgressBar(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>()).Returns(_mockProgressBar);
        }

        private void Construct(string titleText = "Test", bool useNullFont = false)
        {
            _sut = new ProgressLayer(_mockEngine, _mockSpriteBatch, (useNullFont == false) ? _mockFont : null, titleText);
        }

        #region Construct Tests - Normal
        [TestMethod]
        public void ConstructShouldCreateProgressBar()
        {
            IProgressBar mockProgressBar = Substitute.For<IProgressBar>();
            _mockGameObjectFactory.CreateProgressBar(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>()).Returns(mockProgressBar);
            _mockEngine.ScreenManager.World.Returns(new Point(1000, 500));

            Construct();

            _mockGameObjectFactory.Received(1).CreateProgressBar(_mockEngine, new RectangleF(200, 237.5f, 600, 25), _mockSpriteBatch);
        }

        [TestMethod]
        public void ConstructShouldAddProgressBackToEntities()
        {
            IProgressBar mockProgressBar = Substitute.For<IProgressBar>();
            _mockGameObjectFactory.CreateProgressBar(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>()).Returns(mockProgressBar);
            _mockEngine.ScreenManager.World.Returns(new Point(1000, 500));

            Construct();

            Assert.IsTrue(_sut.Entities.Contains(mockProgressBar));
        }

        [TestMethod]
        public void ConstructWithTitleAndFontShouldMeasureString()
        {
            Construct();

            _mockFont.Received(1).MeasureString("Test");
        }

        [TestMethod]
        public void ConstructWithTitleAndFontShouldCreateTextLabel()
        {
            _mockFont.MeasureString("Test").Returns(new SizeF(50, 25));
            _mockEngine.ScreenManager.World.Returns(new Point(1000, 500));
            Construct();

            _mockGameObjectFactory.Received(1).CreateGenericTextLabel(
                _mockEngine,
                new Vector2(475, 237.5f - 10f - 25f),
                _mockSpriteBatch,
                _mockFont,
                "Test",
                Color.White
            );
        }

        [TestMethod]
        public void ConstructWithTitleAndFontShouldAddTextLabelToEntities()
        {
            ITextLabel mockLabel = Substitute.For<ITextLabel>();
            _mockGameObjectFactory.CreateGenericTextLabel(Arg.Any<IEngine>(), Arg.Any<Vector2>(), Arg.Any<ISpriteBatch>(), Arg.Any<ISpriteFont>(), Arg.Any<string>(), Arg.Any<Color>()).Returns(mockLabel);

            Construct();

            Assert.IsTrue(_sut.Entities.Contains(mockLabel));
        }
        #endregion

        #region Construct Tests - Null Font
        [TestMethod]
        public void ConstructWithNullFontShouldNotAddTextLabelEntity()
        {
            ITextLabel mockLabel = Substitute.For<ITextLabel>();
            _mockGameObjectFactory.CreateGenericTextLabel(Arg.Any<IEngine>(), Arg.Any<Vector2>(), Arg.Any<ISpriteBatch>(), Arg.Any<ISpriteFont>(), Arg.Any<string>(), Arg.Any<Color>()).Returns(mockLabel);

            Construct("Test", true);

            Assert.IsFalse(_sut.Entities.Contains(mockLabel));
        }
        #endregion

        #region Construct Tests - Empty Title
        [TestMethod]
        public void ConstructWithEmptyTitleShouldNotAddTextLabelEntity()
        {
            ITextLabel mockLabel = Substitute.For<ITextLabel>();
            _mockGameObjectFactory.CreateGenericTextLabel(Arg.Any<IEngine>(), Arg.Any<Vector2>(), Arg.Any<ISpriteBatch>(), Arg.Any<ISpriteFont>(), Arg.Any<string>(), Arg.Any<Color>()).Returns(mockLabel);

            Construct("");

            Assert.IsFalse(_sut.Entities.Contains(mockLabel));
        }
        #endregion

        #region Property Tests
        [TestMethod]
        public void PropertyGetShouldReturnValueFromProgressBar_Minimum()
        {
            _mockProgressBar.Minimum.Returns(10);
            Construct();
            Assert.AreEqual<float>(10, _sut.Minimum);
        }

        [TestMethod]
        public void PropertySetShouldSetValueOnProgressBar_Minimum()
        {
            Construct();
            _sut.Minimum = 10;
            _mockProgressBar.Received(1).Minimum = 10;
        }

        [TestMethod]
        public void PropertyGetShouldReturnValueFromProgressBar_Maximum()
        {
            _mockProgressBar.Maximum.Returns(10);
            Construct();
            Assert.AreEqual<float>(10, _sut.Maximum);
        }

        [TestMethod]
        public void PropertySetShouldSetValueOnProgressBar_Maximum()
        {
            Construct();
            _sut.Maximum = 10;
            _mockProgressBar.Received(1).Maximum = 10;
        }

        [TestMethod]
        public void PropertyGetShouldReturnValueFromProgressBar_Value ()
        {
            _mockProgressBar.Value.Returns(10);
            Construct();
            Assert.AreEqual<float>(10, _sut.Value);
        }

        [TestMethod]
        public void PropertySetShouldSetValueOnProgressBar_Value()
        {
            Construct();
            _sut.Value = 10;
            _mockProgressBar.Received(1).Value = 10;
        }

        [TestMethod]
        public void ProgressBarBoundsShouldReturnBoundsOfProgressBar()
        {
            IProgressBar mockProgressBar = Substitute.For<IProgressBar>();
            mockProgressBar.Bounds.Returns(new RectangleF(1, 2, 3, 4));
            _mockGameObjectFactory.CreateProgressBar(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>()).Returns(mockProgressBar);
            _mockEngine.ScreenManager.World.Returns(new Point(1000, 500));
            Construct();

            Assert.AreEqual<RectangleF>(new RectangleF(1, 2, 3, 4), _sut.ProgressBarBounds);
        }
        #endregion
    }
}
