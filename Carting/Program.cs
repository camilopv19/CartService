using BLL;
using DAL.Data;
using DAL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using System.Diagnostics;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API v1", Version = "v1" });
    c.SwaggerDoc("v2", new OpenApiInfo { Title = "API v2", Version = "v2" });
    // Set the comments path for the Swagger JSON and UI.
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

//API versioning
builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
});
// Add versioned controllers
builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});
// Add services to the container.
builder.Services.AddControllers();

// Bind LiteDbOptions from appsettings.json
builder.Services.Configure<LiteDbOptions>(builder.Configuration.GetSection("LiteDbOptions"));
builder.Services.AddSingleton<ILiteDbContext, LiteDbContext>();

// Bind Cart Repo and Svc
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddTransient<ICartService, ICartservice>();

var app = builder.Build();

// Open a console window
await OpenConsoleWindow(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
        options.SwaggerEndpoint("/swagger/v2/swagger.json", "API v2");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

// Method to open a console window
async Task OpenConsoleWindow(WebApplication appInstance)
{
    ProcessStartInfo psi = new ProcessStartInfo
    {
        FileName = "cmd.exe",
        RedirectStandardInput = true,
        RedirectStandardOutput = true,
        CreateNoWindow = false,
        UseShellExecute = false
    };

    Process process = new Process { StartInfo = psi };
    process.Start();

    // Resolve dependencies and instantiate services
    using (var scope = appInstance.Services.CreateScope())
    {
        var serviceProvider = scope.ServiceProvider;

        // Instantiate Cartservice
        var cartService = serviceProvider.GetRequiredService<ICartService>();

        // Instantiate MessageService and pass Cartservice
        var messageService = new MessageService(cartService);
        //var messageService = new MessageService();

        // Start receiving messages
        var result = messageService.Receive();
        //messageService.Receive2();

        // Wait for the first message to be received
        await messageService.WaitForMessage();

        Console.WriteLine(" Press [enter] to exit.");
        Console.ReadLine();
    }
}