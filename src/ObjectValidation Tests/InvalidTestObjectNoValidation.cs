using wan24.ObjectValidation;

namespace ObjectValidation_Tests
{
    [NoValidation]
    public class InvalidTestObjectNoValidation : InvalidTestObject2
    {
        public InvalidTestObjectNoValidation() : base() { }
    }
}
