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
            //    .AddControllersAsServices();//AddControllersAsServices 将控制器声明周期管理权限交给ioc容器，默认是.netcore mvc框架管理控制器声明周期
            //services.Replace(ServiceDescriptor.Transient<IControllerActivator, CustomControllerActivator>()); //替换mvc默认的生成类
            //控制器实现Aop拦截 建议使用框架自带的Filter

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "DynamicProxy_AOP", Version = "v1" });
            });
            services.AddMemoryCache();

            // 添加 Castle 的代理生成器
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
