namespace MunicipalityTaxes.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Newtonsoft.Json;
    using ValidationAttributes;

    public class MunicipalityTax //: IValidatableObject
    {
        [Key]
        [JsonIgnore]
        public int Id { get; set; }

        /// <example>
        /// Vilnius
        /// </example>
        [Required(ErrorMessage = "Required {0}", AllowEmptyStrings = false)]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Municipality { get; set; }

        /// <example>
        /// 2019-01-01
        /// </example>
        [Required]
        public DateTime DateFrom { get; set; }

        /// <example>
        /// 2019-01-02
        /// </example>
        [Required]
        [DatesAreValid]
        public DateTime DateTo { get; set; }

        /// <summary>
        /// Values:
        /// VeryHigh = 5
        /// High = 4
        /// Medium = 3
        /// Low = 2
        /// VeryLow = 1
        /// </summary>
        /// <example>
        /// 5
        /// </example>
        [Required]
        [Priority]
        public PriorityEnum Priority { get; set; }

        /// <example>
        /// 2.2
        /// </example>
        [Required]
        public decimal Tax { get; set; }
    }
}
