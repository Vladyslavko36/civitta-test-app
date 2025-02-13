
using System.Reflection;
using CivittaTest.API.Middlewares;
using CivittaTest.API.Services.Implementation;
using CivittaTest.API.Services.Interfaces;
using CivittaTest.DAL.Context;
using Microsoft.EntityFrameworkCore;

namespace CivittaTest.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder
                .Configuration
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            builder.Services.AddControllers();
            builder.Services.AddAuthorization();

            builder.Services.AddSwaggerGen(c=> c.IncludeXmlComments(GetXmlCommentsFilePath()));
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DBConnection")));

            builder.Services.AddScoped<IEnricoService, EnricoService>();
            builder.Services.AddScoped<IHolidayOperationsService, HolidayOperationsService> ();

            var app = builder.Build();

            UpdateDatabase(app);

            app.UseSwagger(c =>
            {
                c.RouteTemplate = "swagger/{documentName}/swagger.json";
            });

            app.UseSwaggerUI();

            app.UseMiddleware<GlobalErrorHandler>();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }

        private static void UpdateDatabase(WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            using var context = scope.ServiceProvider.GetService<AppDbContext>();

            context!.Database.Migrate();
        }

        private static string GetXmlCommentsFilePath()
        {
            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            return Path.Combine(AppContext.BaseDirectory, xmlFilename);
        }
    }
}
