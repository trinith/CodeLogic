using System;
using ArbitraryPixel.CodeLogic.Common;
using ArbitraryPixel.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace ArbitraryPixel.CodeLogic.Common_Tests
{
    [TestClass]
    public class ConsoleLogger_Tests
    {
        private ConsoleLogger _sut;
        private IDateTimeFactory _mockDateTimeFactory;
        private IDebug _mockDebug;

        [TestInitialize]
        public void Initialize()
        {
            _mockDateTimeFactory = Substitute.For<IDateTimeFactory>();
            _mockDebug = Substitute.For<IDebug>();
        }

        private void Construct()
        {
            _sut = new ConsoleLogger(_mockDateTimeFactory, _mockDebug);
        }

        #region Constructor Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowException_DateTimeFacory()
        {
            _sut = new ConsoleLogger(null, _mockDebug);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowException_Debug()
        {
            _sut = new ConsoleLogger(_mockDateTimeFactory, null);
        }
        #endregion

        #region WriteLine Tests
        [TestMethod]
        public void WriteLineShouldCallDebugWriteLine()
        {
            Construct();

            _sut.WriteLine("test");

            _mockDebug.Received(1).WriteLine("CLDBG: test");
        }

        [TestMethod]
        public void WriteLineWhenUseTimeStampsTrueShouldCallDebugWriteLine()
        {
            Construct();
            _sut.UseTimeStamps = true;
            _mockDateTimeFactory.Now.ToString(Arg.Any<string>()).Returns("ASDF");

            _sut.WriteLine("test");

            _mockDebug.Received(1).WriteLine("CLDBG: [ASDF] test");
        }
        #endregion
    }
}
