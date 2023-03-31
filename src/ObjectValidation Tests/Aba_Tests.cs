using wan24.ObjectValidation;

namespace ObjectValidation_Tests
{
    [TestClass]
    public class Aba_Tests
    {
        [TestMethod]
        public void AbaRtn_Tests()
        {
            // MICR
            Assert.AreEqual(AbaFormats.MICR, AbaValidation.GetAbaFormat("111000025"));
            Assert.IsTrue(AbaValidation.ValidateAbaRtn("111000025"));
            Assert.IsTrue(AbaValidation.ValidateAbaRtn("111000025", AbaFormats.MICR));
            Assert.IsFalse(AbaValidation.ValidateAbaRtn("111000025", AbaFormats.Fraction));
            Assert.IsTrue(AbaValidation.ValidateAbaRtn(AbaValidation.Normalize("1110 0002 5")));
            Assert.IsFalse(AbaValidation.ValidateAbaRtn("111000026"));
            // Fraction
            Assert.AreEqual(AbaFormats.Fraction, AbaValidation.GetAbaFormat("01-0002/1110"));
            Assert.IsTrue(AbaValidation.ValidateAbaRtn("01-0002/1110"));
            Assert.IsFalse(AbaValidation.ValidateAbaRtn("01-0002/1110", AbaFormats.MICR));
            Assert.IsTrue(AbaValidation.ValidateAbaRtn("01-0002/1110", AbaFormats.Fraction));
            Assert.IsTrue(AbaValidation.ValidateAbaRtn(AbaValidation.Normalize("01 - 0002 1110")));
            Assert.IsFalse(AbaValidation.ValidateAbaRtn("01-0002/0010"));
        }
    }
}
