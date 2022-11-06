using AngleSharp;
using HonkaiCharacterBrowser.Models;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace HonkaiCharacterBrowser.Helpers;

public class WebHelper
{
    public const string BaseUrl = "https://www.bh3.com";

    public static async Task<IEnumerable<Valkyrie>> GetValkyriesAsync()
    {
        var url = BaseUrl + "/valkyries";

        var context = new BrowsingContext(Configuration.Default.WithDefaultLoader());
        var doc = await context.OpenAsync(url);

        var result = new List<Valkyrie>();
        foreach (var div in doc.QuerySelectorAll("div.valkyries-item > div"))
        {
            var cname = div.QuerySelector(".valkyries-item__title div:nth-child(1)").TextContent.Trim();
            var ename = div.QuerySelector(".valkyries-item__title div:nth-child(3)").TextContent.Trim();

            foreach (var a in div.QuerySelectorAll(".roles a"))
            {
                var charUrl = a.GetAttribute("href");
                var iconUrl = a.QuerySelector("img").GetAttribute("src");

                result.Add(new Valkyrie
                {
                    ChineseName = cname,
                    EnglishName = ename,
                    Url = charUrl,
                    IconUrl = iconUrl
                });
            }
        }
        return result;
    }

    public static async Task<JArray> GetValkyrieInfosAsync()
    {
        var client = new HttpClient();
        var text = await client.GetStringAsync("https://www.bh3.com/content/bh3Cn/getContentList?pageSize=200&pageNum=1&channelId=181");
        return JObject.Parse(text)["data"]["list"] as JArray;
    }

    public static async Task<(string, string)> GetArmorBirthdayAsync(string url)
    {
        url = BaseUrl + url;
        var context = new BrowsingContext(Configuration.Default.WithDefaultLoader());
        var doc = await context.OpenAsync(url);

        var armor = doc.QuerySelector(".valkyries-detail-bd__card div.name").TextContent.Trim();
        var birthday = doc.QuerySelector(".valkyries-detail-bd__card div.info-item:nth-child(4)").TextContent.Trim();

        return (armor, birthday);
    }
}