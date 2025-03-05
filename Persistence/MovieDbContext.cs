//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MovieManager.Models;

namespace MovieManager.Persistence
{
    public class MovieDbContext(DbContextOptions<MovieDbContext> options) : DbContext(options)
    {
        //no need
        //public MovieDbContext(DbContextOptions<MovieDbContext> options) : base(options)
        //{
        //}
        public DbSet<Movie> Movies => Set<Movie>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //creates a new schema called app, and sets it as the default schema for all entities instead of public schema
            modelBuilder.HasDefaultSchema("app");
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MovieDbContext).Assembly);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseAsyncSeeding(async (context, _, cancellationToken) =>
                {
                    var sampleMovie = await context.Set<Movie>().FirstOrDefaultAsync(b => b.Title == "Sonic the Hedgehog 3");
                    if (sampleMovie == null)
                    {
                        sampleMovie = Movie.Create("Sonic the Hedgehog 3", "Fantasy", new DateTimeOffset(new DateTime(2025, 1, 3), TimeSpan.Zero), 7);
                        await context.Set<Movie>().AddAsync(sampleMovie);
                        await context.SaveChangesAsync();
                    }
                })
                .UseSeeding(async (context, _) =>
                {
                    var sampleMovie = context.Set<Movie>().FirstOrDefault(b => b.Title == "Sonic the Hedgehog 3");
                    if (sampleMovie == null)
                    {
                        sampleMovie = Movie.Create("Sonic the Hedgehog 3", "Fantasy", new DateTimeOffset(new DateTime(2025, 1, 3), TimeSpan.Zero), 7);
                        await context.Set<Movie>().AddAsync(sampleMovie);
                        await context.SaveChangesAsync();
                    }
                });
        }

    }
}
