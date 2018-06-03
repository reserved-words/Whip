using System;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace Whip.Web.HtmlHelpers
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
            linkIcon.AddCssClass($"fa fa-{icon}");

            tagBuilder.InnerHtml = linkIcon.ToString();
            
            return MvcHtmlString.Create(tagBuilder.ToString());
        }
    }
}