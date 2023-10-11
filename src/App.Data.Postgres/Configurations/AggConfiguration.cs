using App.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Data.Postgres.Configurations;

public class AggConfiguration: IEntityTypeConfiguration<Agg>
{
    public void Configure(EntityTypeBuilder<Agg> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(p => p.Id).ValueGeneratedOnAdd();
        builder.Property(c => c.Text)
            .HasColumnType("text");
    }
}