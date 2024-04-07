
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using InventoryService.API.IoCContainer;
using InventoryService.API.Serilog;
using Microsoft.OpenApi.Models;
using SpanJson.AspNetCore.Formatter;
using Serilog;

namespace InventoryService.API
{
    public class Program
    {
        private static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            ConfigureWebHost(builder);
            ConfigureServices(builder.Services, builder.Environment);
            var app = ConfigureWebApp(builder);
            await app.RunAsync();
        }

        private static void ConfigureWebHost(WebApplicationBuilder webApplicationBuilder)
        {
            webApplicationBuilder.Host
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureContainer<ContainerBuilder>((context, container) =>
                    container.BuildContext(context.Configuration)
                )
                .UseSerilog((_, provider, loggerConfiguration) => BuildLogger(provider, loggerConfiguration));
        }

        private static WebApplication ConfigureWebApp(WebApplicationBuilder builder)
        {
            var app = builder.Build();
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.UseRouting();
            app.UseCors();
            if (builder.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "backend v1"));
            }
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/ReportGeneratorMOAutomation/health");
            });
            return app;
        }

        private static void ConfigureServices(IServiceCollection services, IWebHostEnvironment webHostEnvironment)
        {
            services.AddHttpContextAccessor();
            services.AddControllers().AddSpanJson();
            services.AddHealthChecks();
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.AllowAnyOrigin();
                        builder.AllowAnyHeader();
                        builder.AllowAnyMethod();
                    });
            });
            services.AddLogging();
            if (webHostEnvironment.IsDevelopment())
            {
                services.AddSwaggerGen(options =>
                {
                    options.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Title = "API endpoints (ONLY FOR DEVELOPMENT)"
                    });
                });
            }
        }

        private static void BuildLogger(IServiceProvider provider, LoggerConfiguration loggerConfiguration)
        {
            provider.GetRequiredService<LogCreator>();
            ChangeToken.OnChange(() =>
            {
                var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(60));
                var cancellationChangeToken = new CancellationChangeToken(cancellationTokenSource.Token);
                return cancellationChangeToken;
            },
                LogCreator.UpdateLogLevel);

            LogCreator.ConfigureLogging(loggerConfiguration);
        }
    }
}
