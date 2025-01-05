// See https://aka.ms/new-console-template for more information

using project_2_html_serializer;
using System.Text.RegularExpressions;

async Task<string> Load(string path)
{
    if (path.StartsWith("C:/"))
    {
        // טעינת קובץ מקומי
        return await File.ReadAllTextAsync(path);
    }
    else
    {
        // טעינת תוכן דרך HTTP/HTTPS
        HttpClient client = new HttpClient();
        var response = await client.GetAsync(path);
        return await response.Content.ReadAsStringAsync();
    }
}

// דוגמה לשימוש
string ask = ".container";
Selector s = Selector.CreateSelector(ask);
Selector.PrintSelectorTree(s);

Console.WriteLine(string.Join(", ", HtmlHelper.Instance.tags)); // מדפיס את המערך
Console.WriteLine("-------------");
Console.WriteLine(string.Join(", ", HtmlHelper.Instance.voidTags)); // מדפיס את המערך
var myHtml = await Load("https://learn.malkabruk.co.il/practicode/projects/pract-2/");
var cleanHtml = new Regex("[\\s&&[^ ]]+").Replace(myHtml, "");
var cleanHtml2 = new Regex("&nbsp;").Replace(cleanHtml, "");
var htmlLines = new Regex("<(.*?)>").Split(cleanHtml2).Where(x => !string.IsNullOrWhiteSpace(x));
HtmlElement root = new HtmlElement("root");
HtmlElement current = root;
Console.WriteLine("-------------");

Console.WriteLine("-------------");
foreach (var line in htmlLines)
{

    if (line == "/html")
        break;
    if (line.StartsWith("/"))
        current = current.Parent;
    else
    {
        string firstWord = line.Split(' ')[0];
        if (HtmlHelper.Instance.tags.Contains(firstWord) || HtmlHelper.Instance.voidTags.Contains(firstWord))
        {
            HtmlElement newElement = new HtmlElement(firstWord);
            newElement.Parent = current;
            var regex = new Regex(@"(\S+)=(""[^""]*"")");
            var parts = regex.Matches(line);
            foreach (Match match in parts)
            {
                string part = match.Value; // קבלת המחרוזת מההתאמה
                if (part.StartsWith("id"))
                {
                    newElement.Id = part.Split('=')[1];
                }
                else if (part.StartsWith("class"))
                {
                    var match2 = Regex.Match(part, @"class=""([^""]*)""");
                    string classesPart = match2.Groups[1].Value;
                    List<string> classes = new List<string>(classesPart.Split(' ', StringSplitOptions.RemoveEmptyEntries));
                    newElement.Classes.AddRange(classes);
                }
                else
                {
                    newElement.Attributes.Add(part);
                }

            }
                current.AddChild(newElement);
            if (!HtmlHelper.Instance.voidTags.Contains(firstWord) || !line.EndsWith("/"))
                current = newElement;
        }
        else
        {
                current.InnerHtml = line;
        }

    }
}


static void PrintTree(HtmlElement element, int indent)
{
    Console.WriteLine(new string(' ', indent) + element.ToString());
    foreach (var child in element.Children)
    {
        PrintTree(child, indent + 1);
    }
}


PrintTree(root, 0); // הדפסה של העץ
var toCheck = root.Children[0].Children[0].Children[0].Descendants();
foreach (var child in toCheck)
{
    Console.WriteLine(child.Name);
}
Console.WriteLine("-------");
var toCheck2 = root.Children[0].Children[0].Children[0].Ancestors();
foreach (var parent in toCheck2)
{
    Console.WriteLine(parent);
}

var results = root.FindBySelector(s);

Console.WriteLine("-------");

foreach (var element in results)
{
    Console.WriteLine(element);
}
