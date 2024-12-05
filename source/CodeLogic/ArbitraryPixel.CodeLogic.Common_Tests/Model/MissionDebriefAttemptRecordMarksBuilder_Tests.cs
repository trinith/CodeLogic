using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArbitraryPixel.CodeLogic.Common.Model;
using ArbitraryPixel.CodeLogic.Common.Entities;
using ArbitraryPixel.Common;
using NSubstitute;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Common.Graphics;
using Microsoft.Xna.Framework;
using ArbitraryPixel.Common.Drawing;
using System.Linq;

namespace ArbitraryPixel.CodeLogic.Common_Tests.Model
{
    [TestClass]
    public class MissionDebriefAttemptRecordMarksBuilder_Tests
    {
        private MissionDebriefAttemptRecordMarksBuilder _sut;
        private ITextureEntityFactory _mockFactory;
        private IRandom _mockRandom;
        private IEngine _mockEngine;
        private IDeviceModel _mockModel;
        private ISpriteBatch _mockSpriteBatch;
        private ITexture2D _mockMarkTexture;
        private ITexture2D _mockEqualityTexture;

        [TestInitialize]
        public void Initialize()
        {
            _mockRandom = Substitute.For<IRandom>();
            _mockFactory = Substitute.For<ITextureEntityFactory>();
            _mockEngine = Substitute.For<IEngine>();
            _mockModel = Substitute.For<IDeviceModel>();
            _mockSpriteBatch = Substitute.For<ISpriteBatch>();

            ICodeSequence mockSequence = Substitute.For<ICodeSequence>();
            _mockModel.TargetSequence.Returns(mockSequence);

            mockSequence.Length.Returns(2);
            mockSequence.Code.Returns(new CodeValue[] { CodeValue.Red, CodeValue.Blue });

            _mockModel.CodeColourMap.GetColour(Arg.Any<CodeValue>()).Returns(Color.Pink, Color.Purple);

            _mockMarkTexture = Substitute.For<ITexture2D>();
            _mockEqualityTexture = Substitute.For<ITexture2D>();
            _mockEngine.AssetBank.Get<ITexture2D>("MissionDebriefMarks").Returns(_mockMarkTexture);
            _mockEngine.AssetBank.Get<ITexture2D>("MissionDebriefEqualityMarks").Returns(_mockEqualityTexture);

            _mockMarkTexture.Width.Returns(100);
            _mockMarkTexture.Height.Returns(25);

            _mockEqualityTexture.Width.Returns(50);
            _mockEqualityTexture.Height.Returns(25);
        }

        private void Construct()
        {
            _sut = new MissionDebriefAttemptRecordMarksBuilder(_mockEngine, _mockRandom, _mockFactory);
        }

        #region Constructor Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowException_Host()
        {
            _sut = new MissionDebriefAttemptRecordMarksBuilder(null, _mockRandom, _mockFactory);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowException_Random()
        {
            _sut = new MissionDebriefAttemptRecordMarksBuilder(_mockEngine, null, _mockFactory);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowException_Factory()
        {
            _sut = new MissionDebriefAttemptRecordMarksBuilder(_mockEngine, _mockRandom, null);
        }

        [TestMethod]
        public void ConstructorShouldGetTextureFromBank_MissionDebriefMarks()
        {
            Construct();

            _mockEngine.AssetBank.Received(1).Get<ITexture2D>("MissionDebriefMarks");
        }

        [TestMethod]
        public void ConstructorShouldGetTextureFromBank_MissionDebriefEqualityMarks()
        {
            Construct();

            _mockEngine.AssetBank.Received(1).Get<ITexture2D>("MissionDebriefEqualityMarks");
        }
        #endregion

        #region CreateMarksForAttemptRecord Tests
        [TestMethod]
        public void CreateMarksForAttemptRecordShouldCreateExpectedTextureEntities()
        {
            Construct();

            ISequenceAttemptRecord mockAttemptRecord = Substitute.For<ISequenceAttemptRecord>();
            mockAttemptRecord.Code.Returns(new CodeValue[] { CodeValue.Red, CodeValue.Green });
            ICodeValueColourMap mockColourMap = Substitute.For<ICodeValueColourMap>();
            mockColourMap.GetColour(Arg.Any<CodeValue>()).Returns(Color.Pink, Color.Purple);
            _mockRandom.Next(Arg.Any<int>(), Arg.Any<int>()).Returns(2, 0);

            _sut.CreateMarksForAttemptRecord(mockAttemptRecord, mockColourMap, new Vector2(100, 50), _mockSpriteBatch);

            Received.InOrder(
                () =>
                {
                    _mockFactory.Create(
                        _mockEngine,
                        new RectangleF(100, 50, 25, 25),
                        _mockSpriteBatch,
                        _mockMarkTexture,
                        Color.Pink,
                        new Rectangle(50, 0, 25, 25)
                    );

                     _mockFactory.Create(
                        _mockEngine,
                        new RectangleF(125, 50, 25, 25),
                        _mockSpriteBatch,
                        _mockMarkTexture,
                        Color.Purple,
                        new Rectangle(0, 0, 25, 25)
                    );
                }
            );
        }

        [TestMethod]
        public void CreateMarksForAttemptRecordShouldReturnExpectedArray()
        {
            Construct();

            ISequenceAttemptRecord mockAttemptRecord = Substitute.For<ISequenceAttemptRecord>();
            mockAttemptRecord.Code.Returns(new CodeValue[] { CodeValue.Red, CodeValue.Green });
            ICodeValueColourMap mockColourMap = Substitute.For<ICodeValueColourMap>();

            ITextureEntity mockEntityA = Substitute.For<ITextureEntity>();
            ITextureEntity mockEntityB = Substitute.For<ITextureEntity>();
            _mockFactory.Create(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ITexture2D>(), Arg.Any<Color>(), Arg.Any<Rectangle?>()).Returns(mockEntityA, mockEntityB);

            ITextureEntity[] actual = _sut.CreateMarksForAttemptRecord(mockAttemptRecord, mockColourMap, new Vector2(100, 50), _mockSpriteBatch);

            Assert.IsTrue(
                actual.SequenceEqual(
                    new ITextureEntity[]
                    {
                        mockEntityA,
                        mockEntityB,
                    }
                )
            );
        }
        #endregion

        #region CreateEqualityMarksForAttemptRecord Tests
        [TestMethod]
        public void CreateEqualityMarksForAttemptRecordShouldCreateExpectedTextureEntities()
        {
            Construct();

            ISequenceAttemptRecord mockRecord = Substitute.For<ISequenceAttemptRecord>();
            mockRecord.Result.Returns(new SequenceIndexCompareResult[] { SequenceIndexCompareResult.Equal, SequenceIndexCompareResult.PartialEqual, SequenceIndexCompareResult.PartialEqual, SequenceIndexCompareResult.Equal });

            _sut.CreateEqualityMarksForAttemptRecord(mockRecord, new Vector2(100, 50), _mockSpriteBatch);

            Received.InOrder(
                () =>
                {
                    _mockFactory.Create(
                        _mockEngine,
                        new RectangleF(100, 50, 25, 25),
                        _mockSpriteBatch,
                        _mockEqualityTexture,
                        Color.White,
                        new Rectangle(25, 0, 25, 25)
                    );

                    _mockFactory.Create(
                        _mockEngine,
                        new RectangleF(126, 50, 25, 25),
                        _mockSpriteBatch,
                        _mockEqualityTexture,
                        Color.White,
                        new Rectangle(0, 0, 25, 25)
                    );

                    _mockFactory.Create(
                        _mockEngine,
                        new RectangleF(100, 76, 25, 25),
                        _mockSpriteBatch,
                        _mockEqualityTexture,
                        Color.White,
                        new Rectangle(0, 0, 25, 25)
                    );

                    _mockFactory.Create(
                        _mockEngine,
                        new RectangleF(126, 76, 25, 25),
                        _mockSpriteBatch,
                        _mockEqualityTexture,
                        Color.White,
                        new Rectangle(25, 0, 25, 25)
                    );
                }
            );
        }

        [TestMethod]
        public void CreateEqualityMarksForAttemptRecordShouldReturnExpectedArray()
        {
            Construct();

            ISequenceAttemptRecord mockRecord = Substitute.For<ISequenceAttemptRecord>();
            mockRecord.Result.Returns(new SequenceIndexCompareResult[] { SequenceIndexCompareResult.Equal, SequenceIndexCompareResult.PartialEqual, SequenceIndexCompareResult.NotEqual, SequenceIndexCompareResult.NotEqual });

            ITextureEntity mockEntityA = Substitute.For<ITextureEntity>();
            ITextureEntity mockEntityB = Substitute.For<ITextureEntity>();
            _mockFactory.Create(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ITexture2D>(), Arg.Any<Color>(), Arg.Any<Rectangle?>()).Returns(mockEntityA, mockEntityB);

            ITextureEntity[] actual = _sut.CreateEqualityMarksForAttemptRecord(mockRecord, new Vector2(100, 50), _mockSpriteBatch);

            Assert.IsTrue(
                actual.SequenceEqual(
                    new ITextureEntity[]
                    {
                        mockEntityA,
                        mockEntityB,
                    }
                )
            );
        }
        #endregion
    }
}
