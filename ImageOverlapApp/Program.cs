using Serilog;
using ImageOverlapApp.Services;

try
{
	var builder = WebApplication.CreateBuilder(args);

	Log.Logger = new LoggerConfiguration()
		.ReadFrom.Configuration(builder.Configuration)
		.CreateLogger();

	builder.Host.UseSerilog();

	builder.Services.AddControllers();
	builder.Services.AddEndpointsApiExplorer();
	builder.Services.AddSwaggerGen();
	builder.Services.AddScoped<IUploadService, UploadService>();
	builder.Services.AddScoped<IImageComparisonService, ImageComparisonService>();

	var app = builder.Build();

	if (app.Environment.IsDevelopment())
	{
		app.UseSwagger();
		app.UseSwaggerUI();
	}

	app.UseHttpsRedirection();
	app.UseStaticFiles();
	app.UseAuthorization();
	app.MapControllers();

	Log.Information("🚀 ImageOverlapApp iniciado com sucesso.");
	app.Run();
}
catch (Exception ex)
{
	Log.Fatal(ex, "❌ Falha ao iniciar o ImageOverlapApp.");
	throw;
}
finally
{
	Log.CloseAndFlush();
}