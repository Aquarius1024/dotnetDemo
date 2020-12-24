using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FakeXiecheng.API.Database;
using FakeXiecheng.API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;


namespace FakeXiecheng.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            // 每次访问都创建一个，各自独立互不干扰
            //services.AddTransient<ITouristRouteRepository, MockTouristRouteRepository>(); 假数据

            services.AddTransient<ITouristRouteRepository, TouristRouteRepository>(); // 真实数据
            // 创建单独一个实例，公用一个仓库
            //services.AddSingleton
            // 创建一个，通过事物限制多次修改
            //services.AddScoped

            services.AddDbContext<AppDbContext>(option => {
                // 连接外部数据库
                //option.UseSqlServer("server=localhost; Database=FakeXiechengDb; User Id=sa; Password=root;");
                // 连接自带数据库
                //option.UseSqlServer(@"Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=FakeXiechengDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

                option.UseSqlServer(Configuration["DbContext:ConnectionString"]);

                //option.UseMySql(Configuration["DbContext:MySQLConnectionString"]);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                // endpoints.MapGet("/", async context =>
                // {
                //     await context.Response.WriteAsync("Hello World!");
                // });
            });
        }
    }
}
