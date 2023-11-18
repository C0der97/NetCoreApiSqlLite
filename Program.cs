var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("*")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddLogging(logging =>
{
    logging.AddConsole();
});



builder.Services.AddSingleton<ISqlLiteProvider, SqlLiteProvider>();

var app = builder.Build();

app.UseCors();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseHttpsRedirection();

var logger = app.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("Application started");

app.UseAuthorization();

app.MapControllers();

app.Run();
