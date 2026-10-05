using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using WebProject.Discount.Context;
using WebProject.Discount.Services;

// Npgsql 6, DateTime'ı varsayılan olarak 'timestamp with time zone'a eşliyor ve
// Kind=Utc olmayan değerleri reddediyor. Tarihler formdan Kind=Unspecified
// geldiği için eski (SQL Server ile aynı) davranışı koruyoruz.
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.Authority = builder.Configuration["IdentityServerUrl"];
    options.Audience = "ResourceDiscount";
    options.RequireHttpsMetadata = false;
});

// Add services to the container.
builder.Services.AddTransient<DapperContext>();
builder.Services.AddTransient<IDiscountService, DiscountService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DapperContext>();
    db.Database.Migrate();

    // İlk açılışta denemek için örnek kuponlar (tablo boşsa)
    if (!db.Coupons.Any())
    {
        var today = DateTime.UtcNow.Date;
        db.Coupons.AddRange(
            new WebProject.Discount.Entities.Coupon { Code = "HOSGELDIN10", Rate = 10, IsActive = true, ValidDate = today.AddYears(1) },
            new WebProject.Discount.Entities.Coupon { Code = "YAZ20", Rate = 20, IsActive = true, ValidDate = today.AddMonths(6) },
            new WebProject.Discount.Entities.Coupon { Code = "SUPER30", Rate = 30, IsActive = true, ValidDate = today.AddMonths(1) },
            new WebProject.Discount.Entities.Coupon { Code = "ESKI50", Rate = 50, IsActive = false, ValidDate = today.AddYears(-1) });
        db.SaveChanges();
    }
}

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
