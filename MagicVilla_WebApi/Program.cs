
using MagicVilla_WebApi.DataStore;
using MagicVilla_WebApi.Logging;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            //Log.Logger = new LoggerConfiguration().MinimumLevel.Debug().WriteTo.File("log/villaLogger.txt", rollingInterval: RollingInterval.Day).CreateLogger();
            //builder.Host.UseSerilog();

            builder.Services.AddControllers().AddNewtonsoftJson();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<ILogging, Logging.Logging>();
            builder.Services.AddAutoMapper(typeof(MappingConfig));
            builder.Services.AddDbContext<ApplicationDbContext>(op =>
            {
                op.UseSqlServer(builder.Configuration.GetConnectionString("sql"));
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
