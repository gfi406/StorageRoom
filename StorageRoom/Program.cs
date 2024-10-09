using Microsoft.EntityFrameworkCore;
using Npgsql;
using StorageRoom;
using System;

var builder = WebApplication.CreateBuilder(args);

// Добавляем контекст базы данных с поддержкой PostgreSQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IPassengerService, PassengerService>();
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    });
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    try
    {
        // Проверяем, возможно ли подключиться к базе данных
        dbContext.Database.CanConnect();
        Console.WriteLine("Успешное подключение к базе данных.");
        var initializer = new DatabaseInitializer(dbContext);
        await initializer.InitializeAsync();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Ошибка подключения к базе данных: {ex.Message}");
        throw new Exception("Не удалось подключиться к базе данных. Проверьте настройки подключения.");
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();


app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
