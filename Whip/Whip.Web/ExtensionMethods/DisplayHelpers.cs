using System;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace Whip.Web.ExtensionMethods
{
    public static class DisplayHelpers
    {
        public static MvcHtmlString DisplayWithBreaksFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            var model = html.Encode(metadata.Model).Replace("\n", "<br />");

            if (string.IsNullOrEmpty(model))
                return MvcHtmlString.Empty;

            return MvcHtmlString.Create(model);
        }

        public static MvcHtmlString DisplayLinkFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, string urlFormat, string icon)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            var model = html.Encode(metadata.Model);

            if (string.IsNullOrEmpty(model))
                return MvcHtmlString.Empty;

            var tagBuilder = new TagBuilder("a");
            tagBuilder.Attributes.Add("href", string.Format(urlFormat, model));
            tagBuilder.Attributes.Add("target", "_blank");
            
            var linkIcon = new TagBuilder("i");
            linkIcon.AddCssClass($"fa fa-{icon} fa-2x");

            tagBuilder.InnerHtml = linkIcon.ToString();
            
            return MvcHtmlString.Create(tagBuilder.ToString());
        }

        public static MvcHtmlString DisplayRowFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, string urlFormat, string text, string icon)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            var model = html.Encode(metadata.Model);

            if (string.IsNullOrEmpty(model))
                return MvcHtmlString.Empty;

            var rowTag = new TagBuilder("tr");
            var cellTag1 = new TagBuilder("td");
            var cellTag2 = new TagBuilder("td");

            var iconTag = new TagBuilder("i");
            iconTag.AddCssClass($"fa fa-{icon} fa-2x");
            cellTag1.InnerHtml = iconTag.ToString();

            var anchorTag = new TagBuilder("a");
            anchorTag.Attributes.Add("href", string.Format(urlFormat, model));
            anchorTag.Attributes.Add("target", "_blank");
            anchorTag.InnerHtml = text;
            cellTag2.InnerHtml = anchorTag.ToString();

            rowTag.InnerHtml = cellTag1.ToString() + cellTag2.ToString();

            return MvcHtmlString.Create(rowTag.ToString());
        }

        public static string GetCategoryID(this string categoryKey)
        {
            return categoryKey == "#" 
                ? "ArtistNumber" 
                : categoryKey == "" 
                ? "ArtistBlank"
                : $"Artist{categoryKey}";
        }
    }
}