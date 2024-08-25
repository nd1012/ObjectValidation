using wan24.ObjectValidation;
using wan24.Tests;

namespace ObjectValidation_Tests
{
    [TestClass]
    public class Luhn_Tests : TestBase
    {
        [TestMethod]
        public void Luhn_Checksum_Tests()
        {
            Assert.IsTrue(LuhnChecksum.Validate("49927398716"));
            Assert.IsTrue(LuhnChecksum.Validate("1234567812345670"));
            Assert.IsTrue(LuhnChecksum.Validate("361568"));
            Assert.IsTrue(LuhnChecksum.Validate("79927398713"));
            Assert.IsTrue(LuhnChecksum.Validate("361576"));
            Assert.IsFalse(LuhnChecksum.Validate("49927398717"));
            Assert.IsFalse(LuhnChecksum.Validate("1234567812345678"));
        }
    }
}
