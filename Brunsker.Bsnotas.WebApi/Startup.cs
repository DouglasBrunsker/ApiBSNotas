using Brunsker.Bsnotas.WebApi.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Brunsker.Bsnotas.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public IConfigurationRoot ConfigurationRoot { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplicationServicesExtensions(Configuration);

            services.AddSwaggerDocumentation();

            services.AddCors(options =>
                options.AddPolicy("CorsPolicy", builder => builder
                .AllowCredentials()
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                )
            );
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("CorsPolicy");
            //app.UseDeveloperExceptionPage();

            app.AddApplicationBuilderExtencios();
        }
    }
}
