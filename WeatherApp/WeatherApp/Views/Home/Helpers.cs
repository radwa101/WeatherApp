using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace WeatherApp.Views.Home
{
    public static class Helpers
    {
        public static MvcHtmlString RadioButtonWithLabelFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, 
                        Expression<Func<TModel, TProperty>> expression, List<SelectListItem> items)
        {
            StringBuilder htmlRadioButtons = new StringBuilder();

            object currentValue = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData).Model;
            string property = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData).PropertyName;

            foreach (var item in items)
            {
                // Build the radio button html tag
                TagBuilder htmlRadio = new TagBuilder("input");
                htmlRadio.MergeAttribute("type", "radio");
                htmlRadio.MergeAttribute("id", property + item.Value);
                htmlRadio.MergeAttribute("name", property);
                htmlRadio.MergeAttribute("value", (string)item.Value);

                if (currentValue != null && item.Value.ToString() == currentValue.ToString()) htmlRadio.MergeAttribute("checked", "checked");

                // Build the label html tag
                TagBuilder htmlLabel = new TagBuilder("label");
                htmlLabel.MergeAttribute("for", property + item.Value);
                htmlLabel.SetInnerText((string)item.Text);

                // Return the concatenation of both tags
                htmlRadioButtons.Append(htmlRadio.ToString(TagRenderMode.SelfClosing) + htmlLabel.ToString() + "<br/>");
            }
            return MvcHtmlString.Create(htmlRadioButtons.ToString());
        }
    }
}