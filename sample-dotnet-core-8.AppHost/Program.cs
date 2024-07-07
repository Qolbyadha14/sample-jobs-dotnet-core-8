var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.hangfire_jobs>("hangfire");

builder.Build().Run();
