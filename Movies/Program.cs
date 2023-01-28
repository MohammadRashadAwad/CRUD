using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Movies.Models;
using Movies.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(option => option.UseSqlServer(connectionString));
builder.Services.AddCors();
builder.Services.AddTransient<IGenreService, GenreService>();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Awad",
        Description = "My first Api",
        Contact = new OpenApiContact
        {
            Name = "Mohammad Awad",
            Url = new Uri("https://www.google.jo/?hl=ar"),
            Email="awad.520@outlook.com",

        },
        License=new OpenApiLicense
        {
            Name="My License",
            Url=new Uri("https://www.google.jo/?hl=ar"),
        },

    }); 
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(c=>c.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()); 
app.UseAuthorization();

app.MapControllers();

app.Run();
