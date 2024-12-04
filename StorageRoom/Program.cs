using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql;
using StorageRoom;
using StorageRoom.Service;
using StorageRoom.Service.serv;
using System;
using RabbitMQ.Client;
using System.Text;
using EasyNetQ;





var builder = WebApplication.CreateBuilder(args);
builder.Services.AddResponseCompression( options =>
{
    options.EnableForHttps = true;
});

// Connection PostgreSQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));





// Connection RbitMQ
var bus = RabbitHutch.CreateBus(builder.Configuration.GetConnectionString("AutoRabbitMQ"),
    register => register.EnableNewtonsoftJson());
builder.Services.AddSingleton<IConnection>(sp =>
{
    var connectionFactory = new ConnectionFactory()
    {
        Uri = new Uri(builder.Configuration.GetConnectionString("AutoRabbitMQ"))
    };
    return connectionFactory.CreateConnection();
});

builder.Services.AddScoped<RabbitMqChannelFactory>();

builder.Services.AddSingleton<IBus>(bus);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.EnableAnnotations();
    options.DocInclusionPredicate((docName, apiDesc) => true);
});


// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    });

builder.Services.AddScoped<IPassengerService, PassengerService>();
builder.Services.AddScoped<IBaggageService, BaggageService>();
builder.Services.AddScoped<IFlightService, FlightService>();


var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    try
    {
        // test connect to our database
        dbContext.Database.CanConnect();
        Console.WriteLine("Connection is Ok");
        var initializer = new DatabaseInitializer(dbContext);
        await initializer.InitializeAsync();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
        throw new Exception("Connection error");
    }
}
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseResponseCompression();
app.UseRouting();


app.UseAuthorization();
app.MapControllers();
//app.mapcontrollerroute(
//    name: "default",
//    pattern: "{controller=home}/{action=index}/{id?}");

app.Run();
