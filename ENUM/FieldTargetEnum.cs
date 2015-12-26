using System;
using System.Xml.Serialization;

namespace  WAFMetastoreComparator.ENUMS
{
    [Serializable, XmlRoot("FIELDTARGET", IsNullable = true)]
    public enum FieldTargetEnum
    {
        [NonSerialized]
        None = 0,
        FieldSourceDatabase = 1,
        FieldSourceProfile = 2,
        FieldSourceConstant = 3,
        FieldSourceExternal = 4,
    }
}
