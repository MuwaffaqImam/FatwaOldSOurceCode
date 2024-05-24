using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using System.Threading.Tasks;
using TeamWork.Hubs;
using TeamWork.Service.DependencyInjection;

namespace TeamWork
{
    public class Startup
    {
        public Startup(IHostEnvironment env)
        {
            CurrentHostEnvironment = env;
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }
        public IConfiguration Configuration { get; }
        public IHostEnvironment CurrentHostEnvironment { get; set; }

        readonly string AllowSpecificOrigins = "AllowOrigin";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddDependencyInjectionService(Configuration, CurrentHostEnvironment);
            //Authentication Middleware
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetSection("AppSettings:Token").Value)),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];
                            // If the request is for our hub...
                            var path = context.HttpContext.Request.Path;

                            if (!string.IsNullOrEmpty(accessToken) &&
                                path.StartsWithSegments("/hubs"))
                            {
                                // Read the token out of the query string
                                context.Token = accessToken;
                            }
                            return Task.CompletedTask;
                        }
                    };
                });
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            //Implement Policy
            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequierUsersAdminRole", policy => policy.RequireRole("UsersAdmin"));
                options.AddPolicy("RequierSuperAdminRole", policy => policy.RequireRole("SuperAdmin"));
                options.AddPolicy("RequierAdminRole", policy => policy.RequireRole("Admin"));
            });
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddHttpContextAccessor();

            //Add This To Allow Any request From Any Applications , void problem CORS
            services.AddCors(options =>
            {
                options.AddPolicy(AllowSpecificOrigins, builder =>
                    builder.WithOrigins(Configuration.GetSection("AllowedClient").Value).AllowAnyMethod().AllowAnyHeader().AllowCredentials());
            });
            services.AddSignalR();
            services.AddMvc();
            services.Configure<DataProtectionTokenProviderOptions>(o => o.TokenLifespan = TimeSpan.FromHours(3));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            // Stop as long as we don't have https certificate.
            // app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(AllowSpecificOrigins);

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ChatHub>("/hubs/chathub");
            });
        }
    }
}
