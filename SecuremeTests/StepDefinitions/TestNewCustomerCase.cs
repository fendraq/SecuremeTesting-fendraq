using Microsoft.Playwright;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using FluentAssertions;
using Xunit;

namespace SecuremeTests.StepDefinitions;

[Binding]
public class TestNewCustomerCase
{
  private IPage _page;
  private IBrowser _browser;
  private IBrowserContext _context;

  [BeforeScenario]
  public async Task Setup()
  {
    var playwright = await Playwright.CreateAsync();
    _browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
    {
      Headless = false,
      SlowMo = 1000
    });
    _context = await _browser.NewContextAsync();
    _page = await _context.NewPageAsync();
  }

  [AfterScenario]
  public async Task TearDown()
  {
    await _context.CloseAsync();
    await _browser.CloseAsync();
  }


  [Given(@"I am on the Webshop page")]
  public async Task GivenIAmOnTheWebshopPage()
  {
    await _page.GotoAsync("http://localhost:5173");
  }

  [When(@"I click on the Add new case-icon")]
  public async Task WhenIClickOnTheIcon()
  {
    await _page.GetByTestId("CommentIcon").ClickAsync();
  }

  [When(@"I see a form with input fields")]
  public async Task WhenISeeAFormWithInputFields()
  {
    await _page.WaitForSelectorAsync("[role='dialog']");
  }

  [When(@"I choose Shipping category")]
  public async Task WhenIChooseShippingCategory()
  {
    await _page.GetByText("Category").ClickAsync();
    await _page.GetByText("Shipping").ClickAsync();
  }
  // alternativ: 
  // 1. Click the dropdown to expand it
  //await _page.Locator("div[role='combobox']").ClickAsync();

// 2. Click the item inside the dropdown menu
  // await _page.GetByRole(AriaRole.Option, new() { Name = "Shipping" }).ClickAsync();


  [When(@"I type ""(.*)"" in the E-postadress field")]
  public async Task WhenITypeInTheEPostadressField(string p0)
  {
    await _page.FillAsync("input[name='E-postadress']", p0);
  }

  [When(@"I type ""(.*)"" in the Förnamn field")]
  public async Task WhenITypeInTheFornamnField(string peter)
  {
    await _page.FillAsync("input[name='Förnamn']", peter);
  }

  [When(@"I type ""(.*)"" in the Efternamn field")]
  public async Task WhenITypeInTheEfternamnField(string svensson)
  {
    await _page.FillAsync("input[name='Efternamn']", svensson);
  }

  [When(@"I type ""(.*)"" in the Rubrik field")]
  public async Task WhenITypeInTheRubrikField(string leveransproblem)
  {
    await _page.FillAsync("input[name='Rubrik']", leveransproblem);
  }

  [When(@"I type ""(.*)"" in Beskriv ditt ärende field")]
  public async Task WhenITypeInBeskrivDittArendeField(string p0)
  {
    await _page.FillAsync("input[text=Beskriv]", p0);
  }

  [When(@"I click on the Send button")]
  public async Task WhenIClickOnTheSendButton()
  {
    await _page.ClickAsync("[name='Send']");
  }

  [Then(@"I will get an alert saying ""(.*)""")]
  public async Task ThenIWillGetAnAlertSaying(string p0)
  {
    _page.Dialog += async (_, dialog) =>
    {
      Console.WriteLine($"Alert text: {dialog.Message}");
      await dialog.AcceptAsync();
    };
  }
}