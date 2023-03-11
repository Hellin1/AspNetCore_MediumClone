using HtmlAgilityPack;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Buffers;
using System.IO;
using System.Text;

namespace MediumClone.UI.ViewComponents
{
    public class TruncateHtml : ViewComponent
    {
        public IViewComponentResult Invoke(string html, int length)
        {
            var document = new HtmlDocument();
            document.LoadHtml(html);

            var truncatedNodes = TruncateNodes(document.DocumentNode, length);

            var truncatedHtml = truncatedNodes.OuterHtml;
            if (html.Length > length)
            {
                truncatedHtml += " ...";
            }

            return View("Default", truncatedHtml);
        }

        private HtmlNode TruncateNodes(HtmlNode node, int length)
        {
            if (node.NodeType == HtmlNodeType.Text)
            {
                if (node.InnerLength <= length)
                {
                    return node.CloneNode(true);
                }
                else
                {
                    return HtmlTextNode.CreateNode(node.InnerHtml.Substring(0, length));
                }
            }
            else if (node.NodeType == HtmlNodeType.Element)
            {
                var truncatedElement = node.CloneNode(false);
                foreach (var childNode in node.ChildNodes)
                {
                    var truncatedChildNode = TruncateNodes(childNode, length);
                    if (truncatedElement.InnerLength + truncatedChildNode.InnerLength <= length)
                    {
                        truncatedElement.AppendChild(truncatedChildNode);
                    }
                    else if (truncatedElement.InnerLength < length)
                    {
                        var remainingLength = length - truncatedElement.InnerLength;
                        var truncatedRemainingNode = TruncateNodes(truncatedChildNode, remainingLength);
                        truncatedElement.AppendChild(truncatedRemainingNode);
                        break;
                    }
                    else
                    {
                        break;
                    }
                }
                return truncatedElement;
            }
            else
            {
                return node.CloneNode(true);
            }
        }


        //public static string Truncate(this string input, int length)
        //{
        //    if (string.IsNullOrEmpty(input))
        //        return string.Empty;

        //    var htmlString = new HtmlString(input);
        //    var plainText = htmlString.Value;

        //    if (plainText.Length <= length)
        //        return input;

        //    plainText = plainText.Substring(0, length);
        //    plainText = plainText.Substring(0, plainText.LastIndexOf(" "));

        //    return new HtmlString(plainText + "...").ToString();
        //}
    }
}
