using BO;
using System.ComponentModel.DataAnnotations;

namespace PE_SE173338_PE.DTO
{
    public class SilverJewelryDTO
    {
        public string SilverJewelryId { get; set; } = "0";

        [Required]
        [RegularExpression(@"^([A-Z][a-zA-Z0-9\s]*)$", ErrorMessage = "SilverJewelryName must start with a capital letter and contain only letters, digits, and spaces.")]

        public string SilverJewelryName { get; set; } = null!;

        [Required]
        public string? SilverJewelryDescription { get; set; }
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "MetalWeight must be a non-negative value.")]

        public decimal? MetalWeight { get; set; }
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be a non-negative value.")]

        public decimal? Price { get; set; }

        [Required]
        [Range(1900, int.MaxValue, ErrorMessage = "ProductionYear must be 1900 or later.")]

        public int? ProductionYear { get; set; }
        [Required]
        public DateTime? CreatedDate { get; set; }
        [Required]
        public string? CategoryId { get; set; }

        public virtual CategoryDTO? Category { get; set; }
    }
}
