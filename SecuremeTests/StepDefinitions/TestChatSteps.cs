using Microsoft.Playwright;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using FluentAssertions;
using Xunit;

namespace SecuremeTests.StepDefinitions;

[Binding]
public class TestChatSteps
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
  
  [Given(@"I have received a mail with a chat link")]
  public async Task GivenIHaveRecievedAMailWithAChatLink()
  {
    await _page.GotoAsync("http://localhost:3000");
  }
  
  [When(@"I click on the link with token ""(.*)""")]
  public async Task WhenIClickOnTheLinkWithToken(string p0)
  {
    await _page.GotoAsync("http://localhost:3000/chat-page/9ce82c4e-d015-488f-b305-69a9ec22c3d0");
  }

  [Then(@"I am redirected to a unique chat")]
  public async Task ThenIAmRedirectedToAUniqueChat()
  {
    var headingText = await _page.InnerTextAsync("h1");
    headingText.Should().Be("Chat");
  }

  [Given(@"I am on the customer chat-page")]
  public async Task GivenIAmOnTheCustomerChatPage()
  {
    await _page.GotoAsync("http://localhost:3000/chat-page/9ce82c4e-d015-488f-b305-69a9ec22c3d0");
  }

  [When(@"I input a message in the textarea")]
  public async Task WhenIInputAMessageInTheTextarea(string multilineText)
  {
    await _page.FillAsync("textarea", multilineText);
  }

  [When(@"click the button ""(.*)""")]
  public async Task WhenClickTheButton(string p0)
  {
    await _page.GetByRole(AriaRole.Button, new() { Name = p0 }).ClickAsync();
  }

  [When(@"refresh the page")]
  public async Task WhenRefreshThePage()
  {
    await _page.GotoAsync("http://localhost:3000/chat-page/9ce82c4e-d015-488f-b305-69a9ec22c3d0");
  } 
  
  [Then(@"I will see the message with a timestamp in the chat window")]
  public async Task ThenIWillSeeTheMessageWithATimestampInTheChatWindow()
  {
    
    var messages = await _page.QuerySelectorAllAsync(".chat-message");
    var lastMessage = messages.Last();
    var messageText = await lastMessage.InnerTextAsync();

    messageText.Should().Be("I would like to get some help now!");

    
    /*var timestamps = await _page.QuerySelectorAllAsync(".chat-message-timestamp");
    var lastTimestamp = timestamps.Last();
    var timestampText = await lastTimestamp.InnerTextAsync();
    
    timestampText.Should().NotBeNullOrWhiteSpace();*/
  }

  [Given(@"I am on the My Cases view")]
  public async Task GivenIAmOnTheMyCasesView()
  {
    await _page.GotoAsync("http://localhost:3000/my-case");
  }

  [Given(@"I see the list with my cases")]
  public async Task GivenISeeTheListWithMyCases()
  {
    bool visible = await _page.IsVisibleAsync("name='User Cases");
    visible.Should().BeTrue();
  }
}