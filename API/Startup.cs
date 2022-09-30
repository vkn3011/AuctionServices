using EAuction.API.Model;
using EAuction.DataAccessProvider;
using EAuction.Service;
using EAuction.Service.BidsService;
using EAuction.Service.BuyerService;
using EAuction.Service.ProductService;
using EAuction.Service.SellerService;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace EAuction.API
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
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:4200")
                                            .AllowAnyHeader()
                                            .AllowAnyMethod();
                    });
            });
            services.Configure<BookStoreDatabaseSettings>(
            Configuration.GetSection("MongoConnection"));
            services.AddSingleton<IDataAccessProvider, MongoDataAccessProvider>();
            //services.AddScoped<BookService>();
            services.AddScoped<SellerService>();
            services.AddScoped<ProductService>();
            services.AddScoped<BuyerService>();
            services.AddScoped<BidService>();
            services.AddMvc();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "E-Auction",
                    Version = "v1",

                });
                //c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                //{
                //    Version = "v1",
                //    Title = "E-Auction",
                //    Description = "E-Auction ASP.NET Core Web API",
                //    //TermsOfService = new Uri("https://rijsat.com/terms"),
                //    Contact = new Microsoft.OpenApi.Models.OpenApiContact
                //    {
                //        Name = "E-Auction",
                //        Email = string.Empty,
                //        //Url = new Uri("https://rijsat.com/spboyer"),
                //    },
                //    License = new Microsoft.OpenApi.Models.OpenApiLicense
                //    {
                //        Name = "Use under Open Source",
                //        //Url = new Uri("https://rijsat.com/license"),
                //    }
                //});
                //var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                //c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

           
            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors(builder =>
            {
                builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
            });
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
            });
        }
    }
}
