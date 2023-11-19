using StudentApi.Bussiness;
using StudentApi.DataAccess;

var builder = WebApplication.CreateBuilder(args);

var  MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy  =>
                      {
                          policy.WithOrigins("http://localhost:4200")
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                      });
});

builder.Services.AddLogging(logging =>
{
    logging.AddConsole();
});



builder.Services.AddSingleton<ISqlLiteProvider, SqlLiteProvider>();
builder.Services.AddSingleton<IStudentBL, StudentBL>();

var app = builder.Build();

app.UseCors(MyAllowSpecificOrigins);

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
