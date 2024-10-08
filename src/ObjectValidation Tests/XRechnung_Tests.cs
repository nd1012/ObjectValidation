﻿using wan24.ObjectValidation;
using wan24.Tests;

namespace ObjectValidation_Tests
{
    [TestClass]
    public class XRechnung_Tests : TestBase
    {
        [TestMethod]
        public void XRechnung_Route_Tests()
        {
            Assert.IsTrue(XRechnungRouting.Validate("04011000-1234512345-06"));
            Assert.IsFalse(XRechnungRouting.Validate("04011000-1234512345-07"));
        }
    }
}
