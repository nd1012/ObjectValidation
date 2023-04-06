using System.ComponentModel.DataAnnotations;

namespace wan24.ObjectValidation
{
    /// <summary>
    /// Item data type validation attribute
    /// </summary>
    public class ItemDataTypeAttribute : ItemValidationAttribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dataType">Data type</param>
        public ItemDataTypeAttribute(DataType dataType) : base(new DataTypeAttribute(dataType)) { }
    }
}
