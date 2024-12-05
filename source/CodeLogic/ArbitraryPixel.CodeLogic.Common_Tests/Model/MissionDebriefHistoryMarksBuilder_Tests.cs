using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArbitraryPixel.CodeLogic.Common.Model;
using ArbitraryPixel.CodeLogic.Common.Entities;
using ArbitraryPixel.Common;
using ArbitraryPixel.Platform2D.Engine;
using NSubstitute;
using ArbitraryPixel.Common.Graphics;
using Microsoft.Xna.Framework;
using ArbitraryPixel.Common.Drawing;
using System.Linq;
using System.Collections.Generic;

namespace ArbitraryPixel.CodeLogic.Common_Tests.Model
{
    [TestClass]
    public class MissionDebriefHistoryMarksBuilder_Tests
    {
        private MissionDebriefHistoryMarksBuilder _sut;
        private ITextureEntityFactory _mockFactory;
        private IRandom _mockRandom;
        private IEngine _mockEngine;
        private IMissionDebriefAttemptRecordMarksBuilder _mockAttemptRecordBuilder;
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
            _mockAttemptRecordBuilder = Substitute.For<IMissionDebriefAttemptRecordMarksBuilder>();

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
            _sut = new MissionDebriefHistoryMarksBuilder(_mockEngine, _mockRandom, _mockFactory, _mockAttemptRecordBuilder);
        }

        #region Constructor Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowException_Host()
        {
            _sut = new MissionDebriefHistoryMarksBuilder(null, _mockRandom, _mockFactory, _mockAttemptRecordBuilder);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowException_Random()
        {
            _sut = new MissionDebriefHistoryMarksBuilder(_mockEngine, null, _mockFactory, _mockAttemptRecordBuilder);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowException_Factory()
        {
            _sut = new MissionDebriefHistoryMarksBuilder(_mockEngine, _mockRandom, null, _mockAttemptRecordBuilder);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowException_AttemptRecordBuilder()
        {
            _sut = new MissionDebriefHistoryMarksBuilder(_mockEngine, _mockRandom, _mockFactory, null);
        }

        [TestMethod]
        public void ConstructorShouldGetTextureFromBank_MissionDebriefMarks()
        {
            Construct();

            _mockEngine.AssetBank.Received(1).Get<ITexture2D>("MissionDebriefMarks");
        }
        #endregion

        #region CreateAttemptHistoryMarks Tests
        [TestMethod]
        public void CreateAttemptHistoryMarksShouldCallCreateMarksForAttemptRecordExpectedNumberOfTimes()
        {
            Construct();
            ISequenceAttemptCollection mockAttemptCollection = Substitute.For<ISequenceAttemptCollection>();
            _mockModel.Attempts.Returns(mockAttemptCollection);

            List<ISequenceAttemptRecord> attempts = new List<ISequenceAttemptRecord>();
            for (int i = 0; i < 10; i++)
            {
                Action a = () =>
                {
                    ISequenceAttemptRecord record = Substitute.For<ISequenceAttemptRecord>();
                    record.Code.Returns(new CodeValue[] { CodeValue.Red, CodeValue.Green, CodeValue.Blue, CodeValue.Yellow });
                    record.Result.Returns(new SequenceIndexCompareResult[] { SequenceIndexCompareResult.Equal, SequenceIndexCompareResult.PartialEqual, SequenceIndexCompareResult.NotEqual, SequenceIndexCompareResult.NotEqual });
                    attempts.Add(record);
                };

                a();
            }
            mockAttemptCollection.GetEnumerator().Returns(attempts.GetEnumerator());

            _sut.CreateAttemptHistoryMarks(_mockModel, new Vector2(100, 50), _mockSpriteBatch);

            _mockAttemptRecordBuilder.Received(10).CreateEqualityMarksForAttemptRecord(Arg.Any<ISequenceAttemptRecord>(), Arg.Any<Vector2>(), _mockSpriteBatch);
        }

        [TestMethod]
        public void CreateAtytemptHistoryMarksShouldCreateMarksForRecordWithExpectedAnchors()
        {
            Construct();
            ISequenceAttemptCollection mockAttemptCollection = Substitute.For<ISequenceAttemptCollection>();
            _mockModel.Attempts.Returns(mockAttemptCollection);

            List<ISequenceAttemptRecord> attempts = new List<ISequenceAttemptRecord>();
            for (int i = 0; i < 10; i++)
            {
                Action a = () =>
                {
                    ISequenceAttemptRecord record = Substitute.For<ISequenceAttemptRecord>();
                    record.Code.Returns(new CodeValue[] { CodeValue.Red, CodeValue.Green, CodeValue.Blue, CodeValue.Yellow });
                    record.Result.Returns(new SequenceIndexCompareResult[] { SequenceIndexCompareResult.Equal, SequenceIndexCompareResult.PartialEqual, SequenceIndexCompareResult.NotEqual, SequenceIndexCompareResult.NotEqual });
                    attempts.Add(record);
                };

                a();
            }
            mockAttemptCollection.GetEnumerator().Returns(attempts.GetEnumerator());

            _sut.CreateAttemptHistoryMarks(_mockModel, new Vector2(100, 50), _mockSpriteBatch);

            Received.InOrder(
                () =>
                {
                    _mockAttemptRecordBuilder.CreateMarksForAttemptRecord(Arg.Any<ISequenceAttemptRecord>(), Arg.Any<ICodeValueColourMap>(), new Vector2(100, 50 + 0 * 26), Arg.Any<ISpriteBatch>());
                    _mockAttemptRecordBuilder.CreateMarksForAttemptRecord(Arg.Any<ISequenceAttemptRecord>(), Arg.Any<ICodeValueColourMap>(), new Vector2(100, 50 + 1 * 26), Arg.Any<ISpriteBatch>());
                    _mockAttemptRecordBuilder.CreateMarksForAttemptRecord(Arg.Any<ISequenceAttemptRecord>(), Arg.Any<ICodeValueColourMap>(), new Vector2(100, 50 + 2 * 26), Arg.Any<ISpriteBatch>());
                    _mockAttemptRecordBuilder.CreateMarksForAttemptRecord(Arg.Any<ISequenceAttemptRecord>(), Arg.Any<ICodeValueColourMap>(), new Vector2(100, 50 + 3 * 26), Arg.Any<ISpriteBatch>());
                    _mockAttemptRecordBuilder.CreateMarksForAttemptRecord(Arg.Any<ISequenceAttemptRecord>(), Arg.Any<ICodeValueColourMap>(), new Vector2(100, 50 + 4 * 26), Arg.Any<ISpriteBatch>());

                    _mockAttemptRecordBuilder.CreateMarksForAttemptRecord(Arg.Any<ISequenceAttemptRecord>(), Arg.Any<ICodeValueColourMap>(), new Vector2(253, 50 + 0 * 26), Arg.Any<ISpriteBatch>());
                    _mockAttemptRecordBuilder.CreateMarksForAttemptRecord(Arg.Any<ISequenceAttemptRecord>(), Arg.Any<ICodeValueColourMap>(), new Vector2(253, 50 + 1 * 26), Arg.Any<ISpriteBatch>());
                    _mockAttemptRecordBuilder.CreateMarksForAttemptRecord(Arg.Any<ISequenceAttemptRecord>(), Arg.Any<ICodeValueColourMap>(), new Vector2(253, 50 + 2 * 26), Arg.Any<ISpriteBatch>());
                    _mockAttemptRecordBuilder.CreateMarksForAttemptRecord(Arg.Any<ISequenceAttemptRecord>(), Arg.Any<ICodeValueColourMap>(), new Vector2(253, 50 + 3 * 26), Arg.Any<ISpriteBatch>());
                    _mockAttemptRecordBuilder.CreateMarksForAttemptRecord(Arg.Any<ISequenceAttemptRecord>(), Arg.Any<ICodeValueColourMap>(), new Vector2(253, 50 + 4 * 26), Arg.Any<ISpriteBatch>());
                }
            );
        }

        [TestMethod]
        public void CreateAttemptHistoryMarksShouldReturnExpectedArray()
        {
            Construct();
            ISequenceAttemptCollection mockAttemptCollection = Substitute.For<ISequenceAttemptCollection>();
            _mockModel.Attempts.Returns(mockAttemptCollection);

            List<ISequenceAttemptRecord> attempts = new List<ISequenceAttemptRecord>();
            for (int i = 0; i < 10; i++)
            {
                Action a = () =>
                {
                    ISequenceAttemptRecord record = Substitute.For<ISequenceAttemptRecord>();
                    record.Code.Returns(new CodeValue[] { CodeValue.Red, CodeValue.Green, CodeValue.Blue, CodeValue.Yellow });
                    record.Result.Returns(new SequenceIndexCompareResult[] { SequenceIndexCompareResult.Equal, SequenceIndexCompareResult.PartialEqual, SequenceIndexCompareResult.NotEqual, SequenceIndexCompareResult.NotEqual });
                    attempts.Add(record);
                };

                a();
            }
            mockAttemptCollection.GetEnumerator().Returns(attempts.GetEnumerator());

            List<ITextureEntity> expectedMarks = new List<ITextureEntity>();
            List<ITextureEntity> expectedEquality = new List<ITextureEntity>();
            List<ITextureEntity> expected = new List<ITextureEntity>();
            for (int i = 0; i < 10; i++)
            {
                ITextureEntity mockMark = Substitute.For<ITextureEntity>();
                ITextureEntity mockEquality = Substitute.For<ITextureEntity>();

                expectedMarks.Add(mockMark);
                expectedEquality.Add(mockEquality);
                expected.Add(mockMark);
                expected.Add(mockEquality);
            }
            int markInc = 0;
            int equalityInc = 0;
            _mockAttemptRecordBuilder.CreateMarksForAttemptRecord(Arg.Any<ISequenceAttemptRecord>(), Arg.Any<ICodeValueColourMap>(), Arg.Any<Vector2>(), Arg.Any<ISpriteBatch>()).Returns(
                x =>
                {
                    return new ITextureEntity[] { expectedMarks[markInc++] };
                }
            );
            _mockAttemptRecordBuilder.CreateEqualityMarksForAttemptRecord(Arg.Any<ISequenceAttemptRecord>(), Arg.Any<Vector2>(), Arg.Any<ISpriteBatch>()).Returns(
                x =>
                {
                    return new ITextureEntity[] { expectedEquality[equalityInc++] };
                }
            );

            ITextureEntity[] actual = _sut.CreateAttemptHistoryMarks(_mockModel, new Vector2(100, 50), _mockSpriteBatch);

            Assert.IsTrue(actual.SequenceEqual<ITextureEntity>(expected));
        }
        #endregion

        #region CreateFinalCodeMarks Tests
        [TestMethod]
        public void CreateFinalCodeMarksShouldCreateExpectedTextureEntities()
        {
            Construct();
            _mockRandom.Next(Arg.Any<int>(), Arg.Any<int>()).Returns(0, 1);

            _sut.CreateFinalCodeMarks(_mockModel, new Vector2(100, 200), _mockSpriteBatch);

            Received.InOrder(
                () =>
                {
                    _mockFactory.Create(
                        _mockEngine,
                        new RectangleF(100, 200, 25, 25),
                        _mockSpriteBatch,
                        _mockMarkTexture,
                        Color.Pink,
                        new Rectangle(0, 0, 25, 25)
                    );

                    _mockFactory.Create(
                        _mockEngine,
                        new RectangleF(125, 200, 25, 25),
                        _mockSpriteBatch,
                        _mockMarkTexture,
                        Color.Purple,
                        new Rectangle(25, 0, 25, 25)
                    );
                }
            );
        }

        [TestMethod]
        public void CreateFinalCodeMarksShouldReturnExpectedArray()
        {
            ITextureEntity mockEntityA = Substitute.For<ITextureEntity>();
            ITextureEntity mockEntityB = Substitute.For<ITextureEntity>();
            _mockFactory.Create(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ITexture2D>(), Arg.Any<Color>(), Arg.Any<Rectangle?>()).Returns(mockEntityA, mockEntityB);

            Construct();

            ITextureEntity[] actual = _sut.CreateFinalCodeMarks(_mockModel, new Vector2(100, 200), _mockSpriteBatch);
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
