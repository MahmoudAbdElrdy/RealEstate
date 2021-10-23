using AutoMapper;
using Mapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RealEstate.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using TanvirArjel.Extensions.Microsoft.DependencyInjection;

namespace RealEstate.Api
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
            #region MVC Settings

            services.AddCors();

            services.AddControllers(options => options.EnableEndpointRouting = false);
           
            services.AddControllers().AddJsonOptions(op => op.JsonSerializerOptions.PropertyNamingPolicy = null);
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });

           

            // services.AddDistributedMemoryCache();

            #endregion
            #region DbContext

            //services.AddEntityFrameworkSqlServer().AddDbContext<RealEstateContext>(options =>
            // options.UseLazyLoadingProxies(false).UseSqlServer(Configuration["ConnectionStrings:RealEstateConnection"]));
           #region Context
            services.AddDbContext<RealEstateContext>(opt =>
            {
                opt.UseSqlServer(Configuration.GetConnectionString("RealEstateConnection"), sqlOptions =>
                {
                    //sqlOptions.MigrationsAssembly("RealEstate.Data");
                });
            });
            #endregion
            #endregion
            #region 
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "JwtBearer";
                options.DefaultChallengeScheme = "JwtBearer";
            })
         .AddJwtBearer("JwtBearer", jwtOptions =>
         {
             jwtOptions.TokenValidationParameters = new TokenValidationParameters()
             {
                   // The SigningKey is defined in the TokenController class
                   IssuerSigningKey = Settings.SIGNING_KEY,
                 ValidateIssuer = false,
                 ValidateAudience = false,
                 ValidateIssuerSigningKey = true,
                 ValidateLifetime = true,
                 ClockSkew = TimeSpan.FromMinutes(1500)
             };
         });
            #endregion
            #region ScopedServiceAttribute
            string[] assembliesToBeScanned = new string[] { "RealEstate.DataAccess" };
            services.AddServicesOfAllTypes(assembliesToBeScanned);
            services.AddServicesWithAttributeOfType<ScopedServiceAttribute>(assembliesToBeScanned);
           
            services.AddServicesWithAttributeOfType<SingletonServiceAttribute>(assembliesToBeScanned);
           
            services.AddHttpContextAccessor();
            #endregion
            #region Mapper
            if (services != null)
            {
                var provider = services.BuildServiceProvider();
                BaseEntityExtension.Configure(provider.GetService<IMapper>());
                provider.GetRequiredService<IServiceScopeFactory>();
            }

            services.AddAutoMapper(new Assembly[] {
                typeof(AutoMapperProfile).GetTypeInfo().Assembly,
            })
            .AddHttpContextAccessor()
            .AddHttpClient();
            #endregion
            #region AddNewtonsoftJson
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.Converters.Add(new StringEnumConverter(new CamelCaseNamingStrategy()));
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });
            #endregion
           
         
            #region Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Version = "v1",
                    Title = "CRM API",
                    Description = "RealEstate CRM ASP.NET Core Web API"
                });

                c.AddSecurityDefinition("Bearer",
                  new OpenApiSecurityScheme()
                  {
                      In = ParameterLocation.Header,
                      Description = "Please enter the word 'Bearer' followed by space and JWT",
                      Name = "Authorization",
                      Type = SecuritySchemeType.ApiKey,
                      Scheme = "Bearer"
                  });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme()
                        {
                            Reference = new OpenApiReference()
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    },
                });
            });
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "RealEstate.Api v1"));
            }
            app.UseCors(x => x
              .AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
      
    }

}
