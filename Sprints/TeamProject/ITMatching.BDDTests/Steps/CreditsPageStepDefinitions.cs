using System;
using ITMatching.Models;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using System.Text;
using System.Threading;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace ITMatching.BDDTests.Steps
{
    [Binding]
    public class CreditsPageStepDefinitions
    {
        private readonly ScenarioContext _ctx;
        private string _hostBaseName = @"https://it-matching.azurewebsites.net/";
        private string _creditPage;
        private IWebDriver _driver = new ChromeDriver();

        public CreditsPageStepDefinitions(ScenarioContext scenarioContext)
        {
            _creditPage = $"{_hostBaseName}Home/Credits";
            _ctx = scenarioContext;
            _ctx["WebDriver"] = _driver;
        }

        [Given(@"Currently, I am on the home page")]
        public void GivenCurrentlyIAmOnTheHomePage()
        {
            IWebDriver driver = (IWebDriver)_ctx["WebDriver"];
            driver.Navigate().GoToUrl(_hostBaseName);
        }

        [Given(@"Currently, I am on the credits page")]
        public void GivenCurrentlyIAmOnTheCreditsPage()
        {
            IWebDriver driver = (IWebDriver)_ctx["WebDriver"];
            driver.Navigate().GoToUrl(_creditPage);
        }

        [When(@"I click on the credits page link")]
        public void WhenIClickOnTheCreditsPageLink()
        {
            IWebDriver driver = (IWebDriver)_ctx["WebDriver"];
            driver.FindElement(By.LinkText("Credits")).Click();
        }

        [When(@"I click on the next arrow button")]
        public void WhenIClickOnTheNextArrowButton()
        {
            IWebDriver driver = (IWebDriver)_ctx["WebDriver"];
            string position = driver.FindElement(By.CssSelector("div.slider-container div.left-slide")).GetCssValue("transform");
            _ctx["position"] = position;
            driver.FindElement(By.ClassName("up-button")).Click();
        }

        [When(@"I click on the prev arrow button")]
        public void WhenIClickOnThePrevArrowButton()
        {
            IWebDriver driver = (IWebDriver)_ctx["WebDriver"];
            string position = driver.FindElement(By.CssSelector("div.slider-container div.left-slide")).GetCssValue("transform");
            _ctx["position"] = position;
            driver.FindElement(By.ClassName("down-button")).Click();
        }

        [Then(@"display the credits page")]
        public void ThenDisplayTheCreditsPage()
        {
            IWebDriver driver = (IWebDriver)_ctx["WebDriver"];
            Assert.That(driver.Url, Is.EqualTo(_creditPage));
        }

        [Then(@"change the photo, title and description")]
        public void ThenChangeThePhotoTitleAndDescription()
        {
            IWebDriver driver = (IWebDriver)_ctx["WebDriver"];
            string position = driver.FindElement(By.CssSelector("div.slider-container div.left-slide")).GetCssValue("transform");
            Assert.That(_ctx["position"], Is.Not.EqualTo(position));
        }
    }
}
