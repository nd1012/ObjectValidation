using wan24.ObjectValidation;
using wan24.Tests;

namespace ObjectValidation_Tests
{
    [TestClass]
    public class EuVatId_Tests : TestBase
    {
        [TestMethod]
        public void VatId_Tests()
        {
            Assert.IsTrue(EuVatId.Validate("ATU99999999"));
            Assert.IsFalse(EuVatId.Validate("AT99999999"));
        }
    }
}
