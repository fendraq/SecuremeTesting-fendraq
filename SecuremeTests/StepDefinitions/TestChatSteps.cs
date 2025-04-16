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
    //Vänta in DOM
    await _page.WaitForSelectorAsync(".chat-message", new PageWaitForSelectorOptions { Timeout = 5000 });
    await _page.WaitForSelectorAsync(".chat-message-timestamp", new PageWaitForSelectorOptions { Timeout = 5000 });
    
    //Spara data
    var messages = await _page.QuerySelectorAllAsync(".chat-message");
    messages.Should().NotBeEmpty("Expected at least one chat message to be displayed.");
    var lastMessage = messages.Last();
    var messageText = await lastMessage.InnerTextAsync();
    
    var timestamps = await _page.QuerySelectorAllAsync(".chat-message-timestamp");
    timestamps.Should().NotBeEmpty("Expected at least one timestamp to be displayed.");
    var lastTimestamp = timestamps.Last();
    var timestampText = await lastTimestamp.InnerTextAsync();
    
    //Assert
    messageText.Should().Be("I would like to get some help now!");
    timestampText.Should().NotBeNullOrWhiteSpace();
  }

  [Given(@"I am on the My Cases view")]
  public async Task GivenIAmOnTheMyCasesView()
  {
    await _page.GotoAsync("http://localhost:3000/my-case");
  }

  [Given(@"I see the list with my cases")]
  public async Task GivenISeeTheListWithMyCases()
  {
    // Vänta på elementet
    await _page.WaitForSelectorAsync("main > h3", new PageWaitForSelectorOptions { Timeout = 5000 });

    // Välj element
    var myCasesHeading = await _page.QuerySelectorAsync("main > h3");

    // kolla så att elementet finns
    myCasesHeading.Should().NotBeNull("Expected an <h3> element inside <main>.");

    // plocka ut rubriken
    var myCasesHeadingText = await myCasesHeading.InnerTextAsync();

    // Assert
    myCasesHeadingText.Should().Be("My Case");
    Console.WriteLine($"Main heading text: {myCasesHeadingText}");
  }
  
  [When(@"I click on the case with title ""(.*)""")]
  public async Task WhenIClickOnTheCaseWithTitle(string p0)
  {
    await _page.GetByText(p0).ClickAsync();
  }

  [Then(@"I will se the same chat as the customer ""(.*)"" with the message ""(.*)"" sent")]
  public async Task ThenIWillSeTheSameChatAsTheCustomerWithTheMessageSent(string peter, string p1)
  {
    //Vänta in DOM
    await _page.WaitForSelectorAsync(".chat-message", new PageWaitForSelectorOptions { Timeout = 5000 });
    await _page.WaitForSelectorAsync(".chat-message-timestamp", new PageWaitForSelectorOptions { Timeout = 5000 });

    var messages = await _page.QuerySelectorAllAsync(".chat-message");
    var lastMessage = messages.Last();
    var messageText = await lastMessage.InnerTextAsync();

    messageText.Should().Be("I would like to get some help now!");
    
    var timestamps = await _page.QuerySelectorAllAsync(".chat-message-timestamp");
    var lastTimestamp = timestamps.Last();
    var timestampText = await lastTimestamp.InnerTextAsync();
    
    timestampText.Should().NotBeNullOrWhiteSpace();
    
    
  }
}