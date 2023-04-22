using System.ComponentModel.DataAnnotations;
using wan24.ObjectValidation;

namespace ObjectValidation_Tests
{
    public class ValidTestObject : IValidatableObject
    {
        public ValidTestObject() => Recursion = this;

        public ValidTestObject Recursion { get; set; }

        public string StringProperty { get; set; } = string.Empty;

        [NoValidation, MinLength(3), MaxLength(5), RegularExpression("^[a-z]{3,5}$")]
        public string ValidatedStringProperty { get; set; } = "test";

        [MinLength(3), MaxLength(5), RegularExpression("^[a-z]{3,5}$")]
        public string NotValidatedStringProperty { get; set; } = "test";

        [NoValidation]
        public string IgnoredStringProperty { get; set; } = null!;

        [Range(1, 5)]
        public int IntProperty { get; set; } = 1;

        [Required]
        public string? RequiredProperty { get; set; } = "test";

        [CountLimit(1,2)]
        public Dictionary<string, object> DictPropertyValue { get; set; } = new()
        {
            {"a",new() },
            {"b",new() }
        };

        [CountLimit(1, 2)]
        public Dictionary<object, string> DictPropertyKey { get; set; } = new()
        {
            {new(), "a" },
            {new(), "b" }
        };

        [CountLimit(1, 2)]
        [ItemNullable]
        public Dictionary<object, object?> DictPropertyKeyValue { get; set; } = new()
        {
            {new(), new() },
            {new(), null }
        };

        [CountLimit(1, 2)]
        public List<object> ListProperty { get; set; } = new() { new(), new() };

        [CountLimit(1, 2)]
        [ItemNullable]
        public List<object?> ListPropertyNullable { get; set; } = new() { new(), null };

        [CountLimit(1, 2)]
        public object[] ArrProperty { get; set; } = new object[] { new(), new() };

        [CountLimit(1, 2)]
        [ItemNullable]
        public object?[] ArrPropertyNullable { get; set; } = new object?[] { new(), null };

        [CountLimit(1, 2)]
        public IEnumerable<object> EnumerableProperty { get; set; } = new object[] { new(), new() }.AsEnumerable();

        [CountLimit(1, 2)]
        [ItemNullable]
        public IEnumerable<object?> EnumerablePropertyNullable { get; set; } = new object?[] { new(), null }.AsEnumerable();

        public ValidTestObject2 ObjectProperty { get; set; } = new InvalidTestObjectNoValidation();

        [ItemStringLength(5, ItemValidationTargets.Key)]
        [ItemStringLength(5)]
        public Dictionary<string, string> DictProperty2 { get; set; } = new()
        {
            {"test","test" },
            {"test2","test2" }
        };

        [ItemStringLength(5)]
        public List<string> ListProperty2 { get; set; } = new()
        {
            "test",
            "test2"
        };

        [ItemStringLength(5)]
        public string[] ArrProperty2 { get; set; } = new string[]
        {
            "test",
            "test2"
        };

        [ItemStringLength(5)]
        public IEnumerable<string> EnumerableProperty2 { get; set; } = new string[]
        {
            "test",
            "test2"
        }.AsEnumerable();

        [ItemNoValidation]
        public string[] ItemIgnoredProperty { get; set; } = new string[]
        {
            "test",
            null!
        };

        [CountLimit(2, 3)]
        [ItemCountLimit(2, 3)]
        [ItemStringLength(5, ArrayLevel = 1)]
        public string[][] DeepArrProperty { get; set; } = new string[][]
        {
            new string[] { "test", "test2"},
            new string[] { "test", "test2"},
        };

        public bool RequiredIfConditional { get; set; }

        [RequiredIf(nameof(RequiredIfConditional), true)]
        public string? ConditionalProperty { get; set; }

        [AllowedValues(TestEnum.Valid), DeniedValues(TestEnum.Invalid)]
        public TestEnum EnumProperty { get; set; } = TestEnum.Valid;

        public TestEnum Enum2Property { get; set; } = TestEnum.Valid;

        IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            List<ValidationResult> results = new();
            Validator.TryValidateProperty(ValidatedStringProperty, new(this, serviceProvider: null, items: null) { MemberName = nameof(ValidatedStringProperty) }, results);
            if (StringProperty != string.Empty) results.Add(new($"Invalid {nameof(StringProperty)} value", new string[] { nameof(StringProperty) }));
            return results;
        }
    }
}
