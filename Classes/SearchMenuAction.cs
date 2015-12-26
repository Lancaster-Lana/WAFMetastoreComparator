using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Xml.Serialization;

namespace  WAFMetastoreComparator
{
	[Serializable, XmlRoot("SEARCHMENUACTION")]
	public class SearchMenuAction : BaseXMLElement, IComparable
	{
		[XmlElement(typeof(Security), ElementName = "SECURITY")]
		public List<Security> Security { get; set; }

		[XmlAttribute("LABEL")]
		[DefaultValue("")]	//to ignore empty string serialization
		public string Label { get; set; }

		[XmlAttribute("URL")]
		[DefaultValue("")]	//to ignore empty string serialization
		public string Url { get; set; }

		[XmlAttribute("TABLENAME")]
		[DefaultValue("")]	//to ignore empty string serialization
		public string TableName { get; set; }

		[XmlAttribute("ACTION")]
		[DefaultValue("")]	//to ignore empty string serialization
		public string Action { get; set; }

		[XmlAttribute("SEARCHNAME")]
		[DefaultValue("")]	//to ignore empty string serialization
		public string SearchName { get; set; }

		[XmlAttribute("SUBMIT_SEARCH_FORM")]
		public bool SubmitSearchForm { get; set; }

		public override string ToXML()
		{
			var sb = new StringBuilder();
			sb.Append("<SEARCHMENUACTION NAME='" + this.Name + "' >");
			return sb.ToString(); //return Entity.Serialize(this);
		}

		public override string ToString()
		{
			return ToXML();
		}

		public int CompareTo(object obj)
		{
			if (obj == null) return -1;
			return String.Compare(Name, ((SearchMenuAction)obj).Name, StringComparison.Ordinal);
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
