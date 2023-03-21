namespace ObjectValidation_Tests
{
    public class InvalidTestObject3 : ValidTestObject3
    {
        public InvalidTestObject3() : base()
        {
            StringProperty = "test";
            ValidatedStringProperty = "test123";
            NotValidatedStringProperty = "TEST";
            IntProperty = 0;
            RequiredProperty = null;
            DictPropertyValue["b"] = null!;
            DictPropertyKey[new InvalidTestObject2()] = string.Empty;
            DictPropertyKeyValue[new InvalidTestObject2()] = null!;
            ListProperty[1] = null!;
            ListPropertyNullable[0] = new InvalidTestObject2();
            ArrProperty[0] = null!;
            ArrPropertyNullable[0] = new InvalidTestObject2();
            EnumerableProperty = new object[] { null!, new() }.AsEnumerable();
            EnumerablePropertyNullable = new object?[] { new InvalidTestObject2(), null }.AsEnumerable();
            ObjectProperty = new InvalidTestObject2();
        }
    }
}
