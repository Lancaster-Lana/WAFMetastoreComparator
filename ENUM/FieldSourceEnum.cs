using System;
using System.Xml.Serialization;

namespace  WAFMetastoreComparator.ENUMS
{
	[Serializable, XmlRoot("FIELDSOURCE", IsNullable = true)]
	public enum FieldSourceEnum
	{
        [NonSerialized]
		FieldSourceDatabase = 0, //TODO: default
		FieldSourceProfile = 1,
		FieldSourceConstant = 2,
		FieldSourceExternal = 3,
		FieldSourceNone = 4
	}
}