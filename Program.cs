using System.Data.Common;
using Microsoft.OpenApi.Models;
using MySqlConnector;
using Npgsql;

namespace MovieMinimalAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c => 
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Movie API", Description = "Streaming de films", Version = "v1"});
            });

            

            var app = builder.Build();
            app.UseSwagger();
            app.UseSwaggerUI(c => 
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Movie API V1");
            });
            // Configure the HTTP request pipeline.
       /*    if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

         */   app.UseHttpsRedirection();

            app.MapGet("/movies/{1}", async () =>
            {

            });
            app.MapGet("/movies", GetAllMovies);

            app.Run();
        }

        private static IEnumerable<Movie> GetAllMovies()
        {
            var movies = new List<Movie>();

            var connectionString = "Server=localhost;Database=streaming;Uid=alexiscolp;Pwd=enwnzk24b;";
            using var connection = new MySqlConnection(connectionString);
            connection.Open();

            var query = "SELECT * FROM movie";

            using var cmd = new MySqlCommand(query, connection);
            using var reader = cmd.ExecuteReader();
            {
                while (reader.Read())
                {
                    var movieToAdd = new Movie
                    {
                        Id = Convert.ToInt32(reader["id_movie"]),
                        Title = reader["title"].ToString(),
                        ReleaseYear = Convert.ToInt32(reader["release_year"]),
                        CreateDate = Convert.ToDateTime(reader["created_date"])
                    };

                    movies.Add(movieToAdd);
                }
            }

            return movies;
        }
    }
}