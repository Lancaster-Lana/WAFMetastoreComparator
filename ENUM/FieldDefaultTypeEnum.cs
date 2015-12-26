using System;
using System.Xml.Serialization;

namespace  WAFMetastoreComparator.ENUMS
{
	[Serializable, XmlRoot("DEFAULTTYPE", IsNullable = true)]
	public enum FieldDefaultTypeEnum
	{
		[NonSerialized]
		FieldDefaultTypeNone = 0,
		FieldDefaultTypeConstant = 1,
		FieldDefaultTypeProfile = 2,
		FieldDefaultTypeDefaultSql = 3,
		FieldDefaultTypeCustomSql = 4,
		FieldDefaultTypeExternal = 5,
		FieldDefaultTypeToday = 6,
		FieldDefaultTypeNow = 7
	}
}