using Microsoft.EntityFrameworkCore;
using MovieManager.DTOs;
using MovieManager.Models;
using MovieManager.Persistence;

namespace MovieManager.Services
{
    public class MovieService : IMovieService
    {
        private readonly MovieDbContext _context;
        private readonly ILogger<MovieService> _logger;

        public MovieService(MovieDbContext dbContext, ILogger<MovieService> logger)
        {
            _context = dbContext;
            _logger = logger;
        }

        public async Task<MovieDto> CreateMovieAsync(CreateMovieDto command)
        {
            var movie = Movie.Create(command.Title, command.Genre, command.ReleaseDate, command.Rating);

            await _context.Movies.AddAsync(movie);
            await _context.SaveChangesAsync();

            return new MovieDto(
                movie.Id,
                movie.Title,
                movie.Genre,
                movie.ReleaseDate,
                movie.Rating
            );
        }

        public async Task<IEnumerable<MovieDto>> GetAllMoviesAsync()
        {
            return await _context.Movies
                .AsNoTracking()
                .Select(movie => new MovieDto(
                    movie.Id,
                    movie.Title,
                    movie.Genre,
                    movie.ReleaseDate,
                    movie.Rating
                )).ToListAsync();
        }

        public async Task<MovieDto> GetMovieByIdAsync(Guid id)
        {
            var movie = await _context.Movies
                                .AsNoTracking()
                                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
                return null;

            return new MovieDto(
                movie.Id,
                movie.Title,
                movie.Genre,
                movie.ReleaseDate,
                movie.Rating
            );
        }

        public async Task UpdateMovieAsync(Guid id, UpdateMovieDto command)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie is null)
                throw new ArgumentNullException($"Invalid Movie Id.");
            movie.Update(command.Title, command.Genre, command.ReleaseDate, command.Rating);
            await _context.SaveChangesAsync();

        }

        public async Task DeleteMovieAsync(Guid id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if(movie != null)
            {
                _context.Movies.Remove(movie);
                await _context.SaveChangesAsync();
            }
        }
    }
}
