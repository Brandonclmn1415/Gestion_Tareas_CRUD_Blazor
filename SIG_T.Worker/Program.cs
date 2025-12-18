using SIG_T.Worker;

var builder = Host.CreateApplicationBuilder(args);

// Register the Worker service
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
