using Microsoft.EntityFrameworkCore;
using MovieManager.Endpoints;

//using Npgsql.EntityFrameworkCore.PostgreSQL;
using MovieManager.Persistence;
using MovieManager.Services;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddTransient<IMovieService, MovieService>();
builder.Services.AddDbContext<MovieDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseNpgsql(connectionString);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.WithTheme(ScalarTheme.Kepler)
        .WithDarkModeToggle(true)
        .WithClientButton(true);
    });
}

//not for prod - use ef core migrations for prod
await using (var serviceScope = app.Services.CreateAsyncScope())
await using (var dbContext = serviceScope.ServiceProvider.GetRequiredService<MovieDbContext>())
{
    await dbContext.Database.EnsureCreatedAsync();
}

app.MapGet("/", () => "Hello World!").
    Produces(200, typeof(string));

//app.MapMethods
app.MapMovieEndpoints();

app.UseHttpsRedirection();

app.Run();