using System.Net.Sockets;

namespace wan24.ObjectValidation
{
    /// <summary>
    /// Item IP address validation attribute
    /// </summary>
    /// <remarks>
    /// Constructor
    /// </remarks>
    /// <param name="family">Address family</param>
    /// <param name="target">Validation target</param>
    public class ItemIpAttribute(AddressFamily family = AddressFamily.InterNetwork, ItemValidationTargets target = ItemValidationTargets.Item)
        : ItemValidationAttribute(target,new IpAttribute(family))
    {
    }
}
