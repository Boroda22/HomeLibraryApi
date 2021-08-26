using HomeLibraryApi.Services;
using Microsoft.EntityFrameworkCore;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using HomeLibraryApi.Models;

namespace HomeLibraryApi
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
            services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);
            services.AddDbContext<StorageContext>(db => db.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = "JwtBearer";
                options.DefaultChallengeScheme = "JwtBearer";
            })
                .AddJwtBearer(options =>
                        {
                            options.RequireHttpsMetadata = false;
                            options.TokenValidationParameters = new TokenValidationParameters
                            {
                                // укзывает, будет ли валидироваться издатель при валидации токена
                                ValidateIssuer = true,
                                // строка, представляющая издателя
                                ValidIssuer = AuthOptions.ISSUER,
                                // будет ли валидироваться потребитель токена
                                ValidateAudience = true,
                                // установка потребителя токена
                                ValidAudience = AuthOptions.AUDIENCE,
                                // будет ли валидироваться время существования
                                ValidateLifetime = true,
                                // установка ключа безопасности
                                IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                                // валидация ключа безопасности
                                ValidateIssuerSigningKey = false,
                            };
                        });
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "HomeLibraryApi", Version = "v1" }); });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "HomeLibraryApi"));
            }

            app.UseAuthentication();

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
