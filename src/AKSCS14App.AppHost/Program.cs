var builder = DistributedApplication.CreateBuilder(args);

var sqlite = builder.AddSqlite("sqlitedb",
    databasePath: Path.Combine(builder.AppHostDirectory, "data"),
    databaseFileName: "app.db");

builder.AddProject<Projects.AKSCS14App_Api>("api")
    .WithReference(sqlite);

builder.Build().Run();