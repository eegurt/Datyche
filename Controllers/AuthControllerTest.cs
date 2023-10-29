using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Datyche.Controllers;

[TestFixture]
public class AuthLoginTest
{
    private IWebDriver driver;

    [SetUp]
    public void SetUp()
    {
        driver = new ChromeDriver();
    }

    [Test]
    public void LoginTest()
    {
        driver.Navigate().GoToUrl("https://localhost:5000/auth/login");

        IWebElement usernameInput = driver.FindElement(By.Id("Username"));
        usernameInput.SendKeys("Mark");

        IWebElement passwordInput = driver.FindElement(By.Id("Password"));
        passwordInput.SendKeys("Mark1234");

        IWebElement loginButton = driver.FindElement(By.CssSelector("input[type='submit']"));
        loginButton.Click();

        IWebElement welcomeHeader = driver.FindElement(By.TagName("h1"));
        Assert.IsTrue(welcomeHeader.Text.Contains("Mark"));
    }

    [TearDown]
    public void TearDown()
    {
        driver.Quit();
    }
}