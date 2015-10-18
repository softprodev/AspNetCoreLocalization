﻿using System.Collections.Generic;
using System.Globalization;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Localization;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Logging;

namespace AspNet5Localization
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.AddMvc()
                .AddViewLocalization()
                .AddDataAnnotationsLocalization();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.MinimumLevel = LogLevel.Information;
            loggerFactory.AddConsole();
            loggerFactory.AddDebug();

            //var requestLocalizationOptions = new RequestLocalizationOptions
            //{
            //    DefaultRequestCulture = new RequestCulture(new CultureInfo("en-US")),
            //    SupportedCultures = new List<CultureInfo>
            //    {
            //        new CultureInfo("en-US"), new CultureInfo("de-CH"), new CultureInfo("fr-CH"), new CultureInfo("it-CH")
            //    },
            //    SupportedUICultures = new List<CultureInfo>
            //    {
            //        new CultureInfo("en-US"), new CultureInfo("de-CH"), new CultureInfo("fr-CH"), new CultureInfo("it-CH")
            //    }
            //};
            //requestLocalizationOptions.RequestCultureProviders = new List<IRequestCultureProvider>();
            //requestLocalizationOptions.RequestCultureProviders.Add(new AcceptLanguageHeaderRequestCultureProvider());
            //requestLocalizationOptions.RequestCultureProviders.Add(new QueryStringRequestCultureProvider());
            //app.UseRequestLocalization(requestLocalizationOptions);

            app.UseRequestLocalization(new RequestLocalizationOptions());
            app.UseIISPlatformHandler();

            app.UseStaticFiles();

            app.UseMvc();

            
        }
    }
}
