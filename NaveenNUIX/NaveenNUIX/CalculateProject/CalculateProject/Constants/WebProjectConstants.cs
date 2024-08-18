using AventStack.ExtentReports;
using OpenQA.Selenium;
using System.Reflection;

namespace CalculateProject.Constants
{
    public class WebProjectConstants
    {
        public static IWebDriver? webdriver = null;
        public static ExtentReports extentReports =null;
        public static ExtentTest extentTest;
        public static ExtentTest featureName;
        public static ExtentTest scenario;
        public static string assemblyDirectory = AssemblyDirectory;
        public static string TestArtifacts = assemblyDirectory + @"\TestArtifacts\";
        public static string TestReports = assemblyDirectory + @"\TestReports\";
        public static string TestOutputs = assemblyDirectory + @"\TestOutputs\";
        public static ScenarioContext scenarioContext;
        public static Dictionary<string, string> environmentKeyValuePairs;
        public static string runconfigEnvironment = string.Empty;

        public static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

    }
}
