namespace taskt.Core
{
    public class MyURLs
    {
        public const string GitProjectURL = "https://github.com/saucepleez/taskt";
        public const string GitReleaseURL = "https://github.com/saucepleez/taskt/releases";
        public const string GitIssueURL = "https://github.com/saucepleez/taskt/issues/new";
        public const string WikiURL = "https://github.com/saucepleez/taskt-wiki/blob/master/home.md";
        public const string OfficialSiteURL = "http://www.taskt.net/";
        public const string GitterURL = "https://gitter.im/taskt-rpa/Lobby";
        public const string WikiBaseURL = "https://github.com/saucepleez/taskt-wiki/blob/master/";

        public const string LatestJSONURL = "http://www.taskt.net/updates/latest.json";

        public const string ChromeDriverURL = "https://chromedriver.chromium.org/downloads";
        public const string EdgeDriverURL = "https://developer.microsoft.com/en-us/microsoft-edge/tools/webdriver/";
        public const string GeckoDriverURL = "https://github.com/mozilla/geckodriver/releases/";
        public const string IEDriverURL = "https://www.selenium.dev/downloads/";

        // format documents
        public const string NumberFormatURL1 = "https://docs.microsoft.com/en-us/dotnet/standard/base-types/standard-numeric-format-strings";
        public const string NumberFormatURL2 = "https://docs.microsoft.com/en-us/dotnet/standard/base-types/custom-numeric-format-strings";

        public const string DateTimeFormatURL1 = "https://docs.microsoft.com/en-us/dotnet/standard/base-types/standard-date-and-time-format-strings";
        public const string DateTimeFormatURL2 = "https://docs.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings";


        public static string GetWikiURL(string commandName, string groupName)
        {
            string page = commandName.ToLower().Replace(" ", "-").Replace("/", "-") + "-command.md";
            string parent = groupName.ToLower().Replace(" ", "-").Replace("/", "-");
            return WikiBaseURL + parent + "/" + page;
        }
    }
}
