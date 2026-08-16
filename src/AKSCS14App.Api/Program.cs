using CS14App.Api.Compliance;
using CS14App.Api.Services;

using Microsoft.Extensions.Compliance.Classification;
using Microsoft.OpenApi;

using Serilog;
using Serilog.Extensions.Hosting;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("Starting up CS14App.Api");

    var builder = WebApplication.CreateBuilder(args);

    // ロガー設定読み込み＆初期化
    Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(builder.Configuration)
        .Enrich.FromLogContext()
        .CreateLogger();
    builder.Logging.ClearProviders();
    builder.Logging.AddSerilog(Log.Logger, dispose: true);

    builder.Services.AddSingleton(Log.Logger);
    builder.Services.AddSingleton<DiagnosticContext>();
    builder.Services.AddSingleton<IDiagnosticContext>(sp => sp.GetRequiredService<DiagnosticContext>());

    // Init swagger
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

    // .NETコンプライアンス有効化
    builder.Services.AddRedaction(options =>
        options.SetRedactor<PhoneNumberRedactor>(new DataClassificationSet(AppTaxonomy.PhoneNumber)));
    builder.Logging.EnableRedaction(options => options.ApplyDiscriminator = false);

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