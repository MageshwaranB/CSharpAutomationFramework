using NUnit.Framework;
using OpenQA.Selenium;
using PracticeAutomationFramework.PageObjects;
using PracticeAutomationFramework.Utilites;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace PracticeAutomationFramework.Tests
{
    class ProductPageTest : BaseClass
    {
        [Test, Category("Regression")]
        public void SuccessfullLandingOnProductPageTest()
        {
            String[] expectedProd = { "Sauce Labs Bike Light", "Sauce Labs Bolt T-Shirt" };
            LoginPage loginPage = new LoginPage(GetDriver());
            String[] actualProductsSelected= loginPage.SimpleLoginWithMethodChaining(ConfigurationManager.AppSettings["username"], ConfigurationManager.AppSettings["password"])
                      .NavigateToProductPage(new ProductPage(GetDriver()))
                      .AddTheProductsToTheCart(expectedProd,GetDriver());
            Assert.AreEqual(expectedProd,actualProductsSelected);
        }

        
    }
}
