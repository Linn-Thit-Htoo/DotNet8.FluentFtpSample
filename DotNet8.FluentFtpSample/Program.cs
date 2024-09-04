using DotNet8.FluentFtpSample.Services;
using Serilog;
using Serilog.Formatting.Compact;
using Serilog.Templates;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.File(
        new CompactJsonFormatter(),
        "logs/logFile.txt",
        rollingInterval: RollingInterval.Hour
    )
    .WriteTo.Console(new ExpressionTemplate("[{@t:HH:mm:ss} {@l:u3} {SourceContext}] {@m}\n{@x}"))
    .CreateLogger();

builder.Host.UseSerilog();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<FtpService>();

DotNetEnv.Env.Load(".env");

var app = builder.Build();

//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
