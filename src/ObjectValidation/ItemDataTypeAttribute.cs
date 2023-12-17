using System.ComponentModel.DataAnnotations;

namespace wan24.ObjectValidation
{
    /// <summary>
    /// Item data type validation attribute
    /// </summary>
    /// <remarks>
    /// Constructor
    /// </remarks>
    /// <param name="dataType">Data type</param>
    public class ItemDataTypeAttribute(DataType dataType) : ItemValidationAttribute(new DataTypeAttribute(dataType))
    {
    }
}
