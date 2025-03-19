using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MagicVilla_WebApi.Models.ModelsConfigs
{

    public class VillaNumberConfig : IEntityTypeConfiguration<VillaNumber>
    {
        public void Configure(EntityTypeBuilder<VillaNumber> builder)
        {
            builder.HasKey(x => x.VillaNo);
            builder.Property(x => x.VillaNo).ValueGeneratedNever();
            builder.HasOne(x => x.Villa).WithOne(x => x.VillaNumber).HasForeignKey<VillaNumber>(x => x.VillaId);
        }
    }
}

