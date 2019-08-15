using System;
using System.Web.Mvc;
using System.Text;
using SportsStore.WebUI.Models;

namespace SportsStore.WebUI.HtmlHelpers
{
    //class helper for creating a pagination link button 
    public static class PagingHelpers
    {
        public static MvcHtmlString PageLinks(this HtmlHelper html,PagingInfo info, Func<int, string> pageUrl)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 1; i <= info.TotalPages; i++)
            {
                TagBuilder tag = new TagBuilder("a");
                tag.MergeAttribute("href", pageUrl(i));
                tag.InnerHtml = i.ToString();
                if(i == info.CurrentPage)
                {
                    tag.AddCssClass("selected");
                    tag.AddCssClass("btn btn-primary");
                }
                else
                {
                    tag.AddCssClass("btn btn-secondary");
                }                
                result.Append(tag.ToString());
            }
            return MvcHtmlString.Create(result.ToString());
        }
    }
}