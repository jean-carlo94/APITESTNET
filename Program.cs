using APITEST.Common.Interfaces;
using APITEST.Data;
using APITEST.Models;
using APITEST.Modules.Auth.Automappers;
using APITEST.Modules.Auth.DTOs;
using APITEST.Modules.Auth.Interfaces;
using APITEST.Modules.Auth.Services;
using APITEST.Modules.Auth.Validators;
using APITEST.Modules.ProductCategories.Validators;
using APITEST.Modules.Products.Automappers;
using APITEST.Modules.Products.DTOs;
using APITEST.Modules.Products.Repository;
using APITEST.Modules.Products.Services;
using APITEST.Modules.Products.Validators;
using APITEST.Modules.ProductsCategory.Automappers;
using APITEST.Modules.ProductsCategory.DTOs;
using APITEST.Modules.ProductsCategory.Repository;
using APITEST.Modules.ProductsCategory.Services;
using APITEST.Modules.Users.Automappers;
using APITEST.Modules.Users.DTOs;
using APITEST.Modules.Users.Repository;
using APITEST.Modules.Users.Services;
using APITEST.Modules.Users.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("ApiCorsPolicy",
        builder => builder
            .WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .WithExposedHeaders("Content-Disposition")
            .AllowAnyMethod()
            .AllowCredentials()
    );
});

// Database Conection
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

//AuthJWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"])),
            ClockSkew = TimeSpan.Zero
        };
    });

// Add services to the container.
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();

//Validators
builder.Services.AddScoped<IValidator<UserInsertDto>, UserInsertValidator>();
builder.Services.AddScoped<IValidator<UserUpdateDto>, UserUpdateValidator>();
builder.Services.AddScoped<IValidator<AuthRegisterDto>, AuthRegisterValidator>();
builder.Services.AddScoped<IValidator<AuthLoginDto>, AuthLoginValidator>();
builder.Services.AddScoped<IValidator<ProductCategoryInsertDto>, ProductCategoryInsertValidator>();
builder.Services.AddScoped<IValidator<ProductCategoryUpdateDto>, ProductCategoryUpdateValidator>();
builder.Services.AddScoped<IValidator<ProductInsertDto>, ProductInsertValidator>();
builder.Services.AddScoped<IValidator<ProductUpdateDto>, ProductUpdateValidator>();

//Mappers
builder.Services.AddAutoMapper(typeof(UserMappingProfile));
builder.Services.AddAutoMapper(typeof(AuthMappingProfile));
builder.Services.AddAutoMapper(typeof(ProductCategoriesMappingProfile));
builder.Services.AddAutoMapper(typeof(ProductMappingProfile));

//Services
builder.Services.AddKeyedScoped<ICommonService<UserDto, UserInsertDto, UserUpdateDto>, UserService>("userService");
builder.Services.AddKeyedScoped<ICommonService<ProductCategoryDto, ProductCategoryInsertDto, ProductCategoryUpdateDto>, ProductCategoryService>("productCategoryService");
builder.Services.AddKeyedScoped<ICommonService<ProductDto, ProductInsertDto, ProductUpdateDto>, ProductService>("productService");

//Repository
builder.Services.AddScoped<IRepository<User>, UserRepository>();
builder.Services.AddScoped<IRepository<ProductCategory>, ProductCategoryRepository>();
builder.Services.AddScoped<IRepository<Product>, ProductRepository>();

//Controller
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(config =>
{
    config.SwaggerDoc("v1", new() { Title = "CCL TEST API", Version = "v1" });

    // Define the OAuth2.0 scheme that's in use (i.e., Implicit Flow)
    config.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    config.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
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

app.UseCors("ApiCorsPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();
