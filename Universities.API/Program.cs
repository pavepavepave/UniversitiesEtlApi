using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Universities.DB.DbContexts;
using Universities.DB.Repository;
using Universities.ETL;

namespace Universities.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDbContext<UniversitiesDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
        
        builder.Services.AddHttpClient();
        builder.Services.AddTransient<IUniversityRepository, UniversityRepository>();
        builder.Services.AddTransient<UniversitiesDataHandler>();
        
        builder.Services.AddControllers();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "UniversitiesAPI", Version = "v1" });
        });
        
        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.MapControllers();

        app.Run();
    }
}