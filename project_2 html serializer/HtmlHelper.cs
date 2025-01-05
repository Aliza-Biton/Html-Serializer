using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json; // הוספת using לפעולה JsonSerializer.Deserialize


namespace project_2_html_serializer
{
    internal class HtmlHelper
    {
        private readonly static HtmlHelper _instance = new HtmlHelper();
        public static HtmlHelper Instance => _instance;
        public string[] tags;
        public string[] voidTags;


            private HtmlHelper()
            {
                var content1 = File.ReadAllText("HtmlTags.json");
                var content2 = File.ReadAllText("HtmlVoidTags.json");
            this.tags = JsonSerializer.Deserialize<string[]>(content1); // המרה למערך
            this.voidTags = JsonSerializer.Deserialize<string[]>(content2); // המרה למערך 
        }
        }
}
