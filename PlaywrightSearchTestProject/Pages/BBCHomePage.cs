using Microsoft.Playwright;

namespace PlaywrightSearchTestProject.Pages;

// This class represents the Google homepage
// It knows how to interact with all the elements on that page
public class BBCHomePage
{
    private readonly IPage _page;  // the browser tab

    // Constructor — give this class a page to work with
    public BBCHomePage(IPage page)
    {
        _page = page;
    }

    // --- LOCATORS (private — callers don't need to know HOW we find things) ---
    // The search button in the navbar — clicking this opens the search popup
    //private ILocator SearchButton => _page.GetByRole(AriaRole.Button, new() { Name = "Search" });
    private ILocator SearchButton => _page.GetByRole(AriaRole.Link, new() { Name = "Search BBC" });

    // The search box inside the popup — only appears AFTER clicking the button
    private ILocator SearchBox => _page.GetByPlaceholder("Search the BBC");

    // --- ACTIONS (public — these are what tests call) ---

    public async Task GoToAsync()
    {
        await _page.GotoAsync(TestSettings.BaseUrl);
    }

    public async Task SearchAsync(string query)
    {
        // Accept terms if the popup appears
        var acceptButton = _page.GetByRole(AriaRole.Button, new() { Name = "Accept additional cookies" });
        if (await acceptButton.IsVisibleAsync())
        {
            await acceptButton.ClickAsync();
        }
        await SearchButton.ClickAsync();           // click to open the popup
        await SearchBox.WaitForAsync();            // wait for popup to appear
        await SearchBox.FillAsync(query);          // type in the search box
        await SearchBox.PressAsync("Enter");       // press Enter
    }

    // --- ASSERTIONS (public — expose meaningful checks) ---

    public async Task ShouldBeOnBBCAsync()
    {
        await Assertions.Expect(_page).ToHaveTitleAsync(
            new System.Text.RegularExpressions.Regex("BBC"));
    }
}


