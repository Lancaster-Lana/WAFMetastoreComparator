using System;
using System.Text;
using System.Xml.Serialization;

namespace  WAFMetastoreComparator
{
	[Serializable, XmlRoot("FORMMENUACTION")]
	public class FormMenuAction : FormAction
	{
		public override string ToXML()
		{
			var sb = new StringBuilder();
			sb.Append("<FORMMENUACTION NAME='" + this.Name + "' >");
			return sb.ToString(); 
		}
	}
}
