
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

namespace  WAFMetastoreComparator
{
	[Serializable, XmlRoot("INCLUDE")]
	public class Include : BaseXMLElement
	{
		[XmlAttribute(AttributeName = "FILENAME")]
		public string FileName { get; set; }

		[XmlIgnore]
		//To display in listBox
		public  override string Name
		{
			get
			{
				return FileName;
			}
		}

		public override string ToString()
		{
			return String.Format("<INCLUDE FILE_NAME={0} >", FileName);
		}
	}

	[Serializable, XmlRoot("STRING")]
	public class MetastoreString : BaseXMLElement
	{
		//[XmlAttribute("NAME")]
		//public string Name { get; set; }

		[XmlAttribute("VALUE")]
		[DefaultValue("")]	//to ignore empty string serialization
		public string Value { get; set; }

		public override string ToString()
		{
			return String.Format("STRING name={0}", Name);
		}
	}

	[Serializable, XmlRoot("SYSTEMSTRING")]
	public class SystemString : BaseXMLElement
	{
		[XmlAttribute("NAME")]
		public string Name { get; set; }

		[XmlAttribute("VALUE")]
		[DefaultValue("")]	//to ignore empty string serialization
		public string Value { get; set; }

		public override string ToString()
		{
			return String.Format("SYSTEMSTRING name={0}", Name);
		}
	}

	[Serializable, XmlRoot("METASTORE")]//, Namespace = "http://enticy.com/WAF/Metastore/1.0")]
	//[XmlInclude(typeof(Table))]
	public class Metastore : BaseXMLElement
	{
		#region Properties

		[XmlAttribute(AttributeName = "APPLICATION_NAME")]
		public string AppName { get; set; }

		[XmlAttribute(AttributeName = "COMMENT")]
		public string Comment { get; set; }



		[XmlElement("INCLUDE")]
		public List<Include> Includes { get; set; }

		[XmlElement(typeof(Table), ElementName = "TABLE")]
		public List<Table> Tables { get; set; }

		[XmlArray(ElementName = "STRINGS")]
		[XmlArrayItem(typeof(MetastoreString), ElementName = "STRING")]
		public List<MetastoreString> Strings { get; set; }

		//[XmlElement(typeof(SystemString),ElementName = "SYSTEMSTRINGS")]
		[XmlArray(ElementName = "SYSTEMSTRINGS")]
		[XmlArrayItem(typeof(SystemString), ElementName = "SYSTEMSTRING")]

		public List<SystemString> SystemStrings { get; set; }

		//public XmlSerializerNamespaces Namespaces = new XmlSerializerNamespaces();
		//public XmlSerializerNamespaces Namespaces { get; set; }
		//public XmlSerializerNamespaces _namespaces { get; set; }

		//[XmlAttribute(AttributeName = "xmlns", Form = XmlSchemaForm.Qualified)]
		//public string xmlns { get; set; }
		//[XmlNamespaceDeclarations]
		//public XmlSerializerNamespaces xmlns
		//{
		//	get
		//	{
		//		if (_namespaces == null)
		//		{
		//			_namespaces = new XmlSerializerNamespaces();
		//			_namespaces.Add("", "http://enticy.com/WAF/Metastore/1.0");
		//		}
		//		return _namespaces;
		//	}
		//	set
		//	{
		//		_namespaces = value;
		//	}
		//}

		#endregion

		#region Methods

		public Metastore()
		{
			//xmlns = new XmlSerializerNamespaces();
			//xmlns.Add("", "http://enticy.com/WAF/Metastore/1.0");

			this.Includes = new List<Include>();
			this.Strings = new List<MetastoreString>();
			this.SystemStrings = new List<SystemString>();
			this.Tables = new List<Table>();
		}

		public Metastore(string name)
			: this()
		{
			this.AppName = name;
		}

		public static Metastore Deserialize(string xmlString, bool defaultNamespaceAttribute = false)
		{
			//XmlString = "<?xml version=\"1.0\" encoding=\"utf-16\"?>" + XmlString;
			var sr = new System.IO.StringReader(xmlString);

			Metastore meta;
			var xmlSerializer = !defaultNamespaceAttribute
													? new XmlSerializer(typeof(Metastore))
													: new XmlSerializer(typeof(Metastore), "http://enticy.com/WAF/Metastore/1.0");

			xmlSerializer.UnknownElement += xmlSerializer_UnknownElement;
			xmlSerializer.UnknownAttribute += xmlSerializer_UnknownAttribute;
			xmlSerializer.UnknownNode += xmlSerializer_UnknownNode;

			meta = (Metastore)xmlSerializer.Deserialize(sr);
			sr.Close();

			return meta;
		}

		private static void xmlSerializer_UnknownNode(object sender, XmlNodeEventArgs e)
		{

		}
		static void xmlSerializer_UnknownAttribute(object sender, XmlAttributeEventArgs e)
		{
			//TODO:
			//_listOfCustAttrs.Add(e.Attr.Name, new CustomAttribute(e.Attr.Name, e.Attr.GetType(), e.Attr.Value));
		}

		private static void xmlSerializer_UnknownElement(object sender, XmlElementEventArgs e)
		{
			//_listOfCustProps.Add(e.Element.Name, new CustomProperty(e.Element.Name, e.Element.OuterXml));
		}

		#region  Elements Methods

		public void AddInclude(Include include)
		{
			if (this.Includes == null)
				this.Includes = new List<Include>();
			this.Includes.Add(include);
		}

		public void RemoveInclude(Include include)
		{
			if (this.Includes != null)
				this.Includes.Remove(include);
		}

		public void AddTable(Table table)
		{
			//attribute.Parent = this;
			if (this.Tables == null)
				this.Tables = new List<Table>();
			this.Tables.Add(table);
		}

		public void RemoveTable(Table table)
		{
			if (this.Tables != null)
				this.Tables.Remove(table);
		}

		public void AddString(MetastoreString str)
		{
			if (this.Strings == null)
				this.Strings = new List<MetastoreString>();
			this.Strings.Add(str);
		}

		public void RemoveString(MetastoreString str)
		{
			if (this.Strings != null)
				this.Strings.Remove(str);
		}

		public void AddSystemString(SystemString str)
		{
			if (this.SystemStrings == null)
				this.SystemStrings = new List<SystemString>();
			this.SystemStrings.Add(str);
		}

		public void RemoveSystemString(SystemString str)
		{
			if (this.SystemStrings != null)
			this.SystemStrings.Remove(str);
		}

		#endregion

		public override string ToString()
		{
			return String.Format("Metastore APPLICATION_NAME={0}", AppName);
		}

		#endregion
	}
}