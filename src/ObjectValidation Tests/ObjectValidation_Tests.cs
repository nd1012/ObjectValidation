using System.ComponentModel.DataAnnotations;
using wan24.ObjectValidation;
using wan24.Tests;

namespace ObjectValidation_Tests
{
    [TestClass]
    public class ObjectValidation_Tests : TestBase
    {
        public ObjectValidation_Tests()
        {
            ValidateObject.Logger = (message) => Console.WriteLine(message);
            ValidationTemplates.PropertyValidations["test"] = new()
            {
                new MinLengthAttribute(3),
                new MaxLengthAttribute(5),
                new RegularExpressionAttribute("^[a-z]{3,5}$")
            };
            ValidationTemplates.ItemValidations["test"] = new()
            {
                new ItemStringLengthAttribute(5, ItemValidationTargets.Key),
                new ItemStringLengthAttribute(5)
            };
        }

        [TestMethod, Timeout(3000)]
        public void General_Tests()
        {
            List<ValidationResult> results = new();

            // Valid object
            Assert.IsTrue(new ValidTestObject().TryValidateObject(results));
            Assert.AreEqual(0, results.Count);

            // Invalid object
            Assert.IsFalse(new InvalidTestObject().TryValidateObject(results));
            Assert.AreNotEqual(0, results.Count);
            List<string> failedMembers = (from res in results
                                          from name in res.MemberNames
                                          select name)
                .Distinct()
                .ToList();
            Assert.IsFalse(failedMembers.Contains(nameof(ValidTestObject.Recursion)));
            Assert.IsTrue(failedMembers.Contains(nameof(ValidTestObject.StringProperty)));
            Assert.IsTrue(failedMembers.Contains(nameof(ValidTestObject.ValidatedStringProperty)));
            Assert.IsTrue(failedMembers.Contains(nameof(ValidTestObject.ReferencedStringProperty)));
            Assert.IsTrue(failedMembers.Contains(nameof(ValidTestObject.TemplateStringProperty)));
            Assert.IsTrue(failedMembers.Contains(nameof(ValidTestObject.TemplateStringProperty2)));
            Assert.IsTrue(failedMembers.Contains(nameof(ValidTestObject.NotValidatedStringProperty)));
            Assert.IsFalse(failedMembers.Contains(nameof(ValidTestObject.IgnoredStringProperty)));
            Assert.IsTrue(failedMembers.Contains(nameof(ValidTestObject.IntProperty)));
            Assert.IsTrue(failedMembers.Contains(nameof(ValidTestObject.RequiredProperty)));
            Assert.IsTrue(failedMembers.Contains($"{nameof(ValidTestObject.DictPropertyValue)}[value#2]"));
            Assert.IsTrue(failedMembers.Contains(nameof(ValidTestObject.DictPropertyKey)));
            Assert.IsTrue(failedMembers.Contains(nameof(ValidTestObject.DictPropertyKeyValue)));
            Assert.IsTrue(failedMembers.Contains($"{nameof(ValidTestObject.ListProperty)}[2]"));
            Assert.IsTrue(failedMembers.Contains($"{nameof(ValidTestObject.ListPropertyNullable)}[1].{nameof(ValidTestObject.StringProperty)}"));
            Assert.IsTrue(failedMembers.Contains($"{nameof(ValidTestObject.ArrProperty)}[1]"));
            Assert.IsTrue(failedMembers.Contains($"{nameof(ValidTestObject.ArrPropertyNullable)}[1].{nameof(ValidTestObject.StringProperty)}"));
            Assert.IsFalse(failedMembers.Any(n => n.StartsWith($"{nameof(ValidTestObject.EnumerableProperty)}.")));
            Assert.IsTrue(failedMembers.Contains($"{nameof(ValidTestObject.EnumerablePropertyNullable)}[1].{nameof(ValidTestObject.StringProperty)}"));
            Assert.IsTrue(failedMembers.Contains($"{nameof(ValidTestObject.ObjectProperty)}.{nameof(ValidTestObject.StringProperty)}"));
            Assert.IsTrue(failedMembers.Contains($"{nameof(ValidTestObject.DictProperty2)}[value#1]"));
            Assert.IsTrue(failedMembers.Contains($"{nameof(ValidTestObject.DictProperty2)}[value#2]"));
            Assert.IsTrue(failedMembers.Contains($"{nameof(ValidTestObject.DictProperty2)}[value#3]"));
            Assert.IsTrue(failedMembers.Contains($"{nameof(ValidTestObject.DictProperty2)}[key#3]"));
            Assert.IsTrue(failedMembers.Contains($"{nameof(ValidTestObject.ReferencedDictProperty)}[value#1]"));
            Assert.IsTrue(failedMembers.Contains($"{nameof(ValidTestObject.ReferencedDictProperty)}[value#2]"));
            Assert.IsTrue(failedMembers.Contains($"{nameof(ValidTestObject.ReferencedDictProperty)}[value#3]"));
            Assert.IsTrue(failedMembers.Contains($"{nameof(ValidTestObject.ReferencedDictProperty)}[key#3]"));
            Assert.IsTrue(failedMembers.Contains($"{nameof(ValidTestObject.TemplateDictProperty)}[value#1]"));
            Assert.IsTrue(failedMembers.Contains($"{nameof(ValidTestObject.TemplateDictProperty)}[value#2]"));
            Assert.IsTrue(failedMembers.Contains($"{nameof(ValidTestObject.TemplateDictProperty)}[value#3]"));
            Assert.IsTrue(failedMembers.Contains($"{nameof(ValidTestObject.TemplateDictProperty)}[key#3]"));
            Assert.IsTrue(failedMembers.Contains($"{nameof(ValidTestObject.ListProperty2)}[1]"));
            Assert.IsTrue(failedMembers.Contains($"{nameof(ValidTestObject.ListProperty2)}[2]"));
            Assert.IsTrue(failedMembers.Contains($"{nameof(ValidTestObject.ArrProperty2)}[1]"));
            Assert.IsTrue(failedMembers.Contains($"{nameof(ValidTestObject.ArrProperty2)}[2]"));
            Assert.IsTrue(failedMembers.Contains($"{nameof(ValidTestObject.EnumerableProperty2)}[2]"));
            Assert.IsTrue(failedMembers.Contains($"{nameof(ValidTestObject.ItemIgnoredProperty)}"));
            Assert.IsFalse(failedMembers.Contains($"{nameof(ValidTestObject.ItemIgnoredProperty)}[1]"));
            Assert.IsFalse(failedMembers.Contains($"{nameof(ValidTestObject.ItemIgnoredProperty)}[2]"));
            Assert.IsFalse(failedMembers.Contains($"{nameof(ValidTestObject.DeepArrProperty)}[1][1]"));
            Assert.IsFalse(failedMembers.Contains($"{nameof(ValidTestObject.DeepArrProperty)}[2][0]"));
            Assert.IsTrue(failedMembers.Contains(nameof(ValidTestObject.RequiredProperty)));
            Assert.IsTrue(failedMembers.Contains(nameof(ValidTestObject.EnumProperty)));
            Assert.IsTrue(failedMembers.Contains(nameof(ValidTestObject.Enum2Property)));
            Assert.IsTrue(failedMembers.Contains(nameof(ValidTestObject.HostProperty)));
            Assert.AreEqual(54, results.Count);

            // Validation exception
            Assert.IsFalse(results.HasValidationException());
            results.Clear();
            Assert.ThrowsException<ObjectValidationException>(() => new InvalidTestObject().ValidateObject(results));
            Assert.IsTrue(results.HasValidationException());

            // IObjectValidatable / ValidatableObject
            results.Clear();
            ValidTestObject3 valid = new();
            Assert.IsTrue(Validator.TryValidateObject(valid, new(valid), results, validateAllProperties: false));
            InvalidTestObject3 invalid = new();
            Assert.IsFalse(Validator.TryValidateObject(invalid, new(invalid), validationResults: null, validateAllProperties: false));
        }
    }
}
