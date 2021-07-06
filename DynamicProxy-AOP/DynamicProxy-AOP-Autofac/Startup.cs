using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.DynamicProxy;

using DynamicProxy_AOP_Autofac.Common.CastleDynamicProxy;
using DynamicProxy_AOP_Autofac.Common.Interceptors;
using DynamicProxy_AOP_Autofac.Extensions;
using DynamicProxy_AOP_Autofac.Models;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

using System;
using System.Linq;
using System.Reflection;

namespace DynamicProxy_AOP_Autofac
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public ILifetimeScope Container { get; set; }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers().AddControllersAsServices();
            //AddControllersAsServices将控制器生命周期管理权限交给IOC容器，默认是MVC框架管理生命周期

            services.AddHttpContextAccessor();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "DynamicProxy_AOP_Autofac", Version = "v1" });
            });

            services.AddMemoryCache();

        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            var controllerTypes = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => typeof(ControllerBase).IsAssignableFrom(t)).ToArray();

            var customInterceptors = new Type[] { typeof(LogInterceptor), typeof(CacheInterceptor) };

            builder.RegisterTypes(controllerTypes)
                .PropertiesAutowired();
                //.EnableClassInterceptors()
                //.AddInterceptors(customInterceptors); //添加拦截器
            //controller生命周期交给ioc容器管理需要启用AddControllersAsServices功能
            //控制器不建议使用Castle.Core拦截器，可以使用框架自带的Filter功能实现

            builder.RegisterType<TestUser>()
                   .As<ITestUser>()
                   .EnableInterfaceInterceptors()
                   .AddInterceptors(customInterceptors) //添加拦截器
                   .InstancePerLifetimeScope();
            //InstancePerDependency(默认值)  等同于 AddTransient 瞬时生命周期,每个使用到的地方都会创建一个新的实例
            //InstancePerLifetimeScope 等同于 AddScope 范围生命周期，一次请求创建一个实例
            //SingleInstance 单例

            builder.RegisterType<LogInterceptor>();
            builder.RegisterType<CacheInterceptor>();
            builder.RegisterGeneric(typeof(CustomAsyncDeterminationInterceptor<>));


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DynamicProxy_AOP_Autofac v1"));
            }

            Container = app.ApplicationServices.GetAutofacRoot();

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
