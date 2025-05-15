using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configurar Serilog antes de qualquer outra coisa
Log.Logger = new LoggerConfiguration()
	.ReadFrom.Configuration(builder.Configuration)
	.CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IImageComparisonService, ImageComparisonService>();

var app = builder.Build();

app.UseStaticFiles();
app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

try
{
	Log.Information("Iniciando o aplicativo...");
	app.Run();
}
catch (Exception ex)
{
	Log.Fatal(ex, "Erro fatal na inicialização");
}
finally
{
	Log.CloseAndFlush();
}