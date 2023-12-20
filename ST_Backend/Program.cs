


using ST_Backend.Al;
using ST_Backend.Controllers;

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
// Configuration dosyas�n� y�kle
builder.Configuration.AddJsonFile("appsettings.json");
// Yapay_Zeka servisi tan�mlama
builder.Services.AddSingleton<Yapay_Zeka>();
// Filtreleme servisi tan�mlama
builder.Services.AddSingleton<Filtreleme>();


// MusicController servisi tan�mlama
builder.Services.AddTransient<MusicController>();

var app = builder.Build();

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
