using System;
using System.Xml.Serialization;
using System.Text;

namespace  WAFMetastoreComparator
{
	[Serializable, XmlRoot("FORMTABACTION")]
	public class FormTabAction : FormAction
	{
		public FormTabAction()
		{
		}

		public FormTabAction(string name)
		{
			this.Name = name;
		}

		public override string ToXML()
		{
			var sb = new StringBuilder();
			sb.Append("<FORMTABACTION NAME='" + this.Name + "' >");
			return sb.ToString(); 
		}
	}
}