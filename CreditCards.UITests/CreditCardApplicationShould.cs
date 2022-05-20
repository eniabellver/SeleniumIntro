using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Support;
using Xunit;
using Xunit.Abstractions;

namespace CreditCards.UITests
{
    [Trait("Category", "Application")]
    public class CreditCardApplicationShould
    {
        private const string HomeUrl = "http://localhost:44108/";
        private const string ApplyUrl = "http://localhost:44108/Apply";

        private readonly ITestOutputHelper output;

        public CreditCardApplicationShould(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void BeInitiatedFromHomePage_NewLowRate()
        {
            using (IWebDriver driver = new ChromeDriver())
            {
                driver.Navigate().GoToUrl(HomeUrl);

                IWebElement applyButton = driver.FindElement(By.Name("ApplyLowRate"));
                applyButton.Click();

                Assert.Equal("Credit Card Application - Credit Cards", driver.Title);
                Assert.Equal(ApplyUrl, driver.Url);
            }
        }

        [Fact]
        public void BeInitiatedFromHomePage_EasyApplication()
        {
            using (IWebDriver driver = new ChromeDriver())
            {
                driver.Navigate().GoToUrl(HomeUrl);

                IWebElement carouselNextButton = driver.FindElement(By.CssSelector("[data-slide='next']"));
                carouselNextButton.Click();

                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(1));

                IWebElement applyButton = wait.Until((d) => d.FindElement(By.LinkText("Easy: Apply Now!")));
                applyButton.Click();

                //DemoHelper.Pause(1000); //allow carousel animation to load
                //IWebElement applyButton = driver.FindElement(By.LinkText("Easy: Apply Now!"));
                //applyButton.Click();

                Assert.Equal("Credit Card Application - Credit Cards", driver.Title);
                Assert.Equal(ApplyUrl, driver.Url);
            }
        }

        [Fact]
        public void BeInitiatedFromHomePage_CustomerService()
        {
            using (IWebDriver driver = new ChromeDriver())
            {
                

                output.WriteLine($"{DateTime.Now.ToLongTimeString()} Navigating to '{HomeUrl}'");
                driver.Navigate().GoToUrl(HomeUrl);
               
                output.WriteLine($"{DateTime.Now.ToLongTimeString()} Finding element using explicit wait");
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(35));

                Func<IWebDriver, IWebElement> findEnabledAndVisible = delegate (IWebDriver d)
                {
                    var e = d.FindElement(By.ClassName("customer-service-apply-now"));

                    if (e is null)
                    {
                        throw new NotFoundException();
                    }

                    if (e.Enabled && e.Displayed)
                    {
                        return e;
                    }

                    throw new NotFoundException();
                };

                IWebElement applyButton = wait.Until(findEnabledAndVisible);

                output.WriteLine($"{DateTime.Now.ToLongTimeString()} Found element displayed={applyButton.Displayed} Enabled={applyButton.Enabled}");
                output.WriteLine($"{DateTime.Now.ToLongTimeString()} Clicking element");
                applyButton.Click();

                Assert.Equal("Credit Card Application - Credit Cards", driver.Title);
                Assert.Equal(ApplyUrl, driver.Url);
            }
        }

        [Fact]
        public void BeInitiatedFromHomePage_RandomGreeting()
        {
            using (IWebDriver driver = new ChromeDriver())
            {
                driver.Navigate().GoToUrl(HomeUrl);

                IWebElement randomGreetingApplyLink = driver.FindElement(By.PartialLinkText("- Apply Now!"));
                randomGreetingApplyLink.Click();

                Assert.Equal("Credit Card Application - Credit Cards", driver.Title);
                Assert.Equal(ApplyUrl, driver.Url);
            }
        }

        [Fact]
        public void BeInitiatedFromHomePage_RandomGreeting_UsingXPath()
        {
            using (IWebDriver driver = new ChromeDriver())
            {
                driver.Navigate().GoToUrl(HomeUrl);

                IWebElement randomGreetingApplyLink
                    = driver.FindElement(By.XPath("//a[text()[contains(.,'- Apply Now!')]]"));
                randomGreetingApplyLink.Click();

                Assert.Equal("Credit Card Application - Credit Cards", driver.Title);
                Assert.Equal(ApplyUrl, driver.Url);
            }
        }
    }
}
