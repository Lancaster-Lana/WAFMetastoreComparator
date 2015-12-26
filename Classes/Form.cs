using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;
using WAFMetastoreComparator.ENUMS;

namespace  WAFMetastoreComparator
{
    [Serializable, XmlRoot("FORM")]
    //[XmlInclude(typeof(LinkedForm))]
    public class Form : BaseXMLElement
    {
        [XmlElement(typeof(Security), ElementName = "SECURITY")]
        public List<Security> Security { get; set; }

        [XmlAttribute("SECURECONNECTION")]
        [DefaultValue(SecureConnectionEnum.KeepDefault)]
        public SecureConnectionEnum SecureConnection { get; set; }

        [XmlAttribute("FORMSUBMISSION")]
        [DefaultValue(FormSubmissionEnum.UseDefault)]
        public FormSubmissionEnum FormSubmission { get; set; }

        [XmlAttribute("TITLE")]
        [DefaultValue("")]	//to ignore empty string serialization
        public string Title { get; set; }

        [XmlAttribute("READONLY")]
        [DefaultValue(true)]
        public bool Readonly { get; set; }

        [XmlAttribute("TABLENAVIGATEFORM")]
        [DefaultValue("")]	//to ignore empty string serialization
        public string TableNavigateForm { get; set; }

        [XmlAttribute("TABLENEWFORM")]
        [DefaultValue("")]	//to ignore empty string serialization
        public string TableNewForm { get; set; }

        [XmlAttribute("TABLENAVIGATECOLNAME")]
        [DefaultValue("")]	//to ignore empty string serialization
        public string TableNavigateColForm { get; set; }

        [XmlAttribute("TABLEMINEMPTYROWS")]
        [DefaultValue(0)]//3
        public int TableInEmptyRows { get; set; }

        [XmlAttribute("TABLECOLLAPSESTATE")]
        [DefaultValue(TableCollapseStateEnum.None)]//4
        public TableCollapseStateEnum TableCollapseState { get; set; }

        [XmlAttribute(AttributeName = "GRIDCOLUMNS")]
        [DefaultValue(0)]
        public int GridColumnsCount { get; set; }

        [XmlAttribute(AttributeName = "BUSINESSRULESSERVER")]
        [DefaultValue("")]	//to ignore empty string serialization
        public string BusinessRulesServer { get; set; }

        [XmlAttribute(AttributeName = "XMLGENERATORSERVER")]
        [DefaultValue("")]	//to ignore empty string serialization
        public string XMLGeneratorServer { get; set; }

        [XmlAttribute("FOCUSFIELD")]
        [DefaultValue("")]	//to ignore empty string serialization
        public string FocusField { get; set; }

        [XmlAttribute("HIDE_HELP_LINK")]
        [DefaultValue(false)]
        public bool HideHelpLink { get; set; }

        [XmlAttribute("HIDE_EMAIL_LINK")]
        [DefaultValue(false)]
        public bool HideEmailLink { get; set; }

        [XmlAttribute("HIDE_PRINT_LINK")]
        [DefaultValue(false)]
        public bool HidePrintLink { get; set; }

        [XmlAttribute("SHOW_REQUIRED_INSTRUCTION_BOTTOM")]
        [DefaultValue(false)]
        public bool ShowRequiredInstructionBottom { get; set; }

        [XmlAttribute("SHOW_ADD_NEW_ROW")]
        [DefaultValue(false)]
        public bool ShowAddNewRow { get; set; }

        [XmlAttribute("SHOW_DELETE")]
        [DefaultValue(false)]
        public bool ShowDelete { get; set; }

        [XmlAttribute("SHOW_ACTIONS_MENU_BOTTOM")]
        [DefaultValue(false)]
        public bool ShowActionsMenuBottom { get; set; }

        [XmlAttribute("SHOW_SORT_COLUMNS")]
        [DefaultValue(false)]
        public bool ShowSortColumns { get; set; }

        [XmlAttribute("SHOW_ACTIONS_TAB")]
        [DefaultValue(false)]
        public bool ShowActionsTab { get; set; }

        [XmlAttribute("SELECTEDTAB")]
        [DefaultValue("")]
        public string SelectedTab { get; set; }

        [XmlAttribute("WHERECLAUSE")]
        [DefaultValue("")]	//to ignore empty string serialization
        public string WhereClause { get; set; }

        [XmlAttribute("ORDERCLAUSE")]
        [DefaultValue("")]	//to ignore empty string serialization
        public string OrderClause { get; set; }

        [XmlAttribute("XSLTNAME")]
        [DefaultValue("")]	//to ignore empty string serialization
        public string XsltName { get; set; }

        [XmlElement(typeof(FormField), ElementName = "FORMFIELD")]
        public List<FormField> FormFields { get; set; }

        /// <summary>
        /// TODO: Fetch from existing actions names of the parent table
        /// </summary>
        [XmlAttribute("DEFAULTMENUACTION")]
        public string DefaultMenuActions { get; set; }

        [XmlElement(typeof(FormMenuAction), ElementName = "FORMMENUACTION")]
        public List<FormMenuAction> FormMenuActions { get; set; }

        [XmlElement(typeof(FormTabAction), ElementName = "FORMTABACTION")]
        public List<FormTabAction> FormTabActions { get; set; }

        [XmlElement(typeof(FormRowAction), ElementName = "FORMROWACTION")]
        public List<FormRowAction> FormRowActions { get; set; }

        [XmlElement(typeof(LinkedForm), ElementName = "LINKEDFORM")]
        public List<LinkedForm> LinkedForms { get; set; }

        public Form()
        {
            this.Security = new List<Security>();
            this.FormFields = new List<FormField>();
            this.FormMenuActions = new List<FormMenuAction>();
            this.FormTabActions = new List<FormTabAction>();
            this.FormRowActions = new List<FormRowAction>();
        }

        public override string ToXML()
        {
            var sb = new StringBuilder();
            sb.Append("<FORM NAME='" + this.Name + "' >");
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

        public void AddFormField(FormField field)
        {
            if (this.FormFields == null)
                this.FormFields = new List<FormField>();

            this.FormFields.Add(field);
        }

        public void RemoveFormField(FormField field)
        {
            if (this.FormFields != null)
                this.FormFields.Remove(field);
        }

        public void AddFormMenuAction(FormMenuAction action)
        {
            if (this.FormMenuActions == null)
                this.FormMenuActions = new List<FormMenuAction>();

            this.FormMenuActions.Add(action);
        }

        public void AddFormMenuActions(IList<FormMenuAction> actionsList)
        {
            if (this.FormMenuActions == null)
                this.FormMenuActions = new List<FormMenuAction>();

            //this.FormMenuActions.AddRange(actionsList);

            IList<FormMenuAction> clearList = actionsList.Where(ma => !FormMenuActions.Contains(ma)).ToList();
            this.FormMenuActions.AddRange(clearList);

            //foreach (var menuaction in actionsList)
            //	if (!FormMenuActions.Contains(menuaction))
            //		this.FormMenuActions.Add(menuaction);

        }

        public void RemoveFormMenuAction(FormMenuAction action)
        {
            if (this.FormMenuActions != null)
                this.FormMenuActions.Remove(action);
        }

        public void RemoveFormMenuActions(IList<string> actionsList)
        {
            if (this.FormMenuActions != null)
                this.FormMenuActions.RemoveAll(a => actionsList.Contains(a.Name));
        }

        public void ClearFormMenuActions()
        {
            if (this.FormMenuActions != null)
                this.FormMenuActions.Clear();
        }

        public void AddFormRowAction(FormRowAction action)
        {
            if (this.FormRowActions == null)
                this.FormRowActions = new List<FormRowAction>();

            this.FormRowActions.Add(action);
        }
        public void RemoveFormRowAction(FormRowAction action)
        {
            if (this.FormRowActions != null)
                this.FormRowActions.Remove(action);
        }

        public void AddFormTabAction(FormTabAction action)
        {
            if (this.FormTabActions == null)
                this.FormTabActions = new List<FormTabAction>();

            this.FormTabActions.Add(action);
        }
        public void RemoveFormTabAction(FormTabAction action)
        {
            if (this.FormTabActions != null)
                this.FormTabActions.Remove(action);
        }


        public void AddLinkedForm(LinkedForm linkedForm)
        {
            if (this.LinkedForms == null)
                this.LinkedForms = new List<LinkedForm>();

            this.LinkedForms.Add(linkedForm);
        }

        public void RemoveLinkedForm(LinkedForm linkedForm)
        {
            if (this.LinkedForms != null)
                this.LinkedForms.Remove(linkedForm);
        }

        #endregion

        private static object _obj = new object();
        private Dictionary<string, string> _attributes;
        /// <summary>
        /// Type simple attributes-value (non-arrays)
        /// </summary>
        [XmlIgnore]
        public Dictionary<string, string> Attributes
        {
            get
            {
                 //XmlNode xmlNode = this.ToXML();
                 //var attributesNodes = xmlNode.Attributes.Cast<XmlAttribute>();
                 //attributes = attributesList.ToDictionary(a => a.Name, a => a.Value);

                lock (_obj)
                {
                    if (_attributes == null)
                    {
                        PropertyInfo[] properties = typeof(Form).GetProperties();
                        _attributes = new Dictionary<string, string>();

                        foreach (PropertyInfo prop in properties)
                        {
                            string attrName = prop.Name;

                            object attrValue = prop.GetValue(this, null); //prop.GetConstantValue().ToString();
                            if (attrValue is ICollection || attrValue == null)
                                continue;
                            if (!_attributes.ContainsKey(attrName))
                                _attributes.Add(attrName, Convert.ToString(attrValue));
                        }
                    }
                }
                return _attributes;

            }
        }
    }

    [Serializable, XmlRoot("LINKEDFORM")]
    public class LinkedForm : Form
    {
        [XmlAttribute("TABLENAME")]
        public string TableName { get; set; }

        [XmlAttribute("FORMNAME")]
        public string FormName { get; set; }

        /// <summary>
        /// Fake field for GUI dispalay in list\combobox
        /// </summary>
        [XmlIgnore]
        public string Description
        {
            get
            {
                return String.Format("_t:{0} _f:{1}", TableName, FormName);
            }
        }

        [XmlAttribute("FOREIGNKEYCOLUMN")]
        [DefaultValue("")]	//to ignore empty string serialization
        public string FKColumn { get; set; }

        public override string ToXML()
        {
            var sb = new StringBuilder();
            sb.Append("<LINKEDFORM ='" + this.Description + "' >");
            return sb.ToString();
        }

        public override string ToString()
        {
            return ToXML();
        }
    }
}