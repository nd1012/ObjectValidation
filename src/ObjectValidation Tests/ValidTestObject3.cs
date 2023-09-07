using System.ComponentModel.DataAnnotations;
using wan24.ObjectValidation;

namespace ObjectValidation_Tests
{
    public class ValidTestObject3 : ValidatableObjectBase
    {
        public ValidTestObject3() : base() => Recursion = this;

        public ValidTestObject3 Recursion { get; set; }

        public string StringProperty { get; set; } = string.Empty;

        [NoValidation, MinLength(3), MaxLength(5), RegularExpression("^[a-z]{3,5}$")]
        public string ValidatedStringProperty { get; set; } = "test";

        [MinLength(3), MaxLength(5), RegularExpression("^[a-z]{3,5}$")]
        public string NotValidatedStringProperty { get; set; } = "test";

        [Range(1, 5)]
        public int IntProperty { get; set; } = 1;

        [Required]
        public string? RequiredProperty { get; set; } = "test";

        [CountLimit(1, 2)]
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

        [ItemNoValidation(skipNullValueCheck: true)]
        public string[] ItemIgnoredProperty { get; set; } = new string[]
        {
            "test",
            null!
        };
    }
}
