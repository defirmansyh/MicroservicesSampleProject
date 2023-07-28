using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TiVi.AuthService.DataAccess;
using TiVi.AuthService.Services;
using TiVi.AuthService.Services.Interfaces;
using TiVi.AuthService.Utilities;
using TiVi.AuthService.Utilities.GlobalErrorHandling;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Net;
using TiVi.AuthService.Models.Base;
using TiVi.AuthService.Models;
using Microsoft.OpenApi.Models;

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

    #region Bind Token Management Config
    services.AddOptions<TokenManagement>().BindConfiguration("tokenManagement");
    #endregion

    #region Register Action Filter
    services.Configure<ApiBehaviorOptions>(opt =>
    {
        opt.SuppressModelStateInvalidFilter = true;
    });
    services.AddMvc(ops => ops.Filters.Add<ActionFilterException>());
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

    bool CustomLifetimeValidator(DateTime? notBefore, DateTime? expires, SecurityToken tokenToValidate, TokenValidationParameters @param)
    {
        if (expires != null)
        {
            return expires > DateTime.UtcNow;
        }
        return false;
    }
    #endregion

    #region Other way to Register Authentication Scheme
    //builder.Services.AddAuthentication(options =>
    //{
    //    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    //    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    //    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    //})
    //.AddJwtBearer(options =>
    //{
    //    options.SaveToken = true;
    //    options.RequireHttpsMetadata = false;
    //    options.TokenValidationParameters = new TokenValidationParameters()
    //    {
    //        ValidateIssuer = true,
    //        ValidateAudience = true,
    //        ValidAudience = DataHelper.GetAppsettingValue("tokenManagement:audience"),
    //        ValidIssuer = DataHelper.GetAppsettingValue("tokenManagement:issuer"),
    //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(DataHelper.GetAppsettingValue("tokenManagement:secret")))
    //    };
    //    //options.TokenValidationParameters = new TokenValidationParameters
    //    //{
    //    //    ValidateIssuerSigningKey = true,
    //    //    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(DataHelper.GetAppsettingValue("tokenManagement:secret"))),
    //    //    ValidateIssuer = false,
    //    //    ValidateAudience = false
    //    //};

    //});
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
    services.AddScoped<IUserAccountService,UserAccountService>();
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
