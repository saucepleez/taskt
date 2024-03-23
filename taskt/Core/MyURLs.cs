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

        public const string LatestJSONURL = "https://raw.githubusercontent.com/saucepleez/taskt/development-branch/taskt/latest.json";

        public const string ChromeDriverURL = "https://googlechromelabs.github.io/chrome-for-testing/";
        public const string EdgeDriverURL = "https://developer.microsoft.com/en-us/microsoft-edge/tools/webdriver/";
        public const string GeckoDriverURL = "https://github.com/mozilla/geckodriver/releases/";
        public const string IEDriverURL = "https://www.selenium.dev/downloads/";

        // format documents
        public const string NumberFormatURL1 = "https://docs.microsoft.com/en-us/dotnet/standard/base-types/standard-numeric-format-strings";
        public const string NumberFormatURL2 = "https://docs.microsoft.com/en-us/dotnet/standard/base-types/custom-numeric-format-strings";

        public const string DateTimeFormatURL1 = "https://docs.microsoft.com/en-us/dotnet/standard/base-types/standard-date-and-time-format-strings";
        public const string DateTimeFormatURL2 = "https://docs.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings";

        // NuGets, etc
        public const string BouncyCastleURL = "https://www.bouncycastle.org/csharp/";
        public const string HTMLAgilityPackURL = "https://www.nuget.org/packages/HtmlAgilityPack/";
        public const string IMAPXURL = "https://github.com/azanov/imapx";
        public const string JetBrainAnnotationsURL = "https://www.nuget.org/packages/JetBrains.Annotations/";
        public const string Log4NetURL = "";    //?
        public const string MailKitURL = "http://www.mimekit.net/";
        public const string MarkddigURL = "https://github.com/xoofx/markdig";
        public const string MimeKitURL = "http://www.mimekit.net/";
        public const string NewtonSoftJSONURL = "https://www.nuget.org/packages/newtonsoft.json/";
        public const string OneNoteOCRURL = "https://github.com/ignatandrei/OneNoteOCR";
        public const string RestSharpURL = "https://restsharp.dev/";
        public const string SeleniumURL = "https://github.com/SeleniumHQ/selenium";
        public const string SelilogURL = "https://serilog.net/";
        public const string SelilogFormattingCompactURL = "https://github.com/serilog/serilog-formatting-compact";
        public const string SelilogSinksFileURL = "https://serilog.net/";
        public const string SharpCompressURL = "https://github.com/adamhathcock/sharpcompress";
        public const string SharpSimpleNLGURL = "https://github.com/nickhodge/SharpSimpleNLG";
        public const string SuperSocketClientEngineURL = "https://github.com/kerryjiang/SuperSocket.ClientEngine";
        public const string TaskSchedulerURL = "https://github.com/dahall/TaskScheduler";
        public const string WebSocket4NetURL = "https://github.com/kerryjiang/WebSocket4Net";
        public const string ZstdSharpPortURL = "https://github.com/oleg-st/ZstdSharp";

        public static string GetWikiURL(string commandName, string groupName)
        {
            string page = commandName.ToLower().Replace(" ", "-").Replace("/", "-") + "-command.md";
            string parent = groupName.ToLower().Replace(" ", "-").Replace("/", "-");
            return WikiBaseURL + parent + "/" + page;
        }
    }
}
