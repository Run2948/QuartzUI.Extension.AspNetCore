using QuartzUI.Extension.AspNetCore.BaseJobs;
using QuartzUI.Extension.AspNetCore.BaseService;
using QuartzUI.Extension.AspNetCore.EFContext;
using QuartzUI.Extension.AspNetCore.Service;
using QuartzUI.Extension.AspNetCore.Tools;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace QuartzUI.Extension.AspNetCore.Extensions
{
    public static class QuartzUIExtension
    {
        public static IServiceCollection AddQuartzUI(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction = null)
        {
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromHours(6);
                options.Cookie.HttpOnly = true;
            });
            services.AddRazorPages();
            services.AddHttpClient();
            services.AddHttpContextAccessor();
            if (optionsAction != null)
            {
                services.AddDbContext<QuarzEFContext>(optionsAction);
                services.AddScoped<IQuartzLogService, EFQuartzLogService>();
                services.AddScoped<IQuartzService, EFQuartzService>();
            }
            else
            {
                services.AddScoped<QuartzFileHelper>();
                services.AddScoped<IQuartzLogService, FileQuartzLogService>();
                services.AddScoped<IQuartzService, FileQuartzService>();
            }

            services.AddScoped<HttpResultfulJob>();
            services.AddScoped<ClassLibraryJob>();
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
            services.AddSingleton<IJobFactory, ASPDIJobFactory>();
            services.AddScoped<IQuartzHandle, QuartzHandle>();
            return services;
        }

        /// <summary>
        /// 自动注入定时任务类
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddQuartzClassJobs(this IServiceCollection services)
        {
            var baseType = typeof(IJobService);
            var path = AppDomain.CurrentDomain.RelativeSearchPath ?? AppDomain.CurrentDomain.BaseDirectory;
            var referencedAssemblies = Directory.GetFiles(path, "*.dll");
            List<Type> typelist = new List<Type>();
            foreach (var item in referencedAssemblies)
            {
                try
                {
                    var assembly = Assembly.LoadFrom(item);
                    Type[] ts = assembly.GetTypes();
                    typelist.AddRange(ts.ToList());
                }
                catch (Exception)
                {

                    continue;
                }
            }
            var types = typelist
            .Where(x => x != baseType && baseType.IsAssignableFrom(x)).ToArray();
            var implementTypes = types.Where(x => x.IsClass).ToArray();
            var interfaceTypes = types.Where(x => x.IsInterface).ToArray();
            foreach (var implementType in implementTypes)
            {
                var interfaceType = implementType.GetInterfaces().First();
                services.AddScoped(interfaceType, implementType);
            }
            return services;
        }

        public static IApplicationBuilder UseQuartz(this IApplicationBuilder builder)
        {
            builder.UseSession();
            builder.UseRouting();
            builder.UseStaticFiles();
            builder.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();

            });
            IServiceProvider services = builder.ApplicationServices;
            using (var serviceScope = services.CreateScope())
            {
                var dd = serviceScope.ServiceProvider.GetService<IQuartzHandle>();
                dd.InitJobs();
            }

            return builder;
        }
    }
}
