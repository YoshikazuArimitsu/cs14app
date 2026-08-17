var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.AKSCS14App_Api>("api");

builder.Build().Run();