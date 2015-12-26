using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace  WAFMetastoreComparator
{
	[Serializable, XmlRoot("FORMMENUACTION")]
	public class FormAction : BaseXMLElement, IComparable
	{
		[XmlElement(typeof(Security), ElementName = "SECURITY")]
		public List<Security> Security { get; set; }

		public override string ToString()
		{
			return ToXML();
		}

		public int CompareTo(object obj)
		{
			if (obj == null) return -1;
			return String.Compare(Name, ((FormMenuAction)obj).Name, StringComparison.Ordinal);
		}
	}
}
