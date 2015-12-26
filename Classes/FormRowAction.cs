using System;
using System.Xml.Serialization;
using System.Text;

namespace  WAFMetastoreComparator
{
	//[SerializableAttribute, XmlRoot]
	//public enum AttributeRequiredLevel { None, SystemRequired, Required, Recommended, ReadOnly }

	[Serializable, XmlRoot("FORMROWACTION")]
	public class FormRowAction : FormAction
	{
		public FormRowAction()
		{
		}

		public FormRowAction(string name)
		{
			this.Name = name;
		}

		public override string ToXML()
		{
			var sb = new StringBuilder();
			sb.Append("<FORMROWACTION NAME='" + this.Name + "' >");
			return sb.ToString(); //return Entity.Serialize(this);
		}
	}
}