using System;
using System.Xml.Serialization;

namespace  WAFMetastoreComparator.ENUMS
{
	[Serializable, XmlRoot("SECURECONNECTION", IsNullable = true)]
	public enum SecureConnectionEnum
	{
		[NonSerialized]
		KeepDefault = 0,
		RequireSSL = 1,
		RequireNoSSL = 2,
		KeepCurrent = 3
	}
}