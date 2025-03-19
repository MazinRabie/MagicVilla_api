namespace MagicVilla_WebApi.Models
{
    public class VillaNumber
    {
        public int VillaNo { get; set; }
        public int VillaId { get; set; }
        public Villa Villa { get; set; }
        public string SpecialDetails { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime UpdatedDate { get; set; } = DateTime.Now;
    }
}
