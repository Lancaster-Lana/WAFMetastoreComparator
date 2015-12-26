using System;
using System.Xml.Serialization;

namespace  WAFMetastoreComparator.ENUMS
{
	[Serializable, XmlRoot("VALIDVALUESTYPE", IsNullable = true)]
	public enum FieldValidValuesTypeEnum
	{
		[NonSerialized]
		None = 0,
		FieldValidValuesTypeDefaultSql = 1, //TODO: 
		FieldValidValuesTypeCustomSql = 2,
		FieldValidValuesTypeList = 3,
	}
}