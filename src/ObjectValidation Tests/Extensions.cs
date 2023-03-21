using System.ComponentModel.DataAnnotations;

namespace ObjectValidation_Tests
{
    public static class Extensions
    {
        public static void Print(this List<ValidationResult> results)
        {
            Console.WriteLine($"Count: {results.Count}");
            foreach (ValidationResult res in results)
                Console.WriteLine($"{string.Join(", ", res.MemberNames)}: {res.ErrorMessage}");
        }
    }
}
