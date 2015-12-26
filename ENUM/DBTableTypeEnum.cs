using System;
using System.Xml.Serialization;

namespace  WAFMetastoreComparator.ENUMS
{
	[Serializable, XmlRoot("DBTABLETYPE", IsNullable = true)]
	public enum DBTableTypeEnum
	{
		// Type of table in the database is not specified so no SQL optimization is done
		[NonSerialized]
		DBTableTypeUnspecified = 0,
		//The table is a real table in the database
		DBTableTypeTable = 1,
		//The table is a view in the database
		DBTableTypeView = 2,
		// The table is a SQL Server table-valued function in the database
		DBTableTypeSQLServerTableValuedFunction = 3
	}
}