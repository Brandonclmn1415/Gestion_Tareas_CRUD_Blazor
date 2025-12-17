using SIG_T.Worker;
using SIG_T.API.Data;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateApplicationBuilder(args);

// Register DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register shared application services from API (TaskService, ActividadService, ...)
builder.Services.AddApplicationServices(builder.Configuration);

// Register the Worker service
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
