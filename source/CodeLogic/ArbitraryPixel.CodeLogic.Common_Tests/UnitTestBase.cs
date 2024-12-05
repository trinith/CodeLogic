using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArbitraryPixel.CodeLogic.Common_Tests
{
    [TestClass]
    public class UnitTestBase<T>
    {
        protected T _sut = default(T);

        protected virtual void OnInitializing() { }
        protected virtual void OnInitialized() { }
        protected virtual T OnCreateSUT() { return default(T); }

        [TestInitialize]
        public void Initialize()
        {
            OnInitializing();
            _sut = OnCreateSUT();
            OnInitialized();
        }
    }
}
