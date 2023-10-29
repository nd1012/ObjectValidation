using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace wan24.ObjectValidation
{
    /// <summary>
    /// Options validator helper
    /// </summary>
    public static class OptionsValidator
    {
        /// <summary>
        /// Add an options validator
        /// </summary>
        /// <typeparam name="T">Options type</typeparam>
        /// <param name="services">Services</param>
        /// <returns>Services</returns>
        public static IServiceCollection AddOptionsValidator<T>(this IServiceCollection services) where T : class
            => services.AddSingleton<IValidateOptions<T>, OptionsValidator<T>>();

        /// <summary>
        /// Add an options validator
        /// </summary>
        /// <typeparam name="T">Options type</typeparam>
        /// <param name="services">Services</param>
        /// <param name="name">Name to limit the validator to</param>
        /// <returns>Services</returns>
        public static IServiceCollection AddOptionsValidator<T>(this IServiceCollection services, string name) where T : class
            => services.AddSingleton<IValidateOptions<T>, OptionsValidator<T>>(serviceProvider => new(name:name));

        /// <summary>
        /// Add options validators for the given options types
        /// </summary>
        /// <param name="services">Services</param>
        /// <param name="types">Options types</param>
        /// <returns>Services</returns>
        public static IServiceCollection AddOptionsValidation(this IServiceCollection services, params Type[] types)
        {
            for (int i = 0, len = types.Length; i < len; i++)
            {
                if (types[i].IsInterface || types[i].IsValueType)
                    throw new ArgumentException($"Type #{i + 1} ({types[i]}) can't be used for {nameof(OptionsValidator)} (must be a class type)", nameof(types));
                services.AddSingleton(types[i], typeof(OptionsValidator<>).MakeGenericType(types[i]));
            }
            return services;
        }

        /// <summary>
        /// Add an options validator
        /// </summary>
        /// <typeparam name="T">Options type</typeparam>
        /// <param name="builder">Options builder</param>
        /// <returns>Services</returns>
        public static OptionsBuilder<T> Validate<T>(this OptionsBuilder<T> builder) where T : class
        {
            builder.Services.AddOptionsValidator<T>();
            return builder;
        }

        /// <summary>
        /// Add an options validator
        /// </summary>
        /// <typeparam name="T">Options type</typeparam>
        /// <param name="builder">Options builder</param>
        /// <param name="name">Name to limit the validator to</param>
        /// <returns>Services</returns>
        public static OptionsBuilder<T> Validate<T>(this OptionsBuilder<T> builder, string name) where T : class
        {
            builder.Services.AddOptionsValidator<T>(name);
            return builder;
        }
    }

    /// <summary>
    /// Options validator
    /// </summary>
    /// <typeparam name="T">Options type</typeparam>
    public sealed class OptionsValidator<T> : IValidateOptions<T> where T : class
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="serviceProvider">Service provider</param>
        /// <param name="name">Name to limit this instance to</param>
        public OptionsValidator(IServiceProvider? serviceProvider = null, string? name = null)
        {
            ServiceProvider = serviceProvider;
            Name = name;
        }

        /// <summary>
        /// Service provider
        /// </summary>
        public IServiceProvider? ServiceProvider { get; }

        /// <summary>
        /// Name which this instance is limited to
        /// </summary>
        public string? Name { get; }

        /// <inheritdoc/>
        ValidateOptionsResult IValidateOptions<T>.Validate(string? name, T options)
        {
            if (Name is not null && name != Name) return ValidateOptionsResult.Skip;
            options.ValidateObject(out List<ValidationResult> results, name, members: null, ServiceProvider);
            return results.Count == 0
                ? ValidateOptionsResult.Success
                : ValidateOptionsResult.Fail(from result in results
                                             select $"[Member {GetMemberName(name, result.MemberNames)}] {result.ErrorMessage}");
            //TODO .NET 8: Use ValidateOptionsResultBuilder
        }

        /// <summary>
        /// Get a member name
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="memberNames">Member names</param>
        /// <returns>Member name</returns>
        internal static string GetMemberName(string? name, IEnumerable<string> memberNames)
        {
            string? memberName = memberNames.FirstOrDefault(),
                member = memberName ?? "(unknown)";
            if (name is not null && memberName is not null && member.Length > name.Length && member.StartsWith(name) && member[name.Length] == '.')
                return member;
            StringBuilder sb = new(name);
            if (name is not null) sb.Append('.');
            sb.Append(member);
            return sb.ToString();
        }
    }
}
