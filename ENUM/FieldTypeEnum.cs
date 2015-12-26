using System;
using System.Xml.Serialization;

namespace  WAFMetastoreComparator.ENUMS
{
	[Serializable, XmlRoot("FIELDTYPE", IsNullable = true)]
	public enum FieldTypeEnum
	{
		[NonSerialized]
		None = 0,
		FieldTypeVarchar = 0, //TODO:
		FieldTypeNumber = 1,
		FieldTypeBoolean = 2,
		FieldTypeDate = 3,
		FieldTypeLongVarchar = 4,
		FieldTypeCLOB = 4,
		FieldTypeText = 4,
		FieldTypeValidValues = 5,
		FieldTypeForeignKey = 6,
		FieldTypeMultiLine = 7,
		FieldTypeInteger = 8,
		FieldTypePercent = 9,
		FieldTypeCurrency = 10,
		FieldTypeTime = 11,
		FieldTypeDateTime = 12,
		FieldTypeZipCode = 13,
		FieldTypePhone = 14,
		FieldTypeState = 15,
		FieldTypePath = 16,
		FieldTypeURL = 17,
		FieldTypeUserID = 18,
		FieldTypeSQLServerLastModified = 19,
		FieldTypeSQLServerTimeStamp = 20,
		FieldTypeOracleLastModified = 21,
		FieldTypeOracleRowID = 22,
		FieldTypeSQLServerIdentity = 23,
		FieldTypeOracleSequence = 24,
		FieldTypeNextKey = 25,
		FieldTypePassword = 26,
		FieldTypeHTML = 27,
		FieldTypeEmail = 28,
		FieldTypeBirthDate = 29,
		FieldTypeDivider = 30,
		FieldTypeDividerLabel = 31,
		FieldTypeFile = 32,
		FieldTypeSSN = 33,
		FieldTypeXML = 34,
		FieldTypeBLOB = 35,
		FieldTypeImage = 35,
		FieldTypeNCLOB = 36,
		FieldTypeNText = 36,
		FieldTypeUTCDateTime = 37
	}

}