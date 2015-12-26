using System;
using System.ComponentModel;
using System.Xml.Serialization;
using System.IO;
using System.Text;

namespace  WAFMetastoreComparator
{
	[Serializable, XmlRoot("SEARCHROWACTION")]
	public class SearchRowAction : BaseXMLElement, IComparable
	{
		public SearchRowAction()
		{
		}

		public SearchRowAction(string name)
		{
			this.Name = name;
		}

		public override string ToXML()
		{
			var sb = new StringBuilder();
			sb.Append("<SEARCHROWACTION NAME='" + this.Name + "' >");
			return sb.ToString(); //return Entity.Serialize(this);
		}

		public override string ToString()
		{
			return ToXML();
		}

		public int CompareTo(object obj)
		{
			if (obj == null) return -1;
			return String.Compare(Name, ((SearchRowAction)obj).Name, StringComparison.Ordinal);
		}
	}
}