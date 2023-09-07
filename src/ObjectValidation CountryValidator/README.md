# ObjectValidation

This library contains some object validation helper:

- `(Try)ValidateObject` extension for validating any object type
- `Validate(Value|Item)` extension for validating any value using a set of 
validation attributes or a pre-defined validation template
- `NoValidationAttribute` for excluding types or properties from validation
- `ObjectValidationException` for handling validation errors
- Validation events
- `CountLimitAttribute` for limiting dictionary, list, array, collection and 
enumerable lengths
- `ValidatableObjectBase` for implementing automatic validated types
- Deep dictionary and list key/value validation
- `ICountable` and `ILongCountable` interfaces for count limitation
- `ItemNullableAttribute` for (non-)nullable dictionary or list item validation
- SWIFT validation attributes for ISO 13616 IBAN and ISO 9362 BIC (SWIFT codes)
- ABA RTN validation attributes (MICR and fraction formats are supported)
- IP address validation attribute for IPv4 and IPv6
- Country ISO 3166-1 alpha-2 code validation
- Currency ISO 4217 code validation
- Money amount validation
- Luhn checksum validation
- XRechnung routing validation
- European VAT ID validation
- XML validation
- Conditional value requirements
- Enumeration value validation
- Validation references
- Validation templates (also conditional)

It has been developed with the goal to offer an automatted deep object 
validation with support for deep dictionaries and lists contents, too.

All public instance properties with a public getter method will be validated, 
unless there's a `NoValidationAttribute` present. You can use any 
`ValidationAttribute` to define property value constraints.

Also the `IValidatableObjectBase` interface is supported by using the 
`Validator.TryValidateObject` method with the `validateAllProperties` 
parameter having the value `true` (the `IValidationObject` validation method 
will be called in a second round). Using this interface you can implement some 
more advanced custom validations, if required.

**NOTE**: The ObjectValidation doesn't replace the classic built in .NET 
object validation - it extends the existing routines with deep validation and 
more. Btw. you don't need to use the ObjectValidation methods, if you only 
want to profit from the included general validation attributes for SWIFT etc.

## How to get it

The libraries are available as NuGet packages:

- [ObjectValidation](https://www.nuget.org/packages/ObjectValidation/)
- [ObjectValidation-CountryValidator](https://www.nuget.org/packages/ObjectValidation-CountryValidator/)

## License

The **ObjectValidation** is licensed using the **MIT license**.

The **ObjectValidation-CountryValidator** extension is licensed using the 
**Apache-2.0 license**.

## Additional validations

| Validation | Attribute |
| --- | --- |
| Validations by event handlers | (see `ValidationExtensions`) |
| Dictionary list and enumerable count limit | `CountLimitAttribute` |
| Non-nullable property values (`null` values will fail, even if the type is nullable) | `DisallowNullAttribute` |
| Nullable property values (does allow a `null` value, even if the type is not nullable) | `AllowNullAttribute` |
| Dictionary, list and array key/value validation (including validating (non-)null items) | `ItemNullableAttribute` |
| ISO 13616 IBAN and ISO 9362 BIC (SWIFT code) validation | `IbanAttribute`, `BicAttribute` |
| ABA RTN validation (MICR/fraction) validation | `AbaRtnAttribute` |
| IP address validation | `IpAttribute` |
| Country ISO 3166-1 alpha-2 code validation | `CountryAttribute` |
| Currency ISO 4217 code validation | `CurrencyAttribute` |
| Money amount validation | `AmountAttribute` |
| Luhn checksum validation | `LuhnChecksumAttribute` |
| XRechnung routing validation | `XRechnungRouteAttribute` |
| European VAT ID validation | `EuVatIdAttribute` |
| XML validation | `XmlAttribute` |
| Conditional value requirement | `RequiredIfAttribute` |
| Allowed/denied values | `AllowedValuesAttribute`, `DeniedValuesAttribute` |
| Enumeration value | (none - using the type) |
| Validation references | `ValidationReferenceAttribute` |
| Validation templates | `ValidationTemplateAttribute`, `ValidationTemplateIfAttribute` |

## Deep object validation

Nested property object value validation is supported for:

- Dictionaries
- Lists
- Arrays
- Collections
- Enumerables
- Any non-value types

## Length/count validation

The length/count of dictionaries, lists, arrays and enumerables can be 
validated using the `CountLimitAttribute` (which may not work for enumerables, 
if not used trough ObjectValidation methods!).

### Limiting countables

By implementing the `ICountable` or `ILongCountable` interfaces you can use 
the `CountLimitAttribute` for limiting the minimum/maximum count of an object.

## `null` values

The object validation will generate an error, if a non-null property has a 
`null` value (or any non-null-expected value is `null`).

```cs
public string StringProperty { get; set; } = null!// This will generate an error
```

If a non-null property was initialized with `null`, and the code forgot to set 
another value, the ObjectValidation will create an error for this property.

Another feature is the validation of (non-)null dictionary or list items:

```cs
[ItemNullable]
public List<string?> NullableList { get; set; } = new() { null };// Ok

[ItemNullable]
public List<string> NonNullList { get; set; } = new() { null! };// Ok

public List<string?> NullableList2 { get; set; } = new() { null };// This will generate an error

public List<string> NonNullList2 { get; set; } = new() { null! };// This will generate an error
```

Using the `ItemNullableAttribute` you can define, if the dictionary value or 
the list item may be `null`.

Nullability can also be defined by the .NET `AllowNullAttribute` and 
`DisallowNullAttribute` - `DisallowNullAttribute` has priority. Those 
attributes can be used to override the nullability of a property type.

## General examples

```cs
// Only determine if valid/invalid
if(!anyObject1.TryValidateObject())
{
    // Object invalid
}

// Get validation messages in a new list
if(!anyObject2.TryValidateObject(out List<ValidationResult> results))
{
    // Object invalid
}

// Get validation messages in an existing list
results.Clear();
if(!anyObject3.TryValidateObject(results)) 
{
    // Object invalid
}

// Check for exceptions during validation
if(results.HasValidationException())
{
    Console.WriteLine(results.First(r => r.ErrorMessage?.StartsWith(ValidationExtensions.VALIDATION_EXCEPTION_PREFIX)).ErrorMessage);
}

// Fluent syntax without the "Try" method name prefix (will throw a ObjectValidationException on error)
return anyObject4.ValidateObject();
```

**NOTE**: Any `ValidationResult` will let the validation fail in total!

In case you want to forward error messages to a peer, you may want to exclude 
exceptions and their stack trace. You can use the `HasValidationException` 
method and a LINQ expression like the one from the example to filter out any 
exception in the results.

`ValidatableObjectBase` types may be validated automatic (depending on the app 
context), because they implement the `IValidatableObject` interface. The 
`ValidatableObjectBase` executes the ObjectValidation method internal. For 
using the `ValidatableObject` base class, simply extend from it, and call the 
base constructor from your constructor methods.

**TIP**: You should use the `ValidatableObject` base type, if possible! By 
only implementing the `IValidatableObject` interface your type may be 
validated automatic, but not by the ObjectValidation library!

In case you can't extend from `ValidatableObjectBase`, you can implement the 
`IValidatableObject` and `IObjectValidatable` interfaces like this:

```cs
public class YourType : AnyBaseType, IObjectValidatable
{
    ...

    // Implement IValidatableObject
    IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext context)
        => ValidatableObject.ObjectValidatable(this);
}
```

## Conditional value requirement

If a property value is required in case another property has a specified value:

```cs
[Bic]
public string BIC { get; set; } = null!;

[Iban, RequiredIf(nameof(ABA), RequiredIfNull = true)]
public string? IBAN { get; set; }

[AbaRtn, RequiredIf(nameof(IBAN), RequiredIfNull = true)]
public string? ABA { get; set; }
```

In this example, a BIC is required in combination with an IBAN or an ABA RTN. 
The `RequiredIfAttribute.RequiredIfNull` is set to `true` to check for IBAN 
and ABA, if the other property value is `null`: In case ABA is `null`, IBAN is 
required. In case IBAN is `null`, ABA is required.

Another example:

```cs
public bool DeliveryAddress { get; set; }

[RequiredIf(nameof(DeliveryAddress), true)]
public string? DeliveryName { get; set; }

[RequiredIf(nameof(DeliveryAddress), true)]
public string? DeliveryStreet { get; set; }

[RequiredIf(nameof(DeliveryAddress), true)]
public string? DeliveryZip { get; set; }

[RequiredIf(nameof(DeliveryAddress), true)]
public string? DeliveryCity { get; set; }

[RequiredIf(nameof(DeliveryAddress), true), Country]
public string? DeliveryCountry { get; set; }
```

In case the value of `DeliveryAddress` is `true`, all delivery address 
properties are required to have a value.

**NOTE**: Because the validation attribute needs to access the validated 
object properties, it's required to work with valid validation contexts, which 
contain the validated object instance.

## Dictionary and list key/value validation

By implementing the `IItemValidationAttribute` interface, you can define 
validation attributes that are going to be applied on each key/value of a 
dictionary or a list. There's an almost generic `ItemValidationAttribute` 
which allows to construct with any `ValidationAttribute`. For the .NET built 
in validation attributes (most of them) there are adapting attributes, which 
start with `Item` and continue with the original attribute name. By setting a 
`target` in the `ItemValidationAttribute` constructor, you can define, if a 
validation should only be applied to a dictionary key. Examples:

```cs
[CountLimit(1, 10)]// Dictionary object validation
[ItemRequired(ItemValidationTarget.Key), ItemStringLength(100, ItemValidationTarget.Key)]// Key validation
[ItemRequired, ItemStringLength(255)]// Value validation
public Dictionary<string, string> Dict { get; }

[CountLimit(1, 10)]// List object validation
[ItemRequired, ItemStringLength(255)]// Item validation
public List<string> List { get; }
```

The dictionary needs to have at last one, maximum 10 key/value pairs, where 
the key length needs to be between 1-100 characters, and the value length 
needs to be 1-255 characters.

The list needs to have at last one, maximum 10 items, where each item length 
is limited to 1-255 characters.

Using the `ItemNoValidationAttribute` you can disable key/value validation for 
a property. In case of an enumerable value, the enumeration needs to be 
executed in case there's a `CountLimitAttribute` present.

These item validation adapters exist:

| Property validation | Item validation |
| --- | --- |
| `ValidationAttribute` | `ItemValidationAttribute` |
| `CountLimitAttribute` | `ItemCountLimitAttribute` |
| `RequiredAttribute` | `ItemRequiredAttribute` |
| `CompareAttribute` | `ItemCompareAttribute` |
| `CreditCardAttribute` | `ItemCreditCardAttribute` |
| `EmailAddressAttribute` | `ItemEmailAddressAttribute` |
| `MaxLengthAttribute` | `ItemMaxLengthAttribute` |
| `MinLengthAttribute` | `ItemMinLengthAttribute` |
| `NoValidationAttribute` | `ItemNoValidationAttribute` |
| `PhoneAttribute` | `ItemPhoneAttribute` |
| `RangeAttribute` | `ItemRangeAttribute` |
| `RegularExpressionAttribute` | `ItemRegularExpressionAttribute` |
| `StringLengthAttribute` | `ItemStringLengthAttribute` |
| `UrlAttribute` | `ItemUrlAttribute` |
| `DataTypeAttribute` | `ItemDataTypeAttribute` |
| `IbanAttribute` | `ItemIbanAttribute` |
| `BicAttribute` | `ItemBicAttribute` |
| `AbaRtnAttribute` | `ItemAbaRtnAttribute` |
| `IpAttribute` | `ItemIpAttribute` |
| `CountryAttribute` | `ItemCountryAttribute` |
| `CurrencyAttribute` | `ItemCurrencyAttribute` |
| `AmountAttribute` | `ItemAmountAttribute` |
| `LuhnChecksumAttribute` | `ItemLuhnChecksumAttribute` |
| `XRechnungRouteAttribute` | `ItemXRechnungRouteAttribute` |
| `EuVatIdAttribute` | `ItemEuVatIdAttribute` |
| `AllowedValuesAttribute` | `ItemAllowedValuesAttribute` |
| `DeniedValuesAttribute` | `ItemDeniedValuesAttribute` |
| `CustomValidationAttribute` | `ItemCustomValidationAttribute` |
| `ValidationReferenceAttribute` | `ItemValidationReferenceAttribute` |
| `ValidationReferenceIfAttribute` | `ItemValidationReferenceIfAttribute` |
| `ValidationTemplateAttribute` | `ItemValidationTemplateAttribute` |
| `ValidationTemplateIfAttribute` | `ItemValidationTemplateIfAttribute` |

You can use the `ItemNoValidationAttribute` at the class level to prevent from 
validating and dictionary or list contents.

**NOTE**: By setting the `ArrayLevel` (starts with zero) of an 
`ItemValidationAttribute`, you can specify to use the item for the desired 
array level only. This enables array of array etc. item validations. Entering 
a deeper array level counts as recursion. The array level can be set for 
dictionary keys, too, if they're a dictionary or a list.

Use the `ItemNullableAttribute`, if the dictionary value or list item may be 
`null` (even if you wrote `T?` in your code, because the nullability 
information may not be available during validation!).

## Enumeration value validation

An enumeration can be a value list or combined flags. Both variants are 
validated by checking if

- the value contains undefined flags
- the value is an undefined enumeration value

This ensures, that only defined enumeration (flag) values can be used.

## Validation references

Using a validation reference attribute you can inherit validation attributes 
from another property, and even from another type:

```cs
public class A
{
    [StringLength(3)]
    public string Property { get; set; } = string.Empty;
}

public class B
{
    [ValidationReference(typeof(A), nameof(A.Property))]
    public string Property { get; set; } = string.Empty;
}

B obj = new()
{
    Property = "1234"
};
obj.ValidateObject();// Will fail, 'cause A.Property limits the string length to 3 characters
```

Now `B.Property` uses the validation attributes from `A.Property`.

**CAUTION**: Attributes which would skip property/item validation will be 
ignored!

**NOTE**: The validation will fail with the first validation result of an 
attribute from the target property.

## Direct value validation

Actually the .NET validation attributes are being used at properties, but in 
case you'd like to validate a value which is not hosted by a types property, 
you'll have to instance and call the validation methods of the attributes 
manually.

The `ValueValidation` extension methods allow to apply a set of validation 
attributes to any value, and even a pre-defined validation template:

```cs
// Using validation attribute instances
IEnumerable<ValidationResult> results = anyValue.ValidateValue(new EmailAddressAttribute(), ...);
if(results.Any()) throw new Exception("Invalid value");

// Using pre-defined validation templates from ValidationTemplates
ValidationTemplates.PropertyValidations["email"] = new()
{
    new EmailAddressAttribute(),
    ...
};
IEnumerable<ValidationResult> results = anyValue.ValidateValue("email");
if(results.Any()) throw new Exception("Invalid value");
```

Using the `ValidateItem` extension methods, you can also validate a value as 
an item using `IItemValidationAttribute` implementing attributes or pre-
defined templates from `ValidationTemplates.ItemValidations`.

**NOTE**: Instead of returning only the first validation failure when using 
validation templates on properties (`ValidationTemplateAttribute`), the 
template using `Validate(Value|Item)` methods will yield all validation 
results for a value.

The `Validate(AsValue|Item)` extension will throw an 
`ObjectValidationException`, while the `TryValidate(AsValue|Item)` extension 
returns a boolean indicator, if the value has been validated without any error.

## Validation templates

Similar to validation references you can use validation templates, which are 
not bound to another existing property, but managed in the 
`ValidationTemplates` class:

```cs
// Define a property validation template
ValidationTemplates.PropertyValidations["Template name"] = new()
{
    new StringLengthAttribute(3)
};

// Apply the template
public class YourType
{
    [ValidationTemplate("Template name")]
    public string Property { get; set; } = string.Empty;
}

YourType obj = new()
{
    Property = "1234"
};
obj.ValidateObject();// Will fail, 'cause the validation template limits the string length to 3 characters
```

The same is also available for item validation, using the 
`ValidationTemplates.ItemValidations` store, and the 
`ItemValidationTemplateAttribute`.

**CAUTION**: Attributes which would skip property/item validation will be 
ignored!

**NOTE**: The validation will fail with the first validation result of an 
attribute from the template, if not validated using the ObjectValidation 
methods.

The `ValidationTemplateIfAttribute` allows to apply a template only in case a 
condition was met (just as with the `RequiredIfAttribute`).

## Deny and force type validation

Using the `ValidatableTypes` class you can define types which are not 
validated, or which are forced to be vlidated, in addition to the default type 
filter, which doesn't validate a type if

- it's a non-enum value type
- it's an array
- the type was marked with a `NoValidationAttribute` attribute

A denied type can't be forced to be validated.

You can add a generic type definition for generic (final) types, and also 
abstract types or interfaces. Anyway, when determining if a type should be 
validated, its generic type definition (if the given type is generic) will 
be checked, too - but not inherited (maybe abstract) types or interfaces.

Per default these types won't be validated:

- `string`
- `object`
- `IQueryable<>`

**NOTE**: `string` and `object` shouldn't be removed from the denied type 
list!

By attaching to the `ValidatableTypes.OnIsTypeValidatable`, you can make a 
conditional exception for an usually not validated type.

## Force to fail with an exception

If you set the parameter `throwOnError` value to `true`, the validation will 
throw a `ObjectValidationException`, as soon as an object was invalidated.

## Validation of a property group

By giving a list of member names to validate as `members` parameter, you can 
avoid validating all properties which could be validated (f.e. you could 
validate only a property group).

## Handling validation events

| Event | Description |
| --- | --- |
| `OnObjectValidation` | You can perform validations before any other validations have been executed. When the event was cancelled, there won't be any following validation, and the produced result will be used. |
| `OnObjectValidationFailed` | Raised, if the object validation failed. You may add additional error messages, before the validation method returns. |
| `OnObjectPropertyValidation` | You can perform validations before any other validations have been executed for the property. When the event was cancelled, there won't be any following validation, and the produced result will be used for the current property. The validation will then continue with the next property. |
| `OnObjectPropertyValidationFailed` | Raised, if the object validation failed. You may add additional error messages, before the validation method continues with the next property. When the event was cancelled, the following property validations will be skipped. |

If the event arguments don't offer a `PropertyInfo` in the `Property` 
property, the event was raised for the validated object.

An event handler can set a failed state by setting the `Result` property of 
the event arguments to `false`.

**WARNING**: Do not set the value `true` to the `Result` property! An already 
failed state can't be deleted, unless the original result `OriginalResult` was 
`true`.

The object validation will fail, if there was any validation result, or the 
overall result is `false`.

## Object validation methods

In addition to the .NET object validation methods you can validate objects 
like this:

```cs
// Will throw an exception on error
try
{
    obj.ValidateObject(out List<ValidationResult> results);
}
catch(ObjectValidationException ex)
{
    // Handle the object errors
}

// Won't throw an exception
if(!obj.TryValidateObject(out List<ValidationResult> results))
{
    // Handle the object errors
}

// Will execute an error handler (which may throw, f.e.)
bool valid = obj.EnsureValidObject((obj, results) => ...);

// The handler may correct object validation errors (or throw, f.e.) and return a valid object
obj = obj.GetValidObject((obj, results) => ...);
```

For nullable objects you can use the static methods from `ValidateObject`.

## Logging

If you set a log delegate to `ValidateObject.Logger`, you can log all object 
validation errors:

```cs
ValidateObject.Logger = (message) => ...;
```

Per default messages will be logged to the attached debugger.

## Found a bug?

If the object validation doesn't work for you as expected, or you have any 
idea for improvements, please open an issue - I'd be glad to help and make 
ObjectValidation become even better! Push requests are welcome, too :)

## Good to know

### Recursion protection

The object validation implements a recursion detection and won't end up in an 
endless loop, unless you produce an endless loop within your custom 
`IValidatableObject` validation implementation or the event handling.

You can define a maximum recursion depth in the 
`ValidationExtensions.MaxDepth` property (the default is 32). If the depth 
would be exceeded during an object validation, this would result in a 
`ObjectValidationException`, which would be catched as a validation error 
result, when not setting `throwOnError` to `true`.

**WARNING**: If you disable the maximum recursion depth validation, this may 
end up in a `StackOverflowException`, which will crash your application.

### Maximum number of error messages

You can limit the maximum number of returned error messages in the 
`ValidationExtensions.MaxErrors` property (the default is 200). The object 
validation would stop, once it was detected that the number of errors exceeds 
this limit. Your validation result list will never contain more error messages 
than defined in this limit.

**WARNING**: Disabling the maximum number of error messages may end up in huge 
object lists, which could lead to memory usage problems and finally a crash of 
your application!

### ASP.NET

ASP.NET (7) automatic validates `IValidatableObject` objects when 
unserializing for an API controller call. The ObjectValidation library will be 
used automatic, if you've used the `ValidatableObjectBase` base class for your 
types, which you want to be validated automatic. Simply use it as base class 
for your DTO objects, and you don't need to care about validation anymore.

You should also understand the difference between `NoValidateAttribute` and 
`NeverValidateAttribute`: `NoValidateAttribute` affects the ObjectValidation 
validation, while `NeverValidateAttribute` affects all validations. So you can 
use the `NoValidateAttribute` to skip double validation by the 
ObjectValidation methods, if required.

### Nullability

The nullability of properties can be determined using reflections, even if one 
returns a generic type. But if f.e. the return type is a 
`YourType : Dictionary<string, string?>`, the object validation isn't able to 
determine the nullability of the second generic type argument of the base 
type, because the nullability information will be discarded during your code 
compilation. For the dictionary item validation please specify the 
`ItemNullableAttribute` in that case. The same applies to deep array 
validations.

Nullability can also be defined by the .NET `AllowNullAttribute` and 
`DisallowNullAttribute` - `DisallowNullAttribute` has priority. Those 
attributes can be used to override the nullability of a property type.

### More validations

Have a look at the 
[CountryValidator](https://github.com/anghelvalentin/CountryValidator) 
project for more validations like

- Social Security Numbers
- Personal Identity Numbers
- More VAT IDs
- Tax IDs for individuals
- Tax IDs for companies
- ZIP codes

for many countries.

The `ObjectValidation-CountryValidator` packet includes references to this 
packet, and exports item validation attributes:

- `ItemCompanyTINAttribute`
- `ItemPersonTINAttribute`
- `ItemSSNAttribute`
- `ItemVATAttribute`
- `ItemZipCodeAttribute`

**NOTE**: The main ObjectValidation library includes validation for European 
VAT IDs only. By using this packet, you can use VAT ID validation for many 
countries around the world.

**CAUTION**: Since the CountryValidator is licensed under Apache-2.0 license, 
I decided to license the `ObjectValidation-CountryValidator` under the same 
license:

Copyright 2023 Andreas Zimmermann, wan24.de

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

[http://www.apache.org/licenses/LICENSE-2.0](http://www.apache.org/licenses/LICENSE-2.0)

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.

### Internal validation information object

An event handler can access the list of seen objects, which is used to prevent 
an endless recursion. The first object of that list is an `IValidationInfo` 
object, which contains some validation context information:

| Property | Description |
| --- | --- |
| `Seen` | Seen objects list |
| `CurrentDepth` | Current recursion depth |
| `ArrayLevel` | Current array level |

**CAUTION**: Please DO NOT remove or exchange this object!

Since array item validations don't call event handlers, the `ArrayLevel` 
property will always be `0`.

## Upcoming changes with .NET 8

Some object validations which I've implemented in the ObjectValidation library 
are now part of the .NET 8 preview. I won't remove them in v1.x, but in v2.x, 
which will target .NET 8.
