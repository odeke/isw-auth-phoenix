using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using PhoenixAuth.BusinessLogic.Models;
using PhoenixAuth.BusinessLogic.Services.ClientRegistration;
using PhoenixAuth.BusinessLogic.Services.Crypto;
using PhoenixAuth.BusinessLogic.Services.HttpClientWrapper;
using PhoenixAuth.BusinessLogic.Services.IswAuth;
using PhoenixAuth.BusinessLogic.Services.KeyExchange;
using PhoenixAuth.BusinessLogic.Services.PartnerAuth;
using PhoenixAuth.BusinessLogic.Services.ReadConfiguration;
using System;
using System.Linq;
using System.Net.Http.Headers;
using static PhoenixAuth.BusinessLogic.Models.EndPoints;
using PhoenixAuth.BusinessLogic.BusinessEntities;
using PhoenixAuth.BusinessLogic.Services.CompleteRegistration;
using PhoenixAuth.BusinessLogic.Services.CashOut;

namespace PhoenixAuth
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();//User secrets

            services.AddDbContext<PhoenixAuthDbContext>(options =>
            {
                options.UseSqlServer(Configuration["AppKeys:DefaultConnection"]);//.EnableSensitiveDataLogging();
            });
            
            // Add our Config object so it can be injected
            services.Configure<AppSecretsConfig>(option => Configuration.GetSection("AppKeys").Bind(option));

            services.ConfigureWritable<AppSecretsConfig>(Configuration.GetSection("AppKeys"));

            
            //Phoenix API Client
            services.AddHttpClient(PhoenixClient, c =>
                {
                    c.BaseAddress = new Uri(Configuration["AppKeys:PhoenixBaseUrl"]);
                    c.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                });
            
            services.AddHttpContextAccessor();
            
            services.AddMvc(options =>
            {
                options.ReturnHttpNotAcceptable = true;
                options.Filters.Add(typeof(RequestAsyncActionFilter)); //Add request log filter
                options.OutputFormatters.RemoveType<HttpNoContentOutputFormatter>();
            })
                .AddNewtonsoftJson(opt =>
                {
                    opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    opt.SerializerSettings.Formatting = Formatting.Indented;
                    opt.SerializerSettings.NullValueHandling = NullValueHandling.Include;
                })
                
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "ISW Auth C# Integration",
                    Version = "v1",
                    Description = "ISW Auth Integration C# sample code",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact { Name = "Morgan Kamoga", Email = "morgan.kamoga@interswitchgroup.com", Url = new Uri("https://twitter.com/spboyer"), },
                    License = new OpenApiLicense { Name = "Use under LICX", Url = new Uri("https://example.com/license"), }
                });
                
                c.ResolveConflictingActions(apiDescription => apiDescription.First());

                //var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                //c.IncludeXmlComments(xmlPath);

            });

            // Add application services.
            DiContainer(services);
        }

        private static void DiContainer(IServiceCollection services)
        {

            //Base
            //services.AddTransient<IHttpHeaderService, HttpHeaderService>();
            services.AddTransient<IReadConfigurationService, ReadConfigurationService>();
            services.AddTransient<IPhoenixHttpClient, PhoenixHttpClient>();
            services.AddTransient<IClientRegistrationService, ClientRegistrationService>();
            services.AddTransient<IKeyExchangeService, KeyExchangeService>();
            services.AddTransient<IIswAuthService, IswAuthService>();
            services.AddTransient<ICryptoService, CryptoService>();
            services.AddTransient<IPartnerAuthService, PartnerAuthService>();
            services.AddScoped<DbContext, PhoenixAuthDbContext>();
            services.AddScoped<ICompleteRegistration, CompleteRegistration>();


            services.AddScoped<ICashOutService, CashOutService>();
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, PhoenixAuthDbContext quickTellerDbContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            
            app.UseHttpsRedirection();
            
            app.UseRouting();
            
            //app.UseAuthentication();
            app.UseAuthorization();

            app.InitializeDatabase(quickTellerDbContext);

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("swagger/v1/swagger.json", "ISW Auth Integration");
                c.RoutePrefix = string.Empty;
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
