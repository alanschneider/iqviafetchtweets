using IqviaFetchTweets.Interfaces;
using IqviaFetchTweets.Middleware;
using IqviaFetchTweets.OldApi.Interfaces;
using IqviaFetchTweets.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IqviaFetchTweets
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
            services.AddSingleton<ITweetsSettings>(new TweetsSettings(Configuration));
            services.AddSingleton<IOldApi>(new OldApi.OldApi(Configuration));
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseMiddleware(typeof(HttpErrorMiddleware));
            app.UseMvc();
        }
    }
}
