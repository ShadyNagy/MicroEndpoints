using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MicroEndpoints.EndpointApp.DomainModel;

namespace MicroEndpoints.EndpointApp.DataAccess.Config;

public class AuthorConfig : IEntityTypeConfiguration<Author>
{
  public void Configure(EntityTypeBuilder<Author> builder)
  {
    builder.Property(e => e.Name)
        .IsRequired();

    builder.Property(e => e.PluralsightUrl)
        .IsRequired();

    builder.HasData(SeedData.Authors());
  }
}
