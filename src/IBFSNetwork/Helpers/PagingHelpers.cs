using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text;
using IBFSNetwork.Models;
using System.Text.Encodings.Web;

namespace IBFSNetwork.Helpers
{
//    public static class PagingHelpers
//    {
//        public static HtmlString PageLinks(this IHtmlHelper html,
//      PageInfo pageInfo, Func<int, string> pageUrl)
//        {
//            pageInfo.TotalPages
//            pageInfo.TotalItems
//            pageNumber
//            pageInfo.PageSize
//            int curCount = 10;
//            if (curCount > pageInfo.TotalPages)
//                curCount = pageInfo.TotalPages;

//            int currenpage = pageInfo.PageNumber;
//            if (currenpage < pageInfo.TotalPages)
//                ; //create right ...

//            TagBuilder ul = new TagBuilder("ul");
//            int istart = (currenpage - 1) * pageInfo.PageSize + 1;
//            create left next
//           TagBuilder li0 = new TagBuilder("li");
//            li0.AddCssClass("page-item");
//            if (currenpage > 1)
//            {
//                li0.AddCssClass("disabled");
//            }
//            TagBuilder ahref = new TagBuilder("a");

//            < li class="page-item disabled">
     
//            for (int i = istart; i <= curCount; i++)
//            {
//                TagBuilder li = new TagBuilder("li");
//        li.AddCssClass("page-item");
//                TagBuilder a = new TagBuilder("a");
//        a.MergeAttribute("href", pageUrl(i));
//                a.InnerHtml.AppendHtml(i.ToString());
//                a.AddCssClass("page-link");
//                 если текущая страница, то выделяем ее,
//                 например, добавляя класс
//                if (i == pageInfo.PageNumber)
//                {
//                    li.AddCssClass("active");
//                    a.AddCssClass("btn-primary");
//                }
//                else
//                    a.AddCssClass("btn btn-default");
//                li.InnerHtml.AppendHtml(a);
//                ul.InnerHtml.AppendHtml(li);

//            }
//ul.AddCssClass("pagination");
//            var writer = new System.IO.StringWriter();
//ul.WriteTo(writer, HtmlEncoder.Default);
//            return new HtmlString(writer.ToString());

//        }
//        public static HtmlString PageLinksOld(this IHtmlHelper html,
//       PageInfo pageInfo, Func<int, string> pageUrl)
//{
//    StringBuilder result = new StringBuilder();
//    TagBuilder ul = new TagBuilder("ul");
//    for (int i = 1; i <= pageInfo.TotalPages; i++)
//    {
//        TagBuilder li = new TagBuilder("li");
//        li.AddCssClass("page-item");
//        TagBuilder a = new TagBuilder("a");
//        a.MergeAttribute("href", pageUrl(i));
//        a.InnerHtml.AppendHtml(i.ToString());
//        a.AddCssClass("page-link");
//        если текущая страница, то выделяем ее,
//                 например, добавляя класс
//                if (i == pageInfo.PageNumber)
//        {
//            li.AddCssClass("active");
//            a.AddCssClass("btn-primary");
//        }
//        else
//            a.AddCssClass("btn btn-default");
//        li.InnerHtml.AppendHtml(a);
//        ul.InnerHtml.AppendHtml(li);

//    }
//    ul.AddCssClass("pagination");
//    var writer = new System.IO.StringWriter();
//    ul.WriteTo(writer, HtmlEncoder.Default);
//    return new HtmlString(writer.ToString());

//}

//public static HtmlString CreateList(this IHtmlHelper html, string[] items)
//{
//    string result = "<ul>";
//    foreach (string item in items)
//    {
//        result += $"<li>{item}</li>";
//    }
//    result += "</ul>";
//    return new HtmlString(result);
//}
//    }
}
