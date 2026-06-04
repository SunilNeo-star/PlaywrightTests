using Microsoft.Playwright;

namespace PlaywrightSearchTestProject.Pages;

public class SearchResultsPage
{
    private readonly IPage _page;

    public SearchResultsPage(IPage page)
    {
        _page = page;
    }

    // Check that search results appeared
    public async Task ShouldShowResultsForAsync(string query)
    {
        await Assertions.Expect(_page).ToHaveURLAsync(
            new System.Text.RegularExpressions.Regex($".*q=.*{System.Uri.EscapeDataString(query).Substring(0, 5)}.*"));
    }

    // Count search result links
    public async Task<int> GetResultCountAsync()
    {
        var results = _page.GetByTestId("main-content").GetByRole(AriaRole.Link);
        return await results.CountAsync();
    }
}


