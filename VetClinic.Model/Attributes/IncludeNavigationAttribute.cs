namespace VetClinic.Model.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class IncludeNavigationAttribute : Attribute
    {
        public string NavigationPropertyName { get; }

        public IncludeNavigationAttribute(string navigationPropertyName)
        {
            NavigationPropertyName = navigationPropertyName;
        }
    }
}

