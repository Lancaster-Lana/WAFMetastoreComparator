using System;
using System.Xml.Serialization;

namespace  WAFMetastoreComparator.ENUMS
{
	[Serializable, XmlRoot("PRIMARYKEYTYPE", IsNullable = true)]
	public enum PrimaryKeyTypeEnum
	{
		[NonSerialized]//		[XmlEnum("")]
		PKeyNone = 0,				// No key at all
		PKeySQLServerIdentity = 1,	// Handled by Server (SQL Server)   
		PKeyOracleSequence = 2,
		PKeyCustom = 3,
		PKeySQLServerTrigger = 4,
		PKeySQLServerCalculated = 5
	}
}