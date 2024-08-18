using CalculateProject.Constants;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using WebDriverManager.DriverConfigs.Impl;
using WebDriverManager;
using AventStack.ExtentReports.Gherkin.Model;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using OpenQA.Selenium.Interactions;
using CalculateProject.Util;
using System.Configuration;

namespace CalculateProject.Hooks
{
    [Binding]
    public sealed class Hooks
    {
        CommonActions actions = new CommonActions();
        public Hooks(ScenarioContext scenarioContext)
        {
            WebProjectConstants.scenarioContext = scenarioContext;
        }

        [BeforeTestRun]
        public static void InitializeReport()
        {
            //Initialize Extent report before test starts
            var htmlReporter = new ExtentHtmlReporter(WebProjectConstants.TestReports);
            htmlReporter.Config.Theme = AventStack.ExtentReports.Reporter.Configuration.Theme.Standard;
            ////Attach report to reporter
            WebProjectConstants.extentReports = new ExtentReports();
            WebProjectConstants.extentReports.AttachReporter(htmlReporter);
        }

        [BeforeFeature]
        public static void BeforeFeature(FeatureContext featureContext)
        {
            WebProjectConstants.featureName = WebProjectConstants.extentReports.CreateTest<Feature>(featureContext.FeatureInfo.Title);
        }

        [BeforeScenario(Order = 0)]
        public void SetEnvironmentVariables()
        {
            WebProjectConstants.scenario = WebProjectConstants.featureName.CreateNode<Scenario>(WebProjectConstants.scenarioContext.ScenarioInfo.Title);
            string environment = Environment.GetEnvironmentVariable("Test_Env").ToLower();
            var codeBasePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
            Regex solutionPath = new Regex(@"(?<!fil)[A-Za-z]:\\+[\S\s]*?(?=\\+bin)");
            string settingFilePath = solutionPath.Match(codeBasePath).Value + "\\" + Environment.GetEnvironmentVariable("Test_FilePath").ToLower();
            WebProjectConstants.environmentKeyValuePairs = new Dictionary<string, string>();
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(settingFilePath);
            XmlNodeList parameterNodes = xmlDocument.GetElementsByTagName("TestRunParameters")[0].ChildNodes;
            for (int i = 0; i < parameterNodes.Count; i++)
            {
                WebProjectConstants.environmentKeyValuePairs.Add(parameterNodes[i].Attributes.GetNamedItem("name").InnerText.ToString(), parameterNodes[i].Attributes.GetNamedItem("value").InnerText.ToString());
            }
            if (string.IsNullOrEmpty(environment))
                throw new Exception("configuration exception please provide environment value as staging, production");
            else
                WebProjectConstants.runconfigEnvironment = environment;
            //EnvironmentVariables.SetUp();
            //EnvironmentVariables.SetUp(ConfigurationManager.AppSettings["environment"]);
        }

        [BeforeScenario(Order = 1)]
        public void InitializeWebDriver()
        {
            string browser = WebProjectConstants.environmentKeyValuePairs.FirstOrDefault(c => c.Key.Equals("Test_Browser")).Value;
            switch (browser.ToLower())
            {
                case "chrome":
                    new DriverManager().SetUpDriver(new ChromeConfig());
                    ChromeOptions chromeOptions = new ChromeOptions();
                    chromeOptions.AddExcludedArgument("enable-automation");
                    chromeOptions.AddAdditionalOption("useAutomationExtension", false);
                    chromeOptions.AddArguments("--test-type");
                    chromeOptions.AddArguments("--disable-extensions");
                    WebProjectConstants.webdriver = new ChromeDriver(chromeOptions);
                    break;
                case "firefox":
                    new DriverManager().SetUpDriver(new FirefoxConfig());
                    FirefoxOptions firefoxOptions = new FirefoxOptions();
                    firefoxOptions.AddAdditionalOption("useAutomationExtension", false);
                    firefoxOptions.AddArguments("--test-type");
                    firefoxOptions.AddArguments("--disable-extensions");
                    WebProjectConstants.webdriver = new FirefoxDriver(firefoxOptions);
                    break;
                case "edge":
                    new DriverManager().SetUpDriver(new EdgeConfig());
                    EdgeOptions edgeOptions = new EdgeOptions();
                    edgeOptions.AddAdditionalOption("useAutomationExtension", false);
                    edgeOptions.AddArguments("--test-type");
                    edgeOptions.AddArguments("--disable-extensions");
                    WebProjectConstants.webdriver = new EdgeDriver(edgeOptions);
                    break;
                default:
                    new DriverManager().SetUpDriver(new ChromeConfig());
                    WebProjectConstants.webdriver = new ChromeDriver();
                    break;
            }

            WebProjectConstants.webdriver.Manage().Timeouts().PageLoad.Add(TimeSpan.FromMinutes(1));
            WebProjectConstants.webdriver.Manage().Window.Maximize();
        }

        [BeforeStep]
        public void Beforestep()
        {
            var stepType = WebProjectConstants.scenarioContext.StepContext.StepInfo.StepDefinitionType.ToString();
            if (stepType == "Given")
                WebProjectConstants.extentTest = WebProjectConstants.scenario.CreateNode<Given>(ScenarioStepContext.Current.StepInfo.Text);
            else if (stepType == "When")
                WebProjectConstants.extentTest = WebProjectConstants.scenario.CreateNode<When>(ScenarioStepContext.Current.StepInfo.Text);
            else if (stepType == "Then")
                WebProjectConstants.extentTest = WebProjectConstants.scenario.CreateNode<Then>(ScenarioStepContext.Current.StepInfo.Text);
            else if (stepType == "And")
                WebProjectConstants.extentTest = WebProjectConstants.scenario.CreateNode<And>(ScenarioStepContext.Current.StepInfo.Text);
        }

        [AfterStep]
        public void InsertReportingSteps()
        {

            string stepType = WebProjectConstants.scenarioContext.StepContext.StepInfo.StepDefinitionType.ToString();
            PropertyInfo pInfo = typeof(ScenarioContext).GetProperty("ScenarioExecutionStatus", BindingFlags.Instance | BindingFlags.Public);
            MethodInfo getter = pInfo.GetGetMethod(nonPublic: true);
            object TestResult = getter.Invoke(WebProjectConstants.scenarioContext, null);

            if (WebProjectConstants.scenarioContext.TestError != null)
            {
                var mediaEntity = MediaEntityBuilder.CreateScreenCaptureFromPath(actions.TakeScreenShotOfAction(), WebProjectConstants.scenarioContext.ScenarioInfo.Title.Trim()).Build();
                if (stepType == "Given")
                    WebProjectConstants.scenario.CreateNode<Given>(ScenarioStepContext.Current.StepInfo.Text).Fail(WebProjectConstants.scenarioContext.TestError.Message, mediaEntity);
                else if (stepType == "When")
                    WebProjectConstants.scenario.CreateNode<When>(ScenarioStepContext.Current.StepInfo.Text).Fail(WebProjectConstants.scenarioContext.TestError.Message, mediaEntity);
                else if (stepType == "Then")
                    WebProjectConstants.scenario.CreateNode<Then>(ScenarioStepContext.Current.StepInfo.Text).Fail(WebProjectConstants.scenarioContext.TestError.Message, mediaEntity);
                else if (stepType == "StepDefinition")
                    WebProjectConstants.extentTest = WebProjectConstants.scenario.CreateNode<And>(ScenarioStepContext.Current.StepInfo.Text);

            }
        }

        [AfterScenario]
        public void AfterScenario()
        {
            WebProjectConstants.webdriver!.Quit();
            WebProjectConstants.webdriver.Dispose();
        }

        [AfterTestRun]
        public static void TearDownReport()
        {
            WebProjectConstants.extentReports.Flush();
        }

    }
}
