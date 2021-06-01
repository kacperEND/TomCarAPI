using DotLiquid;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

namespace Application.Template
{
    /// <summary>
    /// Templating engine based on Shopify Liquid.
    /// </summary>
    /// <remarks>
    /// Refer https://github.com/Shopify/liquid/wiki/Liquid-for-Designers for examples.
    /// Refer http://dotliquidmarkup.org/try-online for live template design.
    /// </remarks>
    public class DotLiquidTemplateEngine : ITemplateEngine
    {
        public string Parse(string template, object model)
        {
            var liquidTemplate = DotLiquid.Template.Parse(template);
            RegisterSafeType(model.GetType());
            return liquidTemplate.Render(Hash.FromAnonymousObject(model));
        }

        public string Parse(string template, IDictionary<string, object> model)
        {
            var liquidTemplate = DotLiquid.Template.Parse(template);
            return liquidTemplate.Render(Hash.FromDictionary(model));
        }

        public static void RegisterSafeType(Type type)
        {
            if (DotLiquid.Template.GetSafeTypeTransformer(type) != null)
                return;

            DotLiquid.Template.RegisterSafeType(type, x => new DropProxy(x, x.GetType().GetMembers(BindingFlags.Instance | BindingFlags.Public).Select(y => y.Name).ToArray()));

            foreach (PropertyInfo property in type.GetProperties().Where(x => x.PropertyType.IsClass && x.PropertyType.IsPublic && x.PropertyType != typeof(string)))
            {
                RegisterSafeType(property.PropertyType);
            }
        }
    }
}