namespace ObjectValidation_Tests
{
    public class InvalidTestObject : ValidTestObject
    {
        public InvalidTestObject() : base()
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
            EnumerableProperty= new object[] { null!, new() }.AsEnumerable();
            EnumerablePropertyNullable = new object?[] { new InvalidTestObject2(), null }.AsEnumerable();
            ObjectProperty = new InvalidTestObject2();
            DictProperty2["test"] = null!;
            DictProperty2["test2"] = "test123";
            DictProperty2["test123"] = "test123";
            ListProperty2[0] = null!;
            ListProperty2[1] = "test123";
            ArrProperty2[0] = null!;
            ArrProperty2[1] = "test123";
            EnumerableProperty2 = new string[] { null!, "test123" }.AsEnumerable();
            ItemIgnoredProperty = null!;
            DeepArrProperty[0][1] = "test123";
            DeepArrProperty[1][0] = null!;
            RequiredIfConditional = true;
            EnumProperty = TestEnum.Invalid;
            Enum2Property = (TestEnum)3;
        }
    }
}
