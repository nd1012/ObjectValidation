﻿using System.ComponentModel.DataAnnotations;

namespace wan24.ObjectValidation
{
    /// <summary>
    /// Item phone number attribute
    /// </summary>
    /// <remarks>
    /// Constructor
    /// </remarks>
    /// <param name="target">Validation target</param>
    public class ItemPhoneAttribute(ItemValidationTargets target = ItemValidationTargets.Item) : ItemValidationAttribute(target, new PhoneAttribute())
    {
    }
}
