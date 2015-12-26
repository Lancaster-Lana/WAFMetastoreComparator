
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using System.IO;
using WAFMetastoreComparator.ENUMS;

namespace  WAFMetastoreComparator
{
	[Serializable, XmlRoot("TABLE")]
	//[XmlInclude(typeof(DBTableTypeEnum))]
	//[XmlInclude(typeof(PrimaryKeyTypeEnum))]
	//[XmlInclude(typeof(Action))]
	//[XmlInclude(typeof(TableField))]
	//[XmlInclude(typeof(Form))]
	//[XmlInclude(typeof(Search))]
	public class Table : BaseXMLElement
	{
		#region Properties

		//[XmlAttribute(AttributeName = "NAME")]
		//public string Name { get; set; }

		[XmlElement(typeof(Security), ElementName = "SECURITY")]
		public List<Security> Security { get; set; }

		[XmlAttribute("READONLY")]
		[DefaultValue(true)]
		public bool Readonly { get; set; }

		[XmlAttribute(AttributeName = "DBTABLENAME")]
		[DefaultValue("")]	//to ignore empty string serialization
		public string DBTableName { get; set; }

		[XmlAttribute(AttributeName = "CONNECTIONSTRING")]
		[DefaultValue("")]	//to ignore empty string serialization
		public string ConnectionString { get; set; }
		

		[XmlAttribute(AttributeName = "DBTABLETYPE")]
		[DefaultValue(DBTableTypeEnum.DBTableTypeUnspecified)]
		public DBTableTypeEnum DBTableType { get; set; }

		[XmlAttribute("PRIMARYKEYTYPE")]
		[DefaultValue(PrimaryKeyTypeEnum.PKeyNone)]
		public PrimaryKeyTypeEnum PrimaryKeyType { get; set; }

		[XmlAttribute(AttributeName = "PRIMARYKEYCOLNAME")]
		[DefaultValue("")]	//to ignore empty string serialization
		public string PrimaryKeyColName { get; set; }

		[XmlAttribute("PRIMARYVALUECOLNAME")]
		[DefaultValue("")]	//to ignore empty string serialization
		public string PrimaryValueColName { get; set; } //table.column

		[XmlAttribute("XMLGENERATORSERVER")]
		[DefaultValue("")]	//to ignore empty string serialization
		public string XMLGeneratorServer { get; set; }

		[XmlAttribute("HTMLGENERATORSERVER")]
		[DefaultValue("")]	//to ignore empty string serialization
		public string HTMLGeneratorServer { get; set; }

		[XmlAttribute("BUSINESSRULESSERVER")]
		[DefaultValue("")]	//to ignore empty string serialization
		public string BusinessRulesServer { get; set; }

		[XmlAttribute("SEARCHRULESSERVER")]
		[DefaultValue("")]	//to ignore empty string serialization
		public string SearchRulesServer { get; set; }

		[XmlAttribute("FK_SUGGESTSEARCH")]
		[DefaultValue("")]	//to ignore empty string serialization
		public string FK_SuggestSearch { get; set; }

		[XmlElement(typeof(Action), ElementName = "ACTION")]
		public List<Action> Actions { get; set; }

		[XmlElement(typeof(TableField), ElementName = "FIELD")]
		public List<TableField> Fields { get; set; }

		[XmlElement(typeof(Form), ElementName = "FORM")]
		public List<Form> Forms { get; set; }

		[XmlElement(typeof(Search), ElementName = "SEARCH")]
		public List<Search> Searches { get; set; }

		[XmlAttribute(AttributeName = "COMMENT")]
		[DefaultValue("")]	//to ignore empty string serialization
		public string Comment { get; set; }

		[XmlAttribute(AttributeName = "ATTACHMENT")]
		[DefaultValue("")]	//to ignore empty string serialization
		public string Attachment { get; set; }

		//[XmlArray(ElementName = "Orders")]
		//[XmlArrayItem(typeof(Order), ElementName = "order")]
		#endregion

		#region Methods

		public Table() 
		{
			this.Actions = new List<Action>();
			this.Fields = new List<TableField>();
			this.Forms = new List<Form>();
			this.Searches = new List<Search>();
		}

		public Table(string tableName): this()
		{
			this.Name = tableName;
		}

		//public override string ToXML()
		//{
		//	var sb = new StringBuilder();
		//	sb.Append("<TABLE NAME='" + this.Name + "' />");
		//	return sb.ToString();
		//	//return Table.Serialize(this);
		//}

		#region  Elements Methods

		public void AddField(TableField field)
		{
			//attribute.Parent = this;
			this.Fields.Add(field);
		}

		public void RemoveField(TableField field)
		{
			this.Fields.Remove(field);
		}

		public void AddAction(Action action)
		{
			//attribute.Parent = this;
			this.Actions.Add(action);
		}

		public void RemoveAction(Action action)
		{
			this.Actions.Remove(action);
		}

		public void AddForm(Form form)
		{
			this.Forms.Add(form);
		}

		public void RemoveForm(Form form)
		{
			this.Forms.Remove(form);
		}

		public void AddSearch(Search search)
		{
			this.Searches.Add(search);
		}

		public void RemoveSearch(Search search)
		{
			this.Searches.Remove(search);
		}


		#endregion

		#endregion

		public static Table Deserialize(string xmlString)
		{
			//XmlString = "<?xml version=\"1.0\" encoding=\"utf-16\"?>" + XmlString;
			TextReader sr = new StringReader(xmlString);
			var xmlSerializer = new XmlSerializer(typeof(Table));

			xmlSerializer.UnknownElement += xmlSerializer_UnknownElement;
			xmlSerializer.UnknownAttribute += xmlSerializer_UnknownAttribute;

			var entity = (Table)xmlSerializer.Deserialize(sr);
			sr.Close();

			return entity;
		}

		private static void xmlSerializer_UnknownAttribute(object sender, XmlAttributeEventArgs e)
		{
			//_listOfCustAttrs.Add(e.Attr.Name, new CustomAttribute(e.Attr.Name, e.Attr.GetType(), e.Attr.Value));
		}

		private static void xmlSerializer_UnknownElement(object sender, XmlElementEventArgs e)
		{
			// _listOfCustProps.Add(e.Element.Name, new CustomProperty(e.Element.Name, e.Element.OuterXml));
		}

		public override string ToString()
		{
			return string.Format("Table {0}", Name);
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