using wan24.ObjectValidation;
using wan24.Tests;

namespace ObjectValidation_Tests
{
    [TestClass]
    public class Swift_Tests : TestBase
    {
        [TestMethod]
        public void Bic_Tests()
        {
            Assert.IsTrue(SwiftValidation.ValidateBic("BOFAUS3NXXX"));
            Assert.IsFalse(SwiftValidation.ValidateBic("BOFAXX3NXX"));
        }

        [TestMethod]
        public void Iban_Tests()
        {
            Assert.IsTrue(SwiftValidation.ValidateIban("DE21301204000000015228"));
            Assert.IsFalse(SwiftValidation.ValidateIban("DE21301204000000015228X"));// Character appended
            Assert.IsFalse(SwiftValidation.ValidateIban("DE21301204000000015229"));// Last number
            Assert.IsFalse(SwiftValidation.ValidateIban("XX21301204000000015228"));// Country
            Assert.IsFalse(SwiftValidation.ValidateIban("DE21401204000000015228"));// Bank ID first number
            Assert.IsFalse(SwiftValidation.ValidateIban("DE22301204000000015228"));// Checksum last number
            Assert.IsFalse(SwiftValidation.ValidateIban("DE213012040000000152281"));// Too long
            (string country, string checksum, string bank, string account) = SwiftValidation.SplitIban("DE21301204000000015228");
            Assert.AreEqual("DE", country);
            Assert.AreEqual("21", checksum);
            Assert.AreEqual("30120400", bank);
            Assert.AreEqual("0000015228", account);
            Assert.ThrowsException<ArgumentException>(() => SwiftValidation.SplitIban("DE21301204000000015228X"));
        }
    }
}
