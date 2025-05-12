
using System.Collections.Generic;

namespace project_2_html_serializer
{
    internal class HtmlElement
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<string> Attributes { get; private set; }
        public List<string> Classes { get; private set; }
        public string InnerHtml { get; set; }
        public HtmlElement Parent { get; set; }
        public List<HtmlElement> Children { get; private set; }

        public HtmlElement( string name, string id = null, List<string> attributes = null, List<string> classes = null, string innerHtml = null, HtmlElement parent = null)
        {
            Id = id;
            Name = name;
            Attributes = attributes ?? new List<string>();
            Classes = classes ?? new List<string>();
            InnerHtml = innerHtml;
            Parent = parent;
            Children = new List<HtmlElement>();
        }


        public void AddChild(HtmlElement child)
        {
            child.Parent = this;
            Children.Add(child);
        }


        public override string ToString()
        {
            string s = null;
            if (Parent != null) {
                s = Parent.Name; }
            string cleanedInnerHtml = InnerHtml?.Replace("\n", "").Replace("\r", "").Trim();
            return $"HtmlElement: Name={Name}, Id={Id}, Attributes={string.Join(", ", Attributes)}, Classes={string.Join(", ", Classes)}, InnerHtml={InnerHtml}, ChildrenCount={Children.Count}, Parent={s}";
        }

        public IEnumerable<HtmlElement> Descendants()
        {
            Queue<HtmlElement> q = new Queue<HtmlElement>();
            foreach (HtmlElement h in this.Children)
            {
                q.Enqueue(h);
            }
            while (q.Count > 0)
            {
                foreach (HtmlElement h in q.Peek().Children)
                {
                    q.Enqueue(h);
                }

                yield return q.Dequeue();
            }
        }

        public IEnumerable<HtmlElement> Ancestors() 
        {
            HtmlElement current = this;
            while (current.Parent != null)
            {
                yield return current.Parent;
                current = current.Parent;
            }
        }
        public override bool Equals(object obj)
        {
            if (obj is not HtmlElement other) return false;

            return Id == other.Id &&
                   Name == other.Name &&
                   Attributes.SequenceEqual(other.Attributes) &&
                   Classes.SequenceEqual(other.Classes) &&
                   InnerHtml == other.InnerHtml &&
                   Children.Count == other.Children.Count;
        }
        public override int GetHashCode()
        {
            var hashCode = new HashCode();
            hashCode.Add(Id);
            hashCode.Add(Name);
            foreach (var attr in Attributes) hashCode.Add(attr);
            foreach (var cls in Classes) hashCode.Add(cls);
            hashCode.Add(InnerHtml);
            return hashCode.ToHashCode();
        }
    }
}