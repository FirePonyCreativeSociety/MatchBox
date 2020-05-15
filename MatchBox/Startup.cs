using AutoMapper;
using MatchBox.Configuration;
using MatchBox.Data;
using MatchBox.Data.Models;
using MatchBox.Internal;
using MatchBox.Models.Mapping;
using MatchBox.Services.Email;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Linq;
using System.Text;

namespace MatchBox
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
            // Configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection(MatchBoxConfiguration.AppSettingsSectionName);
            services.Configure<MatchBoxConfiguration>(appSettingsSection);
            var mbCfg = appSettingsSection.Get<MatchBoxConfiguration>();
            services.AddSingleton<MatchBoxConfiguration>(mbCfg);
            services.AddSingleton<EmailConfiguration>(mbCfg.Email);
            services.AddSingleton<SecurityConfiguration>(mbCfg.Security);
            services.AddSingleton<PasswordConfiguration>(mbCfg.Password);
            services.AddSingleton<UserConfiguration>(mbCfg.User);

            // Configure jwt authentication            
            // TODO: this is not the ideal place to check... or is it?
            if (string.IsNullOrWhiteSpace(mbCfg.Security.JwtIssuerSigningKey))
                throw new Exception($"Unspecified Jwt IssuerSigningKey value in configuration.");
            
            var key = Encoding.ASCII.GetBytes(mbCfg.Security.JwtIssuerSigningKey);
            services.AddAuthentication(x =>
            {
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
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
                    ValidateAudience = false,
                };                
            });

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    // If CORS options are null or empty, we go AnyOrigin, otherwise just the specified ones
                    bool corsWasSetup = (mbCfg.Security?.CorsOrigins != null) && (mbCfg.Security.CorsOrigins.Any());
                    if (corsWasSetup)
                    {
                        builder = builder.WithOrigins(mbCfg.Security.CorsOrigins.ToArray());
                    }
                    else
                    {
                        builder = builder.AllowAnyOrigin();
                    }

                    // Finishes
                    builder.AllowAnyMethod()
                           .AllowAnyHeader();
                           //.SetIsOriginAllowed(hostName => true)
                           //.AllowCredentials(); // This is to avoid this https://stackoverflow.com/questions/53675850/how-to-fix-the-cors-protocol-does-not-allow-specifying-a-wildcard-any-origin
                });
            });            

            // Auto mapper
            services.AddAutoMapper(typeof(APIAutoMapProfile));

            // Database
            services.AddDbContext<MatchBoxDbContext>(opt =>
            {
                var connStr = Configuration.GetConnectionString(MatchBoxDbContext.DbConnectionName);

                // TODO: temporary. This check me pick an RDBMS type
                if (connStr.Contains("Filename", System.StringComparison.OrdinalIgnoreCase))
                    opt.UseSqlite(connStr);
                else
                    opt.UseSqlServer(connStr);
            });

            services.AddIdentity<DbUser, DbRole>(options =>
                    {
                        options.Password.RequireLowercase = mbCfg.Password.RequireLowercase;
                        options.Password.RequireUppercase = mbCfg.Password.RequireUppercase;
                        options.Password.RequireNonAlphanumeric = mbCfg.Password.RequireNonAlphanumeric;
                        options.Password.RequireDigit = mbCfg.Password.RequireDigit;

                        options.Lockout.AllowedForNewUsers = true;
                        options.User.RequireUniqueEmail = true;
                        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(mbCfg.User.LockoutDurationInMinutes);
                        options.Lockout.MaxFailedAccessAttempts = mbCfg.User.MaxFailedAccessAttempts;                                               
                        options.User.AllowedUserNameCharacters = mbCfg.User.AllowedUserNameCharacters;
                    })                
                    .AddEntityFrameworkStores<MatchBoxDbContext>()
                    .AddDefaultTokenProviders();

            services.AddControllers()
                    .AddNewtonsoftJson(options =>
                    {
                        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                    });

            services.AddSwaggerGen(c => 
            {
                c.SwaggerDoc(name: Version.FullVersion, new Microsoft.OpenApi.Models.OpenApiInfo { Title = "MatchBox API", Version = Version.FullVersion });

                c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Description = "Standard Authorization header using the Bearer scheme. Example: 'bearer {token}'",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey
                });

                c.OperationFilter<AuthenticationRequirementsOperationFilter>();
            });

            services.AddTransient<IJwtProducer, JwtProducer>();
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
#pragma warning disable CA1822 // Mark members as static
        public void Configure(IApplicationBuilder app)
#pragma warning restore CA1822 // Mark members as static
        {
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage(); 
            //}

            //app.UseHttpsRedirection();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(url: $"/swagger/{Version.FullVersion}/swagger.json", name: $"MatchBox API {Version.FullVersion}");
            });

            app.UseStaticFiles();

            app.UseRouting();
            app.UseCors();
            
            app.UseAuthorization();
            app.UseAuthentication();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}