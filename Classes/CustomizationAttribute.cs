using System;
using System.IO;
using System.Xml.Serialization;

namespace  WAFMetastoreComparator
{
    public enum AttributeLevel { None, Table, Form, Security }

    public class CustomizationAttribute
    {
        public AttributeLevel Level { get; set; }

        public string Value { get; set; }

        public CustomizationAttribute()
        {
        }
    }
}
