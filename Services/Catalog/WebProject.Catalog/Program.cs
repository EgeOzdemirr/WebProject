using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Reflection;
using WebProject.Catalog.Services;
using WebProject.Catalog.Services.AboutServices;
using WebProject.Catalog.Services.BrandServices;
using WebProject.Catalog.Services.CategoryServices;
using WebProject.Catalog.Services.ContactServices;
using WebProject.Catalog.Services.FeatureServices;
using WebProject.Catalog.Services.FeatureSliderServices;
using WebProject.Catalog.Services.OfferDiscountServices;
using WebProject.Catalog.Services.ProductDetailServices;
using WebProject.Catalog.Services.ProductImageServices;
using WebProject.Catalog.Services.ProductRecommendationServices;
using WebProject.Catalog.Services.ProductServices;
using WebProject.Catalog.Services.SpecialOfferServices;
using WebProject.Catalog.Services.StatisticServices;
using WebProject.Catalog.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.Authority = builder.Configuration["IdentityServerUrl"];
    options.Audience = "ResourceCatalog";
    options.RequireHttpsMetadata = false;
});

// Add services to the container.

// MongoDB yapýlandýrmasý
builder.Services.AddSingleton<IMongoDatabase>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetConnectionString("MongoDB");
    var client = new MongoClient(connectionString);
    return client.GetDatabase("WebProjectCatalogDb");
});

// Recommendation servisi kaydý
builder.Services.AddScoped<IProductRecommendationService, ProductRecommendationService>();

builder.Services.AddScoped<IAboutService, AboutService>();
builder.Services.AddScoped<IBrandService, BrandService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IContactService, ContactService>();
builder.Services.AddScoped<IFeatureService, FeatureService>();
builder.Services.AddScoped<IFeatureSliderService, FeatureSliderService>();
builder.Services.AddScoped<IOfferDiscountService, OfferDiscountService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductDetailService, ProductDetailService>();
builder.Services.AddScoped<IProductImageService, ProductImageService>();
builder.Services.AddScoped<ISpecialOfferService, SpecialOfferService>();
builder.Services.AddScoped<IStatisticService, StatisticService>();


builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("DatabaseSettings"));
builder.Services.AddScoped<IDatabaseSettings>(sp =>
{
    return sp.GetRequiredService<IOptions<DatabaseSettings>>().Value;
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
