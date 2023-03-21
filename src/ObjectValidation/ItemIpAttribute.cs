using System.Net.Sockets;

namespace wan24.ObjectValidation
{
    /// <summary>
    /// Item IP address validation attribute
    /// </summary>
    public class ItemIpAttribute : ItemValidationAttribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="family">Address family</param>
        /// <param name="target">Validation target</param>
        public ItemIpAttribute(AddressFamily family = AddressFamily.InterNetwork, ItemValidationTargets target = ItemValidationTargets.Item):base(target,new IpAttribute(family)) { }
    }
}
