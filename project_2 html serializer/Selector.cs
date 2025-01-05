using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace project_2_html_serializer
{
    internal class Selector
    {
        public string TagName { get; set; }
        public string Id { get; set; }
        public List<string> Classes { get; set; }
        public Selector Parent { get; set; }
        public Selector Child { get; set; }

        public Selector(string tagName = null, string id = null, List<string> classes = null)
        {
            TagName = tagName;
            Id = id;
            Classes = classes ?? new List<string>();
        }

        public static Selector EzCreate(string ask)
        {
            Selector root = new Selector();
            var selectores0 = Regex.Split(ask, @"(?=[#.])");
            foreach (string selector in selectores0)
            {
                if (!selector.StartsWith("#") && !selector.StartsWith("."))
                {
                    if (HtmlHelper.Instance.tags.Contains(selector) || HtmlHelper.Instance.voidTags.Contains(selector))
                        root.TagName = selectores0[0];
                }
                else if (selector.StartsWith("."))
                {
                    root.Classes.Add(selector.Substring(1));
                }
                else if (selector.StartsWith("#"))
                {
                    root.Id = selector.Substring(1);
                }
            }
            return root;
        }
        public static Selector CreateSelector(string ask)
        {
            string[] arr = ask.Split(' ');
            Selector selectores0 = EzCreate(arr[0]);
            Selector current = selectores0;
            for (int i = 1; i < arr.Length; i++)
            {
                Selector newSelector = EzCreate(arr[i]);
                current.Child = newSelector;
                newSelector.Parent = current;
                current = newSelector;
            }
            return selectores0;
        }

        public static void PrintSelectorTree(Selector selector, int level = 0)
        {
            // הדפס את ה-TagName, Id ו-Classes עבור הסלקטור הנוכחי
            StringBuilder sb = new StringBuilder();
            sb.Append(new string(' ', level * 2)); // הוספת רווחים לפי הרמה (indentation)
            sb.Append($"Tag: {selector.TagName ?? "N/A"}");
            sb.Append(selector.Id != null ? $", Id: {selector.Id}" : "");
            sb.Append(selector.Classes.Any() ? $", Classes: {string.Join(", ", selector.Classes)}" : "");

            Console.WriteLine(sb.ToString()); // הדפס את הסלקטור הנוכחי

            // אם יש Child, קרא לפונקציה באופן רקורסיבי על הילד
            if (selector.Child != null)
            {
                PrintSelectorTree(selector.Child, level + 1);
            }
        }


    }
}
