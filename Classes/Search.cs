using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Xml.Serialization;
using WAFMetastoreComparator;

namespace  WAFMetastoreComparator.ENUMS
{
	#region Search classes

	[System.Serializable, XmlRoot("SEARCH")]
	public class Search : BaseXMLElement
	{
		#region Attributes

		//[XmlAttribute("NAME")]
		//public string Name { get; set; }

		[XmlAttribute("SECURECONNECTION")]
		[DefaultValue(SecureConnectionEnum.KeepDefault)]
		public SecureConnectionEnum SecureConnection { get; set; }

		[XmlAttribute("TITLE")]
		public string Title { get; set; }//1

		[XmlAttribute("XSLTNAME")]
		public string XsltName { get; set; }

		[XmlAttribute("SEARCHRULESSERVER")]
		public string SearchRulesServer { get; set; }

		[XmlAttribute("ORDERCLAUSE")]
		public string OrderClause { get; set; }

		[XmlAttribute("SEARCHSUBMITURL")]
		public string SearchSubmitURL { get; set; }

		[XmlAttribute("AUTO_EXECUTE")]
		public bool AutoExecute { get; set; }

		[XmlAttribute("SHOW_ADD_NEW_ROW")]
		[DefaultValue(false)]
		public bool ShowAddNewRow { get; set; }

		[XmlAttribute("SHOW_DELETE")]
		[DefaultValue(false)]
		public bool ShowDelete { get; set; }

		[XmlAttribute("SHOW_SEARCH_PAGE_TOP")]
		public bool ShowSearchPageTop { get; set; }

		[XmlAttribute("SHOW_SEARCH_PAGE_BOTTOM")]
		public bool ShowSearchPageBottom { get; set; }

		[XmlAttribute("SHOW_ACTIONS_MENU_TOP")]
		public bool ShowActionsMenuTop { get; set; }

		[XmlAttribute("SHOW_ACTIONS_MENU_BOTTOM")]
		public bool ShowActionsMenuBottom { get; set; }

		[XmlAttribute("SHOW_ACTIONS_TAB")]
		public bool ShowActionsTab { get; set; }

		[XmlAttribute("SHOW_SORT_COLUMNS")]
		[DefaultValue(false)]
		public bool ShowSortColumns { get; set; }

		[XmlAttribute("SHOW_CSV_LINK")]
		public bool ShowCsvLink { get; set; }

		[XmlAttribute("HIDE_EMAIL_LINK")]
		public bool HideEmailLink { get; set; }

		[XmlAttribute("HIDE_PRINT_LINK")]
		public bool HidePrintLink { get; set; }

		[XmlAttribute("HIDE_HELP_LINK")]
		public bool HideHelpLink { get; set; }

		[XmlAttribute("COUNT_ROWS")]
		[DefaultValue(0)]
		public bool CountRows { get; set; }

		[XmlAttribute("SHOW_FETCH_NEXTN")]
		[DefaultValue(false)]
		public bool ShowFetchNextN { get; set; }

		[XmlAttribute("FETCHSIZE")]
		[DefaultValue(0)]
		public int FetchSize { get; set; }

		[XmlAttribute("RESULTTITLE")]
		[DefaultValue("")]
		public string ResultTitle { get; set; }

		[XmlAttribute("NAVIGATECOLUMNNAME")]
		[DefaultValue("")]
		public string NavigateColumnName { get; set; }

		[XmlAttribute("RESULTNAVIGATEFORM")]
		[DefaultValue("")]
		public string ResultNavigateForm { get; set; }

		[XmlAttribute("RESULTEMPTYMESSAGE")]
		[DefaultValue("")]
		public string ResultEmptyMessage { get; set; }

		[XmlAttribute("REMEMBER_SEARCH_VALUES")]
		[DefaultValue(false)]
		public bool RememberSearchValues { get; set; }

		#endregion

		#region Elements

		[XmlElement(typeof(Security), ElementName = "SECURITY")]
		public List<Security> SearchSecurity { get; set; }

		[XmlElement(typeof(SearchField), ElementName = "SEARCHFIELD")]
		public List<SearchField> SearchFields { get; set; }

		[XmlElement(typeof(SearchMenuAction), ElementName = "SEARCHMENUACTION")]
		public List<SearchMenuAction> SearchMenuActions { get; set; }

		[XmlElement(typeof(SearchRowAction), ElementName = "SEARCHROWACTION")]
		public List<SearchRowAction> SearchRowActions { get; set; }

		[XmlElement(typeof(SearchTabAction), ElementName = "SEARCHTABACTION")]
		public List<SearchTabAction> SearchTabActions { get; set; }

		#endregion

		#region Ctor

		public Search()
		{
			this.SearchSecurity = new List<Security>();
			this.SearchFields = new List<SearchField>();
			this.SearchMenuActions = new List<SearchMenuAction>();
			this.SearchTabActions = new List<SearchTabAction>();
			this.SearchRowActions = new List<SearchRowAction>();
		}

		public override string ToXML()
		{
			var sb = new StringBuilder();
			sb.Append("<SEARCH NAME='" + this.Name + "' >");
			return sb.ToString(); //return Entity.Serialize(this);
		}

		public override string ToString()
		{
			return ToXML();
		}
		#endregion

		#region  Elements Methods

		public void AddSecurity(Security security)
		{
			if (this.SearchSecurity == null)
				this.SearchSecurity = new List<Security>();

			this.SearchSecurity.Add(security);
		}

		public void RemoveSecurity(Security security)
		{
			if (this.SearchSecurity != null)
				this.SearchSecurity.Remove(security);
		}

		public void AddSearchField(SearchField field)
		{
			if (this.SearchFields == null)
				this.SearchFields = new List<SearchField>();

			this.SearchFields.Add(field);
		}

		public void RemoveSearchField(SearchField field)
		{
			if (this.SearchFields != null)
				this.SearchFields.Remove(field);
		}

		public void AddSearchMenuAction(SearchMenuAction action)
		{
			if (this.SearchMenuActions == null)
				this.SearchMenuActions = new List<SearchMenuAction>();

			this.SearchMenuActions.Add(action);
		}
		public void RemoveSearchMenuAction(SearchMenuAction action)
		{
			if (this.SearchMenuActions != null)
				this.SearchMenuActions.Remove(action);
		}

		public void AddSearchRowAction(SearchRowAction action)
		{
			if (this.SearchRowActions == null)
				this.SearchRowActions = new List<SearchRowAction>();

			this.SearchRowActions.Add(action);
		}
		public void RemoveSearchRowAction(SearchRowAction action)
		{
			if (this.SearchRowActions != null)
				this.SearchRowActions.Remove(action);
		}

		public void AddSearchTabAction(SearchTabAction action)
		{
			if (this.SearchTabActions == null)
				this.SearchTabActions = new List<SearchTabAction>();

			this.SearchTabActions.Add(action);
		}
		public void RemoveSearchTabAction(SearchTabAction action)
		{
			if (this.SearchTabActions != null)
				this.SearchTabActions.Remove(action);
		}

		#endregion
	}

	#endregion
}
