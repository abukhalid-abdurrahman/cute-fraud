using Fraud.Concerns.Configurations;
using Fraud.Infrastructure.Implementation.Repository;
using Fraud.Infrastructure.Repository;
using Fraud.Interactor.Cards;
using Fraud.Interactor.MessageBroking;
using Fraud.Interactor.Transactions;
using Fraud.Presentation.Hosts;
using Fraud.Presentation.Services.MessageHandler;
using Fraud.UseCase.Cards;
using Fraud.UseCase.MessageBroking;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Fraud.App
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<Neo4JConfigurations>(Configuration.GetSection("Neo4J"));
            services.Configure<RabbitMqConfigurations>(Configuration.GetSection("RabbitMQ"));
            
            services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<Neo4JConfigurations>>().Value);
            services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<RabbitMqConfigurations>>().Value);
            
            services.AddSingleton<IMessageBrokerService, RabbitMqMessageBrokerService>();
            services.AddScoped<AmountAnalyzer>();
            services.AddScoped<CountAnalyzer>();
            services.AddScoped<PeriodicityAnalyzer>();
            services.AddScoped<ICardAnalyzerUseCase, CardAnalyzerInteractor>();
            services.AddScoped<ICardStateManagement, CardStateManagement>();

            services.AddScoped<ICardRepository, Neo4JCardRepository>();
            services.AddScoped<ITransactionRepository, Neo4JTransactionRepository>();

            services.AddScoped<IMessageHandlerService, MessageHandlerService>();
            services.AddHostedService<RabbitMqHost>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}