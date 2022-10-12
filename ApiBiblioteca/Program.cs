using ApiBiblioteca.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddDbContext<ApiLibrosContext>(options =>
{
    options.UseSqlite($"Data Source={builder.Configuration.GetConnectionString("DefaultConnection")}");
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Api Biblioteca",
        Version = "v1",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Email = "carloswesleyforte@gmail.com".ToUpper(),
            Name = "Wesley Carlos Rodriguez Forte Da Silva".ToUpper(),
            Url = new Uri(@"https://www.linkedin.com/in/wesley-carlos-rodr%C3%ADguez-forte-da-silva-297a73204/")
        }
    }) ;
});

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

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
