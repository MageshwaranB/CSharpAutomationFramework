using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PracticeAutomationFramework.Utilites
{
    class BasicUtility : BaseClass
    {
        
        public static void ScrollToTheElement(IWebElement webElement, IWebDriver driver)
        {
            IJavaScriptExecutor javaScriptExecutor = (IJavaScriptExecutor)driver;
            javaScriptExecutor.ExecuteScript("arguments[0].scrollIntoView(true);", webElement);
        }

    }
}
