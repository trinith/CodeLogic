using System;
using ArbitraryPixel.CodeLogic.Common.Entities;
using ArbitraryPixel.CodeLogic.Common.UI;
using ArbitraryPixel.Platform2D.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using NSubstitute;

namespace ArbitraryPixel.CodeLogic.Common_Tests.UI
{
    [TestClass]
    public class AudioControlsFactoryResult_Tests
    {
        private AudioControlsFactoryResult _sut;
        private IEntity[] _mockEntities;
        private ICheckButton _mockEnableControl;
        private ISlider _mockVolumeControl;
        private Vector2 _mockNextAnchor = new Vector2(11, 22);

        [TestInitialize]
        public void Initialize()
        {
            _mockEnableControl = Substitute.For<ICheckButton>();
            _mockVolumeControl = Substitute.For<ISlider>();

            _mockEntities = new IEntity[]
                {
                    _mockEnableControl,
                    _mockVolumeControl
                };
        }

        private void Construct()
        {
            _sut = new AudioControlsFactoryResult(_mockEntities, _mockEnableControl, _mockVolumeControl, _mockNextAnchor);
        }

        #region Constructor Tests - Null parameter exceptions
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowException_Entities()
        {
            _sut = new AudioControlsFactoryResult(null, _mockEnableControl, _mockVolumeControl, _mockNextAnchor);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowException_EnableControl()
        {
            _sut = new AudioControlsFactoryResult(_mockEntities, null, _mockVolumeControl, _mockNextAnchor);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowException_VolumeControl()
        {
            _sut = new AudioControlsFactoryResult(_mockEntities, _mockEnableControl, null, _mockNextAnchor);
        }
        #endregion

        #region Constructor Tests - Property set from parameters
        [TestMethod]
        public void ConstructorShouldSetPropertyToParameterValue_Entities()
        {
            Construct();

            Assert.AreSame(_mockEntities, _sut.Entities);
        }

        [TestMethod]
        public void ConstructorShouldSetPropertyToParameterValue_EnableControl()
        {
            Construct();

            Assert.AreSame(_mockEnableControl, _sut.EnableControl);
        }

        [TestMethod]
        public void ConstructorShouldSetPropertyToParameterValue_VolumeControl()
        {
            Construct();

            Assert.AreSame(_mockVolumeControl, _sut.VolumeControl);
        }

        [TestMethod]
        public void ConstructorShouldSetPropertyToParameterValue_NextAnchor()
        {
            Construct();

            Assert.AreEqual<Vector2>(_mockNextAnchor, _sut.NextAnchor);
        }
        #endregion
    }
}
