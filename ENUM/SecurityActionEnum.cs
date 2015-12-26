using System;
using System.Xml.Serialization;

namespace  WAFMetastoreComparator.ENUMS
{
	[Serializable, XmlRoot("ACTION", IsNullable = true)]
	public enum SecurityActionEnum
	{
		[NonSerialized]
		None,
		ActionAny,
		ActionSearchDisplay,
		ActionSearchExecute,
		ActionRecordCreate,
		ActionRecordRead,
		ActionRecordUpdate,
		ActionRecordDelete,
		ActionRecordDisplay,
		ActionFieldDisplay,
		ActionFieldModify,
		ActionFieldSearch,
		ActionActionDisplay,
	}
}