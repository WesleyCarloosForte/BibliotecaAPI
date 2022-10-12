using ApiBiblioteca.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddDbContext<ApiLibrosContext>(options =>
{
    options.UseSqlite($"Data Source={builder.Configuration.GetConnectionString("DefaultConnection")}");
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey
            (Encoding.UTF8.GetBytes(builder.Configuration["keyJwt"]) ),
            ClockSkew=TimeSpan.Zero
        };
    });

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


    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header

    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
         new OpenApiSecurityScheme
         {
             Reference= new OpenApiReference
             {
                 Type=ReferenceType.SecurityScheme,
                 Id="Bearer"
             }
         },
         new string[]{}
        }
    });
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    // pick comments from classes, including controller summary comments
    c.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
    // _OR_ enable the annotations on Controller classes [SwaggerTag], if no class comments present
    c.EnableAnnotations();
});

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddIdentity<IdentityUser, IdentityRole>().
    AddEntityFrameworkStores<ApiLibrosContext>().
    AddDefaultTokenProviders();

builder.Services.AddAuthorization(opciones =>
{
    opciones.AddPolicy("admin", policy => policy.RequireClaim("admin"));
    opciones.AddPolicy("user", policy => policy.RequireClaim("user"));
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
