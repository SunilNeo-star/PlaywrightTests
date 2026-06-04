namespace PlaywrightSearchTestProject;

// Central place for test configuration
// In a real project, load these from environment variables or appsettings.json
public static class TestSettings
{
    public static string BaseUrl =>
        Environment.GetEnvironmentVariable("TEST_BASE_URL") ?? "https://www.bbc.co.uk/";

    public static bool Headless =>
        Environment.GetEnvironmentVariable("HEADED") != "1";

    public static int SlowMo =>
        int.Parse(Environment.GetEnvironmentVariable("SLOWMO") ?? "0");
}