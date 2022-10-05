using System.ComponentModel;

namespace MyAlloySite.Config
{
    public class EnumSelectionDescriptionAttribute : DescriptionAttribute
    {
        public string Text { get; set; }
        public object Value { get; set; }
    }
}
