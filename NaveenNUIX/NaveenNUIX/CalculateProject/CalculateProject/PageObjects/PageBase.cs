using CalculateProject.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculateProject.PageObjects
{
    public class PageBase 
    {
        private CalculatorPage? calculatorPage;

        public CalculatorPage CalculatorPage
        {
            get
            {
                if (calculatorPage == null)
                    calculatorPage = new CalculatorPage();
                return calculatorPage;

            }
        }
    }
}
