using System;
using System.Xml.Serialization;
using WAFMetastoreComparator.ENUMS;

namespace  WAFMetastoreComparator
{
	[Serializable, XmlRoot("SECURITY")]
	public class Security : BaseXMLElement
	{
		[XmlAttribute("ACTION")]
		public SecurityActionEnum Action { get; set; }

		[XmlAttribute("ALLOWANY")]
		public bool AllowAny { get; set; }

		[XmlElement("PRIV")]
		public Permission Priv { get; set; }

		[XmlAttribute("COMMENT")]
		public string Comment { get; set; }

		public override string ToString()
		{
			string secStr = string.Format("<SECURITY ACTION ='{0}' ", Action);
			if (Priv != null )//&& Priv.Field1 != null)
				secStr += string.Format(" for {0} ", Priv);
			return secStr;
		}

		public Security()
		{
			//if (Priv == null)
			//	Priv = new Permission();
		}
	}

	[Serializable, XmlRoot("PRIV")]
	public class Permission
	{
		[XmlAttribute("FIELD1")]
		public string Field1 { get; set; }

		[XmlAttribute("FIELD2")]
		public string Field2 { get; set; }

		[XmlAttribute("OP")]
		public SecurityOpEnum Operator { get; set; }

		[XmlAttribute("VALUE")]
		public string Value { get; set; }

		public override string ToString()
		{
			return Field1 != null && Field2 != null
										? String.Format(" {0} {1} {2}", Field1, Operator, Field2)
										: string.Empty;
		}
	}
}
