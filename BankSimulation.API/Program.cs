using BankSimulation.API.Configuration;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddJsonFile("appsettings.Development.json");
}
else
{
    builder.Configuration.AddJsonFile("appsettings.json");
}

builder.AddLoggingConfiguration();

builder.Services.AddConfigurationSwagger();
builder.Services.AddConfigurationDbContext(builder.Configuration);
builder.Services.AddConfigurationAuthentication(builder.Configuration);
builder.Services.AddConfigurationServices();

var app = builder.Build();

app.UseConfiguredMiddlewares();

app.MapControllers();

app.Run();
