using Microsoft.EntityFrameworkCore;
using Npgsql;
using StorageRoom;
using System;

var builder = WebApplication.CreateBuilder(args);

// ��������� �������� ���� ������ � ���������� PostgreSQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    
    try
    {
        // ���������, �������� �� ������������ � ���� ������
        dbContext.Database.CanConnect();
        Console.WriteLine("�������� ����������� � ���� ������.");
        var initializer = new DatabaseInitializer(dbContext);
        await initializer.InitializeAsync();

    }
    catch (Exception ex)
    {
        Console.WriteLine($"������ ����������� � ���� ������: {ex.Message}");
        throw new Exception("�� ������� ������������ � ���� ������. ��������� ��������� �����������.");
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
