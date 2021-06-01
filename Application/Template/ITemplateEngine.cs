using System.Collections.Generic;

namespace Application.Template
{
    public interface ITemplateEngine
    {
        string Parse(string template, object model);

        string Parse(string template, IDictionary<string, object> model);
    }
}