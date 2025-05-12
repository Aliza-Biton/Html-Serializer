
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


    //קריאה לדף אינטרנט
    var myHtml = await Load("https://learn.malkabruk.co.il/practicode/projects/pract-2/");
    //טיפול בתוכן על מנת להמיר אותו
    var cleanHtml = new Regex("[\\s&&[^ ]]+").Replace(myHtml, "");
    var cleanHtml2 = new Regex("&nbsp;").Replace(cleanHtml, "");
    var htmlLines = new Regex("<(.*?)>").Split(cleanHtml2).Where(x => !string.IsNullOrWhiteSpace(x));

    //יצירת אוביקט הבסיס
    HtmlElement root = new HtmlElement("root");
    HtmlElement current = root;

    //יצירת עץ האלמנטים
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
                    string part = match.Value;
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

    // הדפסה של העץ
    Console.WriteLine("___________________");
    Console.WriteLine("הדפסת עץ האלמנטים:");
    PrintTree(root, 0);

    // Descendants הפונקציה
    Console.WriteLine("___________________");
    Console.WriteLine("רשימת הצאצאים:");
    var toCheck = root.Children[0].Children[0].Children[0].Descendants();
    string names = string.Join(", ", toCheck.Select(child => child.Name));
    Console.WriteLine(names);

    // Ancestors הפונקציה
    Console.WriteLine("___________________");
    Console.WriteLine("רשימת האבות:");
    var toCheck2 = root.Children[0].Children[0].Children[0].Ancestors();
    foreach (var parent in toCheck2)
    {
        Console.WriteLine(parent);
    }

    //יצירת עץ סלקטורים
    Console.WriteLine("___________________");
    Console.WriteLine("הדפסת עץ הסלקטורים:");
    string ask = Console.ReadLine();
    Selector mySelector = Selector.CreateSelector(ask);
    Selector.PrintSelectorTree(mySelector);

    //חיפוש לפי סלקטור
    Console.WriteLine("___________________");
    Console.WriteLine("הדפסת חיפוש הסלקטור:");
    var results = root.FindBySelector(mySelector);
    foreach (var element in results)
    {
        Console.WriteLine(element);
    }
