using CountryValidation;
using CountryValidation.DataAnnotations;

/*
 * Copyright 2023 Andreas Zimmermann, wan24.de
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 * http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

namespace wan24.ObjectValidation
{
    /// <summary>
    /// Item company TIN validation attribute
    /// </summary>
    public class ItemCompanyTINAttribute : ItemValidationAttribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="country">Country</param>
        /// <param name="target">Validation target</param>
        public ItemCompanyTINAttribute(Country country = Country.XX, ItemValidationTargets target = ItemValidationTargets.Item) : base(target, new CompanyTINAttribute(country)) { }
    }
}
