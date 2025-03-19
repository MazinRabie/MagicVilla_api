using System.ComponentModel.DataAnnotations;

namespace MagicVilla_WebApi.Models.DTOs
{
    public class VillaNumberCreateDTO
    {
        public int VillaNo { get; set; }
        [Required]
        public int VillaId { get; set; }
        public string SpecialDetails { get; set; }
    }
}
