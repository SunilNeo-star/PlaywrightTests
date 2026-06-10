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
        // Wait for page to fully load first
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Accept cookie popup if it appears — wait up to 5 seconds for it
        var acceptButton = _page.GetByRole(AriaRole.Button,
            new() { Name = "Accept additional cookies" });
        try
        {
            await acceptButton.WaitForAsync(new() { Timeout = 5000 });
            await acceptButton.ClickAsync();
        }
        catch
        {
            // Popup didn't appear — that's fine, continue
        }

        await SearchButton.ClickAsync();
        await SearchBox.WaitForAsync();
        await SearchBox.FillAsync(query);
        await SearchBox.PressAsync("Enter");
    }

    // --- ASSERTIONS (public — expose meaningful checks) ---

    public async Task ShouldBeOnBBCAsync()
    {
        await Assertions.Expect(_page).ToHaveTitleAsync(
            new System.Text.RegularExpressions.Regex("BBC"));
    }
}


