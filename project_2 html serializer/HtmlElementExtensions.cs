using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project_2_html_serializer
{
    internal static class HtmlElementExtensions
    {

        public static bool match(Selector s, HtmlElement h)
        {
            if (s.TagName != null && s.TagName != h.Name)
                return false;
            if (s.Id != null && s.Id != h.Id)
                return false;
            if (s.Classes != null && !s.Classes.All(item => h.Classes.Contains(item)))
                return false;
            return true;
        }

        public static HashSet<HtmlElement> FindBySelector(this HtmlElement root, Selector selector)
        {
            var result = new HashSet<HtmlElement>();
            FindBySelectorRecursive(selector, root, result);
            return result;
        }
        public static void FindBySelectorRecursive(Selector s, HtmlElement h, HashSet<HtmlElement> result)
        {
            if (s == null)
            {
                result.Add(h);
                return;
            }
            else {
                var children = h.Descendants();
                var matches = new List<HtmlElement>();
                foreach (var child in children)
                {
                    if (match(s, child))
                        matches.Add(child);
                }
                foreach (var child in matches)
                {
                    FindBySelectorRecursive(s.Child, child, result);
                }
            }
        }
    }
}
