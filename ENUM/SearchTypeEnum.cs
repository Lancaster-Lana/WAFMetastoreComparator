using System;
using System.Xml.Serialization;

namespace  WAFMetastoreComparator.ENUMS
{
	[Serializable, XmlRoot("SEARCHTYPE", IsNullable = true)]
	public enum SearchTypeEnum
	{
		[NonSerialized]
		None = 0,
		FieldSearchTypeEqual = 0,
		FieldSearchTypeBeginsWith = 1,
		FieldSearchTypeGreater = 2,
		FieldSearchTypeGreaterOrEqual = 3,
		FieldSearchTypeSmaller = 4,
		FieldSearchTypeSmallerOrEqual = 5,
		FieldSearchTypeBetween = 6,
		FieldSearchTypeSoundex = 7,
		FieldSearchTypeContains = 8,
		FieldSearchTypeSoundsOrBegins = 9,
		FieldSearchTypeWordBeginsWith = 10,
		FieldSearchTypeIn = 11,
		FieldSearchTypeNotIn = 12
	}
}