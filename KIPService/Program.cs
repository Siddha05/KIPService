
using KIPService.DbContexts;
using KIPService.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System;

namespace KIPService
{
    public class Program
    {
#pragma warning disable CS1591
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "KIPService API",
                    Description = "Сервис для получения отчетов (ТЗ)",
                    Contact = new OpenApiContact() { Name = "$!r", Email = "siddha05.sb@gmail.com" }
                });
                opt.IncludeXmlComments($"{Path.Combine(AppContext.BaseDirectory, "ReportAPI.xml")}");
            });
            builder.Services.AddDbContext<AppDbContext>(opt =>
            {
                opt.UseNpgsql(builder.Configuration.GetConnectionString("DevConnection"));
                //opt.EnableSensitiveDataLogging();
            });
            builder.Services.AddHostedService<ReportCreator>();

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
