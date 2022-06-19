using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace taskt.Core
{
    public class MyURLs
    {
        public const string GitProjectURL = "https://github.com/rcktrncn/taskt";
        public const string GitReleaseURL = "https://github.com/rcktrncn/taskt/releases";
        public const string GitIssueURL = "https://github.com/rcktrncn/taskt/issues/new";
        public const string WikiURL = "https://github.com/rcktrncn/taskt-wiki/blob/master/home.md";
        public const string OfficialSiteURL = "http://www.taskt.net/";
        public const string GitterURL = "https://gitter.im/taskt-rpa/Lobby";
        public const string WikiBaseURL = "https://github.com/rcktrncn/taskt-wiki/blob/master/";

        public static string GetWikiURL(string commandName, string groupName)
        {
            string page = commandName.ToLower().Replace(" ", "-").Replace("/", "-") + "-command.md";
            string parent = groupName.ToLower().Replace(" ", "-").Replace("/", "-");
            return (Core.MyURLs.WikiBaseURL + parent + "/" + page);
        }
    }
}
