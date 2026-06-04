using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using Microsoft.Testing.Platform.Logging;
using NUnit.Framework;
using NUnit.Framework.Internal;
using PlaywrightSearchTestProject.Pages;
using static System.Net.WebRequestMethods;


namespace PlaywrightSearchTestProject.Tests;

public class BBCSearchTests : PageTest
{
    private BBCHomePage _bBCHomePage = null!;
    private SearchResultsPage _resultsPage = null!;

    // [SetUp] runs BEFORE each test
    [SetUp]
    public void SetUp()
    {
        _bBCHomePage = new BBCHomePage(Page);
        _resultsPage = new SearchResultsPage(Page);
    }

    [Test]
    public async Task SearchingForPlaywrightShowsResults()
    {
        // Arrange — set up the starting state
        await _bBCHomePage.GoToAsync();
        await _bBCHomePage.ShouldBeOnBBCAsync();
            
        // Act — do the thing you're testing
        await _bBCHomePage.SearchAsync("Playwright C#");

        // Assert — verify the outcome
        var resultCount = await _resultsPage.GetResultCountAsync();
        Assert.That(resultCount, Is.GreaterThan(0), "Expected search results to appear");
    }

    [Test]
    public async Task EmptySearchStaysOnBBCHome()
    {
        // Accept terms if the popup appears
        var acceptButton = Page.GetByRole(AriaRole.Button, new() { Name = "Accept all" });
        if (await acceptButton.IsVisibleAsync())
        {
            await acceptButton.ClickAsync();
        }

        await _bBCHomePage.GoToAsync();
        await _bBCHomePage.ShouldBeOnBBCAsync();

        // Just verify title without searching
        await _bBCHomePage.ShouldBeOnBBCAsync();
    }
    // Add this to any PageTest class to control browser behavior
    public override BrowserNewContextOptions ContextOptions()
    {
        return new BrowserNewContextOptions
        {
            // Sets viewport size (browser window dimensions)
            ViewportSize = new ViewportSize { Width = 1920, Height = 1080 },

            // Record a video of every test (saved to test-results/)
            // RecordVideoDir = "test-results/videos/"
        };

    }
}


/* 
 #Run headless (no browser window) — default, good for CI
 dotnet test

# Run with visible browser
#$env:HEADED=1; dotnet test

# Run against a different URL
#$env:TEST_BASE_URL="https://staging.example.com"; dotnet test

# Slow everything down by 500ms to watch what happens
#$env:SLOWMO=500; dotnet test*/

/* // Tells Playwright whether to show the browser and how fast to run
    public override Task<IBrowser> CreateBrowserAsync(
        IPlaywright playwright, BrowserNewContextOptions options)
    {
        return playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = TestSettings.Headless,  // show or hide browser
            SlowMo = TestSettings.SlowMo       // slow down each action
        });
    }
}*/

/*Debugging tip: If a test fails and you don't know why, add this line — it saves a screenshot at that exact moment:
csharp
    
await Page.ScreenshotAsync(new () { Path = "debug-screenshot.png" });
*/
/* Useful commands to remember:
bashdotnet test                          # run all tests
dotnet test --filter "TestName"      # run one specific test
dotnet test --logger "html"          # generate HTML report
$env:HEADED=1; dotnet test           # show the browser while running
*/