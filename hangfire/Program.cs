
using Hangfire;
using Hangfire.PostgreSql;
using hangfire_jobs.DatabaseContext;
using hangfire_jobs.Services;
using HangfireBasicAuthenticationFilter;
using Microsoft.EntityFrameworkCore;

namespace hangfire;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.AddServiceDefaults();

        // Add services to the container.
        builder.Services.AddControllers();
        builder.Services.AddTransient<SampleJobService>();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        IServiceCollection serviceCollection = builder.Services.AddEntityFrameworkNpgsql().AddDbContext<DefaultDbContext>(options => {
            options.UseNpgsql(builder.Configuration.GetConnectionString("defaultConnection"));
        });

        // Add hangfire setup
        builder.Services.AddHangfire(config => config.UsePostgreSqlStorage(c => c.UseNpgsqlConnection(builder.Configuration.GetConnectionString("defaultConnection"))));
        builder.Services.AddHangfireServer();

        var app = builder.Build();

        app.MapDefaultEndpoints();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        // Use authentication and authorization middleware
        app.UseAuthentication();
        app.UseAuthorization();


        // Configure Hangfire dashboard with authentication
        app.UseHangfireDashboard("/dashboard", new DashboardOptions()
        {
            DashboardTitle = "Service Jobs",
            Authorization = new[]{
                new HangfireCustomBasicAuthenticationFilter{
                    User = builder.Configuration.GetSection("HangfireCredentials:UserName").Value,
                    Pass = builder.Configuration.GetSection("HangfireCredentials:Password").Value
                }
            }
        });

        // Jadwalkan job
        RecurringJob.AddOrUpdate<SampleJobService>(job => job.ProcessCronJob(), Cron.Daily);
        BackgroundJob.Enqueue<SampleJobService>(job => job.ProcessBackgroundJob());

        app.MapControllers();

        app.Run();
    }
}
