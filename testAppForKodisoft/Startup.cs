using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using testAppForKodisoft.Models;
using testAppForKodisoft.Services;

namespace testAppForKodisoft
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DataContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddSingleton<IMyCollectionService, MyCollectionService>();
            services.AddTransient<IParseRSS, ParceRSS>();
            services.AddMemoryCache();
            services.AddMvc();
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory,DataContext context)
        {
            
            loggerFactory.AddConsole();
            

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();

            Mapper.Initialize(config =>
            {
                config.CreateMap<AutoblogModel, Feeds>();
                config.CreateMap<EngadgetModel, Feeds>();
                config.CreateMap<MyEntityDb,MyCollection>()
                                .ForMember(x => x.Feeds, 
                                           opt => opt.MapFrom(src => src.SerializedListOfStrings.Split(';')));
                config.CreateMap<MyCollection,MyEntityDb>()
                                .ForMember(x => x.SerializedListOfStrings, 
                                opt => opt.MapFrom(src => string.Join(";", src.Feeds)));
            });
            app.UseCaching();
            app.UseMvc();
            DbInitializer.Initialize(context);
        }
    }
}
