using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using VinorSoft.Tymy.Service;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using VinorSoft.Tymy.Service.Interface;
using VinorSoft.Tymy.Service.Service;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using VinorSoft.Tymy.Service.Model;
using KTApps.Core.App;
using VinorSoft.Tymy.Service.Entities;
using System.Reflection;
using AutoMapper;

namespace VinorSoft.Tymy.API
{
    public class Startup
    {
     
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {

            services.Configure<RequestLocalizationOptions>(options =>
            {
                CultureInfo[] supportedCultures = new[]
                {
                    new CultureInfo("vi-VN"),
                };
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
                options.DefaultRequestCulture = new RequestCulture("vi-VN");
            });

            services.AddMvcCore()
            .AddApiExplorer();
            services.Configure<ApiBehaviorOptions>(opt =>
            {
                opt.SuppressModelStateInvalidFilter = true;
            });
            
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                         .AddJsonOptions(options =>
                         {
                             options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                             options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                             options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;

                         });
            services.AddCors(options =>
            {
                options.AddPolicy("MyAllowsSpecificOrigins",
                builder =>
                {
                    builder
                    .WithOrigins("http://localhost:4200")
                    .AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
                });
            });
            // configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            // configure jwt authentication
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key =Convert.FromBase64String(appSettings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {

                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

           // services.AddAutoMapper(typeof(TymyAutoMapper).GetTypeInfo().Assembly);
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new TymyAutoMapper());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddMvc();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IStaffService, StaffService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<ITableService, TableService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<INotificationService, NotificationService>();


            services.AddDbContext<TymyDbContext>(x => x.UseSqlServer(Configuration.GetConnectionString("Tymy.Context")));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Tymy API", Version = "v1" });
            });
            //services.AddHttpContextAccessor();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            
            //loggerFactory.AddSerilog();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticHttpContext();
            app.UseCors("MyAllowsSpecificOrigins");
            app.UseAuthentication();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Tymy V1");
            });
            app.UseRequestLocalization();
            app.UseMvc();
        }

    }
}
