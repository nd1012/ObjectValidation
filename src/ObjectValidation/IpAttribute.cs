using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Sockets;

namespace wan24.ObjectValidation
{
    /// <summary>
    /// IP address validation attribute
    /// </summary>
    public class IpAttribute : ValidationAttribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="family">Address family</param>
        public IpAttribute(AddressFamily family = AddressFamily.InterNetwork) => AddressFamily = family;

        /// <summary>
        /// Address family
        /// </summary>
        public AddressFamily AddressFamily { get; }

        /// <inheritdoc/>
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
                return new(
                    ErrorMessage ?? (validationContext.MemberName == null ? $"IP address value is NULL" : $"{validationContext.MemberName}: IP address value is NULL"),
                    validationContext.MemberName == null ? null : new string[] { validationContext.MemberName }
                    );
            if (value is not string ipStr)
                return new(
                    ErrorMessage ?? (validationContext.MemberName == null ? $"IP address value is not a {typeof(string)}" : $"{validationContext.MemberName}: IP address value is not a {typeof(string)}"),
                    validationContext.MemberName == null ? null : new string[] { validationContext.MemberName }
                    );
            if (!IPAddress.TryParse(ipStr, out IPAddress? ip))
                return new(
                    ErrorMessage ?? (validationContext.MemberName == null ? $"Invalid IP address" : $"{validationContext.MemberName}: Invalid IP address"),
                    validationContext.MemberName == null ? null : new string[] { validationContext.MemberName }
                    );
            if (ip.AddressFamily != AddressFamily)
                return new(
                    ErrorMessage ?? (validationContext.MemberName == null ? $"Invalid IP address family ({AddressFamily} expected)" : $"{validationContext.MemberName}: Invalid IP address family ({AddressFamily} expected)"),
                    validationContext.MemberName == null ? null : new string[] { validationContext.MemberName }
                    );
            return null;
        }
    }
}
