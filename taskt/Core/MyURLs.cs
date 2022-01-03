using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public static string GetWikiURL(string commandName, string groupName)
        {
            string page = commandName.ToLower().Replace(" ", "-").Replace("/", "-") + "-command.md";
            string parent = groupName.ToLower().Replace(" ", "-").Replace("/", "-");
            return (Core.MyURLs.WikiBaseURL + parent + "/" + page);
        }
    }
}
