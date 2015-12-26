using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Xml.Serialization;
using WAFMetastoreComparator.ENUMS;

namespace  WAFMetastoreComparator
{
	[System.Serializable, XmlRoot("SEARCHFIELD")]
	public class SearchField : BaseXMLElement
	{
		[XmlElement(typeof(Security), ElementName = "SECURITY")]
		public List<Security> Security { get; set; }

        [XmlAttribute(AttributeName = "LABEL")]
        [DefaultValue("")]	//to ignore empty string serialization
        public string Label { get; set; }

        [XmlAttribute(AttributeName = "SEARCHTYPE")]
        [DefaultValue(SearchTypeEnum.None)]
        public SearchTypeEnum SearchType { get; set; }

		[XmlAttribute("HIDE_ON_SEARCH_FORM")]
		public bool HideOnSearchForm { get; set; }

        [XmlAttribute("HIDE_ON_EDIT_FORM")]
        public bool HideOnEditForm { get; set; }

        [XmlAttribute("EXCLUDE_ON_SEARCH_FORM")]
        public bool ExcludeOnSearchForm { get; set; }

        [XmlAttribute("EXCLUDE_ON_SEARCH_RESULTS")]
        public bool ExcludeOnSearchResults { get; set; }

		public override string ToXML()
		{
			var sb = new StringBuilder();
			sb.Append("<SEARCHFIELD NAME='" + this.Name + "' >");
			return sb.ToString();
		}

		public override string ToString()
		{
			return ToXML();
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
