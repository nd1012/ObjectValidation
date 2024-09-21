using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Security.Cryptography;

namespace wan24.ObjectValidation
{
    /// <summary>
    /// Host name or IP address validation attribute
    /// </summary>
    /// <remarks>
    /// Constructor
    /// </remarks>
    public class HostAttribute() : ValidationAttributeBase()
    {
        /// <summary>
        /// If IPv4 addresses are allowed
        /// </summary>
        public bool AllowIPv4 { get; set; } = true;

        /// <summary>
        /// If IPv6 addresses are allowed
        /// </summary>
        public bool AllowIPv6 { get; set; } = true;

        /// <summary>
        /// Check if the hostname (DNS lookup) or IP address (ICMP) exists
        /// </summary>
        public bool CheckIfExists { get; set; }

        /// <summary>
        /// Ping timeout in ms
        /// </summary>
        public int PingTimeout { get; set; } = 300;

        /// <summary>
        /// Use a cache for <see cref="CheckIfExists"/> results?
        /// </summary>
        public bool UseCache { get; set; } = true;

        /// <summary>
        /// Check if a hostname/IP exists using a cache (may throw; for implementing a cache, this method needs to be overridden)
        /// </summary>
        /// <param name="hostName">Hostname</param>
        /// <param name="ip">IP address</param>
        /// <param name="status">IP status</param>
        /// <returns>If exists</returns>
        protected virtual bool CheckIfExistsCache(string? hostName, IPAddress? ip, out IPStatus status)
        {
            if (hostName is not null)
            {
                IPHostEntry hostEntry = Dns.GetHostEntry(hostName);
                status = IPStatus.Unknown;
                return hostEntry.AddressList.Length > 0 || hostEntry.Aliases.Length > 0;
            }
            Contract.Assert(ip is not null);
            using Ping ping = new();
            PingReply pong = ping.Send(ip, PingTimeout, RandomNumberGenerator.GetBytes(count: 32), new()
            {
                DontFragment = true
            });
            status = pong.Status;
            return pong.Status == IPStatus.Success;
        }

        /// <inheritdoc/>
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is null) return null;
            if (value is not string str) return this.CreateValidationResult($"Hostname or IP address value as {typeof(string)} expected", validationContext);
            UriHostNameType type = Uri.CheckHostName(str);
            IPAddress? ip;
            switch (type)
            {
                case UriHostNameType.Dns:
                    if (!CheckIfExists) return null;
                    try
                    {
                        return CheckIfExistsCache(str, ip: null, out _)
                            ? null
                            : this.CreateValidationResult($"Hostname DNS lookup failed", validationContext);
                    }
                    catch (Exception ex)
                    {
                        return this.CreateValidationResult($"Hostname DNS lookup failed: {ex.Message ?? ex.GetType().ToString()}", validationContext);
                    }
                case UriHostNameType.IPv4:
                    if (!IPAddress.TryParse(str, out ip)) return this.CreateValidationResult($"Host IPv4 address parsing failed", validationContext);
                    if (ip.AddressFamily != AddressFamily.InterNetwork) return this.CreateValidationResult($"Detected host IPv4 address parsed to IPv6", validationContext);
                    break;
                case UriHostNameType.IPv6:
                    if (!IPAddress.TryParse(str, out ip)) return this.CreateValidationResult($"Host IPv6 address parsing failed", validationContext);
                    if (ip.AddressFamily != AddressFamily.InterNetworkV6) return this.CreateValidationResult($"Detected host IPv6 address parsed to IPv4", validationContext);
                    break;
                default:
                    return this.CreateValidationResult($"Hostname or IP address value invalid ({type})", validationContext);
            }
            if (!CheckIfExists) return null;
            try
            {
                return CheckIfExistsCache(hostName: null, ip, out IPStatus status)
                    ? null
                    : this.CreateValidationResult($"Ping to {ip} failed: {status}", validationContext);
            }
            catch (Exception ex)
            {
                return this.CreateValidationResult($"Ping to {ip} failed exceptional: {ex.Message ?? ex.GetType().ToString()}", validationContext);
            }
        }
    }
}
