using System.ComponentModel.DataAnnotations;
using System.Xml;

//TODO .NET 7: Remove and use (Item)StringSyntaxAttribute instead

namespace wan24.ObjectValidation
{
    /// <summary>
    /// XML validation attribute
    /// </summary>
    public class XmlAttribute : ValidationAttribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public XmlAttribute() : base() { }

        /// <inheritdoc/>
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is null) return null;
            if (value is not string xml)
                return new(
                    ErrorMessage ?? (validationContext.MemberName is null ? $"XML value as {typeof(string)} expected" : $"{validationContext.MemberName}: XML value as {typeof(string)} expected"),
                    validationContext.MemberName is null ? null : new string[] { validationContext.MemberName }
                    );
            try
            {
                new XmlDocument().LoadXml(xml);
            }
            catch(Exception ex)
            {
                return new(
                    ErrorMessage ?? (validationContext.MemberName is null ? $"Invalid XML value: {ex.Message}" : $"{validationContext.MemberName}: Invalid XML value: {ex.Message}"),
                    validationContext.MemberName is null ? null : new string[] { validationContext.MemberName }
                    );
            }
            return null;
        }
    }
}
