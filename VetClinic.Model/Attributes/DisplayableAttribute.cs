namespace VetClinic.Model.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DisplayableAttribute : Attribute
    {
        public string Header { get; }
        public int Order { get; }

        public DisplayableAttribute(string header, int order = 0)
        {
            Header = header;
            Order = order;
        }
    }
}
