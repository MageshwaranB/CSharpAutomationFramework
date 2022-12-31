using NUnit.Framework;
using PracticeAutomationFramework.PageObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace PracticeAutomationFramework.Tests
{
    [Parallelizable(ParallelScope.Children)]
    class LoginTest : BaseClass
    {
        /*
         * We can run all data sets parallely
         * We can run all tests in a class parallely
         * We can run all tests in a project parallely
         * To acheive this, we can make use of parallelizable
         * To run all data sets parallely, use the scope as All- However, if we run it just like that, all the tests takes the same driver object which in turn
         * results data been send randomly to the testcases. To overcome this issue, we have to make the driver threadsafe
         * To run all tests in a class parallely - add ParallelScope.Children
         */


        [Test]
        [TestCaseSource("ProvideTestData")]
       [Parallelizable(ParallelScope.All)]
        public void SimpleLoginTest(String userName, String pWord)
        {
            LoginPage loginPage = new LoginPage(GetDriver());
            loginPage.ValidLogin(userName, pWord);
        }

        public static IEnumerable<TestCaseData> ProvideTestData()
        {
           yield return new TestCaseData(GetJsonParser().ExtractData("username"), GetJsonParser().ExtractData("password"));
           yield return new TestCaseData(GetJsonParser().ExtractData("username_wrong"), GetJsonParser().ExtractData("password"));
        }

        /*
         * If we want to run selective test cases, we should tag them with category attribute.
         * Test can be run and triggered in command line as well
         * To run the project on terminal. right click on the project -> open the project in terminal
         * Provide the below command in the terminal
         * dotnet test "name of the project and with .csproj extension"
         * To run tests category wise
         *  dotnet test "name of the project and with .csproj extension" --filter TestCategory=Name of the category
         */

        [Test, Category("Smoke")]
        public void AvailableUsernameToTest()
        {
          
            LoginPage login = new LoginPage(GetDriver());
            var allTheNames=login.GetAcceptableUsernameList();
            
        }

    }
}
