using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ST_Backend.Al;
using ST_Backend.Controllers;

namespace ST_Backend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // MusicContext servisi tan�mlama
            builder.Services.AddSingleton<MusicContext>(provider =>
            {
                var connectionString = "mongodb+srv://talhayce:82.Tlhyc.90@webcluster.9t7bghk.mongodb.net/";
                var databaseName = "ST";
                return MusicContext.GetInstance(connectionString, databaseName);
            });

            // Yapay_Zeka servisi tan�mlama
            builder.Services.AddSingleton<Yapay_Zeka>();

            // Filtreleme servisi tan�mlama
            builder.Services.AddSingleton<Filtreleme>();

            // MusicController servisi tan�mlama
            builder.Services.AddTransient<MusicController>();

            var app = builder.Build();

            // CORS ayarlar�
            app.UseCors(options => options
                .AllowAnyOrigin()          // T�m kaynaklardan eri�ime izin ver
                .AllowAnyMethod()          // T�m HTTP metodlar�na izin ver (GET, POST, PUT, DELETE, vb.)
                .AllowAnyHeader());        // T�m HTTP ba�l�klar�na izin ver

            // Swagger yap�land�rmalar�
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Configure the HTTP request pipeline.
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
