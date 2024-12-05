using ArbitraryPixel.CodeLogic.Common.Model;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArbitraryPixel.CodeLogic.Common_Tests.Layers
{
    [TestClass]
    public class BuildInfoOverlayLayerModel_Tests
    {
        private BuildInfoOverlayLayerModel _sut;
        private IBuildInfoStore _mockBuildInfoStore;
        private ITextObjectBuilder _mockTextBuilder;
        private List<ITextObject> _mockTextObjects;

        [TestInitialize]
        public void Initialize()
        {
            _mockBuildInfoStore = Substitute.For<IBuildInfoStore>();
            _mockTextBuilder = Substitute.For<ITextObjectBuilder>();

            _mockBuildInfoStore.Platform.Returns("SomePlatform");
            _mockBuildInfoStore.AssemblyTitle.Returns("SomeTitle");
            _mockBuildInfoStore.Version.Returns("SomeVersion");
            _mockBuildInfoStore.Date.Returns("SomeDate");

            _mockTextObjects = new List<ITextObject>();
            _mockTextBuilder.Build(Arg.Any<string>(), Arg.Any<RectangleF>()).Returns(_mockTextObjects);

            ISpriteFont mockFont = Substitute.For<ISpriteFont>();
            mockFont.MeasureString("ObjA").Returns(new SizeF(100, 20));
            mockFont.MeasureString("ObjB").Returns(new SizeF(50, 10));

            ITextObject textObjA = Substitute.For<ITextObject>();
            textObjA.TextDefinition.Font.Returns(mockFont);
            textObjA.TextDefinition.Text.Returns("ObjA");
            textObjA.Location.Returns(Vector2.Zero);
            ITextObject textObjB = Substitute.For<ITextObject>();
            textObjB.TextDefinition.Font.Returns(mockFont);
            textObjB.TextDefinition.Text.Returns("ObjB");
            textObjB.Location.Returns(new Vector2(0, 20));

            _mockTextObjects.Add(textObjA);
            _mockTextObjects.Add(textObjB);

            _sut = new BuildInfoOverlayLayerModel(_mockBuildInfoStore, _mockTextBuilder);
        }

        #region Constructor Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullStoreShouldThrowException()
        {
            _sut = new BuildInfoOverlayLayerModel(null, _mockTextBuilder);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullTextBuilderShouldThrowException()
        {
            _sut = new BuildInfoOverlayLayerModel(_mockBuildInfoStore, null);
        }

        [TestMethod]
        public void ConstructShouldCallTextBuilderBuild()
        {
            StringBuilder expectedText = new StringBuilder();
            expectedText.Append("{Alignment:Centre}{C:255, 255, 255, 64}");
            expectedText.AppendLine("SomeTitle (SomePlatform)");
            expectedText.AppendLine("SomeVersion - SomeDate");
            expectedText.AppendLine("DO NOT DISTRIBUTE");

            _mockTextBuilder.Received(1).Build(expectedText.ToString(), new RectangleF(Vector2.Zero, new SizeF(1)));
        }

        [TestMethod]
        public void ConstructShouldShiftTextObjectsToAlignWithEdge_ObjectA()
        {
            Vector2 expectedOffset = new Vector2(50, 0);

            _mockTextObjects[0].Received(1).Location = Vector2.Zero + expectedOffset;
        }

        [TestMethod]
        public void ConstructShouldShiftTextObjectsToAlignWithEdge_ObjectB()
        {
            Vector2 expectedOffset = new Vector2(50, 0);

            _mockTextObjects[1].Received(1).Location = new Vector2(0, 20) + expectedOffset;
        }
        #endregion

        #region Property Tests
        [TestMethod]
        public void TextAnchorShouldDefaultZero()
        {
            Assert.AreEqual<Vector2>(Vector2.Zero, _sut.TextAnchor);
        }

        [TestMethod]
        public void TextAnchorShouldReturnSetValue()
        {
            _sut.TextAnchor = new Vector2(11, 22);

            Assert.AreEqual<Vector2>(new Vector2(11, 22), _sut.TextAnchor);
        }

        [TestMethod]
        public void TextAnchorShouldOffsetTextObjectsByOffsetBasedOnPreviousPosition_ObjA()
        {
            _mockTextObjects[0].ClearReceivedCalls();

            _sut.TextAnchor = new Vector2(11, 22);

            _mockTextObjects[0].Received(1).Location = new Vector2(61, 22);
        }

        [TestMethod]
        public void TextAnchorShouldOffsetTextObjectsByOffsetBasedOnPreviousPosition_ObjB()
        {
            _mockTextObjects[1].ClearReceivedCalls();

            _sut.TextAnchor = new Vector2(11, 22);

            _mockTextObjects[1].Received(1).Location = new Vector2(61, 42);
        }

        [TestMethod]
        public void TextObjectsShouldContainExpectedObjects_ObjA()
        {
            Assert.IsTrue(_sut.TextObjects[0] == _mockTextObjects[0]);
        }

        [TestMethod]
        public void TextObjectsShouldContainExpectedObjects_ObjB()
        {
            Assert.IsTrue(_sut.TextObjects[1] == _mockTextObjects[1]);
        }
        #endregion

        #region TextSize Tests
        [TestMethod]
        public void TextSizeShouldReturnExpectedSizeForText_TestA()
        {
            Assert.AreEqual<SizeF>(new SizeF(100, 30), _sut.TextSize);
        }

        [TestMethod]
        public void TextSizeShouldReturnExpectedSizeForText_TestB()
        {
            _mockTextObjects[1].TextDefinition.Font.MeasureString("ObjB").Returns(new SizeF(150, 30));
            Assert.AreEqual<SizeF>(new SizeF(150, 50), _sut.TextSize);
        }
        #endregion
    }
}
