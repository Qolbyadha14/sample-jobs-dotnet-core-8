var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.hangfire>("hangfire");

builder.Build().Run();
