using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using System.IO;
using System.Text;
using WAFMetastoreComparator.ENUMS;

namespace  WAFMetastoreComparator
{
	//[SerializableAttribute, XmlRoot]
	//public enum AttributeRequiredLevel { None, SystemRequired, Required, Recommended, ReadOnly }

	[Serializable, XmlRoot("ACTION")]
	public class Action : BaseXMLElement, IComparable
	{
		//[XmlAttribute("NAME")]
		//public string Name { get; set; }

		[XmlElement(typeof(Security), ElementName = "SECURITY")]
		public List<Security> Security { get; set; }

		[XmlAttribute("SECURECONNECTION")]
		public SecureConnectionEnum SecureConnection { get; set; }

		[XmlAttribute("ACTION")]
		[DefaultValue("")]	//to ignore empty string serialization
		public string ActionName { get; set; }

		[XmlAttribute("LABEL")]
		[DefaultValue("")]	//to ignore empty string serialization
		public string Label { get; set; }

		[XmlAttribute("TABLENAME")]
		[DefaultValue("")]	//to ignore empty string serialization
		public string TableName { get; set; }

		[XmlAttribute("FORMNAME")]
		[DefaultValue("")]	//to ignore empty string serialization
		public string FormName { get; set; }

		[XmlAttribute("DESCRIPTION")]
		[DefaultValue("")]	//to ignore empty string serialization
		public string Description { get; set; }

		[XmlAttribute("TITLE")]
		[DefaultValue("")]	//to ignore empty string serialization
		public string Title { get; set; }

		[XmlAttribute("NEXTACTIONNAME")]
		[DefaultValue("")]	//to ignore empty string serialization
		public string NextActionName { get; set; }

		[XmlAttribute("URL")]
		[DefaultValue("")]	//to ignore empty string serialization
		public string Url { get; set; }

		[XmlAttribute("SUBMIT_EDIT_FORM")]
		[DefaultValue(false)]
		public bool SubmitEditForm { get; set; }

		public Action()
		{
		}

		public Action(string name)
		{
			this.Name = name;
		}

		public override string ToXML()
		{
			var sb = new StringBuilder();
			sb.Append("<ACTION NAME='" + this.Name + "' >");
			return sb.ToString(); //return Entity.Serialize(this);
		}

		public override string ToString()
		{
			return ToXML();
		}

		public int CompareTo(object obj)
		{
			if (obj == null) return -1;
			return Name.CompareTo(((Action)obj).Name);
		}

		#region  Elements Methods

		public void AddSecurity(Security security)
		{
			if (this.Security == null)
				this.Security = new List<Security>();

			this.Security.Add(security);
		}

		public void RemoveSecurity(Security security)
		{
			if (this.Security != null)
				this.Security.Remove(security);
		}

		#endregion
	}
}