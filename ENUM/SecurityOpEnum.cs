using System;
using System.Xml.Serialization;

namespace  WAFMetastoreComparator.ENUMS
{
	[Serializable, XmlRoot("OP", IsNullable = true)]
	public enum SecurityOpEnum
	{
		[NonSerialized]
		None,
		OpEquals,
		OpDoesNotEqual,
		OpIsLessThan,
		OpIsGreaterThan,
		OpIsLessThanOrEqualTo,
		OpIsGreaterThanOrEqualTo,
		OpContains,
		OpDoesNotContain,
		OpIsEmpty,
		OpIsNotEmpty,
		OpIsInRole,
		OpIsNotInRole,
		OpIsAuthenticated,
		OpIsNotAuthenticated,
		OpHasPrivilege,
		OpDoesNotHavePrivilege
	}
}