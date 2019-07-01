using HtmlAgilityPack;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp4
{
  public  class Program
    {

        static void Main(string[] args)
        {
            List<string> listen = new List<string>();
 
            string textFile= "C:\\textlines.txt";
            string[] lines = File.ReadAllLines(textFile);

            foreach (string line in lines)
            {
                string webpageUrl = "https://www.google.com/search?q=" + line.Replace(" ","+") + "+ oldschoolrunescape";
                string targetDomain = "exposureroom.com";

                var linkFinder = new LinkFinder();
                var links = linkFinder.FindLinksToDomainOnWebPage(webpageUrl, targetDomain);
          
       
            bool o = false;
            foreach (var link in links)
            {
           

                if (link.InnerText.Contains("oldschool.runescape") && link.InnerText.Contains("https") && o == false || link.InnerText.Contains("oldschoolrunescape")&& link.InnerText.Contains("https")&&o==false)
                {
                    o = true;
                        string toFind1 = "https";
                    int start = link.InnerText.IndexOf(toFind1) + toFind1.Length;
                    start = start - 5;
                    int end = link.InnerText.Length;
                    string string2 = link.InnerText.Substring(start, end - start);
                    string2 = string2.Replace(" ", string.Empty);
                    Console.WriteLine(string2);
                    listen.Add(string2);
                   
                }
                else
                {

                }
              
            }
            }
            using (TextWriter tw = new StreamWriter("C:\\Users\\dev7\\Desktop\\textlines2.txt"))
            {
                foreach (String s in listen)
                    tw.WriteLine(s);
            }
            Console.ReadLine();
        }

        public class LinkFinder
        {
            public IEnumerable<AnchorTag> FindLinksToDomainOnWebPage(string webpageUrl, string targetDomain)
            {
                HtmlDocument htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(GetWebsiteHtml(webpageUrl));
                var anchorTags = htmlDocument.DocumentNode.SelectNodes("//a");

                foreach (var tag in anchorTags)
                {
                    var hrefValue = tag.GetAttributeValue("href", "");
                    var tempHref = hrefValue.ToUpper();
                    var tempTargetDomain = targetDomain.ToUpper();
                 
                        var anchorTag = new AnchorTag();
                        foreach (var attribute in tag.Attributes)
                            anchorTag.Attributes.Add(attribute.Name, attribute.Value);

                        anchorTag.InnerText = tag.InnerText;
                        yield return anchorTag;
                    
                }
            }

            private string GetWebsiteHtml(string webpageUrl)
            {
                WebClient webClient = new WebClient();
                byte[] buffer = webClient.DownloadData(webpageUrl);
                return Encoding.UTF8.GetString(buffer);
            }
        }

        public class AnchorTag
        {
            public Dictionary<string, string> Attributes { get; private set; }
            public string InnerText { get; set; }

            public AnchorTag()
            {
                Attributes = new Dictionary<string, string>();
            }

            public override string ToString()
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("InnerText: " + InnerText);
                sb.AppendLine("Attributes:");
                foreach (var attribute in Attributes)
                    sb.AppendLine("\t" + attribute.Key + "=" + attribute.Value);
                return sb.ToString();
            }
        }
    }
}
