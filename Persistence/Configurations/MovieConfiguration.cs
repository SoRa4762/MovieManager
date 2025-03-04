using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieManager.Models;

namespace MovieManager.Persistence.Configurations
{
    public class MovieConfiguration : IEntityTypeConfiguration<Movie>
    {
        public void Configure(EntityTypeBuilder<Movie> builder)
        {
            //table name
            builder.ToTable("Movies");
            //set primary key
            builder.HasKey(m => m.Id);
            //config properties
            builder.Property(m => m.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(m => m.Genre)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(m => m.ReleaseDate)
                .IsRequired();

            builder.Property(m => m.Rating)
                .IsRequired();

            // config created and lastModified prop to be handled as immutable and modifiable
            builder.Property(m => m.Created)
                .IsRequired()
                .ValueGeneratedOnAdd();

            builder.Property(m => m.LastModified)
                .IsRequired()
                .ValueGeneratedOnUpdate();

            //optional: add indexes for better performances
            builder.HasIndex(m => m.Title);
        }
    }
}
