using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;

namespace  WAFMetastoreComparator
{
    [Serializable]
    public abstract class BaseXMLElement
    {
        #region Properties

        [XmlAttribute("NAME")]
        [DefaultValue("")]	//to ignore empty string serialization
        public virtual string Name { get; set; }

        #endregion

        #region Memeber Methods

        public virtual string ToXML()
        {
            var stringwriter = new StringWriter();
            var serializer = new XmlSerializer(this.GetType());
            serializer.Serialize(stringwriter, this);
            return stringwriter.ToString();
        }

        public BaseXMLElement LoadFromXMLString(string xmlText)
        {
            var stringReader = new StringReader(xmlText);
            var serializer = new XmlSerializer(this.GetType());
            return serializer.Deserialize(stringReader) as BaseXMLElement;
        }

        public void SaveToXMLFile(string fileName, bool includeNamespaceAttributes = false)
        {
            using (var sr = new StreamWriter(fileName))
            {
                var xmlSerializer = new XmlSerializer(this.GetType());
                if (includeNamespaceAttributes)
                    xmlSerializer.Serialize(sr, this);
                else
                {
                    //create own namespaces for empty output
                    var emptyNS = new XmlSerializerNamespaces();
                    emptyNS.Add("", ""); //add an empty namespace to avoid xmlns:xsd, etc.

                    xmlSerializer.Serialize(sr, this, emptyNS);
                }
            }
        }

        #endregion

        #region Static Methods

        public static T Deserialize<T>(string xmlText, bool defaultNamespaceAttribute = false)
        {
            if (String.IsNullOrWhiteSpace(xmlText)) return default(T);

            using (var stringReader = new StringReader(xmlText))
            {
                XmlSerializer serializer = !defaultNamespaceAttribute
                                                                                        ? new XmlSerializer(typeof(T))
                                                                                        : new XmlSerializer(typeof(T), "http://enticy.com/WAF/Metastore/1.0");

                return (T)serializer.Deserialize(stringReader);
            }
        }

        //public static void Serialize<T>(T entity, string fileName)
        //{
        //	//string XmlString = string.Empty; //"<?xml version=\"1.0\" encoding=\"utf-16\"?>" + XmlString;
        //	using (var sr = new StreamWriter(fileName))
        //	{
        //		var xmlSerializer = new XmlSerializer(entity.GetType() //typeof(T)
        //			//,new[]
        //			//{
        //			//			typeof(Table)
        //			//			//typeof(Form), 
        //			//			//typeof(Search), 
        //			//			//typeof(Security)
        //			//}
        //			);
        //		xmlSerializer.Serialize(sr, entity);
        //	}
        //}
        //public static string Serialize(object dataToSerialize, string fileName)
        //{
        //	if (dataToSerialize == null) return null;

        //	using (var stringwriter = new StringWriter())
        //	{
        //		var serializer = new XmlSerializer(dataToSerialize.GetType());
        //		serializer.Serialize(stringwriter, dataToSerialize);
        //		return stringwriter.ToString();
        //	}
        //}

        #endregion
    }
}