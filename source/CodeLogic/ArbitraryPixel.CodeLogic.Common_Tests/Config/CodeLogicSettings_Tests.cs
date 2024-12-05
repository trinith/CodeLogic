using ArbitraryPixel.CodeLogic.Common.Config;
using ArbitraryPixel.CodeLogic.Common.Model;
using ArbitraryPixel.Platform2D.Config;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;

namespace ArbitraryPixel.CodeLogic.Common_Tests.Config
{
    [TestClass]
    public class CodeLogicSettings_Tests
    {
        private CodeLogicSettings _sut;
        private IConfigStore _mockConfigStore;
        private Dictionary<string, string> _backingStore = new Dictionary<string, string>();

        [TestInitialize]
        public void Initialize()
        {
            _mockConfigStore = Substitute.For<IConfigStore>();

            _mockConfigStore.ContainsKey(Arg.Any<string>()).Returns(x => _backingStore.ContainsKey(x[0].ToString()));
            _mockConfigStore.Get(Arg.Any<string>()).Returns(x => _backingStore[x[0].ToString()]);
            _mockConfigStore.When(x => x.Store(Arg.Any<string>(), Arg.Any<string>())).Do(
                x =>
                {
                    if (_backingStore.ContainsKey(x[0].ToString()))
                        _backingStore[x[0].ToString()] = x[1].ToString();
                    else
                        _backingStore.Add(x[0].ToString(), x[1].ToString());
                }
            );
        }

        private void Construct()
        {
            _sut = new CodeLogicSettings(_mockConfigStore);
        }

        #region Constructor Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowException_ConfigStore()
        {
            _sut = new CodeLogicSettings(null);
        }

        [TestMethod]
        public void ConstructShouldCallConfigStoreLoadCache()
        {
            Construct();

            _mockConfigStore.Received(1).LoadCache();
        }

        [TestMethod]
        public void ConstructShouldReadProperties()
        {
            Construct();

            Received.InOrder(
                () =>
                {
                    _mockConfigStore.Get("SoundEnabled");
                    _mockConfigStore.Get("SoundVolume");
                    _mockConfigStore.Get("MusicEnabled");
                    _mockConfigStore.Get("MusicVolume");
                    _mockConfigStore.Get("LogPanelMode");
                }
            );
        }
        #endregion

        #region Propertry Set Tests
        [TestMethod]
        public void PropertySetShouldWriteToConfigStore_MusicEnabled()
        {
            Construct();
            _mockConfigStore.ClearReceivedCalls();

            _sut.MusicEnabled = true;

            _mockConfigStore.Received(1).Store("MusicEnabled", "True");
        }

        [TestMethod]
        public void PropertySetShouldWriteToConfigStore_MusicVolume()
        {
            Construct();

            _sut.MusicVolume = 0.75f;

            _mockConfigStore.Received(1).Store("MusicVolume", "0.75");
        }

        [TestMethod]
        public void PropertySetShouldWriteToConfigStore_LogPanelMode()
        {
            Construct();

            _sut.LogPanelMode = LogPanelMode.PartialView;

            _mockConfigStore.Received(1).Store("LogPanelMode", "PartialView");
        }

        [TestMethod]
        public void PropertySetShouldWriteToConfigStore_SoundEnabled()
        {
            Construct();
            _mockConfigStore.ClearReceivedCalls();

            _sut.SoundEnabled = true;

            _mockConfigStore.Received(1).Store("SoundEnabled", "True");
        }

        [TestMethod]
        public void PropertySetShouldWriteToConfigStore_SoundVolume()
        {
            Construct();

            _sut.SoundVolume = 0.123f;

            _mockConfigStore.Received(1).Store("SoundVolume", "0.123");
        }
        #endregion

        #region Property Get Tests - Value does not exist in config
        [TestMethod]
        public void PropertyGetWhenNotInCacheShouldSetDefaultValue_MusicEnabled()
        {
            Construct();
            _mockConfigStore.ClearReceivedCalls();
            _backingStore.Clear();

            var x = _sut.MusicEnabled;

            _mockConfigStore.Received(1).Store("MusicEnabled", "True");
        }

        [TestMethod]
        public void PropertyGetWhenNotInCacheShouldSetDefaultValue_MusicVolume()
        {
            Construct();
            _mockConfigStore.ClearReceivedCalls();
            _backingStore.Clear();

            var x = _sut.MusicVolume;

            _mockConfigStore.Received(1).Store("MusicVolume", "0.25");
        }

        [TestMethod]
        public void PropertyGetWhenNotInCacheShouldSetDefaultValue_LogPanelMode()
        {
            Construct();
            _mockConfigStore.ClearReceivedCalls();
            _backingStore.Clear();

            var x = _sut.LogPanelMode;

            _mockConfigStore.Received(1).Store("LogPanelMode", "FullView");
        }

        [TestMethod]
        public void PropertyGetWhenNotInCacheShouldSetDefaultValue_SoundEnabled()
        {
            Construct();
            _mockConfigStore.ClearReceivedCalls();
            _backingStore.Clear();

            var x = _sut.SoundEnabled;

            _mockConfigStore.Received(1).Store("SoundEnabled", "True");
        }

        [TestMethod]
        public void PropertyGetWhenNotInCacheShouldSetDefaultValue_SoundVolume()
        {
            Construct();
            _mockConfigStore.ClearReceivedCalls();
            _backingStore.Clear();

            var x = _sut.SoundVolume;

            _mockConfigStore.Received(1).Store("SoundVolume", "0.5");
        }
        #endregion

        #region Property Get Tests - Value exists in config
        [TestMethod]
        public void PropertyGetWhenInCacheShouldRequestCacheValue_MusicEnabled()
        {
            Construct();
            _mockConfigStore.ClearReceivedCalls();

            var x = _sut.MusicEnabled;

            _mockConfigStore.Received(1).Get("MusicEnabled");
        }

        [TestMethod]
        public void PropertyGetWhenInCacheShouldRequestCacheValue_MusicVolume()
        {
            Construct();
            _mockConfigStore.ClearReceivedCalls();

            var x = _sut.MusicVolume;

            _mockConfigStore.Received(1).Get("MusicVolume");
        }

        [TestMethod]
        public void PropertyGetWhenInCacheShouldRequestCacheValue_LogPanelMode()
        {
            Construct();
            _mockConfigStore.ClearReceivedCalls();

            var x = _sut.LogPanelMode;

            _mockConfigStore.Received(1).Get("LogPanelMode");
        }

        [TestMethod]
        public void PropertyGetWhenInCacheShouldRequestCacheValue_SoundEnabled()
        {
            Construct();
            _mockConfigStore.ClearReceivedCalls();

            var x = _sut.SoundEnabled;

            _mockConfigStore.Received(1).Get("SoundEnabled");
        }

        [TestMethod]
        public void PropertyGetWhenInCacheShouldRequestCacheValue_SoundVolume()
        {
            Construct();
            _mockConfigStore.ClearReceivedCalls();

            var x = _sut.SoundVolume;

            _mockConfigStore.Received(1).Get("SoundVolume");
        }
        #endregion

        #region Property Get Tests - Parsable Value
        [TestMethod]
        public void PropertyGetWithGoodCacheShouldReturnExpectedValue_MusicEnabled()
        {
            Construct();

            Assert.AreEqual<bool>(true, _sut.MusicEnabled);
        }

        [TestMethod]
        public void PropertyGetWithGoodCacheShouldReturnExpectedValue_MusicVolume()
        {
            Construct();

            Assert.AreEqual<float>(0.25f, _sut.MusicVolume);
        }

        [TestMethod]
        public void PropertyGetWithGoodCacheShouldReturnExpectedValue_LogPanelMode()
        {
            Construct();

            Assert.AreEqual<LogPanelMode>(LogPanelMode.FullView, _sut.LogPanelMode);
        }

        [TestMethod]
        public void PropertyGetWithGoodCacheShouldReturnExpectedValue_SoundEnabled()
        {
            Construct();

            Assert.AreEqual<bool>(true, _sut.SoundEnabled);
        }

        [TestMethod]
        public void PropertyGetWithGoodCacheShouldReturnExpectedValue_SoundVolume()
        {
            Construct();

            Assert.AreEqual<float>(0.5f, _sut.SoundVolume);
        }
        #endregion

        #region Property Get Tests - Unparsable Value
        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void PropertyGetWithBadCacheShouldThrowException_MusicEnabled()
        {
            Construct();
            _backingStore["MusicEnabled"] = "123";

            var x = _sut.MusicEnabled;
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void PropertyGetWithBadCacheShouldThrowException_MusicVolume()
        {
            Construct();
            _backingStore["MusicVolume"] = "abcd";

            var x = _sut.MusicVolume;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void PropertyGetWithBadCacheShouldThrowException_LogPanelMode()
        {
            Construct();
            _backingStore["LogPanelMode"] = "abcd";

            var x = _sut.LogPanelMode;
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void PropertyGetWithBadCacheShouldThrowException_SoundEnabled()
        {
            Construct();
            _backingStore["SoundEnabled"] = "123";

            var x = _sut.SoundEnabled;
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void PropertyGetWithBadCacheShouldThrowException_SoundVolume()
        {
            Construct();
            _backingStore["SoundVolume"] = "abcd";

            var x = _sut.SoundVolume;
        }
        #endregion

        #region IConfigStore Passthrough Tests
        [TestMethod]
        public void MethodOrPropertyShouldCallConfigStore_IsTransient_Get()
        {
            Construct();
            _mockConfigStore.ClearReceivedCalls();

            var r = _sut.IsTransient;

            var x = _mockConfigStore.Received(1).IsTransient;
        }

        [TestMethod]
        public void MethodOrPropertyShouldCallConfigStore_IsTransient_Set()
        {
            Construct();
            _mockConfigStore.ClearReceivedCalls();

            _sut.IsTransient = true;

            _mockConfigStore.Received(1).IsTransient = true;
        }

        [TestMethod]
        public void MethodOrPropertyShouldCallConfigStore_CacheChanged()
        {
            Construct();
            _mockConfigStore.ClearReceivedCalls();

            var r = _sut.CacheChanged;

            var x = _mockConfigStore.Received(1).CacheChanged;
        }

        [TestMethod]
        public void MethodOrPropertyShouldCallConfigStore_Store()
        {
            Construct();
            _mockConfigStore.ClearReceivedCalls();

            _sut.Store("abcd", "1234");

            _mockConfigStore.Received(1).Store("abcd", "1234");
        }

        [TestMethod]
        public void MethodOrPropertyShouldCallConfigStore_Get()
        {
            Construct();
            _mockConfigStore.ClearReceivedCalls();

            _sut.Get("MusicEnabled");

            _mockConfigStore.Received(1).Get("MusicEnabled");
        }

        [TestMethod]
        public void MethodOrPropertyShouldCallConfigStore_ContainsKey()
        {
            Construct();
            _mockConfigStore.ClearReceivedCalls();

            _sut.ContainsKey("abcd");

            _mockConfigStore.Received(1).ContainsKey("abcd");
        }

        [TestMethod]
        public void MethodOrPropertyShouldCallConfigStore_PersistCache()
        {
            Construct();
            _mockConfigStore.ClearReceivedCalls();

            _sut.PersistCache();

            _mockConfigStore.Received(1).PersistCache();
        }

        [TestMethod]
        public void MethodOrPropertyShouldCallConfigStore_LoadCache()
        {
            Construct();
            _mockConfigStore.ClearReceivedCalls();

            _sut.LoadCache();

            _mockConfigStore.Received(1).LoadCache();
        }
        #endregion
    }
}
