using Castle.DynamicProxy;

using DynamicProxy_AOP.Common.CastleDynamicProxy;
using DynamicProxy_AOP.Common.ControllerActivator;
using DynamicProxy_AOP.Common.Interceptors;
using DynamicProxy_AOP.Extensions;
using DynamicProxy_AOP.Models;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace DynamicProxy_AOP
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

            services.AddControllers();
            //    .AddControllersAsServices();//AddControllersAsServices ���������������ڹ���Ȩ�޽���ioc������Ĭ����.netcore mvc��ܹ����������������
            //services.Replace(ServiceDescriptor.Transient<IControllerActivator, CustomControllerActivator>()); //�滻mvcĬ�ϵ�������
            //������ʵ��Aop���� ����ʹ�ÿ���Դ���Filter

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "DynamicProxy_AOP", Version = "v1" });
            });
            services.AddMemoryCache();

            // ��� Castle �Ĵ���������
            services.AddSingleton<ProxyGenerator>();

            services.AddTransient<ICustomInterceptor, LogInterceptor>();
            services.AddTransient<ICustomInterceptor, CacheInterceptor>();

            services.AddProxyService<ITestUser, TestUser>(ServiceLifetime.Transient);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DynamicProxy_AOP v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
