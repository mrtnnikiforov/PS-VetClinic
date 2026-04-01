namespace VetClinic.Model.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SearchableAttribute : Attribute
    {
        public string DisplayName { get; }

        public SearchableAttribute(string displayName)
        {
            DisplayName = displayName;
        }
    }
}
