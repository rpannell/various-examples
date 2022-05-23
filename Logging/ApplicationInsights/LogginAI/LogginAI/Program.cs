var builder = WebApplication.CreateBuilder(args);

//setup logging to application insights
builder.Host.ConfigureLogging((hostingContext, logging) =>
{
    //uses the telemetry from environment variable APPLICATIONINSIGHTS_CONNECTION_STRING
    //and logging level from appsetting file
    logging.AddApplicationInsights();
    logging.AddConsole();
});


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

/// <summary>
/// Add the application insights telemetry
/// The APPLICATIONINSIGHTS_CONNECTION_STRING configuration will contain the configuration string to the application insights
/// </summary>
builder.Services.AddApplicationInsightsTelemetry();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();