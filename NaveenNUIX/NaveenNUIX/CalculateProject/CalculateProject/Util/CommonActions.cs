using AventStack.ExtentReports;
using CalculateProject.Constants;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System.Reflection;

namespace CalculateProject.Util
{
    public class CommonActions
    {
        /// <summary>
        /// Find Element by Selector
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        public IWebElement FindElement(By selector)
        {
            try
            {
                IList<IWebElement> elements = WebProjectConstants.webdriver!.FindElements(selector);
                if (elements == null || elements.Count == 0)
                {
                    throw new NoSuchElementException("Unable to locate an element using selector : " + selector);
                }
                if (elements.Count > 1)
                {
                    List<IWebElement> visible = new List<IWebElement>();
                    visible = elements.Where((IWebElement ele) => ele.Displayed).ToList();
                    if (visible.Count != 0)
                    {
                        elements = visible;
                    }
                }
                return elements.ToList()[0];
            }
            catch (NoSuchElementException ex)
            {
                WebProjectConstants.extentTest.Log(Status.Fail, MethodBase.GetCurrentMethod()!.Name + " due to " + ex.Message, MediaEntityBuilder.CreateScreenCaptureFromPath(TakeScreenShotOfAction()).Build());
                return null;
            }
        }

        /// <summary>
        /// Click Webelement Based on Locator
        /// </summary>
        /// <param name="selector"></param>
        public void ClickWebElement(By selector)
        {
            try
            {
                IWebElement element = WebProjectConstants.webdriver!.FindElement(selector);
                element.Click();
            }
            catch (NoSuchElementException ex)
            {
                WebProjectConstants.extentTest.Log(Status.Fail, MethodBase.GetCurrentMethod()!.Name + " due to " + ex.Message, MediaEntityBuilder.CreateScreenCaptureFromPath(TakeScreenShotOfAction()).Build());
            }
        }

        /// <summary>
        /// Get Inner text by webelement locator
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        public string GetInnerText(By selector)
        {
            try
            {
                IWebElement element = WebProjectConstants.webdriver!.FindElement(selector);
                return element.GetAttribute("innerText");
            }
            catch (Exception ex)
            {
                WebProjectConstants.extentTest.Log(Status.Fail, MethodBase.GetCurrentMethod()!.Name + " due to " + ex.Message);
                return string.Empty;
            }
        }

        /// <summary>
        /// Wait untill ElementLocator is found with the time frame (in seconds)
        /// </summary>
        /// <param name="elementLocator"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public IWebElement WaitUntilElementClickable(By elementLocator, int timeout = 30)
        {
            try
            {
                var wait = new WebDriverWait(WebProjectConstants.webdriver, TimeSpan.FromSeconds(timeout));
                return wait.Until(ExpectedConditions.ElementToBeClickable(elementLocator));
            }
            catch (NoSuchElementException)
            {
                //   Console.WriteLine("Element with locator: '" + elementLocator + "' was not found in current context page.");
                WebProjectConstants.extentTest.Log(Status.Fail, MethodBase.GetCurrentMethod()!.Name, MediaEntityBuilder.CreateScreenCaptureFromPath(TakeScreenShotOfAction()).Build());
                return null;
            }
        }

        /// <summary>
        /// click the Element based on the webelement Locator
        /// </summary>
        /// <param name="selector"></param>
        public void Click(By selector)
        {
            try
            {
                IWebElement element = WaitUntilElementClickable(selector);
                IJavaScriptExecutor? js = (WebProjectConstants.webdriver != null) ? (IJavaScriptExecutor)WebProjectConstants.webdriver : null;
                if (js != null && element != null)
                {
                    js.ExecuteScript("arguments[0].focus();", element);
                    Thread.Sleep(1000);
                }
                element!.Click();
            }
            catch (Exception ex)
            {
                WebProjectConstants.extentTest.Log(Status.Fail, MethodBase.GetCurrentMethod()!.Name + ": Selector " + selector.ToString() + " - " + ex.Message, MediaEntityBuilder.CreateScreenCaptureFromPath(TakeScreenShotOfAction()).Build());
            }
        }

        /// <summary>
        /// Launch the URL
        /// </summary>
        /// <param name="url"></param>
        public void LaunchWebURL(string url)
        {
            try
            {
                WebProjectConstants.webdriver!.Navigate().GoToUrl(url);
            }
            catch (Exception ex)
            {
                WebProjectConstants.extentTest.Log(Status.Fail, MethodBase.GetCurrentMethod()!.Name + " due to " + ex.Message);
            }
        }

        /// <summary>
        /// Take the screenshot of a particular action
        /// </summary>
        /// <returns></returns>
        public string TakeScreenShotOfAction()
        {
            try
            {
                string fileName = WebProjectConstants.TestReports + @"Screenshots\" + Guid.NewGuid() + ".png";
                Screenshot screenshot = ((ITakesScreenshot)WebProjectConstants.webdriver!).GetScreenshot();
                screenshot.SaveAsFile(fileName);
                return fileName;
            }
            catch (Exception ex)
            {
                WebProjectConstants.extentTest.Log(Status.Fail, MethodBase.GetCurrentMethod()!.Name + " due to " + ex.Message);
                return string.Empty;
            }
        }

        //public void ReadingAppSettingsFile()
        //{
        //    var builder = new ConfigurationBuilder().AddJsonFile("appSettings.json", false, false);
        //    var config = builder.Build();
        //    string testkey = config["Test_Browser"];
        //    var abc = config["Stage:URL"];
        //}
    }
}
