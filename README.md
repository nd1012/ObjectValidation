# ObjectValidation (BETA)

This library contains some object validation helper:

- `(Try)ValidateObject` extension for validating any object type
- `NoValidationAttribute` for excluding types or properties from validation
- `ObjectValidationException` for handling validation errors
- Validation events
- `CountLimitAttribute` for limiting dictionary, list, array, collection and 
enumerable lengths
- `ValidatableObject` for implementing automatic validated types
- Deep dictionary and list key/value validation
- `ICountable` and `ILongCountable` interfaces for count limitation
- `ItemNullableAttribute` for (non-)nullable dictionary or list item validation
- SWIFT validation attributes for ISO 13616 IBAN and ISO 9362 BIC (SWIFT codes)
- IP address validation attribute for IPv4 and IPv6
- Country ISO 3166-1 alpha-2 code validation
- Currency ISO 4217 code validation
- Money amount validation
- Luhn checksum validation
- XRechnung routing validation
- European VAT ID validation
- XML validation

It has been developed with the goal to offer an automatted deep object 
validation with support for deep dictionaries and lists contents, too.

All public instance properties with a public getter method will be validated, 
unless there's a `NoValidationAttribute` present. You can use any 
`ValidationAttribute` to define property value constraints.

Also the `IValidatableObject` interface is supported by using the 
`Validator.TryValidateObject` method with the `validateAllProperties` 
parameter having the value `true` (the `IValidationObject` validation method 
will be called in a second round). Using this interface you can implement some 
more advanced custom validations, if required.

**NOTE**: The ObjectValidation doesn't replace the classic built in .NET 
object validation - it extends the existing routines with deep validation and 
more. Btw. you don't need to use the ObjectValidation methods, if you only 
want to profit from the included general validation attributes for SWIFT etc.

## Additional validations

- Nullable types (`null` values in non-nullable properties will fail)
- Dictionary list and enumerable count limit using `CountLimitAttribute`
- Validations by event handlers
- Dictionary and list key/value validation (including validating (non-)null 
items using `ItemNullableAttribute`)
- ISO 13616 IBAN and ISO 9362 BIC (SWIFT code) validation using 
`IbanAttribute` and `BicAttribute`
- IP address validation using `IpAttribute`
- Country ISO 3166-1 alpha-2 code validation using `CountryAttribute`
- Currency ISO 4217 code validation using `CurrencyAttribute`
- Money amount validation using `AmountAttribute`
- Luhn checksum validation using `LuhnChecksumAttribute`
- XRechnung routing validation using `XRechnungRouteAttribute`
- European VAT ID validation using `EuVatIdAttribute`
- XML validation using `XmlAttribute`

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
public List<string> NonNullList { get; set; } = new() { null };// Ok

public List<string?> NullableList2 { get; set; } = new() { null };// This will generate an error

public List<string> NonNullList2 { get; set; } = new() { null! };// This will generate an error
```

Using the `ItemNullableAttribute` you can define, if the dictionary value or 
the list item may be `null`.

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

`ValidatableObject` types may be validated automatic (depending on the app 
context), because they implement the `IValidatableObject` interface. The 
`ValidatableObject` executes the ObjectValidation method internal. For using 
the `ValidatableObject` base class, simply extend from it, and call the base 
constructor from your constructor methods.

**TIP**: You should use the `ValidatableObject` base type, if possible! By 
only implementing the `IValidatableObject` interface your type may be 
validated automatic, but not by the ObjectValidation library!

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
[ItemRequired, ItemStringLength(255]// Item validation
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

- `ValidationAttribute` -> `ItemValidationAttribute`
- `CountLimitAttribute` -> `ItemCountLimitAttribute`
- `RequiredAttribute` -> `ItemRequiredAttribute`
- `CompareAttribute` -> `ItemCompareAttribute`
- `CreditCardAttribute` -> `ItemCreditCardAttribute`
- `EmailAddressAttribute` -> `ItemEmailAddressAttribute`
- `MaxLengthAttribute` -> `ItemMaxLengthAttribute`
- `MinLengthAttribute` -> `ItemMinLengthAttribute`
- `NoValidationAttribute` -> `ItemNoValidationAttribute`
- `PhoneAttribute` -> `ItemPhoneAttribute`
- `RangeAttribute` -> `ItemRangeAttribute`
- `RegularExpressionAttribute` -> `ItemRegularExpressionAttribute`
- `StringLengthAttribute` -> `ItemStringLengthAttribute`
- `UrlAttribute` -> `ItemUrlAttribute`
- `IbanAttribute` -> `ItemIbanAttribute`
- `BicAttribute` -> `ItemBicAttribute`
- `IpAttribute` -> `ItemIpAttribute`
- `CountryAttribute` -> `ItemCountryAttribute`
- `CurrencyAttribute` -> `ItemCurrencyAttribute`
- `AmountAttribute` -> `ItemAmountAttribute`
- `LuhnChecksumAttribute` -> `ItemLuhnChecksumAttribute`
- `XRechnungRouteAttribute` -> `ItemXRechnungRouteAttribute`
- `EuVatIdAttribute` -> `ItemEuVatIdAttribute`

You can use the `ItemNoValidationAttribute` at the class level to prevent from 
validating and dictionary or list contents.

**NOTE**: By setting the `ArrayLevel` (starts with zero) of an 
`ItemValidationAttribute`, you can specify to use the item for the desired 
array level only. This enables array of array etc. item validations. Entering 
a deeper array level counts as recursion. The array level can be set for 
dictionary keys, too, if they're a dictionary or a list.

Use the `ItemNullableAttribute`, if the dictionary value or list item may be 
`null` (even if you wrote `T?` in your code, because the nullability 
information my not be available during validation!).

## Force to fail with an exception

If you set the parameter `throwOnError` value to `true`, the validation will 
throw a `ObjectValidationException`, as soon as an object was invalidated.

## Validation of a property group

By giving a list of member names to validate as `members` parameter, you can 
avoid validating all properties which could be validated (f.e. you could 
validate only a property group).

## Handling validation events

- `OnObjectValidation`: You can perform validations before any other 
validations have been executed. When the event was cancelled, there won't be 
any following validation, and the produced result will be used.
- `OnObjectValidationFailed`: Raised, if the object validation failed. You may 
add additional error messages, before the validation method returns.
- `OnObjectPropertyValidation`: You can perform validations before any other 
validations have been executed for the property. When the event was cancelled, 
there won't be any following validation, and the produced result will be used 
for the current property. The validation will then continue with the next 
property.
- `OnObjectPropertyValidationFailed`: Raised, if the object validation failed. 
You may add additional error messages, before the validation method continues 
with the next property. When the event was cancelled, the following property 
validations will be skipped.

If the event arguments don't offer a `PropertyInfo` in the `Property` 
property, the event was raised for the validated object.

An event handler can set a failed state by setting the `Result` property of 
the event arguments to `false`.

**WARNING**: Do not set the value `true` to the `Result` property! An already 
failed state can't be deleted, unless the original result `OriginalResult` was 
`true`.

The object validation will fail, if there was any validation result, or the 
overall result is `false`.

## Found a bug?

If the object validation doesn't work for you as expected, please open an 
issue - I'd be glad to help and make ObjectValidation become even better! Push 
requests are welcome :)

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
used automatic, if you've used the `ValidatableObject` base class for your 
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
`ItemNullableAttribute` in that case. The same is valid for deep array 
validations.

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

The `ObjectValidation.CountryValidator` packet includes references to this 
packet, and exports item validation attributes:

- `ItemCompanyTINAttribute`
- `ItemPersonTINAttribute`
- `ItemSSNAttribute`
- `ItemVATAttribute`
- `ItemZipCodeAttribute`

**NOTE**: The main ObjectValidation library includes validation for European 
VAT IDs only. By using this packet, you can use VAT ID validation for many 
countries around the world.
