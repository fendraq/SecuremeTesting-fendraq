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
      Headless = true,
      //SlowMo = 1000
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

  // Scenario: Adding a new customer case with valid data
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
    await _page.Locator("div[role='combobox']").ClickAsync();
    await _page.GetByRole(AriaRole.Option, new() { Name = "shipping" }).ClickAsync();
  }

  [When(@"I type ""(.*)"" in the E-postadress field")]
  public async Task WhenITypeInTheEPostadressField(string p0)
  {
    await _page.FillAsync("input[name='customer_email']", p0);
  }

  [When(@"I type ""(.*)"" in the Förnamn field")]
  public async Task WhenITypeInTheFornamnField(string peter)
  {
    await _page.FillAsync("input[name='customer_first_name']", peter);
  }

  [When(@"I type ""(.*)"" in the Efternamn field")]
  public async Task WhenITypeInTheEfternamnField(string svensson)
  {
    await _page.FillAsync("input[name='customer_last_name']", svensson);
  }

  [When(@"I type ""(.*)"" in the Rubrik field")]
  public async Task WhenITypeInTheRubrikField(string leveransproblem)
  {
    await _page.FillAsync("input[name='title']", leveransproblem);
  }

  [When(@"I type ""(.*)"" in Beskriv ditt ärende field and click send")]
  public async Task WhenITypeInBeskrivDittArendeField(string p0)
  {
    await _page.FillAsync("textarea[name='case_message']", p0);
  }
  
// https://playwright.dev/docs/dialogs
  [Then(@"I will get an alert saying ""(.*)""")]
  public async Task ThenIWillGetAnAlertSaying(string expectedMessage)
  {
    var tcs = new TaskCompletionSource<string>();
    
    _page.Dialog += async (_, dialog) =>
    {
      await dialog.AcceptAsync();
      tcs.SetResult(dialog.Message);
    };
    
    await _page.GetByText("Send").ClickAsync();

    var dialogMessage = await tcs.Task;
    
    Assert.Equal(expectedMessage, dialogMessage);
  }
  
  // Scenario: Filling a new customer case with wrong email format

  [When(@"I type an invalid email ""(.*)"" in the E-postadress field")]
  public async Task WhenITypeAnInvalidEmailInTheEPostadressField(string p0)
  {
    await _page.FillAsync("input[name='customer_email']", p0);
  }
// https://playwright.dev/docs/dialogs
  [Then(@"I will get an alert error saying ""(.*)""")]
  public async Task ThenIWillGetAnAlertSayingEmailIsInTheWrongFormat(string expectedMessage)
  {
    var tcs = new TaskCompletionSource<string>();
    
    _page.Dialog += async (_, dialog) =>
    {
      await dialog.AcceptAsync();
      tcs.SetResult(dialog.Message);
    };
    
    await _page.GetByText("Send").ClickAsync();

    var dialogMessage = await tcs.Task;
    
    Assert.Equal(expectedMessage, dialogMessage);
  }

  [Then(@"I will se an alert error saying ""(.*)""")]
  public async Task ThenIWillSeAnAlertErrorSaying(string expectedMessage)
  {
    var tcs = new TaskCompletionSource<string>();
    
    _page.Dialog += async (_, dialog) =>
    {
      await dialog.AcceptAsync();
      tcs.SetResult(dialog.Message);
    };
    
    await _page.GetByText("Send").ClickAsync();

    var dialogMessage = await tcs.Task;
    
    Assert.Equal(expectedMessage, dialogMessage);
  }
}