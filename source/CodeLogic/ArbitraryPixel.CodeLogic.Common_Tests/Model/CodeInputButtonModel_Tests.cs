using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArbitraryPixel.CodeLogic.Common.Model;
using NSubstitute;
using ArbitraryPixel.CodeLogic.Common.Entities;

namespace ArbitraryPixel.CodeLogic.Common_Tests.Model
{
    [TestClass]
    public class CodeInputButtonModel_Tests
    {
        private IDeviceModel _mockDeviceModel;
        private CodeInputButtonModel _sut;

        [TestInitialize]
        public void Initialize()
        {
            _mockDeviceModel = Substitute.For<IDeviceModel>();

            _sut = new CodeInputButtonModel(_mockDeviceModel);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullModelShouldThrowException()
        {
            _sut = new CodeInputButtonModel(null);
        }

        #region Property Defaults
        [TestMethod]
        public void PropertyDefaultsToExpectedValue_DeviceModel()
        {
            Assert.AreSame(_mockDeviceModel, _sut.DeviceModel);
        }

        [TestMethod]
        public void PropertyDefaultsToExpectedValue_ScaleValue()
        {
            Assert.AreEqual<float>(0, _sut.ScaleValue);
        }

        [TestMethod]
        public void PropertyDefaultsToExpectedValue_SelectedHighlightAngle()
        {
            Assert.AreEqual<float>(0, _sut.SelectedHighlightAngle);
        }

        [TestMethod]
        public void PropertyDefaultsToExpectedValue_GestureAngle()
        {
            Assert.IsNull(_sut.GestureAngle);
        }

        [TestMethod]
        public void PropertyDefaultsToExpectedValue_SelectorState()
        {
            Assert.AreEqual<CodeInputButtonSelectorState>(CodeInputButtonSelectorState.Closed, _sut.SelectorState);
        }

        [TestMethod]
        public void PropertyDefaultsToExpectedValue_SelectorMode()
        {
            Assert.AreEqual<CodeInputButtonSelectorMode>(CodeInputButtonSelectorMode.Gesture, _sut.SelectorMode);
        }

        [TestMethod]
        public void PropertyDefaultsToExpectedValue_MovedOutOfDeadzone()
        {
            Assert.IsFalse(_sut.MovedOutOfDeadzone);
        }
        #endregion

        #region Property Set Tests
        [TestMethod]
        public void PropertyShouldReturnSetValue_ScaleValue()
        {
            _sut.ScaleValue = 1234;

            Assert.AreEqual<float>(1234, _sut.ScaleValue);
        }

        [TestMethod]
        public void PropertyShouldReturnSetValue_SelectedHighlightAngle()
        {
            _sut.SelectedHighlightAngle = 1234;

            Assert.AreEqual<float>(1234, _sut.SelectedHighlightAngle);
        }

        [TestMethod]
        public void PropertyShouldReturnSetValue_GestureAngle()
        {
            _sut.GestureAngle = 1234;

            Assert.AreEqual<float?>(1234, _sut.GestureAngle);
        }

        [TestMethod]
        public void PropertyShouldReturnSetValue_SelectorState()
        {
            _sut.SelectorState = CodeInputButtonSelectorState.Opening;

            Assert.AreEqual<CodeInputButtonSelectorState>(CodeInputButtonSelectorState.Opening, _sut.SelectorState);
        }

        [TestMethod]
        public void PropertyShouldReturnSetValue_SelectorMode()
        {
            _sut.SelectorMode = CodeInputButtonSelectorMode.Select;

            Assert.AreEqual<CodeInputButtonSelectorMode>(CodeInputButtonSelectorMode.Select, _sut.SelectorMode);
        }

        [TestMethod]
        public void PropertyShouldReturnSetValue_MovedOutOfDeadzone()
        {
            _sut.MovedOutOfDeadzone = true;

            Assert.IsTrue(_sut.MovedOutOfDeadzone);
        }
        #endregion

        #region OpenSelector and CloseSelector Tests
        [TestMethod]
        public void OpenSelectorWhenStateClosedShouldSetStateToOpening()
        {
            _sut.SelectorState = CodeInputButtonSelectorState.Closed;

            _sut.OpenSelector();

            Assert.AreEqual<CodeInputButtonSelectorState>(CodeInputButtonSelectorState.Opening, _sut.SelectorState);
        }

        [TestMethod]
        public void OpenSelectorWhenStateClosingShouldSetStateToOpening()
        {
            _sut.SelectorState = CodeInputButtonSelectorState.Closing;

            _sut.OpenSelector();

            Assert.AreEqual<CodeInputButtonSelectorState>(CodeInputButtonSelectorState.Opening, _sut.SelectorState);
        }

        [TestMethod]
        public void OpenSelectorWhenStateOpeningShouldNotChangeState()
        {
            _sut.SelectorState = CodeInputButtonSelectorState.Opening;

            _sut.OpenSelector();

            Assert.AreEqual<CodeInputButtonSelectorState>(CodeInputButtonSelectorState.Opening, _sut.SelectorState);
        }

        [TestMethod]
        public void OpenSelectorWhenStateOpenedShouldNotChangeState()
        {
            _sut.SelectorState = CodeInputButtonSelectorState.Open;

            _sut.OpenSelector();

            Assert.AreEqual<CodeInputButtonSelectorState>(CodeInputButtonSelectorState.Open, _sut.SelectorState);
        }

        [TestMethod]
        public void CloseSelectorWithAnyStateShouldSetStateClosing()
        {
            _sut.SelectorState = CodeInputButtonSelectorState.Open;

            _sut.CloseSelector();

            Assert.AreEqual<CodeInputButtonSelectorState>(CodeInputButtonSelectorState.Closing, _sut.SelectorState);
        }
        #endregion
    }
}
