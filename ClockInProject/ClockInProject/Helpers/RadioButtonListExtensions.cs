﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ClockInProject.Helpers
{
    public static class RadioButtonListExtensions
    {
        #region -- RadioButtonList (Horizontal) --
           /// <summary>
           /// RadioButtonList.
           /// </summary>
           /// <param name="htmlHelper">The HTML helper.</param>
           /// <param name="name">The name.</param>
           /// <param name="listInfo">RadioButtonListInfo.</param>
           /// <returns></returns>
           public static MvcHtmlString RadioButtonList(this HtmlHelper htmlHelper,
               string name,
               IEnumerable<SelectListItem> listInfo)
           {
               return htmlHelper.RadioButtonList(name, listInfo, (IDictionary<string, object>)null, 0);
           }
    
           /// <summary>
           /// RadioButtonList.
           /// </summary>
           /// <param name="htmlHelper">The HTML helper.</param>
           /// <param name="name">The name.</param>
           /// <param name="listInfo">RadioButtonListInfo.</param>
           /// <param name="htmlAttributes">The HTML attributes.</param>
           /// <returns></returns>
           public static MvcHtmlString RadioButtonList(this HtmlHelper htmlHelper,
               string name,
               IEnumerable<SelectListItem> listInfo,
               object htmlAttributes)
           {
               return htmlHelper.RadioButtonList(
                   name, 
                   listInfo, 
                   (IDictionary<string, object>) new RouteValueDictionary(htmlAttributes), 
                   0);
           }
    
           /// <summary>
           /// RadioButtonList.
           /// </summary>
           /// <param name="htmlHelper">The HTML helper.</param>
           /// <param name="name">The name.</param>
           /// <param name="listInfo">The list info.</param>
           /// <param name="htmlAttributes">The HTML attributes.</param>
           /// <param name="number">每個Row的顯示個數.</param>
           /// <returns></returns>
           public static MvcHtmlString RadioButtonList(this HtmlHelper htmlHelper,
               string name,
               IEnumerable<SelectListItem> listInfo,
               IDictionary<string, object> htmlAttributes,
               int number)
           {
               if (String.IsNullOrEmpty(name))
               {
                   throw new ArgumentException("必須給這些RadioButtonList一個Tag Name", "name");
               }
               if (listInfo == null)
               {
                   throw new ArgumentNullException("listInfo", "必須要給List<RadioButtonListInfo> listInfo");
               }
               if (listInfo.Count() < 1)
               {
                   throw new ArgumentException("List<RadioButtonListInfo> listInfo 至少要有一組資料", "listInfo");
               }
    
               StringBuilder sb = new StringBuilder();
               int lineNumber = 0;
    
               foreach (SelectListItem info in listInfo)
               {
                   lineNumber++;
    
                   TagBuilder builder = new TagBuilder("input");
                   if (info.Selected)
                   {
                       builder.MergeAttribute("checked", "checked");
                   }
                   builder.MergeAttributes<string, object>(htmlAttributes);
                   builder.MergeAttribute("type", "radio");
                   builder.MergeAttribute("value", info.Value);
                   builder.MergeAttribute("name", name);
                   sb.Append(builder.ToString(TagRenderMode.Normal));
    
                   TagBuilder labelBuilder = new TagBuilder("label");
                   labelBuilder.MergeAttribute("for", name);
                   labelBuilder.InnerHtml = info.Text;
                   sb.Append(labelBuilder.ToString(TagRenderMode.Normal));
    
                   if (number == 0 || (lineNumber % number == 0))
                   {
                      sb.Append("<br />");
                  }
              }
              return MvcHtmlString.Create(sb.ToString());
          }
          #endregion
   
          #region -- RadioButtonVertical --
          /// <summary>
          /// Checks the box list vertical.
          /// </summary>
          /// <param name="htmlHelper">The HTML helper.</param>
          /// <param name="name">The name.</param>
          /// <param name="listInfo">The list info.</param>
          /// <param name="htmlAttributes">The HTML attributes.</param>
          /// <param name="columnNumber">The column number.</param>
          /// <returns></returns>
          public static MvcHtmlString RadioButtonVertical(this HtmlHelper htmlHelper,
              string name,
              IEnumerable<SelectListItem> listInfo,
              IDictionary<string, object> htmlAttributes,
              int columnNumber = 1)
          {
              if (String.IsNullOrEmpty(name))
              {
                  throw new ArgumentException("必須給這些RadioButton一個Tag Name", "name");
              }
              if (listInfo == null)
              {
                  throw new ArgumentNullException("listInfo", "必須要給List<RadioButtonInfo> listInfo");
              }
              if (listInfo.Count() < 1)
              {
                  throw new ArgumentException("List<RadioButtonInfo> listInfo 至少要有一組資料", "listInfo");
              }
   
              int dataCount = listInfo.Count();
   
              // calculate number of rows
              int rows = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(dataCount) / Convert.ToDecimal(columnNumber)));
              if (dataCount <= columnNumber || dataCount - columnNumber == 1)
              {
                  rows = dataCount;
              }
   
              TagBuilder wrapBuilder = new TagBuilder("div");
              wrapBuilder.MergeAttribute("style", "float: left; light-height: 25px; padding-right: 5px;");
   
              string wrapStart = wrapBuilder.ToString(TagRenderMode.StartTag);
              string wrapClose = string.Concat(wrapBuilder.ToString(TagRenderMode.EndTag), " <div style=\"clear:both;\"></div>");
              string wrapBreak = string.Concat("</div>", wrapBuilder.ToString(TagRenderMode.StartTag));
   
              StringBuilder sb = new StringBuilder();
              sb.Append(wrapStart);
   
              int lineNumber = 0;
   
              foreach (var info in listInfo)
              {
                  TagBuilder builder = new TagBuilder("input");
                  if (info.Selected)
                  {
                      builder.MergeAttribute("checked", "checked");
                  }
                  builder.MergeAttributes<string, object>(htmlAttributes);
                  builder.MergeAttribute("type", "radio");
                  builder.MergeAttribute("value", info.Value);
                  builder.MergeAttribute("name", name);
                  sb.Append(builder.ToString(TagRenderMode.Normal));
   
                  TagBuilder labelBuilder = new TagBuilder("label");
                  labelBuilder.MergeAttribute("for", name);
                  labelBuilder.InnerHtml = info.Text;
                  sb.Append(labelBuilder.ToString(TagRenderMode.Normal));
   
                  lineNumber++;
   
                  if (lineNumber.Equals(rows))
                  {
                      sb.Append(wrapBreak);
                      lineNumber = 0;
                  }
                  else
                  {
                      sb.Append("<br/>");
                  }
              }
              sb.Append(wrapClose);
              return MvcHtmlString.Create(sb.ToString());
          }
          #endregion
   
          #region -- RadioButtonList (Horizonal, Vertical) --
          /// <summary>
          /// Checks the box list.
          /// </summary>
          /// <param name="htmlHelper">The HTML helper.</param>
          /// <param name="name">The name.</param>
          /// <param name="listInfo">The list info.</param>
          /// <param name="htmlAttributes">The HTML attributes.</param>
          /// <param name="position">The position.</param>
          /// <param name="number">Position.Horizontal則表示每個Row的顯示個數, Position.Vertical則表示要顯示幾個Column</param>
          /// <returns></returns>
          public static MvcHtmlString RadioButtonList(this HtmlHelper htmlHelper,
              string name,
              IEnumerable<SelectListItem> listInfo,
              IDictionary<string, object> htmlAttributes,
              Position position = Position.Horizontal,
              int number = 0)
          {
              if (String.IsNullOrEmpty(name))
              {
                  throw new ArgumentException("必須給這些RadioButtonList一個Tag Name", "name");
              }
              if (listInfo == null)
              {
                  throw new ArgumentNullException("listInfo", "必須要給List<RadioButtonListInfo> listInfo");
              }
              if (listInfo.Count() < 1)
              {
                  throw new ArgumentException("List<RadioButtonListInfo> listInfo 至少要有一組資料", "listInfo");
              }
   
              StringBuilder sb = new StringBuilder();
              int lineNumber = 0;
   
              switch (position)
              {
                  case Position.Horizontal:
   
                      foreach (SelectListItem info in listInfo)
                      {
                          lineNumber++;
                          sb.Append(CreateRadioButtonItem(info, name, htmlAttributes));
   
                          if (number == 0 || (lineNumber % number == 0))
                          {
                              sb.Append("<br />");
                          }
                      }
                      break;
   
                  case Position.Vertical:
   
                      int dataCount = listInfo.Count();
   
                      // 計算最大顯示的列數(rows)
                      int rows = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(dataCount) / Convert.ToDecimal(number)));
                      if (dataCount <= number || dataCount - number == 1)
                      {
                          rows = dataCount;
                      }
   
                      TagBuilder wrapBuilder = new TagBuilder("div");
                      wrapBuilder.MergeAttribute("style", "float: left; light-height: 25px; padding-right: 5px;");
   
                      string wrapStart = wrapBuilder.ToString(TagRenderMode.StartTag);
                      string wrapClose = string.Concat(wrapBuilder.ToString(TagRenderMode.EndTag), " <div style=\"clear:both;\"></div>");
                      string wrapBreak = string.Concat("</div>", wrapBuilder.ToString(TagRenderMode.StartTag));
   
                      sb.Append(wrapStart);
   
                      foreach (SelectListItem info in listInfo)
                      {
                          lineNumber++;
                          sb.Append(CreateRadioButtonItem(info, name, htmlAttributes));
   
                          if (lineNumber.Equals(rows))
                          {
                              sb.Append(wrapBreak);
                              lineNumber = 0;
                          }
                          else
                          {
                              sb.Append("<br/>");
                          }
                      }
                      sb.Append(wrapClose);
                      break;
              }
   
              return MvcHtmlString.Create(sb.ToString());
          }
   
          /// <summary>
          /// CreateRadioButtonItem
          /// </summary>
          /// <param name="info"></param>
          /// <param name="name"></param>
          /// <param name="htmlAttributes"></param>
          /// <returns></returns>
          internal static string CreateRadioButtonItem(
              SelectListItem info,
              string name,
              IDictionary<string, object> htmlAttributes)
          {
              StringBuilder sb = new StringBuilder();
   
              TagBuilder builder = new TagBuilder("input");
              if (info.Selected)
              {
                  builder.MergeAttribute("checked", "checked");
              }
              builder.MergeAttributes<string, object>(htmlAttributes);
              builder.MergeAttribute("type", "radio");
              builder.MergeAttribute("value", info.Value);
              builder.MergeAttribute("name", name);
              sb.Append(builder.ToString(TagRenderMode.Normal));
   
              TagBuilder labelBuilder = new TagBuilder("label");
              labelBuilder.MergeAttribute("for", name);
              labelBuilder.InnerHtml = info.Text;
              sb.Append(labelBuilder.ToString(TagRenderMode.Normal));
   
              return sb.ToString();
          }
          #endregion
    }
}