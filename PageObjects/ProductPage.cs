using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using PracticeAutomationFramework.Utilites;
using SeleniumExtras.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace PracticeAutomationFramework.PageObjects
{
    class ProductPage : BaseClass
    {
        private IWebDriver driver;
        By productTitle = By.ClassName("inventory_item_name");
        By addToCartBtn = By.CssSelector("button[class='btn btn_primary btn_small btn_inventory']");
        public ProductPage(IWebDriver driver)
        {
            this.driver = driver;
            PageFactory.InitElements(driver,this);
        }

        [FindsBy(How = How.XPath, Using = "//div[@class='inventory_item']")]
        IList<IWebElement> allAvailableProduct;

        [FindsBy(How = How.ClassName, Using = "title")]
        IWebElement productPageTitle;

        public void WaitForProductsPageToDisplay()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(6));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.ClassName("title")));
        }

        public IList<IWebElement> GetAllTheProducts()
        {
            return allAvailableProduct;
        }

        public By GetProductName()
        {
            return productTitle;
        }

        public By GetAddToCartElement()
        {
            return addToCartBtn;
        }

        public String[] AddTheProductsToTheCart(String[] expectedProd, IWebDriver driver)
        {
            int count = 0;
            String[] actualProductsSelected=new string[expectedProd.Length];
            foreach (var currentProduct in GetAllTheProducts())
            {
                BasicUtility.ScrollToTheElement(currentProduct, driver);
                if (expectedProd.Contains(currentProduct.FindElement(productTitle).Text))
                {
                    String currentProductName=currentProduct.FindElement(GetProductName()).Text;
                    TestContext.Progress.WriteLine(currentProductName);
                    Thread.Sleep(5000);
                    currentProduct.FindElement(GetAddToCartElement()).Click();
                    Thread.Sleep(5000);
                    actualProductsSelected[count]=currentProductName;
                    count++;
                }
            }
            return actualProductsSelected;
        }
    }
}
