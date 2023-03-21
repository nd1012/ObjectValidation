using wan24.ObjectValidation;

namespace ObjectValidation_Tests
{
    [TestClass]
    public class EuVatId_Tests
    {
        [TestMethod]
        public void VatId_Tests()
        {
            Assert.IsTrue(EuVatId.Validate("ATU99999999"));
            Assert.IsFalse(EuVatId.Validate("AT99999999"));
        }
    }
}
