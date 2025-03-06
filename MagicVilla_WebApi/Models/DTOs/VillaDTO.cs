using System.ComponentModel.DataAnnotations;

namespace MagicVilla_WebApi.Models.DTOs
{
    public class VillaDTO
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(25)]
        public string? Name { get; set; }
        public int sqft { get; set; }
        public int Occupancy { get; set; }
    }
}
