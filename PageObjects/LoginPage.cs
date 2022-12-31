using NUnit.Framework;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace PracticeAutomationFramework.PageObjects
{
    class LoginPage : BaseClass
    {
        private IWebDriver driver;
        ProductPage productPage;
        public LoginPage(IWebDriver driver)
        {
            this.driver = driver;
            PageFactory.InitElements(driver,this);
        }


        // PageObject Factory
        [FindsBy(How = How.Id, Using = "user-name")]
        private IWebElement userNameTxtBox;

        [FindsBy(How = How.Id, Using = "password")]
        private IWebElement passwordTxtBox;

        [FindsBy(How = How.Id, Using = "login-button")]
        private IWebElement loginBtn;

        [FindsBy(How = How.ClassName, Using = "login_credentials")]
        private IWebElement allAcceptableUsernames;

        [FindsBy(How = How.XPath, Using = "//div[contains(@class,'error')]")]
        private IWebElement errorMessage;

        [FindsBy(How = How.ClassName, Using = "title")]
        private IWebElement validationCheck;

        /*
         * In Object Oriented Programming, it's better to go with the concept of encapsulation for initiating the web elements
         * The above web elements are declared as private, so that their scope will be within this class only, and only way to 
         * access is through methods. This is done to make sure the webelements are not modified by some one knowingly or unknowingly
         * If it had public level access, then anyone can call this variable and reset its property
         * Hiding the details without exposing them is called encapsulation
         */

        public IWebElement GetUserNameElement ()
        {
            return userNameTxtBox;
        }

        public IWebElement GetPasswordElement()
        {
            return passwordTxtBox;
        }

        public IWebElement GetLoginBtnElement()
        {
            return loginBtn ;
        }

        public int ValidLogin(String username, String password)
        {
            int count=0;
            userNameTxtBox.SendKeys(username);
            passwordTxtBox.SendKeys(password);
            if (username.Equals("locked_out_user"))
            {
                loginBtn.Click();
                TestContext.Progress.WriteLine(errorMessage.Text);
            }
            else
            {
                loginBtn.Click();
                if (validationCheck.Displayed)
                {
                    count++;
                }
            }
            TestContext.Progress.WriteLine(count);
            return count;
        }

        public LoginPage SimpleLoginWithMethodChaining(String username, String password)
        {
            userNameTxtBox.SendKeys(username);
            passwordTxtBox.SendKeys(password);
            loginBtn.Click();
            return this;
        }
        
        public ProductPage NavigateToProductPage(ProductPage productPage)
        {
            this.productPage = productPage;
            return productPage;
        }

        public IWebElement GetUsernames()
        {
            return allAcceptableUsernames;
        }

        public IList<String> GetAcceptableUsernameList()
        {
            String genericData=allAcceptableUsernames.Text;
            String[] availableUsernameToTest=genericData.Split(":");
            TestContext.Progress.WriteLine("Available Usernames:");
            TestContext.Progress.WriteLine(availableUsernameToTest[1]);
           
            
             var usernames=availableUsernameToTest[1].Split("\n");
           
            IList<String> list = new List<String>(usernames);
            
            return list;
        }
    }

   

}
