using CS14App.Api.Services;

using Microsoft.OpenApi;

using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("Starting up CS14App.Api");

    var builder = WebApplication.CreateBuilder(args);

    // ロガー: appsettings.json の "Serilog" セクションで出力先(Console/File など)を変更できる。
    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext());

    builder.Services.AddControllers();

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "AKSCS14App API",
            Version = "v1",
            Description = "Arimitsu ga Kangaeta Saikyo no C#14App API",
        });
    });

    builder.Services.AddSingleton<IGreetingService, GreetingService>();

    var app = builder.Build();

    app.UseSerilogRequestLogging();

    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "AKSCS14App API v1");
    });

    // DevContainer 等でルートにアクセスした際に Swagger UI を表示する。
    app.MapGet("/", () => Results.Redirect("/swagger"))
        .ExcludeFromDescription();

    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();

    app.Run();
}
catch (Exception ex) when (ex is not HostAbortedException)
{
    Log.Fatal(ex, "AKSCS14App.Api がスタートアップ中に予期せず終了しました");
}
finally
{
    Log.CloseAndFlush();
}

public partial class Program;