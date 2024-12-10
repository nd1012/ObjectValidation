using wan24.ObjectValidation;
using wan24.Tests;

namespace ObjectValidation_Tests
{
    [TestClass]
    public class Duns_Tests : TestBase
    {
        [TestMethod]
        public void DunsNumber_Tests()
        {
            string validDuns1 = "1234-15-048-3782",
                validDuns2 = "15-048-3782",
                validDuns3 = "5-048-3782",
                validDuns4 = "048-3782",
                invalidDuns = "48-3782";

            Assert.IsFalse(DunsValidation.ValidateDuns(validDuns1));
            Assert.IsFalse(DunsValidation.ValidateDuns(validDuns2));
            Assert.IsFalse(DunsValidation.ValidateDuns(validDuns3));
            Assert.IsFalse(DunsValidation.ValidateDuns(validDuns4));
            Assert.IsFalse(DunsValidation.ValidateDuns(invalidDuns));

            Assert.IsTrue(DunsValidation.ValidateDuns(DunsValidation.Normalize(validDuns1)));
            Assert.IsTrue(DunsValidation.ValidateDuns(DunsValidation.Normalize(validDuns2)));
            Assert.IsTrue(DunsValidation.ValidateDuns(DunsValidation.Normalize(validDuns3)));
            Assert.IsTrue(DunsValidation.ValidateDuns(DunsValidation.Normalize(validDuns4)));
            Assert.IsFalse(DunsValidation.ValidateDuns(DunsValidation.Normalize(invalidDuns)));
        }
    }
}
