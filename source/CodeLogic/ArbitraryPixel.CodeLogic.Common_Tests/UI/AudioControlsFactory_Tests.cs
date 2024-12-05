using System;
using System.Linq;
using ArbitraryPixel.CodeLogic.Common;
using ArbitraryPixel.CodeLogic.Common.Entities;
using ArbitraryPixel.CodeLogic.Common.UI;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.Entity;
using ArbitraryPixel.Platform2D.UI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using NSubstitute;

namespace ArbitraryPixel.CodeLogic.Common_Tests.UI
{
    [TestClass]
    public class AudioControlsFactory_Tests
    {
        private AudioControlsFactory _sut;
        private IEngine _mockEngine;
        private RectangleF _bounds = new RectangleF(0, 0, 1000, 750);

        private GameObjectFactory _mockGOF;
        private ISpriteBatch _mockSpriteBatch;
        private Vector2 _anchor = new Vector2(11, 22);
        private SizeF _padding = new SizeF(10, 5);

        [TestInitialize]
        public void Initialize()
        {
            _mockEngine = Substitute.For<IEngine>();

            _mockGOF = Substitute.For<GameObjectFactory>();
            GameObjectFactory.SetInstance(_mockGOF);

            _mockSpriteBatch = Substitute.For<ISpriteBatch>();
        }

        private void Construct()
        {
            _sut = new AudioControlsFactory(_mockEngine, _bounds);
        }

        #region Constructor Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowException_Host()
        {
            _sut = new AudioControlsFactory(null, _bounds);
        }

        [TestMethod]
        public void ConstructShouldSetPropertyValueToConstructorParameter_Host()
        {
            Construct();

            Assert.AreSame(_mockEngine, _sut.Host);
        }

        [TestMethod]
        public void ConstructShouldSetPropertyValueToConstructorParameter_ContentBounds()
        {
            Construct();

            Assert.AreEqual<RectangleF>(_bounds, _sut.ContentBounds);
        }
        #endregion

        #region CreateControls Tests
        [TestMethod]
        public void CreateControlsShouldCreateExpectedCheckButton()
        {
            Construct();

            AudioControlsFactoryResult result = _sut.CreateControls(_anchor, _padding, _mockSpriteBatch, "testFont", "enableLabel", "volumeLabel");

            _mockGOF.Received(1).CreateCheckButton(_mockEngine, new RectangleF(_anchor, new SizeF(100, 100)), _mockSpriteBatch);
        }

        [TestMethod]
        public void CreateControlsShouldAddCheckButtonToEntities()
        {
            ICheckButton mockButton = Substitute.For<ICheckButton>();
            _mockGOF.CreateCheckButton(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>()).Returns(mockButton);

            Construct();

            AudioControlsFactoryResult result = _sut.CreateControls(_anchor, _padding, _mockSpriteBatch, "testFont", "enableLabel", "volumeLabel");

            Assert.IsTrue(result.Entities.ToList().Contains(mockButton));
        }

        [TestMethod]
        public void CreateControlsShouldRetrieveMainMenuContentFontFromAssetBank()
        {
            Construct();

            AudioControlsFactoryResult result = _sut.CreateControls(_anchor, _padding, _mockSpriteBatch, "testFont", "enableLabel", "volumeLabel");

            _mockEngine.AssetBank.Received(1).Get<ISpriteFont>("testFont");
        }

        [TestMethod]
        public void CreateControlsShouldCreateLabelForEnableButton()
        {
            ICheckButton mockButton = Substitute.For<ICheckButton>();
            _mockGOF.CreateCheckButton(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>()).Returns(mockButton);
            mockButton.Bounds.Returns(new RectangleF(_anchor, new SizeF(100f, 100f)));

            ISpriteFont mockFont = Substitute.For<ISpriteFont>();
            mockFont.LineSpacing.Returns(12);
            _mockEngine.AssetBank.Get<ISpriteFont>("testFont").Returns(mockFont);

            Construct();

            AudioControlsFactoryResult result = _sut.CreateControls(_anchor, _padding, _mockSpriteBatch, "testFont", "enableLabel", "volumeLabel");

            _mockGOF.Received(1).CreateGenericTextLabel(
                _mockEngine,
                new Vector2(
                    _anchor.X + 100f + _padding.Width,
                    _anchor.Y + 50f - 12 / 2f
                ),
                _mockSpriteBatch,
                mockFont,
                "enableLabel",
                CodeLogicEngine.Constants.ClrMenuFGHigh
            );
        }

        [TestMethod]
        public void CreateControlsShouldAddEnableLabelToEntities()
        {
            ITextLabel mockLabel = Substitute.For<ITextLabel>();
            _mockGOF.CreateGenericTextLabel(Arg.Any<IEngine>(), Arg.Any<Vector2>(), Arg.Any<ISpriteBatch>(), Arg.Any<ISpriteFont>(), "enableLabel", Arg.Any<Color>()).Returns(mockLabel);

            Construct();

            AudioControlsFactoryResult result = _sut.CreateControls(_anchor, _padding, _mockSpriteBatch, "testFont", "enableLabel", "volumeLabel");

            Assert.IsTrue(result.Entities.ToList().Contains(mockLabel));
        }

        [TestMethod]
        public void CreateControlsShouldCreateVolumeLabel()
        {
            ICheckButton mockButton = Substitute.For<ICheckButton>();
            mockButton.Bounds.Returns(new RectangleF(_anchor, new SizeF(100, 100)));
            _mockGOF.CreateCheckButton(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>()).Returns(mockButton);

            ISpriteFont mockFont = Substitute.For<ISpriteFont>();
            _mockEngine.AssetBank.Get<ISpriteFont>("testFont").Returns(mockFont);

            Construct();

            AudioControlsFactoryResult result = _sut.CreateControls(_anchor, _padding, _mockSpriteBatch, "testFont", "enableLabel", "volumeLabel");

            _mockGOF.Received(1).CreateGenericTextLabel(
                _mockEngine,
                new Vector2(
                    _anchor.X + _padding.Width * 2,
                    _anchor.Y + _padding.Height + 100f
                ),
                _mockSpriteBatch,
                mockFont,
                "volumeLabel",
                CodeLogicEngine.Constants.ClrMenuFGHigh
            );
        }

        [TestMethod]
        public void CreateControlsShouldAddMusicVolumeLabelToEntities()
        {
            ITextLabel mockLabel = Substitute.For<ITextLabel>();
            _mockGOF.CreateGenericTextLabel(Arg.Any<IEngine>(), Arg.Any<Vector2>(), Arg.Any<ISpriteBatch>(), Arg.Any<ISpriteFont>(), "volumeLabel", Arg.Any<Color>()).Returns(mockLabel);

            Construct();

            AudioControlsFactoryResult result = _sut.CreateControls(_anchor, _padding, _mockSpriteBatch, "testFont", "enableLabel", "volumeLabel");

            Assert.IsTrue(result.Entities.ToList().Contains(mockLabel));
        }

        [TestMethod]
        public void CreateControlsShouldCreateVolumeSlider()
        {
            ICheckButton mockButton = Substitute.For<ICheckButton>();
            mockButton.Bounds.Returns(new RectangleF(_anchor, new SizeF(100, 100)));
            _mockGOF.CreateCheckButton(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>()).Returns(mockButton);

            ISpriteFont mockFont = Substitute.For<ISpriteFont>();
            mockFont.LineSpacing.Returns(30);
            _mockEngine.AssetBank.Get<ISpriteFont>("testFont").Returns(mockFont);

            ISlider mockSlider = Substitute.For<ISlider>();
            _mockGOF.CreateSlider(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>()).Returns(mockSlider);

            Construct();

            AudioControlsFactoryResult result = _sut.CreateControls(_anchor, _padding, _mockSpriteBatch, "testFont", "enableLabel", "volumeLabel");

            Received.InOrder(
                () =>
                {
                    _mockGOF.CreateSlider(
                        _mockEngine,
                        new RectangleF(
                            new Vector2(
                                _anchor.X + _padding.Width * 2,
                                _anchor.Y + _padding.Height + 100f + _padding.Height + 30f
                            ),
                            new SizeF(
                                _bounds.Width - ((_anchor.X + _padding.Width * 2) - _bounds.X) - _padding.Width,
                                50
                            )
                        ),
                        _mockSpriteBatch
                    );

                    mockSlider.BackgroundColour = CodeLogicEngine.Constants.ClrMenuFGLow;
                    mockSlider.ForegroundColour = CodeLogicEngine.Constants.ClrMenuFGMid;
                    mockSlider.HotColour = CodeLogicEngine.Constants.ClrMenuFGHigh;
                    mockSlider.Minimum = 0f;
                    mockSlider.Maximum = 1f;
                }
            );
        }

        [TestMethod]
        public void CreateControlsShouldAddVolumeSliderToEntities()
        {
            ISlider mockSlider = Substitute.For<ISlider>();
            _mockGOF.CreateSlider(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>()).Returns(mockSlider);

            Construct();

            AudioControlsFactoryResult result = _sut.CreateControls(_anchor, _padding, _mockSpriteBatch, "testFont", "enableLabel", "volumeLabel");

            Assert.IsTrue(result.Entities.Contains(mockSlider));
        }

        [TestMethod]
        public void CreateControlsShouldReturnExpectedEntities()
        {
            ICheckButton mockEnable = Substitute.For<ICheckButton>();
            ITextLabel mockEnableLabel = Substitute.For<ITextLabel>();
            ITextLabel mockVolumeLabel = Substitute.For<ITextLabel>();
            ISlider mockVolume = Substitute.For<ISlider>();

            _mockGOF.CreateCheckButton(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>()).Returns(mockEnable);
            _mockGOF.CreateGenericTextLabel(Arg.Any<IEngine>(), Arg.Any<Vector2>(), Arg.Any<ISpriteBatch>(), Arg.Any<ISpriteFont>(), Arg.Any<string>(), Arg.Any<Color>()).Returns(mockEnableLabel, mockVolumeLabel);
            _mockGOF.CreateSlider(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>()).Returns(mockVolume);

            Construct();

            AudioControlsFactoryResult result = _sut.CreateControls(_anchor, _padding, _mockSpriteBatch, "testFont", "enableLabel", "volumeLabel");

            Assert.IsTrue(result.Entities.SequenceEqual(new IEntity[] { mockEnable, mockEnableLabel, mockVolumeLabel, mockVolume }));
        }

        [TestMethod]
        public void CreateControlsShouldReturnExpectedEnableControl()
        {
            ICheckButton mockEnable = Substitute.For<ICheckButton>();
            ITextLabel mockEnableLabel = Substitute.For<ITextLabel>();
            ITextLabel mockVolumeLabel = Substitute.For<ITextLabel>();
            ISlider mockVolume = Substitute.For<ISlider>();

            _mockGOF.CreateCheckButton(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>()).Returns(mockEnable);
            _mockGOF.CreateGenericTextLabel(Arg.Any<IEngine>(), Arg.Any<Vector2>(), Arg.Any<ISpriteBatch>(), Arg.Any<ISpriteFont>(), Arg.Any<string>(), Arg.Any<Color>()).Returns(mockEnableLabel, mockVolumeLabel);
            _mockGOF.CreateSlider(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>()).Returns(mockVolume);

            Construct();

            AudioControlsFactoryResult result = _sut.CreateControls(_anchor, _padding, _mockSpriteBatch, "testFont", "enableLabel", "volumeLabel");

            Assert.AreSame(mockEnable, result.EnableControl);
        }

        [TestMethod]
        public void CreateControlsShouldReturnExpectedVolumeControl()
        {
            ICheckButton mockEnable = Substitute.For<ICheckButton>();
            ITextLabel mockEnableLabel = Substitute.For<ITextLabel>();
            ITextLabel mockVolumeLabel = Substitute.For<ITextLabel>();
            ISlider mockVolume = Substitute.For<ISlider>();

            _mockGOF.CreateCheckButton(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>()).Returns(mockEnable);
            _mockGOF.CreateGenericTextLabel(Arg.Any<IEngine>(), Arg.Any<Vector2>(), Arg.Any<ISpriteBatch>(), Arg.Any<ISpriteFont>(), Arg.Any<string>(), Arg.Any<Color>()).Returns(mockEnableLabel, mockVolumeLabel);
            _mockGOF.CreateSlider(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>()).Returns(mockVolume);

            Construct();

            AudioControlsFactoryResult result = _sut.CreateControls(_anchor, _padding, _mockSpriteBatch, "testFont", "enableLabel", "volumeLabel");

            Assert.AreSame(mockVolume, result.VolumeControl);
        }

        [TestMethod]
        public void CreateControlsShouldReturnExpectedNextAnchor()
        {
            ICheckButton mockEnable = Substitute.For<ICheckButton>();
            ITextLabel mockEnableLabel = Substitute.For<ITextLabel>();
            ITextLabel mockVolumeLabel = Substitute.For<ITextLabel>();
            ISlider mockVolume = Substitute.For<ISlider>();

            mockVolume.Bounds.Returns(new RectangleF(200, 100, 400, 300));

            ISpriteFont mockFont = Substitute.For<ISpriteFont>();
            mockFont.LineSpacing.Returns(30);

            _mockGOF.CreateCheckButton(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>()).Returns(mockEnable);
            _mockGOF.CreateGenericTextLabel(Arg.Any<IEngine>(), Arg.Any<Vector2>(), Arg.Any<ISpriteBatch>(), Arg.Any<ISpriteFont>(), Arg.Any<string>(), Arg.Any<Color>()).Returns(mockEnableLabel, mockVolumeLabel);
            _mockGOF.CreateSlider(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>()).Returns(mockVolume);

            Construct();

            AudioControlsFactoryResult result = _sut.CreateControls(_anchor, _padding, _mockSpriteBatch, "testFont", "enableLabel", "volumeLabel");

            Vector2 expected = new Vector2(11, 400 + _padding.Height);
            Assert.AreEqual<Vector2>(expected, result.NextAnchor);
        }
        #endregion
    }
}
