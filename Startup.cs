using System;
using System.IO;
using Hangfire;
using Hangfire.MemoryStorage;
using Maxima.Net.SDK.Integracao.Utils;
using Maxima.Net.SDK.Domain.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AutoMapper;
using Maxima.Net.SDK.Domain.Mappings;
using Maxima.Net.SDK.Data;
using Maxima.Net.SDK.Domain.Service;
using Maxima.Net.SDK.Domain.Interfaces;

namespace Maxima.Net.SDK
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", Path.Combine(Directory.GetCurrentDirectory(), "chave_pub_sub_subscrib.json"));
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            //Adicionar as funcionalidades do SDK máxima ao container da aplicação.
            services.AddMaximaSdkService();

            //Adicionar entidades ao container da aplicação.
            services.AddTransient<WorkPubSubMaxima>();
            services.AddTransient<ICidadeApiErp, CidadeApiErp>();
            services.AddDbContext<ErpContext>();

            //Configura o hangfire
            //mais detalhes: https://www.hangfire.io/
            services.AddHangfire(configuration => configuration
               .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
               .UseSimpleAssemblyNameTypeSerializer()
               .UseRecommendedSerializerSettings()
               .UseMemoryStorage(ConfiguracaoHangfire.StorageOptions));
            GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute { Attempts = 2 });

            //Adicionar o hangfire ao container da aplicação.
            services.AddHangfireServer();

            //Inicializa todos os mappers do projeto
            services.AddSingleton(AutoMapperConfig.Initialize());

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseHangfireDashboard("/filas", ConfiguracaoHangfire.DashboardOptions());

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }

    public class AutoMapperConfig
    {
        public static IMapper Initialize()
        {
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MapAll());
            });
            return mapperConfig.CreateMapper();
        }
    }
}
