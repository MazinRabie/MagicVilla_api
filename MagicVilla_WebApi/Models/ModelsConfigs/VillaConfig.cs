using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MagicVilla_WebApi.Models.ModelsConfigs
{
    public class VillaConfig : IEntityTypeConfiguration<Villa>
    {
        public void Configure(EntityTypeBuilder<Villa> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
        }
    }
}
