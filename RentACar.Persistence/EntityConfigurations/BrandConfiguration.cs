using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentACar.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.Persistence.EntityConfigurations;

public class BrandConfiguration : IEntityTypeConfiguration<Brand>
{
    public void Configure(EntityTypeBuilder<Brand> builder)
    {
        builder.ToTable("Brands").HasKey(k => k.Id);
        builder.Property(p => p.Id).HasColumnName("Id");
        builder.Property(p => p.Name).HasColumnName("Name");
        builder.HasIndex(indexExpression: p => p.Name, name: "UK_Brands_Name").IsUnique();

        builder.HasQueryFilter(b => !b.DeletedDate.HasValue);

        //Brand[] brandSeeds = { new(id: Guid.NewGuid(), name: "BMW"), new(id: Guid.NewGuid(), name: "Mercedes") };
        //builder.HasData(brandSeeds);
    }
}
