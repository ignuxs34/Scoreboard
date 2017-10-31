﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ScoreboardServer.Database;
using ScoreboardServer.Repositories;
using ScoreboardServer.Services;
using Swashbuckle.AspNetCore.Swagger;

namespace ScoreboardServer
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
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("scoreboard_server", new Info());
                c.OperationFilter<AuthorizationHeaderParameterOperationFilter>();
            });

            /*services.AddIdentityServer().AddDeveloperSigningCredential()
                .AddInMemoryApiResources(Config.GetApiResources()).AddInMemoryClients(Config.GetClients());*/

            //services.AddMvcCore().AddAuthorization().AddJsonFormatters();

            /*services.AddAuthentication("Bearer").AddIdentityServerAuthentication(options =>
            {
                options.Authority = "http://localhost:65258";
                options.RequireHttpsMetadata = false;

                options.ApiName = "api1";
            });*/

            string domain = $"https://{Configuration["Auth0:Domain"]}/";
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = domain;
                options.Audience = Configuration["Auth0:ApiIdentifier"];
            });

            services.AddMvc();

            services.AddScoped<ITeamsService, TeamsService>();
            services.AddScoped<ITeamsRepository, TeamsRepository>();
            services.AddScoped<IPlayersService, PlayersService>();
            services.AddScoped<IPlayersRepository, PlayersRepository>();
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite("Filename=./database.db"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseIdentityServer();

            app.UseAuthentication();

            app.UseMvc();

            const string swaggerUrl = "/swagger/scoreboard_server/swagger.json";
            app.UseSwagger()
                .UseSwaggerUI(c =>
                {
                    c.DocExpansion("none");
                    c.SwaggerEndpoint(swaggerUrl, "Scoreboard Server");
                });
        }
    }
}
