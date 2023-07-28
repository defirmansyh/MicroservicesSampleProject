using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using TiVi.UserCatalogService.Utilities;
using TiVi.UserCatalogService.DataAccess;
using TiVi.UserCatalogService.Models;
using TiVi.UserCatalogService.Services;
using TiVi.UserCatalogService.Services.Interfaces;
using TiVi.UserCatalogService.Utilities;
using TiVi.UserCatalogService.Utilities.GlobalErrorHandling;

var builder = WebApplication.CreateBuilder(args);

ConfigureServices(builder.Services);

// Add services to the container.
builder.Services.AddHttpContextAccessor();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

var app = builder.Build();
app.ConfigureExceptionHandlerWithTraceId();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

void ConfigureServices(IServiceCollection services)
{
    #region Open Connection Database
    services.AddDbContext<TiViContext>(options =>
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();
        //var encryptedConn = EncryptionHelper.Decrypt(configuration.GetConnectionString("LocalConnection"));
        options.UseSqlServer(configuration.GetConnectionString("LocalConnection"));
    });
    #endregion

    #region Authentication Scheme
    services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("tokenManagement:secret").Value)),
            ValidateIssuer = true,
            ValidateAudience = true,
            ClockSkew = TimeSpan.Zero,
            ValidIssuer = builder.Configuration.GetSection("tokenManagement:issuer").Value,
            ValidAudience = builder.Configuration.GetSection("tokenManagement:audience").Value,
            RequireExpirationTime = true,
            ValidateLifetime = true
        };
    });
    #endregion

    #region Register AutoMapper
    var mappingProfile = new MapperConfiguration(mc =>
    {
        mc.AddProfile(new AutoMapperProfile());
    });

    IMapper mapper = mappingProfile.CreateMapper();
    services.AddSingleton(mapper);
    #endregion

    #region Register DI
    services.AddScoped<ICatalogMovieService, CatalogMovieService>();
    #endregion

    #region Add Auth Text Box Swagger
    services.AddSwaggerGen(options => {
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter token",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "bearer"
        });
        options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
    });
    #endregion
}
