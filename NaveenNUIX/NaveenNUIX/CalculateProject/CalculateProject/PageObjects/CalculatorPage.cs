using AventStack.ExtentReports;
using CalculateProject.Constants;
using CalculateProject.Util;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static MongoDB.Bson.Serialization.Serializers.SerializerHelper;

namespace CalculateProject.PageObjects
{
    public class CalculatorPage : CommonActions
    {
        public readonly By calculatorResult = By.XPath("//div[@id ='scihistory']/div");
        public readonly By addBtn = By.Id("add");
        public readonly By subtractBtn = By.Id("subtract");
        public readonly By multipleBtn = By.Id("multiply");
        public readonly By divisionBtn = By.Id("divide");
        public readonly By equalBtn = By.CssSelector("input[value='=']");
        public readonly By cancelBtn = By.XPath("//input[@value='C']");
        //public readonly By numberBtn = By.XPath($"//input[@value='{0}']");
        public readonly By finalResult = By.XPath("//div[@id ='display']/div");

        /// <summary>
        /// Click the given number by looping
        /// </summary>
        /// <param name="number"></param>
        public void EnterTheNumber(int number)
        {
            string actualNumber = number.ToString();
            for (int i = 0; i < actualNumber.Length; i++)
            {
                Click(By.XPath($"//input[@value='{actualNumber[i]}']"));
            }
        }

        /// <summary>
        /// Add two given numbers as string parameters
        /// </summary>
        /// <param name="firstNumber"></param>
        /// <param name="secondNumber"></param>
        public void AddTwoNumbers(string firstNumber, string secondNumber)
        {
            Click(cancelBtn);
            EnterStringNumber(firstNumber);
            Click(addBtn);
            EnterStringNumber(secondNumber);
        }

        /// <summary>
        /// Add two given numbers as integer parameters
        /// </summary>
        /// <param name="firstNumber"></param>
        /// <param name="secondNumber"></param>
        public void AddTwoNumbers(int firstNumber, int secondNumber)
        {
            Click(cancelBtn);
            EnterTheNumber(firstNumber);
            Click(addBtn);
            EnterTheNumber(secondNumber);
            Click(equalBtn);
        }

        /// <summary>
        /// Subtract given two numbers
        /// </summary>
        /// <param name="firstNumber"></param>
        /// <param name="secondNumber"></param>
        public void SubtractTwoNumbers(int firstNumber, int secondNumber)
        {
            Click(cancelBtn);
            EnterTheNumber(firstNumber);
            Click(subtractBtn);
            EnterTheNumber(secondNumber);
            Click(equalBtn);
        }

        /// <summary>
        /// Split the numbers by comma separator
        /// </summary>
        /// <param name="numbers"></param>
        public void IntegratedArthematic(string numbers, string arthamaticOperations)
        {
            List<string> allNumbers = numbers.Split(',').ToList();
            foreach (string item in allNumbers)
            {
                for (int i = 0; i < item.Length; i++)
                {
                    Click(By.XPath($"//input[@value='{item[i]}']"));
                    Thread.Sleep(2000);
                }
                ClickOperation(arthamaticOperations);
            }
        }

        /// <summary>
        /// Click Add, Subtract,Multiple,division operation
        /// </summary>
        /// <param name="arthamaticOperations"></param>
        public void ClickOperation(string arthamaticOperations)
        {
            switch (arthamaticOperations.ToLower())
            {
                case "addition":
                    Click(addBtn);
                    break;
                case "subtraction":
                    Click(subtractBtn);
                    break;
                case "multiplication":
                    Click(multipleBtn);
                    break;
                case "division":
                    Click(divisionBtn);
                    break;
                default:
                    Click(addBtn);
                    break;
            }
        }

        /// <summary>
        /// Multiple given two numbers
        /// </summary>
        /// <param name="firstNumber"></param>
        /// <param name="secondNumber"></param>
        public void MultipleNumbers(int firstNumber, int secondNumber)
        {
            Click(cancelBtn);
            EnterTheNumber(firstNumber);
            Click(multipleBtn);
            EnterTheNumber(secondNumber);
        }

        /// <summary>
        /// Division of List of given numbers
        /// </summary>
        /// <param name="allNumbers"></param>
        public void DivisionOfNumbers(List<int> allNumbers)
        {
            foreach (int item in allNumbers)
            {
                Click(By.XPath($"//input[@value='{item}']"));
                Thread.Sleep(2000);
                Click(divisionBtn);
            }
            Click(equalBtn);
        }

        /// <summary>
        /// Division of two given numbers
        /// </summary>
        /// <param name="firstNumber"></param>
        /// <param name="secondNumber"></param>
        public void DivisionOfNumbers(int firstNumber, int secondNumber)
        {
            Click(cancelBtn);
            EnterTheNumber(firstNumber);
            Click(divisionBtn);
            EnterTheNumber(secondNumber);
        }

        /// <summary>
        /// Perform all arthematic Operations on two given numbers
        /// </summary>
        /// <param name="firstNumber"></param>
        /// <param name="secondNumber"></param>
        /// <param name="arthamaticOperations"></param>
        public void ArthematicOperation(string firstNumber, string secondNumber, string arthamaticOperations)
        {

            Click(cancelBtn);
            EnterStringNumber(firstNumber);
            ClickOperation(arthamaticOperations);
            EnterStringNumber(secondNumber);
        }

        /// <summary>
        /// Enter the number as string parameter
        /// </summary>
        /// <param name="actualNumber"></param>
        public void EnterStringNumber(string actualNumber)
        {
            int actualNumberLength = 0;
            while (actualNumberLength < actualNumber.Length)
            {
                Click(By.XPath($"//input[@value='{actualNumber[actualNumberLength]}']"));
                actualNumberLength++;
            }
        }

        /// <summary>
        /// Validate the result
        /// </summary>
        /// <param name="result"></param>
        public void ValidateResult(int result)
        {
            try
            {
                int actualResult = Convert.ToInt32(GetInnerText(finalResult));
                Assert.AreEqual(result, actualResult, $"expected result {result} is not same as actual result {actualResult}");
            }
            catch (Exception ex)
            {
                WebProjectConstants.extentTest.Log(Status.Fail, MethodBase.GetCurrentMethod()!.Name + " due to " + ex.Message, MediaEntityBuilder.CreateScreenCaptureFromPath(TakeScreenShotOfAction()).Build());
            }
        }

    }
}
