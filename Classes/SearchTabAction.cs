using System;
using System.ComponentModel;
using System.Xml.Serialization;
using System.IO;
using System.Text;

namespace  WAFMetastoreComparator
{
	[Serializable, XmlRoot("SEARCHTABACTION")]
	public class SearchTabAction : BaseXMLElement, IComparable
	{
		public SearchTabAction()
		{
		}

		public SearchTabAction(string name)
		{
			this.Name = name;
		}

		public override string ToXML()
		{
			var sb = new StringBuilder();
			sb.Append("<SEARCHTABACTION NAME='" + this.Name + "' >");
			return sb.ToString(); 
		}

		public override string ToString()
		{
			return ToXML();
		}

		public int CompareTo(object obj)
		{
			if (obj == null) return -1;
			return String.Compare(Name, ((SearchTabAction)obj).Name, StringComparison.Ordinal);
		}
	}
}