namespace ObjectValidation_Tests
{
    public class InvalidTestObject2 : ValidTestObject2
    {
        public InvalidTestObject2() : base() => StringProperty = null!;
    }
}
