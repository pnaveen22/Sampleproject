using CalculateProject.Constants;
using CalculateProject.PageObjects;

namespace CalculateProject.StepDefinitions
{
    [Binding]
    public sealed class CalculatorStepDefinitions : PageBase
    {

        [Given(@"Launch Calculator Application")]
        public void GivenLaunchCalculatorApplication()
        {
            CalculatorPage.LaunchWebURL(WebProjectConstants.environmentKeyValuePairs.FirstOrDefault(c => c.Key.Equals("nuix_url")).Value);
        }

        [Given(@"Enter first number as (.*)")]
        public void GivenEnterFirstNumberAs(int number)
        {
            CalculatorPage.EnterTheNumber(number);
        }

        [When(@"the firstnumber is added with second number (.*)")]
        public void WhenTheFirstnumberIsAddedWithSecondNumber(int secondNumber)
        {
            CalculatorPage.Click(CalculatorPage.addBtn);
            CalculatorPage.EnterTheNumber(secondNumber);
        }

        [Then("the result should be (.*)")]
        public void ThenTheResultShouldBe(int result)
        {
            CalculatorPage.Click(CalculatorPage.equalBtn);
            CalculatorPage.ValidateResult(result);
        }

        [When(@"the firstnumber is subtracted with second number (.*)")]
        public void WhenTheFirstnumberIsSubtractedWithSecondNumber(int secondNumber)
        {
            CalculatorPage.Click(CalculatorPage.subtractBtn);
            CalculatorPage.EnterTheNumber(secondNumber);
        }

        [Given("the first number is (.*)")]
        public void GivenTheFirstNumberIs(int number)
        {
            CalculatorPage.EnterTheNumber(number);
        }

        [When(@"the second number (.*) is multiplied")]
        public void WhenTheSecondNumberIsMultiplied(int secondNumber)
        {
            CalculatorPage.Click(CalculatorPage.multipleBtn);
            CalculatorPage.EnterTheNumber(secondNumber);
        }

        [When(@"Enter ""([^""]*)"" and ""([^""]*)"" with ""([^""]*)""")]
        public void WhenEnterAndWith(string firstNumber, string secondNumber, string arithmeticOperation)
        {
            CalculatorPage.ArthematicOperation(firstNumber, secondNumber, arithmeticOperation);
        }

        [When(@"the firstnumber is divided by second number (.*)")]
        public void WhenTheFirstnumberIsDividedBySecondNumber(int secondNumber)
        {
            CalculatorPage.Click(CalculatorPage.divisionBtn);
            CalculatorPage.EnterTheNumber(secondNumber);
        }

        [When(@"I perform (addition|subtraction|multiplication|division) on ""([^""]*)"" Number")]
        public void WhenIPerformAdditionSubstractionMultiplicationDivisionOnNumber(string operation, string allNumbers)
        {
            CalculatorPage.IntegratedArthematic(allNumbers, operation);
        }

    }
}
