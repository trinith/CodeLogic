using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArbitraryPixel.CodeLogic.Common.Layers;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Common.Graphics;
using NSubstitute;
using ArbitraryPixel.Platform2D.Text;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ArbitraryPixel.CodeLogic.Common.Model;
using ArbitraryPixel.Common;
using ArbitraryPixel.Common.Drawing;

namespace ArbitraryPixel.CodeLogic.Common_Tests.Layers
{
    [TestClass]
    public class BuildInfoOverlayLayer_Tests
    {
        private const int UPDATE_LOW = 15;
        private const int UPDATE_HIGH = 45;

        private BuildInfoOverlayLayer _sut;

        private IEngine _mockEngine;
        private ISpriteBatch _mockSpriteBatch;
        private IBuildInfoOverlayLayerModel _mockModel;
        private IRandom _mockRandom;

        [TestInitialize]
        public void Initialize()
        {
            _mockEngine = Substitute.For<IEngine>();
            _mockSpriteBatch = Substitute.For<ISpriteBatch>();
            _mockModel = Substitute.For<IBuildInfoOverlayLayerModel>();
            _mockRandom = Substitute.For<IRandom>();

            _mockEngine.ScreenManager.World.Returns(new Point(1000, 750));
            _mockModel.TextSize.Returns(new SizeF(100, 50));

            _mockRandom.Next(Arg.Any<int>(), Arg.Any<int>()).Returns(11, 22, 33);

            _sut = new BuildInfoOverlayLayer(_mockEngine, _mockSpriteBatch, _mockModel, _mockRandom);
        }

        #region Constructor Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullModelShouldThrowException()
        {
            _sut = new BuildInfoOverlayLayer(_mockEngine, _mockSpriteBatch, null, _mockRandom);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullRandomShouldThrowException()
        {
            _sut = new BuildInfoOverlayLayer(_mockEngine, _mockSpriteBatch, _mockModel, null);
        }

        [TestMethod]
        public void ConstructShouldSetSpriteSortModeDeferred()
        {
            Assert.AreEqual<SpriteSortMode>(SpriteSortMode.Deferred, _sut.SpriteSortMode);
        }

        [TestMethod]
        public void ConstructShouldSetBlendStateNonPreMultiplied()
        {
            Assert.AreEqual<BlendState>(BlendState.NonPremultiplied, _sut.BlendState);
        }

        [TestMethod]
        public void ConstructShouldRequestExpectedRandomNumbers()
        {
            Received.InOrder(
                () =>
                {
                    _mockRandom.Next(0, 900);
                    _mockRandom.Next(0, 700);
                }
            );
        }

        [TestMethod]
        public void ConstructShouldSetModelTextAnchorToExpectedValue()
        {
            _mockModel.Received(1).TextAnchor = new Vector2(11, 22);
        }

        [TestMethod]
        public void ConstructShouldCreateRandomIntervalUntilNextUpdate()
        {
            _mockRandom.Received(1).Next(UPDATE_LOW, UPDATE_HIGH);
        }
        #endregion

        #region Update Tests
        [TestMethod]
        public void UpdateAfterIntervalExpiresShouldMoveModelTextAnchor()
        {
            _mockRandom.Next(Arg.Any<int>(), Arg.Any<int>()).Returns(38, 44, 55);

            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(33)));

            _mockModel.Received(1).TextAnchor = new Vector2(44, 55);
        }

        [TestMethod]
        public void UpdateAfterIntervalExpiresShouldGenerateExpectedNumberSet()
        {
            _mockRandom.ClearReceivedCalls();
            _mockRandom.Next(Arg.Any<int>(), Arg.Any<int>()).Returns(38, 44, 55);

            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(33)));

            Received.InOrder(
                () =>
                {
                    _mockRandom.Next(UPDATE_LOW, UPDATE_HIGH);
                    _mockRandom.Next(0, 900);
                    _mockRandom.Next(0, 700);
                }
            );
        }
        #endregion

        #region Draw Tests
        [TestMethod]
        public void DrawShouldPerformExpectedTasks()
        {
            List<ITextObject> textObjects = new List<ITextObject>();
            _mockModel.TextObjects.Returns(textObjects);

            ITextObject mockObjA = Substitute.For<ITextObject>();
            ITextObject mockObjB = Substitute.For<ITextObject>();
            textObjects.Add(mockObjA);
            textObjects.Add(mockObjB);

            Matrix mockMatrix = Matrix.CreateScale(0.75f);
            _mockEngine.ScreenManager.ScaleMatrix.Returns(mockMatrix);

            _sut.Draw(new GameTime());

            Received.InOrder(
                () =>
                {
                    _mockSpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, null, null, mockMatrix);
                    _mockSpriteBatch.DrawString(mockObjA.TextDefinition.Font, mockObjA.TextDefinition.Text, mockObjA.Location, mockObjA.TextDefinition.Colour);
                    _mockSpriteBatch.DrawString(mockObjB.TextDefinition.Font, mockObjB.TextDefinition.Text, mockObjB.Location, mockObjB.TextDefinition.Colour);
                    _mockSpriteBatch.End();
                }
            );
        }
        #endregion
    }
}
