// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio EchoBot v4.18.1

using DietBot.ComputerVision;
using DietBot.CosmosDB;
using DietBot.Dialogs;
using DietBot.Diets.Repository;
using DietBot.Diets.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Azure;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Bot.Connector.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DietBot
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient().AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.MaxDepth = HttpHelper.BotMessageSerializerSettings.MaxDepth;
            });

            // Create the Bot Framework Authentication to be used with the Bot Adapter.
            services.AddSingleton<BotFrameworkAuthentication, ConfigurationBotFrameworkAuthentication>();

            // Create the Bot Adapter with error handling enabled.
            services.AddSingleton<IBotFrameworkHttpAdapter, AdapterWithErrorHandler>();

            // Create the bot as a transient. In this case the ASP Controller is expecting an IBot.
            services.AddTransient<IBot, Bots.DietBot>();

            // Create the User state. (Used in this bot's Dialog implementation.)
            services.AddSingleton<UserState>();

            // Create the Conversation state. (Used by the Dialog system itself.)
            services.AddSingleton<ConversationState>();

            services.AddSingleton<DietDialog>();

            services.AddSingleton<IComputerVisionService, ComputerVisionService>();

            services.Configure<ComputerVisionOptions>(
                Configuration.GetSection(ComputerVisionOptions.Section));

            services.Configure<CosmosDbOptions>(
                Configuration.GetSection(CosmosDbOptions.Section));

            var cosmosOptions = Configuration.GetSection(CosmosDbOptions.Section).Get<CosmosDbOptions>();
            var cosmosClient = new CosmosClientBuilder(cosmosOptions.Endpoint, cosmosOptions.AuthKey)
                .WithSerializerOptions(new CosmosSerializationOptions()
                {
                    PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
                })
                .Build();

            services.AddSingleton(cosmosClient);

            services.AddSingleton<IDietRepository, DietCosmosRepository>();

            services.AddSingleton<IDietService, DietService>();

            services.AddSingleton<IStorage, MemoryStorage>();

            // Use partitioned CosmosDB for storage, instead of in-memory storage.
            //services.AddSingleton<IStorage>(
            //    new CosmosDbPartitionedStorage(
            //        new CosmosDbPartitionedStorageOptions
            //        {
            //            CosmosDbEndpoint = cosmosOptions.Endpoint,
            //            AuthKey = cosmosOptions.AuthKey,
            //            DatabaseId = cosmosOptions.DatabaseId,
            //            ContainerId = cosmosOptions.ContainerId,
            //            CompatibilityMode = false,
            //        }));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseDefaultFiles()
                .UseStaticFiles()
                .UseWebSockets()
                .UseRouting()
                .UseAuthorization()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });

            // app.UseHttpsRedirection();
        }
    }
}
