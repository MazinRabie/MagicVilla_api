
using MagicVilla_WebApi.DataStore;
using MagicVilla_WebApi.Logging;
using MagicVilla_WebApi.Repository;
using MagicVilla_WebApi.Repository.IRepository;
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

            builder.Services.AddControllers().AddNewtonsoftJson(op => op.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            //builder.Services.AddControllers().AddJsonOptions(op => op.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve);

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<ILogging, Logging.Logging>();
            builder.Services.AddScoped<IVillaRepo, VillaRepo>();
            builder.Services.AddScoped<IVillaNumberRepo, VillaNumberRepo>();
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
