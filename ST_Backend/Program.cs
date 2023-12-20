


using ST_Backend.Al;
using ST_Backend.Controllers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// MusicContext servisi tanımlama
builder.Services.AddSingleton<MusicContext>(provider =>
{
    var connectionString = "mongodb+srv://talhayce:82.Tlhyc.90@webcluster.9t7bghk.mongodb.net/";
    var databaseName = "ST";
    return MusicContext.GetInstance(connectionString, databaseName);
});
// Configuration dosyasını yükle
builder.Configuration.AddJsonFile("appsettings.json");
// Yapay_Zeka servisi tanımlama
builder.Services.AddSingleton<Yapay_Zeka>();
// Filtreleme servisi tanımlama
builder.Services.AddSingleton<Filtreleme>();


// MusicController servisi tanımlama
builder.Services.AddTransient<MusicController>();

var app = builder.Build();

// Swagger yapılandırmaları
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure the HTTP request pipeline.
app.UseAuthorization();
app.MapControllers();
app.Run();
