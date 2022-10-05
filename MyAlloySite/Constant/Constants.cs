using MyAlloySite.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAlloySite.Constant
{
    public class Constants
    {
        public enum FacetFieldType
        {
            [EnumSelectionDescription(Text = "String", Value = "String")]
            String = 1,
            [EnumSelectionDescription(Text = "List of String", Value = "ListOfString")]
            ListOfString,
            [EnumSelectionDescription(Text = "Integer", Value = "Integer")]
            Integer,
            [EnumSelectionDescription(Text = "2 Decimal Places", Value = "Double")]
            Double,
            [EnumSelectionDescription(Text = "Boolean", Value = "Boolean")]
            Boolean,
            [EnumSelectionDescription(Text = "Enhanced Boolean", Value = "NullableBoolean")]
            NullableBoolean
        }

        public enum FacetDisplayMode
        {
            [EnumSelectionDescription(Text = "Checkbox", Value = "Checkbox")]
            Checkbox = 1,
            [EnumSelectionDescription(Text = "Button", Value = "Button")]
            Button,
            [EnumSelectionDescription(Text = "Color Swatch", Value = "ColorSwatch")]
            ColorSwatch,
            [EnumSelectionDescription(Text = "Size Swatch", Value = "SizeSwatch")]
            SizeSwatch,
            [EnumSelectionDescription(Text = "Numeric Range", Value = "Range")]
            Range,
            [EnumSelectionDescription(Text = "Rating", Value = "Rating")]
            Rating,
            [EnumSelectionDescription(Text = "Slider", Value = "Slider")]
            Slider,
            [EnumSelectionDescription(Text = "Price Range", Value = "PriceRange")]
            PriceRange,
        }

        public enum FacetDisplayDirection
        {
            [EnumSelectionDescription(Text = "Vertical", Value = "Vertical")]
            Vertical = 1,
            [EnumSelectionDescription(Text = "Horizontal", Value = "Horizontal")]
            Horizontal
        }
    }
}
