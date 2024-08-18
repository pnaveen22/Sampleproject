using CalculateProject.Constants;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium.DevTools.V125.CSS;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculateProject.Hooks
{
    public class EnvironmentVariables
    {


        public static void SetUp()
        {
            var builder = new ConfigurationBuilder().AddJsonFile("appSettings.json", false, false);
            var config = builder.Build();
            if (config.GetValue<string>("environment").ToLower() == "stage")
            {
                WebProjectConstants.environmentKeyValuePairs.Add("nuix_stage_url", config.GetValue<string>("Stage:URL"));
                WebProjectConstants.environmentKeyValuePairs.Add("Test_Browser", config.GetValue<string>("Test_Browser"));
               // WebProjectConstants.environmentKeyValuePairs.Add("nuix_" + environment + "_url", ConfigurationManager.AppSettings["nuix_staging_url"]);
               // WebProjectConstants.environmentKeyValuePairs.Add("Test_Browser", ConfigurationManager.AppSettings["Test_Browser"]);
            }
            else if (config.GetValue<string>("environment").ToLower() == "production")
            {
                WebProjectConstants.environmentKeyValuePairs.Add("nuix_stage_url", config.GetValue<string>("Production:URL"));
                WebProjectConstants.environmentKeyValuePairs.Add("Test_Browser", config.GetValue<string>("Test_Browser"));
                //WebProjectConstants.environmentKeyValuePairs.Add("nuix_" + environment + "_url", ConfigurationManager.AppSettings["nuix_production_url"]);
                //WebProjectConstants.environmentKeyValuePairs.Add("Test_Browser", ConfigurationManager.AppSettings["Test_Browser"]);

            }

        }
        public static string GetEnvironmentURL(string environment)
        {
            if (environment == null)
                throw new Exception("CONFIGURATION EXCEPTION :: Please provide environment value(Staging, Production) in App.config file");
            string env = "nuix_" + environment + "_url";
            return ConfigurationManager.AppSettings[env];
        }
    }
}
