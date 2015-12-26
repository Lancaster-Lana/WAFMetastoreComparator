using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Xml.Serialization;
using WAFMetastoreComparator.ENUMS;

namespace  WAFMetastoreComparator
{
	/// <summary>
	///Field class corresponding to attributes types: owner, lookup,  customer,primarykey, uniqueidentifier,
	/// </summary>
	[Serializable, XmlRoot("FIELD")]
	public class TableField : BaseXMLElement
	{
		[XmlElement(typeof(Security), ElementName = "SECURITY")]
		public List<Security> Security { get; set; }

		[XmlAttribute(AttributeName = "ATTRIBUTENAME")]
		[DefaultValue("")]	//to ignore empty string serialization
		public string AttributeName { get; set; }

		/// <summary>
		/// TODO: string with delim ,
		/// </summary>
		[XmlAttribute(AttributeName = "VALIDVALUES")]
		[DefaultValue("")]	//to ignore empty string serialization
		public string ValidValues { get; set; }

		[XmlAttribute(AttributeName = "FIELDTYPE")]
		[DefaultValue(FieldTypeEnum.None)]  //to ignore in XML
		public FieldTypeEnum FieldType { get; set; }

		[XmlAttribute(AttributeName = "DEFAULTTYPE")]
		[DefaultValue(FieldDefaultTypeEnum.FieldDefaultTypeNone)]  //to ignore in XML
		public FieldDefaultTypeEnum DefaultType { get; set; }

        [XmlAttribute(AttributeName = "FIELDTARGET")]
        [DefaultValue(FieldTargetEnum.None)]  //to ignore in XML
        public FieldTargetEnum FieldTarget { get; set; }

		/// <summary>
		/// TODO: Enum with Enable, Disable
		/// </summary>
		[XmlAttribute(AttributeName = "DEFAULT")]
		[DefaultValue("")]	//to ignore empty string serialization
		public string Default { get; set; }

		[XmlAttribute(AttributeName = "LABEL")]
		[DefaultValue("")]	//to ignore empty string serialization
		public string Label { get; set; }

		[XmlAttribute(AttributeName = "COMMENT")]
		[DefaultValue("")]	//to ignore empty string serialization
		public string Comment { get; set; }

		[XmlAttribute(AttributeName = "DBCOLUMNNAME")]
		[DefaultValue("")]	//to ignore empty string serialization
		public string DBColumnName { get; set; }

		[XmlAttribute(AttributeName = "FIELDSOURCE")]
		[DefaultValue("")]	//to ignore empty string serialization
		public string FieldSource { get; set; }

		[XmlAttribute(AttributeName = "VALIDVALUESTYPE")]
		[DefaultValue(FieldValidValuesTypeEnum.None)]
		public FieldValidValuesTypeEnum ValidValuesType { get; set; }

		[XmlAttribute(AttributeName = "FK_DBTABLE")]
		[DefaultValue("")]	//to ignore empty string serialization
		public string FK_DBTABLE { get; set; }

		[XmlAttribute(AttributeName = "FK_PRIMARYKEYCOLNAME")]
		[DefaultValue("")]	//to ignore empty string serialization
		public string FK_PRIMARYKEYCOLNAME { get; set; }

		[XmlAttribute(AttributeName = "FK_PRIMARYVALUECOLNAME")]
		[DefaultValue("")]	//to ignore empty string serialization
		public string FK_PRIMARYVALUECOLNAME { get; set; }

		[XmlAttribute(AttributeName = "FK_DISPLAY_AS_LIST")]
		[DefaultValue(false)]
		public bool FK_DISPLAY_AS_LIST { get; set; }

		[XmlAttribute(AttributeName = "FK_ADDITIONALWHERECLAUSE")]
		[DefaultValue("")]	//to ignore empty string serialization
		public string FK_AdditionalWhereClause { get; set; }

		[XmlAttribute(AttributeName = "FK_ORDERCLAUSE")]
		[DefaultValue("")]	//to ignore empty string serialization
		public string FK_OrderClause { get; set; }

		[XmlAttribute(AttributeName = "READONLY")]
		[DefaultValue(true)]
		public bool ReadOnly { get; set; }

		[XmlAttribute(AttributeName = "REQUIRED")]
		public bool Required { get; set; }

		[XmlAttribute(AttributeName = "REQUIREDERRORTEXT")]
		[DefaultValue("")]	//to ignore empty string serialization
		public string RequiredErrorText { get; set; }

		[XmlAttribute(AttributeName = "USE_FOR_CONCURRENCY")]
		[DefaultValue(false)]
		public bool UserForConcurrency { get; set; }

		[XmlAttribute(AttributeName = "SEARCHTYPE")]
		[DefaultValue(SearchTypeEnum.None)]
		public SearchTypeEnum SearchType { get; set; }

		[XmlAttribute(AttributeName = "EXCLUDE_ON_SEARCH_FORM")]
		[DefaultValue(false)]
		public bool ExcludeOnSearchForm { get; set; }

		[XmlAttribute(AttributeName = "EXCLUDE_ON_SEARCH_WHERE")]
		[DefaultValue(false)]
		public bool ExcludeOnSearchWhere { get; set; }

		[XmlAttribute(AttributeName = "EXCLUDE_ON_EDIT_FORM")]
		[DefaultValue(false)]
		public bool ExcludeOnEditForm { get; set; }

		[XmlAttribute(AttributeName = "FK_SHOW_NAVIGATE")]
		[DefaultValue(false)]
		public bool FK_SHOW_NAVIGATE { get; set; }

		[XmlAttribute(AttributeName = "FK_NAVIGATETABLE")]
		[DefaultValue("")]	//to ignore empty string serialization
		public string FK_NAVIGATETABLE { get; set; }

		[XmlAttribute(AttributeName = "FK_NAVIGATEFORM")]
		[DefaultValue("")]	//to ignore empty string serialization
		public string FK_NAVIGATEFORM { get; set; }

		[XmlAttribute(AttributeName = "WIDTH")]
		[DefaultValue(0)]	//to ignore 0 serialization
		public int Width { get; set; }

		[XmlAttribute(AttributeName = "DISPLAYWIDTH")]
		[DefaultValue(0)]	//to ignore 0serialization
		public int DisplayWidth { get; set; }

		[XmlAttribute(AttributeName = "HIDE_ON_EDIT_FORM")]
		[DefaultValue(false)]
		public bool HIDE_ON_EDIT_FORM { get; set; }

		[XmlAttribute(AttributeName = "EXCLUDE_ON_SEARCH_RESULTS")]
		[DefaultValue(false)]
		public bool EXCLUDE_ON_SEARCH_RESULTS { get; set; }

		[XmlAttribute(AttributeName = "REGULAREXPRESSION")]
		[DefaultValue("")]	//to ignore empty string serialization
		public string ReqularExpression { get; set; }

		[XmlAttribute(AttributeName = "REGULAREXPRESSIONERRORTEXT")]
		[DefaultValue("")]	//to ignore empty string serialization
		public string ReqularExpressionErrorText { get; set; }

		public TableField()
		{

		}

		#region  Non serializable variables and properties.

		//protected SortedList<string, CustomProperty> _customProperties = new SortedList<string, CustomProperty>();
		//protected SortedList<string, CustomAttribute> _customAttributes = new SortedList<string, CustomAttribute>();

		//private static SortedList<string, CustomProperty> _listOfCustProps = new SortedList<string, CustomProperty>();
		//private static SortedList<string, CustomAttribute> _listOfCustAttrs = new SortedList<string, CustomAttribute>();

		/// <summary>
		/// Get type from attributes node
		/// </summary>
		/*  
		 * //protected string _attributetype = "NText";
		private string AttributeType
		{
				get { return _attributetype; }
				set { _attributetype = value; }
		}*/

		///// <summary>
		/////List of unknown attributes of the analizing field.
		///// </summary>
		//[XmlIgnore]
		//public SortedList<string, CustomAttribute> CustomAttributes
		//{
		//	get { return _customAttributes; }
		//	set { _customAttributes = value; }
		//}

		///// <summary>
		/////List of unknown properties of the analizing field.
		///// </summary>
		//[XmlIgnore]
		//public SortedList<string, CustomProperty> CustomProperties
		//{
		//	get { return _customProperties; }
		//	set { _customProperties = value; }
		//}


		///// <summary>
		///// It is property name for all custom attributes of field (a list of custom attributes)
		///// </summary>
		///// <returns></returns>
		//public static string GetCustomAttributeCollectionName()
		//{
		//	return "CustomAttributes";
		//}

		///// <summary>
		///// It is property name for all custom properties of field (a list of custom properties )
		///// </summary>
		///// <returns></returns>
		//public static string GetCustomPropertyCollectionName()
		//{
		//	return "CustomProperties";
		//}

		#endregion

		private static void xmlSerializer_UnknownAttribute(object sender, XmlAttributeEventArgs e)
		{
			//TODO:
			//_listOfCustAttrs.Add(e.Attr.Name, new CustomAttribute(e.Attr.Name, e.Attr.GetType(), e.Attr.Value));
		}

		private static void xmlSerializer_UnknownElement(object sender, XmlElementEventArgs e)
		{
			//_listOfCustProps.Add(e.Element.Name, new CustomProperty(e.Element.Name, e.Element.OuterXml));
		}

		public override string ToXML()
		{
			var sb = new StringBuilder();
			sb.Append("<FIELD NAME='" + this.Name + "' >");
			return sb.ToString(); //return Entity.Serialize(this);
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