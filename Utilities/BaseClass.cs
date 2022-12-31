using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using PracticeAutomationFramework.Utilites;
using System;
using System.Configuration;
using System.IO;
using System.Threading;

namespace PracticeAutomationFramework
{
    public class BaseClass
    {
        // private IWebDriver driver;
        /*
         * This threadlocal driver object knows that everytime we try to create or access this driver object, it will be accessed in a safe manner by a 
         * separate thread. To access the driver from the pool of thread local, we need to use Value property before invoking web driver methods
         */
        public ThreadLocal<IWebDriver> driver = new ThreadLocal<IWebDriver>();
        /*
         * If we want to provide the values at run time like setting up which browser we want the tests to be executed. We can make use of TestParameters
         * If we don't provide any values in the terminal, it will take the browser present in the config file
         * Provide this command in the terminal
         * dotnet test "name of the project with .csproj extension" --TestRunParameters.Parameter\(name=\"name of the string\", value=\"value of the string\")
         */
        String browserName;
        /*
         * Reports should be initialized before any of the tests. We will be making use of annotation called OneTimeSetup
         * Difference between Setup and OneTimeSetup is setup will be called before each tests, whereas OneTimeSetup will be run only once
         * The order of execution OneTimeSetup -> Setup
         */

        public ExtentReports extent;
        public ExtentTest test;
        [OneTimeSetUp]
        public void ReportSetup()
        {
            /*
             * In order to setup the reports, first we need to provide the fully qualified path of the report in project level
             * In the GetParent method, we are navigating from the utilities folder to the main project level. We're providing the path
             * where we want the file to be generated
             */
            var workingDirectory=Environment.CurrentDirectory;
            var projectDirectory=Directory.GetParent(workingDirectory).Parent.Parent.FullName;
            String reportPath=projectDirectory + "//index.html";
            var htmlReporter=new ExtentHtmlReporter(reportPath);
            extent = new ExtentReports();
            extent.AttachReporter(htmlReporter);
            extent.AddSystemInfo("Host Name","Local Host");
            extent.AddSystemInfo("Environment","QA");
            extent.AddSystemInfo("Username","abc@123");
        }

        
        [SetUp]
        public void Setup()
        {
            /*configuration manager is helpful in setting up the base class by reading the data present in the config file. 
             * This is similar to the Properties class in Java 
             */

            /*
             * In the setup, we have to create an entry coming for the report
             * We have to mention the name of the test dynanmically by  making use of the TestContext class
             */
            test=extent.CreateTest(TestContext.CurrentContext.Test.Name);
            browserName=TestContext.Parameters["browserName"];
            if (browserName == null)
            {
                browserName = ConfigurationManager.AppSettings["browser"];
            }
            
            String url=ConfigurationManager.AppSettings["url"];
            InitBrowser(browserName);
            driver.Value. Manage().Window.Maximize();
            driver.Value. Navigate().GoToUrl(url);
            driver.Value.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        }

        public IWebDriver GetDriver()
        {
            return driver.Value;
        }

        public static JsonReader GetJsonParser()
        {
            return new JsonReader();
        }
        public void InitBrowser(String browser)
        {
            /*
             * Creating the object of the below mentioned browser drivers through switch case is called Factory Design Pattern whereby passing
             * just the browser as an argument to the method InitBrowser, we're able to create the object of chrome, edge and firefox drivers
             */
            switch (browser.ToLower())
            {
                case "chrome":
                    driver.Value = new ChromeDriver();
                    break;
                case "edge":
                    driver.Value = new EdgeDriver();
                    break;
                case "firefox":
                    driver.Value = new FirefoxDriver();
                    break;
                default:
                    TestContext.Progress.WriteLine("Given "+browser+" is not one of the browser present. Please provide one of Chrome, Firefox and Edge as the browsers");
                    break;
            }

        }

        [TearDown]
        public void Teardown()
        {
            /*
             * To catch the test result, that can be written inside the TearDown
             */
            var status=TestContext.CurrentContext.Result.Outcome.Status;
            var logs = TestContext.CurrentContext.Result.StackTrace; //To generate the logs in case a test is failed
            DateTime time = DateTime.Now;
            String fileName = "Screenshot_" + time.ToString("h_mm_ss") + ".png";
            if (status == TestStatus.Failed)
            {
                /*
                 * Whenever a script is failing, we can attach a screenshot with the test. We can also provide the screenshot with timestamp and file name
                 */
               
                test.Fail("Test failed", CaptureScreenShot(driver.Value,fileName));
                test.Log(Status.Fail,"Test failed with logtrace "+logs);
            }
            else if (status == TestStatus.Passed)
            {
                test.Pass("Test is passed");
            }
            extent.Flush();
            driver.Value.Close();
        }

        public MediaEntityModelProvider CaptureScreenShot(IWebDriver webDriver,String screenShotName)
        {
            ITakesScreenshot takesScreenshot = (ITakesScreenshot)webDriver;
           var screenShot= takesScreenshot.GetScreenshot().AsBase64EncodedString;
            /*
             * To convert the Base64EncodedString to MediaEntity, we can make use of MediaEntityBuilder
             */
           return MediaEntityBuilder.CreateScreenCaptureFromBase64String(screenShot,screenShotName).Build();

        }
    }
}