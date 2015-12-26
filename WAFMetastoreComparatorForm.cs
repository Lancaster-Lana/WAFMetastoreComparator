using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Linq;
using WAFMetastoreComparator.ENUMS;
using System.ComponentModel;
using WAFMetastoreComparator.Properties;
using WAFMetastoreComparator.Report;

namespace WAFMetastoreComparator
{
	public partial class WAFMetastoreComparatorForm : System.Windows.Forms.Form
	{
		#region Constants

		const string TAG_NAME = "NAME";
		const string TAG_TABLENAME = "TABLENAME"; //key for linkedtable, for example 
		const string TAG_ATTR_KEY = "Key"; //attribute doesn't have a NAME, but pair <key, value> for attribute

		const string TAG_ATTRIBUTE = "ATTRIBUTE";
		const string TAG_TABLE = "TABLE";
		const string TAG_SECURITY = "SECURITY";
		const string TAG_FIELD = "FIELD";
		const string TAG_ACTION = "ACTION";
		const string TAG_FORM = "FORM";
		const string TAG_SEARCH = "SEARCH";

		//Security is a special tag without NAME, ID
		const string TAG_SECURITY_ACTION = "ACTION"; //Action=AllowAny, RecordCreate, RecordRead, RecordUpdate

		#endregion

		#region Variables

		string _firstXMLFilePath;
		XmlDocument _firstXMLDocument;
		DataSet _firstMetastoreDS;//DataSet with first(top) TABLE Attributes Actions Fields Forms

		string _secondXMLFilePath;
		XmlDocument _secoundXMLDocument;
		DataSet _seconMetastoreDS; //DataSet with second(botton) TABLE Attributes Actions Fields Forms


		#region Different current TABLEs elements

		Dictionary<string, string> _firstTableAttributes = new Dictionary<string, string>();
		Dictionary<string, string> _secondTableAttributes = new Dictionary<string, string>();
		List<string> _firstOriginalAttrsNames;
		List<string> _secondOriginalAttrsNames;
		Dictionary<string, DifferPropertiesDictionary> _diffAttrs;//differ attributes of 2 Tables

		Dictionary<string, TableField> _firstTableFields = new Dictionary<string, TableField>();
		Dictionary<string, TableField> _secondTableFields = new Dictionary<string, TableField>();
		List<string> _firstOriginalFieldsNames;
		List<string> _secondOriginalFieldsNames;
		Dictionary<string, DifferPropertiesDictionary> _diffFields;//differ fields of 2 Tables

		Dictionary<string, Action> _firstTableActions = new Dictionary<string, Action>();
		Dictionary<string, Action> _secondTableActions = new Dictionary<string, Action>();
		List<string> _firstOriginalActionsNames;
		List<string> _secondOriginalActionsNames;
		Dictionary<string, DifferPropertiesDictionary> _diffActions;//differ actions of 2 Tables

		Dictionary<string, Form> _firstTableForms = new Dictionary<string, Form>();
		Dictionary<string, Form> _secondTableForms = new Dictionary<string, Form>();
		List<string> _firstOriginalFormsNames;
		List<string> _secondOriginalFormsNames;
		Dictionary<string, DifferPropertiesDictionary> _diffForms;//differ forms of 2 Tables

		Dictionary<string, Search> _firstTableSearches = new Dictionary<string, Search>();
		Dictionary<string, Search> _secondTableSearches = new Dictionary<string, Search>();
		List<string> _firstOriginalSearchesNames;
		List<string> _secondOriginalSearchesNames;
		Dictionary<string, DifferPropertiesDictionary> _diffSearches; //differ searches of 2 Tables


		#endregion

		#region Different current FORMs elements

		List<string> _firstFormOriginalAttrsNames = null;
		List<string> _secondFormOriginalAttrsNames = null;
		Dictionary<string, DifferPropertiesDictionary> _diffFormsAttributes = null;

		List<string> _firstFormOriginalSecurities = null;
		List<string> _secondFormOriginalSecurities = null;
		Dictionary<string, DifferPropertiesDictionary> _diffFormsSecurities = null;

		List<string> _firstFormOriginalFieldsNames = null;
		List<string> _secondFormOriginalFieldsNames = null;
		Dictionary<string, DifferPropertiesDictionary> _diffFormsFields = null;

		List<string> _firstFormOriginalActionsNames = null;
		List<string> _secondFormOriginalActionsNames = null;
		Dictionary<string, DifferPropertiesDictionary> _diffFormsActions = null;

		List<string> _firstFormOriginalLinkedFormsNames = null;
		List<string> _secondFormOriginalLinkedFormsNames = null;
		Dictionary<string, DifferPropertiesDictionary> _diffFormsLinkedForms = null;

		//	_firstFormOriginalAttrsNames, _secondFormOriginalAttrsNames
		//_firsttFormOriginalSecurities, _secondtFormOriginalSecurities
		//_firstFormOriginalFieldsNames, _secondFormOriginalFieldsNames
		//_firstFormOriginalActionsNames, _secondFormOriginalActionsNames

		#endregion

		static string[] wordpadpath = new string[] { };

		#endregion

		#region Properties

		protected string CurrentTable { get; set; }

		protected string CurrentForm
		{
			get
			{
				return currentFormCombo.Text;
			}
		}

		[DefaultValue("attributes")]
		protected FormAnalyzingType CurrentFormAnalyzingPart
		{
			get
			{
				if (formAttributesRadioButton.Checked)
					return FormAnalyzingType.attributes;
				else if (formSecurityRadioButton.Checked)
					return FormAnalyzingType.security;
				else if (formFieldsRadioButton.Checked)
					return FormAnalyzingType.fields;
				else if (formActionsRadioButton.Checked)
					return FormAnalyzingType.actions;
				else if (linkedFormsRadioButton.Checked)
					return FormAnalyzingType.linkedforms;

				return FormAnalyzingType.wholeform;
			}
		}

		#endregion

		#region Ctor

		public WAFMetastoreComparatorForm()
		{
			InitializeComponent();
		}

		public WAFMetastoreComparatorForm(XmlDocument firstMetastoreXML, XmlDocument secoundMetastoreXML)
		{
			InitializeComponent();
			LoadWAFMetastores(firstMetastoreXML, secoundMetastoreXML);
		}

		private void WAFMetastoreForm_Load(object sender, EventArgs e)
		{
			LoadSettings();

			ComparationFinished();
		}

		private void WAFMetastoreForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			Settings.Default.Save();
		}

		#endregion

		#region Menu ToolBars handlers

		private void loadCustomizationsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			StartLoadingMetastoresFiles();
		}

		private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SettingsDialog sd = new SettingsDialog();
			if (sd.ShowDialog() == DialogResult.OK)
				LoadSettings();
		}

		private void analyzeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			bool showOnlyUpdated = showDifferRadioButton.Checked;
			ChangeComparatorView(ComparatorView.analyze, showOnlyUpdated);
		}

		private void treeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			bool showOnlyUpdated = showDifferRadioButton.Checked;
			ChangeComparatorView(ComparatorView.tree, showOnlyUpdated);
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		/// <summary>
		/// Left menu - tables list 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void tablesListBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			ListView list = (ListView)sender;
			if (list.SelectedItems.Count >= 1)
			{
				CurrentTable = ((ListView)sender).SelectedItems[0].Text.Trim();
				currentEntityLabel.Text = CurrentTable;

				NavigateToNodeOnTree(firstTreeView, CurrentTable);
				NavigateToNodeOnTree(secondTreeView, CurrentTable);
			}

			//If tables (the left) listbox table selected in the meantime we  are on "Analyze" (second) tab
			if (viewsElemsTabControl.SelectedTab.Tag.Equals(ComparatorView.analyze.ToString()))
			{
				bool showOnlyUpdated = showDifferRadioButton.Checked;
				AnalyzeCustomizations(showOnlyUpdated);
			}
		}

		private void refreshButton_Click(object sender, EventArgs e)
		{
			AnalyzeCustomizations(true);
		}

		#endregion

		#region Load data to main controls: the left menu list, 1&2 trees of TABLES

		private void LoadSettings()
		{
			firstTreeView.BackColor = Settings.Default.FirstColor;
			secondTreeView.BackColor = Settings.Default.SecondColor;

			diffPropsPictureBox.BackColor = Settings.Default.DifferColor;
			absentPropsPictureBox.BackColor = Settings.Default.AbsentColor;
			currentCellTextBox.Visible = Settings.Default.ShowCurrentCellContentOnForm;
		}

		private void StartLoadingMetastoresFiles()
		{
			//Show Status
			SetStatusMessage(Strings.loadingBegin);

			LoadMetastoresDialog loadDial = new LoadMetastoresDialog();
			if (loadDial.ShowDialog() == DialogResult.OK)
			{
				//1. If Loading XMLDocuments success
				if (LoadWAFMetastores(loadDial.firstCustFilePath, loadDial.secondCustFilePath))
				{
					_firstXMLFilePath = loadDial.firstCustFilePath;
					_secondXMLFilePath = loadDial.firstCustFilePath;

					//2. Detect original tables names
					List<string> entLst1 = GetTablesNamesFromDocument(_firstXMLDocument);
					List<string> entLst2 = GetTablesNamesFromDocument(_secoundXMLDocument);

					List<string> firstOriginalTablesList = new List<string>();
					List<string> secondOriginTablesList = new List<string>();
					List<string> commonTablesList = new List<string>();

					CompareHelper.DetectCommonAndOriginalNames(entLst1, entLst2, out firstOriginalTablesList, out secondOriginTablesList, out commonTablesList);

					//Load list of tables
					LoadTablesToList(firstOriginalTablesList, secondOriginTablesList, commonTablesList);

					//4.Switch Views
					ChangeComparatorView(ComparatorView.tree, false);

					//5. Show status 
					SetStatusMessage(Strings.loadingSuccess);
				}
			}
			else
				SetStatusMessage(String.Empty);
		}

		/// <summary>
		/// Load first abd second Metastore.xml files
		/// </summary>
		/// <param name="firstxmlfile"></param>
		/// <param name="secondxmlfile"></param>
		/// <returns></returns>
		private bool LoadWAFMetastores(string firstxmlfile, string secondxmlfile)
		{
			bool areLoaded = true;

			if ((firstxmlfile == null) || (secondxmlfile == null))
			{
				MessageBox.Show(Strings.customizationFilePathFailed);
				areLoaded = false;
			}
			else
			{
				try
				{
					//1. Load first&secobd Metastore files to compare
					_firstXMLDocument = new XmlDocument();
					_firstXMLDocument.Load(firstxmlfile);

					//DataSet firstMetaDS = new DataSet();
					//firstMetaDS.ReadXml(new XmlTextReader(new StringReader(_firstXMLDocument.DocumentElement.OuterXml)));

					_secoundXMLDocument = new XmlDocument();
					_secoundXMLDocument.Load(secondxmlfile);

					//2. Load data to trees/grids
					LoadTrees(_firstXMLDocument, _secoundXMLDocument);
				}
				catch (Exception ex)
				{
					areLoaded = false;
					MessageBox.Show(ex.Message);
				}
			}
			return areLoaded;
		}

		private bool LoadWAFMetastores(XmlDocument firstxmlDoc, XmlDocument secondxmlDoc)
		{
			bool areLoaded = true;
			try
			{
				_firstXMLDocument = firstxmlDoc;
				_secoundXMLDocument = secondxmlDoc;

				LoadTrees(_firstXMLDocument, _secoundXMLDocument);
			}
			catch (Exception ex)
			{
				areLoaded = false;
				MessageBox.Show(ex.Message);
			}
			return areLoaded;
		}

		private void LoadTablesToList(List<string> firstOriginalTablesList, List<string> secondOriginTablesList, List<string> commonTablesList)
		{
			tablesListBox.Items.Clear();
			foreach (string tablename in firstOriginalTablesList)
			{
				ListViewItem tableItem = new ListViewItem(tablename);
				tableItem.Group = tablesListBox.Groups[0];
				//entItem.ForeColor = Settings.Default.FirstColor;
				tableItem.UseItemStyleForSubItems = true;
				tablesListBox.Items.Add(tableItem);
			}

			foreach (string entname in secondOriginTablesList)
			{
				ListViewItem entItem = new ListViewItem(entname);
				entItem.Group = tablesListBox.Groups[1];
				//entItem.ForeColor = Settings.Default.SecondColor;
				entItem.UseItemStyleForSubItems = true;
				tablesListBox.Items.Add(entItem);
			}

			foreach (string entname in commonTablesList)
			{
				ListViewItem entItem = new ListViewItem(entname);
				entItem.Group = tablesListBox.Groups[2];
				//entItem.ForeColor = Settings.Default.CommonColor;
				entItem.UseItemStyleForSubItems = true;
				tablesListBox.Items.Add(entItem);
			}
		}

		private void LoadTrees(XmlDocument firstCustXML, XmlDocument secoundCustXML)
		{
			BuildTablesTree(firstCustXML, firstTreeView);
			BuildTablesTree(secoundCustXML, secondTreeView);
		}

		private List<string> GetTablesNamesFromDocument(XmlDocument customizationDoc)
		{
			XmlNodeList entitiesLstXML1 = customizationDoc.GetElementsByTagName(TAG_TABLE);//custXML.DocumentElement.SelectNodes(commonEntitiesXMLPath + "/" + TAG_NAME);

			List<string> entLst = new List<string>();
			foreach (XmlNode entityNode in entitiesLstXML1)
			{
				string entityName = entityNode.Attributes.GetNamedItem(TAG_NAME).Value;//.InnerText;
				entLst.Add(entityName);
			}
			return entLst;
		}

		#endregion

		#region Analisys Methods

		private bool ParseCurrentTableCustomizations(XmlDocument customizationDoc,
																								out Dictionary<string, string> attributes, //table attributes
																								out Dictionary<string, TableField> fields,
																								out Dictionary<string, Action> actions,
																								out Dictionary<string, Form> forms,
																								out Dictionary<string, Search> searches,
																								out DataSet commonDS)
		{
			bool isParcedCustomization = true;
			commonDS = new DataSet();

			attributes = new Dictionary<string, string>();

			actions = new Dictionary<string, Action>();
			fields = new Dictionary<string, TableField>();
			searches = new Dictionary<string, Search>();
			forms = new Dictionary<string, Form>();

			List<XmlNode> entityNodeList = customizationDoc.GetElementsByTagName(TAG_TABLE).Cast<XmlNode>().ToList();

			XmlNode currentTableNode = entityNodeList.FirstOrDefault(node => node.Attributes[TAG_NAME].Value.Equals(CurrentTable));

			if (currentTableNode != null)
			{
				//0. Collect Table attributes
				var attributesNodes = currentTableNode.Attributes.Cast<XmlAttribute>();
				DataTable dtAttributes = (attributesNodes.Count() > 0) ? AnalyzeAttributes(attributesNodes, ref attributes) : new DataTable(TAG_ATTRIBUTE);

				//1. Fields
				var fieldsNodes = currentTableNode.ChildNodes.Cast<XmlNode>().Where(subNode => subNode.Name == TAG_FIELD).ToList();
				DataTable dtFields = (fieldsNodes.Count > 0) ? AnalyzeTableFields(fieldsNodes, ref fields) : new DataTable(TAG_FIELD);

				//2. Actions     
				var actionsNodes = currentTableNode.ChildNodes.Cast<XmlNode>().Where(subNode => subNode.Name == TAG_ACTION).ToList();
				DataTable dtActions = (actionsNodes.Count > 0) ? AnalyzeTableActions(actionsNodes, ref actions) : new DataTable(TAG_ACTION);

				//3. Forms
				var formsNodes = currentTableNode.ChildNodes.Cast<XmlNode>().Where(subNode => subNode.Name == TAG_FORM).ToList();
				DataTable dtForms = (formsNodes.Count > 0) ? AnalyzeForms(formsNodes, ref forms) : new DataTable(TAG_FORM);

				//4. Searches
				var searchNodes = currentTableNode.ChildNodes.Cast<XmlNode>().Where(subNode => subNode.Name == TAG_SEARCH).ToList();
				DataTable dtSearches = (searchNodes.Count > 0) ? AnalyzeSearches(searchNodes, ref searches) : new DataTable(TAG_SEARCH);

				//Create s DataSet with including of TABLE 1. attributes 2. fields 3. actions 4. forms 5. searches 
				//5.1
				string tableName = TAG_ATTRIBUTE;//dtAttributes.TableName;
				commonDS.Tables.Add(dtAttributes);
				commonDS.Tables[tableName].PrimaryKey = new DataColumn[] { dtAttributes.Columns[TAG_ATTR_KEY] };
				//5.2
				tableName = TAG_FIELD;//dtFields.TableName;
				commonDS.Tables.Add(dtFields);
				commonDS.Tables[tableName].PrimaryKey = new DataColumn[] { dtFields.Columns[TAG_NAME] };
				//5.3
				tableName = TAG_ACTION;//dtActions.TableName;
				commonDS.Tables.Add(dtActions);
				commonDS.Tables[tableName].PrimaryKey = new DataColumn[] { dtActions.Columns[TAG_NAME] };
				//5.4
				tableName = TAG_FORM;//dtForms.TableName;
				commonDS.Tables.Add(dtForms);
				commonDS.Tables[tableName].PrimaryKey = new DataColumn[] { dtForms.Columns[TAG_NAME] };
				//5.5
				tableName = TAG_SEARCH;//dtSearches.TableName;
				commonDS.Tables.Add(dtSearches);
				commonDS.Tables[tableName].PrimaryKey = new DataColumn[] { dtSearches.Columns[TAG_NAME] };
			}
			else
			{
				isParcedCustomization = false;
			}
			return isParcedCustomization;
		}

		private bool AnalyzeCustomizations(bool showOnlyUpdated)
		{
			SetStatusMessage(Strings.comparisonStarted);

			bool isFinished = true;

			if (String.IsNullOrEmpty(CurrentTable))
			{
				MessageBox.Show(Strings.selectTable);
				isFinished = false;
			}
			else
			{
				//1. Detect differences in Attributes, Fields, Forms properties
				//1.1 Parce 2 metastore files
				bool firstIsParsed = ParseCurrentTableCustomizations(_firstXMLDocument, out _firstTableAttributes, out _firstTableFields, out _firstTableActions, out _firstTableForms, out _firstTableSearches, out _firstMetastoreDS);
				bool secondIsParsed = ParseCurrentTableCustomizations(_secoundXMLDocument, out _secondTableAttributes, out _secondTableFields, out _secondTableActions, out _secondTableForms, out _secondTableSearches, out _seconMetastoreDS);

				if (firstIsParsed && secondIsParsed)
				{
					//1.2 Detect differences bt
					_diffAttrs = Compare(_firstTableAttributes, _secondTableAttributes, out _firstOriginalAttrsNames, out _secondOriginalAttrsNames);

					_diffFields = Compare(_firstTableFields, _secondTableFields, out _firstOriginalFieldsNames, out _secondOriginalFieldsNames);
					_diffActions = Compare(_firstTableActions, _secondTableActions, out _firstOriginalActionsNames, out _secondOriginalActionsNames);
					_diffForms = Compare(_firstTableForms, _secondTableForms, out _firstOriginalFormsNames, out _secondOriginalFormsNames);
					_diffSearches = Compare(_firstTableSearches, _secondTableSearches, out _firstOriginalSearchesNames, out _secondOriginalSearchesNames);

					ComparationFinished();

					//1.3 Load Grids
					SwitchTableAnalyzingGrids(showOnlyUpdated);
				}
				else
				{
					isFinished = false;
					MessageBox.Show(String.Format(Strings.absentTable, CurrentTable), Strings.warning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
					viewsElemsTabControl.SelectedIndex = 0;
				}
			}
			SetStatusMessage(Strings.comparisonFinish);

			return isFinished;
		}

		private DataTable AnalyzeAttributes(IEnumerable<XmlAttribute> attributesList, ref Dictionary<string, string> attributes)
		{
			if (attributesList != null)
			{
				attributes = attributesList.ToDictionary(a => a.Name, a => a.Value);
			}
			return attributes.ToDataTable(TAG_ATTRIBUTE); //create data table with Attributes of Metastore's TABLE
		}

		private DataTable AnalyzeTableSecurities(IList securityList, ref Dictionary<string, Security> securities)
		{
			if (securityList != null)
			{
				foreach (XmlNode securityNode in securityList)
				{
					string fldName = securityNode.Attributes.GetNamedItem(TAG_NAME).Value;
					Security fld = Security.Deserialize<Security>(securityNode.OuterXml, true);
					if (fld != null && !securities.Keys.Contains(fldName))
						securities.Add(fldName, fld);
					else
						MessageBox.Show(String.Format(Strings.fieldIncorrectFormat, fldName));
				}
			}
			return securities.Values.ToDataTable(TAG_SECURITY); //create data table with SECIRITies of Metastore TABLE
		}

		private DataTable AnalyzeTableFields(IList fieldsList, ref Dictionary<string, TableField> fields)
		{
			if (fieldsList != null)
			{
				foreach (XmlNode fieldNode in fieldsList)
				{
					string fldName = fieldNode.Attributes.GetNamedItem(TAG_NAME).Value;
					TableField fld = TableField.Deserialize<TableField>(fieldNode.OuterXml, true);
					if (fld != null && !fields.Keys.Contains(fldName))
						fields.Add(fldName, fld);
					else
						MessageBox.Show(String.Format(Strings.fieldIncorrectFormat, fldName));
				}
			}
			return fields.Values.ToDataTable(TAG_FIELD);//create data table with FIELDs of Metastore TABLE
		}

		private DataTable AnalyzeTableActions(IList actionsList, ref Dictionary<string, Action> actions)
		{
			if (actionsList != null)
			{
				foreach (XmlNode actNode in actionsList)
				{
					Action action = Action.Deserialize<Action>(actNode.OuterXml, true);
					string attrName = action.Name;
					actions.Add(attrName, action);
				}
			}
			return actions.Values.ToDataTable(TAG_ACTION);//create data table with ACTIONs of Metastore TABLE
		}

		private DataTable AnalyzeForms(IList formsList, ref Dictionary<string, Form> forms)
		{
			foreach (XmlNode formNode in formsList)
			{
				string formName = formNode.Attributes.GetNamedItem(TAG_NAME).Value;
				Form form = Form.Deserialize<Form>(formNode.OuterXml, true);
				if (form != null)
					forms.Add(formName, form);
				else
					MessageBox.Show(Strings.formXMLIncorrect);
			}
			return forms.Values.ToDataTable(TAG_FORM);//create data table with FORMs of Metastore TABLE
		}

		private DataTable AnalyzeSearches(IList searchesList, ref Dictionary<string, Search> searches)
		{
			foreach (XmlNode searchNode in searchesList)
			{
				string searchType = searchNode.Attributes.GetNamedItem(TAG_NAME).Value;
				Search search = Search.Deserialize<Search>(searchNode.OuterXml, true);
				if (search != null)
					searches.Add(searchType, search);
				else
					MessageBox.Show(Strings.formXMLIncorrect);
			}
			return searches.Values.ToDataTable(TAG_SEARCH);//create data table with SEARCHes of Metastore TABLE
		}

		#endregion

		#region Tree Methods

		private void BuildTablesTree(XmlDocument customizationDoc, TreeView tree)
		{
			tree.Nodes.Clear();

			XmlNodeList entitiesNodes = customizationDoc.GetElementsByTagName(TAG_TABLE);//customizationDoc.DocumentElement.SelectNodes
			foreach (XmlNode entityNode in entitiesNodes)
			{
				string entityName = entityNode.Attributes[TAG_NAME].Value;//entityNode.SelectSingleNode(TAG_NAME).Attributes[TAG_ORIGINALNAME].Value;//.InnerText;
				TreeNode rootNode = new TreeNode(entityName);
				tree.Nodes.Add(rootNode);
				rootNode.Name = entityName;
				rootNode.Tag = entityName;

				BuildSubTree(rootNode, entityNode);
			}
			//tree.Sort();
		}

		private TreeNode BuildSubTree(TreeNode parentTreeNode, XmlNode xmlNode)
		{
			XmlNodeList mainNodes = xmlNode.ChildNodes;
			foreach (XmlNode node in mainNodes)
			{
				string title = GetNodeAttributesText(node);
				//string title = GetNodeAttributesText(node);
				TreeNode treeNode = new TreeNode(title);
				string subElemName = node.Attributes != null && node.Attributes[TAG_NAME] != null ? node.Attributes[TAG_NAME].Value : "";
				treeNode.Name = parentTreeNode.Name + "/" + node.Name + "_" + subElemName;

				if (node.ChildNodes.Count > 0)
				{
					treeNode.Tag = xmlNode;
					if (node.ChildNodes[0].NodeType != XmlNodeType.Text)
						BuildSubTree(treeNode, node);
					else
						treeNode.Text = treeNode.Text + node.ChildNodes[0].Value + @"</" + node.Name + @">";
				}
				parentTreeNode.Nodes.Add(treeNode);
			}
			return parentTreeNode;
		}

		private string GetNodeAttributesText(XmlNode node)
		{
			string attrStr;
			if (node.NodeType == XmlNodeType.Text)
				attrStr = node.Value;
			else if (node.NodeType == XmlNodeType.Comment)
				attrStr = "comment " + node.Value;
			else
			{
				attrStr = "<" + node.LocalName;

				if (node.Attributes != null)
				{
					foreach (XmlAttribute attr in node.Attributes)
					{
						attrStr += (" " + attr.Name + "=" + attr.Value);
					}
				}
				attrStr += (node.ChildNodes.Count > 0) ? ">" : "/>";
			}
			return attrStr;
		}

		private void NavigateToNodeOnTree(TreeView tree, string tagName, bool collapseTreeBeforeSelect = true)
		{
			if (collapseTreeBeforeSelect) tree.CollapseAll();

			tree.SelectedNode = FindTreeNodeByName(tree, tagName);

			if (tree.SelectedNode != null)
			{
				tree.SelectedNode.Expand();
			}
		}

		private TreeNode FindTreeNodeByName(TreeView tree, string name)
		{
			TreeNode searchNode = null;
			foreach (TreeNode node in tree.Nodes)
			{
				if (String.Compare(node.Name, name, StringComparison.Ordinal) == 0)
				{
					searchNode = node;
					break;
				}
				searchNode = FindTreeNodeByName(node, name);
				if (searchNode != null) break;
			}
			return searchNode;
		}

		private TreeNode FindTreeNodeByName(TreeNode mainnode, string name)
		{
			TreeNode searchNode = null;
			foreach (TreeNode node in mainnode.Nodes)
			{
				if (node.Name.CompareTo(name) == 0)
				{
					searchNode = node;
					break;
				}
				searchNode = FindTreeNodeByName(node, name);
				if (searchNode != null) break;
			}
			return searchNode;
		}

		/// <summary>
		/// Tree [top node] = [table name]
		/// </summary>
		private TreeNode FindTableTreeNodeBySubNode(TreeNode subnode)
		{
			TreeNode searchNode = subnode;

			while (searchNode.Parent != null)
			{
				searchNode = searchNode.Parent;
			}

			return searchNode;
		}

		/// <summary>
		/// Find and select left menu (menu item) by table name
		/// </summary>
		/// <param name="tableName"></param>
		/// <returns></returns>
		private ListViewItem SelectMenuItemByTableName(string tableName)
		{
			ListViewItem tableListItem = tablesListBox.Items.Cast<ListViewItem>().FirstOrDefault(i => i.Text == tableName);
			if (tableListItem != null)
				tableListItem.Selected = true;
			return tableListItem;
		}


		private void firstTreeView_AfterSelect(object sender, TreeViewEventArgs e)
		{
			//1. first tree node selected
			TreeNode selectedTreeNode = e.Node;
			string selectedTreeTableName = FindTableTreeNodeBySubNode(selectedTreeNode).Name;

			//Navigate list (left menu) "table node", if the first tree node is selected manualy
			if (CurrentTable != selectedTreeTableName)
				SelectMenuItemByTableName(selectedTreeTableName);

			//2.If subnode (not table node) selected
			string nodeName = selectedTreeNode.Name;
			if (!string.IsNullOrWhiteSpace(nodeName) && selectedTreeTableName != nodeName)
			{
				//expand first tree node
				firstTreeView.SelectedNode.Expand();

				//navigate to second tree node (if exists)
				NavigateToNodeOnTree(secondTreeView, nodeName, false);
			}
		}

		private void firstTreeView_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
				ShowPathToolTips(firstTreeView, _firstXMLFilePath);
		}

		private void secondTreeView_AfterSelect(object sender, TreeViewEventArgs e)
		{
			TreeNode selectedTreeNode = e.Node; //second tree node selected
			string selectedTreeTableName = FindTableTreeNodeBySubNode(selectedTreeNode).Name;

			//Navigate list (left menu) "table node", if the second tree node is selected manualy
			if (CurrentTable != selectedTreeTableName)
				SelectMenuItemByTableName(selectedTreeTableName);

			//2.If subnode (not table node) selected
			string nodeName = selectedTreeNode.Name;
			if (!string.IsNullOrWhiteSpace(nodeName) && selectedTreeTableName != nodeName)
			{
				//expand second tree node
				secondTreeView.SelectedNode.Expand();

				//navigate to second tree node (if exists)
				NavigateToNodeOnTree(firstTreeView, nodeName, false);
			}
		}

		private void secondTreeView_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
				ShowPathToolTips(secondTreeView, _secondXMLFilePath);
			else
				customizationFilePathToolTip.Active = false;
		}

		#endregion

		#region GridViews methods (load metastore elements lists: fields, actions, forms, searches AND their differences)

		private void SwitchAnalyzingGrids(bool showOnlyUpdated)
		{
			if (formsTabPage.Visible) // Forms tabPage is currently active
				SwitchFormAnalizingPart(showOnlyUpdated);
			else
				SwitchTableAnalyzingGrids(showOnlyUpdated);
		}

		private void SwitchTableAnalyzingGrids(bool showOnlyUpdated)
		{
			TableAnalyzingType currentTableAnalyzingType = TableAnalyzingType.actions;
			if (Enum.TryParse<TableAnalyzingType>(analyzeTabControl.SelectedTab.Tag.ToString(), true, out currentTableAnalyzingType))
			{
				//Display Tables' elements: Actions, Fields
				if ((_firstMetastoreDS != null) && (_seconMetastoreDS != null))
				{
					switch (currentTableAnalyzingType)
					{
						case TableAnalyzingType.attributes:
							LoadElementAttributesDiffGrid(firstAttributesDataGridView, _firstMetastoreDS, _diffAttrs, _firstOriginalAttrsNames, showOnlyUpdated);
							LoadElementAttributesDiffGrid(secondAttributesDataGridView, _seconMetastoreDS, _diffAttrs, _secondOriginalAttrsNames, showOnlyUpdated);
							break;
						case TableAnalyzingType.fields:
							LoadElementsDiffGrid(firstFieldsDataGridView, _firstTableFields, _diffFields, _firstOriginalFieldsNames, showOnlyUpdated);
							LoadElementsDiffGrid(secondFieldsDataGridView, _secondTableFields, _diffFields, _secondOriginalFieldsNames, showOnlyUpdated);
							break;
						case TableAnalyzingType.actions:
							LoadElementsDiffGrid(firstActionsDataGridView, _firstTableActions, _diffActions, _firstOriginalActionsNames, showOnlyUpdated);
							LoadElementsDiffGrid(secondActionsDataGridView, _secondTableActions, _diffActions, _secondOriginalActionsNames, showOnlyUpdated);
							break;
						case TableAnalyzingType.forms:
							LoadElementsDiffGrid(gridFirstForms, _firstTableForms, _diffForms, _firstOriginalFormsNames, showOnlyUpdated);
							LoadElementsDiffGrid(gridSecondForms, _secondTableForms, _diffForms, _secondOriginalFormsNames, showOnlyUpdated);

							//and load combo with FORMS names
							currentFormCombo.Text = "";
							currentFormCombo.DataSource = _firstTableForms.Values.ToList();
							currentFormCombo.DisplayMember = "Name";
							currentFormCombo.Refresh();
							break;
						case TableAnalyzingType.searches:
							LoadElementsDiffGrid(firstSearchesDataGridView, _firstTableSearches, _diffSearches, _firstOriginalSearchesNames, showOnlyUpdated);
							LoadElementsDiffGrid(secondSearchesDataGridView, _secondTableSearches, _diffSearches, _secondOriginalSearchesNames, showOnlyUpdated);
							break;
					}
				}
			}
		}

		private void SwitchFormAnalizingPart(bool showOnlyUpdated)
		{
			SwitchFormAnalizingPart(CurrentFormAnalyzingPart, showOnlyUpdated);
		}

		private void SwitchFormAnalizingPart(FormAnalyzingType analyzingPart, bool showOnlyUpdated)
		{
			CompareForms(_firstTableForms, _secondTableForms,
																	ref _diffFormsAttributes,//new
																	ref _diffFormsSecurities,
																	ref _diffFormsActions,
																	ref _diffFormsFields,
																	ref _diffFormsLinkedForms,
																	ref _firstFormOriginalAttrsNames, ref _secondFormOriginalAttrsNames,
																	ref _firstFormOriginalSecurities, ref _secondFormOriginalSecurities,
																	ref _firstFormOriginalActionsNames, ref _secondFormOriginalActionsNames,
																	ref _firstFormOriginalFieldsNames, ref _secondFormOriginalFieldsNames,
																	ref _firstFormOriginalLinkedFormsNames, ref _secondFormOriginalLinkedFormsNames);

			if (string.IsNullOrEmpty(CurrentForm))
			{
				MessageBox.Show("Form is not selected !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			Form firstForm = _firstTableForms.ContainsKey(CurrentForm) ? _firstTableForms[CurrentForm] : null;
			Form secondForm = _secondTableForms.ContainsKey(CurrentForm) ? _secondTableForms[CurrentForm] : null;

			if (firstForm == null)
			{
				MessageBox.Show(string.Format("First list does't contain FORM {0}", CurrentForm), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			if (secondForm == null)
			{
				MessageBox.Show(string.Format("Second list does't contain FORM {0}", CurrentForm), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				//return;
			}

			//Load Forms elements to grids
			switch (analyzingPart)
			{
				case FormAnalyzingType.wholeform:
					LoadElementsDiffGrid(gridFirstForms, _firstTableForms, _diffForms, _firstOriginalFormsNames, showOnlyUpdated);
					LoadElementsDiffGrid(gridSecondForms, _secondTableForms, _diffForms, _secondOriginalFormsNames, showOnlyUpdated);
					break;
				case FormAnalyzingType.attributes:
					LoadElementAttributesDiffGrid(gridFirstForms, firstForm.Attributes, _diffFormsAttributes, _firstOriginalFormsNames, showOnlyUpdated);
					LoadElementAttributesDiffGrid(gridSecondForms, secondForm.Attributes, _diffFormsAttributes, _secondOriginalFormsNames, showOnlyUpdated);
					break;
				case FormAnalyzingType.security:
					LoadFormSecurityDiffGrid(gridFirstForms, _diffFormsSecurities, _firstFormOriginalSecurities, showOnlyUpdated);
					LoadFormSecurityDiffGrid(gridSecondForms, _diffFormsSecurities, _secondFormOriginalSecurities, showOnlyUpdated);
					break;
				case FormAnalyzingType.fields:
					LoadElementsDiffGrid(gridFirstForms, firstForm.FormFields, _diffFormsFields, _firstFormOriginalFieldsNames, showOnlyUpdated);
					LoadElementsDiffGrid(gridSecondForms, secondForm.FormFields, _diffFormsFields, _secondFormOriginalFieldsNames, showOnlyUpdated);
					break;
				case FormAnalyzingType.actions:
					LoadElementsDiffGrid(gridFirstForms, firstForm.FormMenuActions, _diffFormsActions, _firstFormOriginalActionsNames, showOnlyUpdated);
					LoadElementsDiffGrid(gridSecondForms, firstForm.FormMenuActions, _diffFormsActions, _secondFormOriginalActionsNames, showOnlyUpdated);
					break;
				case FormAnalyzingType.linkedforms:
					LoadElementsDiffGrid(gridFirstForms, firstForm.LinkedForms, _diffFormsLinkedForms, _firstFormOriginalLinkedFormsNames, showOnlyUpdated, TAG_TABLENAME);
					LoadElementsDiffGrid(gridSecondForms, firstForm.LinkedForms, _diffFormsLinkedForms, _secondFormOriginalLinkedFormsNames, showOnlyUpdated, TAG_TABLENAME);
					break;
			}
		}


		private string GetFilterString(string key, Dictionary<string, DifferPropertiesDictionary> diffElemsProps, List<string> originalElemsNames)
		{
			string filterExpr = "";
			int counter = 0;
			List<string> diffElemsList = new List<string>();
			diffElemsList.AddRange(diffElemsProps.Keys);
			diffElemsList.AddRange(originalElemsNames);
			foreach (string name in diffElemsList)
			{
				filterExpr += " '" + name + "'";
				if (counter < diffElemsList.Count - 1) filterExpr += ", ";
				counter++;
			}
			if (!String.IsNullOrEmpty(filterExpr))
				filterExpr = key + " IN (" + filterExpr + ")";
			return filterExpr;
		}


		private void LoadElementAttributesDiffGrid(DataGridView grid, Dictionary<string, string> attrKeyValue, Dictionary<string, DifferPropertiesDictionary> diffElemsProps, List<string> originalElemsNames, bool showOnlyUpdated)
		{
			DataTable datatable = attrKeyValue.ToDataTable(TAG_ATTRIBUTE);
			LoadElementAttributesDiffGrid(grid, datatable, diffElemsProps, originalElemsNames, showOnlyUpdated);
		}

		/// <summary>
		///Load element (Table, Form, Search, Field, Security) attributes (color differ values)
		/// </summary>
		private void LoadElementAttributesDiffGrid(DataGridView grid, DataSet dataDS, Dictionary<string, DifferPropertiesDictionary> diffElemsProps, List<string> originalElemsNames, bool showOnlyUpdated)
		{
			string subTbl = TAG_ATTRIBUTE;
			string key = TAG_ATTR_KEY;
			DataTable datatable = dataDS.Tables[subTbl];

			if (datatable != null)
			{
				datatable.PrimaryKey = new[] { datatable.Columns[key] };
				datatable.Columns[key].Unique = true;
				datatable.Columns[key].SetOrdinal(0);

				LoadElementAttributesDiffGrid(grid, datatable, diffElemsProps, originalElemsNames, showOnlyUpdated);
			}
		}

		private void LoadElementAttributesDiffGrid(DataGridView grid, DataTable datatable, Dictionary<string, DifferPropertiesDictionary> diffElemsProps, List<string> originalElemsNames, bool showOnlyUpdated)
		{
			//string subTbl = TAG_ATTRIBUTE;
			string key = TAG_ATTR_KEY;

			//Load data to DataGrid
			DataView dv = new DataView(datatable);
			dv.Sort = key + " ASC";

			//Filter values          
			if (showOnlyUpdated)
			{
				string filter = GetFilterString(key, diffElemsProps, originalElemsNames);
				if (!String.IsNullOrEmpty(filter))
					dv.RowFilter = filter;
				else
					dv = null;
			}

			grid.DataSource = dv;
			ColorCells(dv, grid, diffElemsProps, originalElemsNames);
			grid.Refresh();
		}

		private void LoadElementsDiffGrid<T>(DataGridView grid, Dictionary<string, T> allElementsDict, Dictionary<string, DifferPropertiesDictionary> diffElemsProps, List<string> originalElemsNames, bool showOnlyUpdated)
		{
			LoadElementsDiffGrid(grid, allElementsDict.Values.ToList(), diffElemsProps, originalElemsNames, showOnlyUpdated);
		}

		private void LoadElementsDiffGrid<T>(DataGridView grid, IEnumerable<T> allElements, Dictionary<string, DifferPropertiesDictionary> diffElemsProps, List<string> originalElemsNames, bool showOnlyUpdated,
			string key = TAG_NAME)
		{
			Type type = typeof(T);
			string subTbl = type.Name; //dataSet subTable (not METASTORE TABLE)
			//string key = TAG_NAME;

			//1. Create DT from elements if type T
			DataTable dataTable = allElements.ToDataTable(subTbl);
			// dataTable.PrimaryKey = new[] { dataTable.Columns[key] }; //TODO:

			//2. Load ds to DataGrid
			DataView dv = new DataView(dataTable);
			if (dataTable.Columns.Count > 0)
			{
				//dataTable.Columns[key].Unique = true;
				dataTable.Columns[key].SetOrdinal(0);
				dv.Sort = key + " ASC";
			}

			//3.Filter differences
			if (showOnlyUpdated)
			{
				string filter = GetFilterString(key, diffElemsProps, originalElemsNames);
				if (!String.IsNullOrEmpty(filter))
					dv.RowFilter = filter;
				else
					dv = null;
			}

			grid.DataSource = dv;
			ColorCells(dv, grid, diffElemsProps, originalElemsNames);
			grid.Refresh();
		}

		private void LoadFormSecurityDiffGrid(DataGridView grid, Dictionary<string, DifferPropertiesDictionary> diffElemsProps, List<string> originalNames, bool showOnlyUpdated)
		{
			string subTbl = FormAnalyzingType.security.ToString();
			string key = TAG_SECURITY_ACTION; //todo: there no Name of security tag

			DataSet formsDS = new DataSet();
			formsDS.Tables.Add(subTbl);

			//1. Create DS of columns
			PropertyInfo[] props = typeof(Security).GetProperties();
			foreach (PropertyInfo prop in props)
			{
				if (!formsDS.Tables[subTbl].Columns.Contains(prop.Name))
				{
					DataColumn newCol = new DataColumn(prop.Name);
					formsDS.Tables[subTbl].Columns.Add(newCol);
				}
			}

			//2. Load data to DataSet    
			formsDS.Tables[subTbl].PrimaryKey = new[] { formsDS.Tables[subTbl].Columns[key] };
			formsDS.Tables[subTbl].Columns[key].Unique = true;
			formsDS.Tables[subTbl].Columns[key].SetOrdinal(0);

			//3. Load DS to grid
			DataView dv = new DataView(formsDS.Tables[subTbl]);
			dv.Sort = key + " ASC";
			if (showOnlyUpdated)
			{
				string filter = GetFilterString(key, diffElemsProps, originalNames);
				if (!String.IsNullOrEmpty(filter))
					dv.RowFilter = filter;
				else
					dv = null;
			}

			grid.DataSource = dv;
			ColorCells(dv, grid, diffElemsProps, originalNames);
			grid.Refresh();
		}

		private void ShowCurrentCellContent(DataGridView dataGridView)
		{
			if ((Settings.Default.ShowCurrentCellContentOnForm) && (dataGridView.CurrentCell != null) && (dataGridView.CurrentCell.Value != null))
				currentCellTextBox.Text = dataGridView.CurrentCell.Value.ToString();
		}

		private void AutoNavigateCrid(string key, DataGridView fromDGV, DataGridView toDGV)
		{
			if (Settings.Default.AutoNavigateSelectedRow && (fromDGV.SelectedRows.Count > 0))
			{
				//1. Get first key =[attribute name]
				string compareFieldKey = fromDGV.SelectedRows[0].Cells[key].Value as string;

				if (toDGV.DataSource != null)
				{
					// int currentRowIndexOffset = fromDGV.SelectedRows[0].Index - fromDGV.FirstDisplayedScrollingRowIndex;

					//2. Navigate to correspond row in second grid
					DataView secondDV = toDGV.DataSource as DataView;
					if (secondDV != null)
					{
						int secondRowIndex = secondDV.Find(compareFieldKey);
						if (secondRowIndex == -1)
						{
							if (toDGV.SelectedRows.Count > 0) toDGV.SelectedRows[0].Selected = false;
						}
						else
						{
							toDGV.Rows[secondRowIndex].Selected = true; //Select correspond row
							toDGV.FirstDisplayedScrollingRowIndex = secondRowIndex;
						}
					}
				}
			}
		}

		#region Compare Methods

		private Dictionary<string, DifferPropertiesDictionary> Compare<T>(
																								Dictionary<string, T> firstList,
																								Dictionary<string, T> secondList,
																								out List<string> firstOriginaElementsNames, out List<string> secondOriginalElementsNames)
		{
			//1. Detect attributes do not have analogs in first/second list
			List<string> commonNames;
			CompareHelper.DetectCommonAndOriginalNames(firstList.Keys.GetEnumerator(), secondList.Keys.GetEnumerator(),
																			out firstOriginaElementsNames,
																			out secondOriginalElementsNames,
																			out commonNames);

			//2. Detect attributes, differ by some properties value
			var differElements = new Dictionary<string, DifferPropertiesDictionary>();

			foreach (string name in commonNames)
			{
				if (firstList.ContainsKey(name) && secondList.ContainsKey(name))
				{
					T firstElement = firstList[name]; //Element is Table, Form, Search, etc tag
					T secondElement = secondList[name];

					//Detect properties are different by value  
					DifferPropertiesDictionary diffPropsList;
					bool areDiffProps = CompareHelper.Compare(firstElement, secondElement, out diffPropsList);
					if (areDiffProps)
						differElements.Add(name, diffPropsList);
				}
			}
			return differElements;
		}

		/// <summary>
		/// Returns forms differences: attributes values, fields, security tags, actions
		/// </summary>
		private bool CompareForms(Dictionary<string, Form> firstForms, Dictionary<string, Form> secondForms,
																ref Dictionary<string, DifferPropertiesDictionary> diffFormAttributes,
																ref Dictionary<string, DifferPropertiesDictionary> diffFormsSecurity,
																ref Dictionary<string, DifferPropertiesDictionary> diffFormsActions,
																ref Dictionary<string, DifferPropertiesDictionary> differFormFields,
																ref Dictionary<string, DifferPropertiesDictionary> diffFormsLinkedForms,
																ref List<string> firstFormOriginalAttributes, ref List<string> secondFormOriginalAttributes,
																ref List<string> firstFormOriginalSecurities, ref List<string> secondFormOriginalSecurities,
																ref List<string> firstFormOriginalActionsNames, ref List<string> secondFormOriginalActionsNames,
																ref List<string> firstFormOriginalFieldsNames, ref List<string> secondFormOriginalFieldsNames,
																ref List<string> firstFormOriginalLinkedFormsNames, ref List<string> secondFormOriginalLinkedFormsNames)
		{

			if (string.IsNullOrEmpty(CurrentForm))
			{
				MessageBox.Show("Form is not selected", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}
			Form firstForm = firstForms.ContainsKey(CurrentForm) ? firstForms[CurrentForm] : null;
			Form secondForm = secondForms.ContainsKey(CurrentForm) ? secondForms[CurrentForm] : null;

			if (secondForm == null)
			{
				MessageBox.Show(string.Format("Second List doesn't contain Form '{0}'", CurrentForm), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return false;
			}

			//1. =======================Compare Forms attributes =======================
			var firstFormAttributesList = firstForm.Attributes;
			var secondFormAttributesList = secondForm.Attributes;

			//1.1. Detect common attributes
			List<string> commonAttributes;
			CompareHelper.DetectCommonAndOriginalNames(firstFormAttributesList.Keys.GetEnumerator(), secondFormAttributesList.Keys.GetEnumerator(),
																								 out firstFormOriginalAttributes, out secondFormOriginalAttributes, out commonAttributes);
			//1.2 Detect common attributes differ by values
			diffFormAttributes = new Dictionary<string, DifferPropertiesDictionary>();
			foreach (var commonAttr in commonAttributes)
			{
				var firstAttr = firstFormAttributesList.FirstOrDefault(attr => attr.Key == commonAttr);
				var secondAttr = secondFormAttributesList.FirstOrDefault(attr => attr.Key == commonAttr);
				DifferPropertiesDictionary diffPropsList;
				bool haveDiffProps = CompareHelper.Compare(firstAttr, secondAttr, out diffPropsList);
				if (haveDiffProps)
					diffFormAttributes.Add(commonAttr, diffPropsList);

			}

			//2.======================= Compare Forms Securities=======================
			var firstFormSecurityList = firstForm.Security.Where(security => !string.IsNullOrWhiteSpace(security.Action.ToString())).ToDictionary(security => security.Action.ToString());
			var secondFormSecurityList = secondForm.Security.ToDictionary(security => security.Action.ToString());

			//2.1. Detect common securities
			List<string> commonSecurities;
			CompareHelper.DetectCommonAndOriginalNames(firstFormSecurityList.Keys.GetEnumerator(), secondFormSecurityList.Keys.GetEnumerator(),
																								out firstFormOriginalSecurities, out secondFormOriginalSecurities, out commonSecurities);

			//2.2 TODO: Detect common securities differ by properties values
			diffFormsSecurity = new Dictionary<string, DifferPropertiesDictionary>();
			foreach (var commonSecurity in commonSecurities)
			{
				Security firstSecurity = firstFormSecurityList.ContainsKey(commonSecurity) ? firstFormSecurityList[commonSecurity] : null;
				Security secondSecurity = secondFormSecurityList.ContainsKey(commonSecurity) ? secondFormSecurityList[commonSecurity] : null;

				if (firstSecurity != null && secondSecurity != null)
				{
					DifferPropertiesDictionary diffPropsList;
					bool haveDiffProps = CompareHelper.Compare(firstSecurity, secondSecurity, out diffPropsList);
					if (haveDiffProps)
						diffFormsSecurity.Add(commonSecurity, diffPropsList);
				}
			}

			//3.======================= Compare forms fields =======================
			var firstFormFieldList = firstForm.FormFields.Where(field => !string.IsNullOrWhiteSpace(field.Name));
			var secondFormFieldList = secondForm.FormFields.Where(field => !string.IsNullOrWhiteSpace(field.Name));

			//3.1. Detect common and original fields
			List<string> commonFieldsNames;
			CompareHelper.DetectCommonAndOriginalNames(firstFormFieldList, secondFormFieldList, out firstFormOriginalFieldsNames, out secondFormOriginalFieldsNames, out commonFieldsNames);

			//3.2. Detect common fields differ by properties values
			differFormFields = new Dictionary<string, DifferPropertiesDictionary>();

			foreach (var commonFieldName in commonFieldsNames)
			{
				FormField firstField = firstFormFieldList.FirstOrDefault(fld => fld.Name == commonFieldName);
				FormField secondField = secondFormFieldList.FirstOrDefault(fld => fld.Name == commonFieldName);
				if (firstField != null && secondField != null)
				{
					DifferPropertiesDictionary diffPropsList;
					bool haveDiffProps = CompareHelper.Compare(firstField, secondField, out diffPropsList);
					if (haveDiffProps)
						differFormFields.Add(commonFieldName, diffPropsList);
				}
			}

			//4.======================= Compare form actions ======================= 
			var firstFormMenuActionsList = firstForm.FormMenuActions.Where(action => !string.IsNullOrWhiteSpace(action.Name));//.ToDictionary<FormMenuAction, string, FormAction>(action => action.Name, action => action);
			var secondFormMenuActionsList = secondForm.FormMenuActions.Where(action => !string.IsNullOrWhiteSpace(action.Name));//.ToDictionary<FormMenuAction, string, FormAction>(action => action.Name, action => action);

			//4.1. Detect common and original form actions
			List<string> commonFormsActionsNames;
			CompareHelper.DetectCommonAndOriginalNames(firstFormMenuActionsList, secondFormMenuActionsList, out firstFormOriginalActionsNames, out secondFormOriginalActionsNames, out commonFormsActionsNames);

			//4.2. Detect common actions differ by properties values
			diffFormsActions = new Dictionary<string, DifferPropertiesDictionary>();

			foreach (var commonActionName in commonFormsActionsNames)
			{
				FormAction firstFormAction = firstFormMenuActionsList.FirstOrDefault(action => action.Name == commonActionName);
				FormAction secondFormAction = secondFormMenuActionsList.FirstOrDefault(action => action.Name == commonActionName);
				if (firstFormAction != null && secondFormAction != null)
				{
					DifferPropertiesDictionary diffPropsList;
					bool haveDiffProps = CompareHelper.Compare(firstFormAction, secondFormAction, out diffPropsList);
					if (haveDiffProps)
						diffFormsActions.Add(commonActionName, diffPropsList);
				}
			}

			//5.======================= Compare form linkedforms =======================
			var firstFormLinkedFormsList = firstForm.LinkedForms.Where(action => !string.IsNullOrWhiteSpace(action.Name));//.ToDictionary<FormMenuAction, string, FormAction>(action => action.Name, action => action);
			var secondFormLinkedFormsList = secondForm.LinkedForms.Where(action => !string.IsNullOrWhiteSpace(action.Name));//.ToDictionary<FormMenuAction, string, FormAction>(action => action.Name, action => action);

			//5.1. Detect common and original form actions
			List<string> commonFormsLinkedFormsNames;
			CompareHelper.DetectCommonAndOriginalNames(firstFormLinkedFormsList, secondFormLinkedFormsList, out firstFormOriginalLinkedFormsNames, out secondFormOriginalLinkedFormsNames, out commonFormsLinkedFormsNames);

			//5.2. Detect common actions differ by properties values
			diffFormsLinkedForms = new Dictionary<string, DifferPropertiesDictionary>();

			foreach (var commonLinkedFormName in commonFormsLinkedFormsNames)
			{
				LinkedForm firstFormLinkedForm = firstFormLinkedFormsList.FirstOrDefault(lnkFrm => lnkFrm.Name == commonLinkedFormName);
				LinkedForm secondFormLinkedForm = secondFormLinkedFormsList.FirstOrDefault(lnkFrm => lnkFrm.Name == commonLinkedFormName);
				if (firstFormLinkedForm != null && secondFormLinkedForm != null)
				{
					DifferPropertiesDictionary diffPropsList;
					bool haveDiffProps = CompareHelper.Compare(firstFormLinkedForm, secondFormLinkedForm, out diffPropsList);
					if (haveDiffProps)
						diffFormsLinkedForms.Add(commonLinkedFormName, diffPropsList);
				}
			}

			//6. =======================Summary for all differences of 2 forms=======================
			bool existsCommonElementsWithDiffValues = (diffFormAttributes.Count + commonSecurities.Count + differFormFields.Count + diffFormsActions.Count + diffFormsLinkedForms.Count > 0);
			bool existsOroginalElements = (firstFormOriginalAttributes.Count + secondFormOriginalAttributes.Count) + (firstFormOriginalSecurities.Count + secondFormOriginalSecurities.Count) + (firstFormOriginalActionsNames.Count + secondFormOriginalActionsNames.Count) + (firstFormOriginalFieldsNames.Count + secondFormOriginalFieldsNames.Count) + (firstFormOriginalLinkedFormsNames.Count + secondFormOriginalLinkedFormsNames.Count) > 0;
			return existsCommonElementsWithDiffValues || existsOroginalElements;
		}

		private void ComparationFinished()
		{
			reportButton.Enabled = true;
			diffButton.Enabled = true;
		}

		#endregion

		#region Display differences

		/// <summary>
		/// Grid cells colorize
		/// </summary>
		private void ColorCells(DataView dv, DataGridView gridView, Dictionary<string, DifferPropertiesDictionary> diffElems, List<string> originalElemsNames)
		{
			//1. Color original elements(attributes|fields)
			ColorOriginalCells(dv, gridView, originalElemsNames);

			//2. Color cells, - element(attribute|field) properties, - which are differ by values             
			ColorDifferPropertiesCells(dv, gridView, diffElems);
		}

		//private void ColorCells(DataView dv, DataGridView gridView, Dictionary<KeyValuePair<string, bool>, DifferPropertiesDictionary> diffElems, List<KeyValuePair<string, bool>> originalElemsNames)
		//{
		//	//1. Color original elements(attributes|fields)
		//	ColorOriginalCells(dv, gridView, originalElemsNames);

		//	//2. Color cells, - element(attribute|field) properties, - which are differ by values             
		//	ColorDifferPropertiesCells(dv, gridView, diffElems);
		//}

		private void ColorOriginalCells(DataView dv, DataGridView gridView, List<string> originalElemsNames)
		{
			foreach (string originalElemKey in originalElemsNames)
			{
				int originalRowIndex = dv.Find(originalElemKey);
				if (originalRowIndex > -1)
					gridView.Rows[originalRowIndex].DefaultCellStyle.BackColor = Settings.Default.AbsentColor;
			}
		}

		private void ColorOriginalCells(DataView dv, DataGridView gridView, List<KeyValuePair<string, bool>> originalElemsNames)
		{
			if (dv != null)
			{
				foreach (KeyValuePair<string, bool> originalElemKey in originalElemsNames)
				{
					int originalRowIndex = dv.Find(new object[2] { originalElemKey.Key, originalElemKey.Value });
					if (originalRowIndex > -1)
						gridView.Rows[originalRowIndex].DefaultCellStyle.BackColor = Settings.Default.AbsentColor;
				}
			}
		}

		private void ColorDifferPropertiesCells(DataView dv, DataGridView gridView, Dictionary<string, DifferPropertiesDictionary> diffElems)
		{
			//Color cells, - element(attribute|field) properties, - which are differ by values             
			foreach (string diffElemKey in diffElems.Keys)
			{
				DifferPropertiesDictionary props = diffElems[diffElemKey];
				int diffRowIndex = dv.Find(diffElemKey);

				if ((diffRowIndex > -1) && (props != null))
				{
					IEnumerator ien = props.Items.Keys.GetEnumerator();
					while (ien.MoveNext())
					{
						string propName = ien.Current.ToString();
						//
						if (gridView.Columns.Contains(propName))
						{
							//Collorize differ property cell
							if (gridView[propName, diffRowIndex] != null)
								gridView[propName, diffRowIndex].Style.BackColor = Settings.Default.DifferColor;
						}
					}
				}
			}
		}

		private void ColorDifferPropertiesCells(DataView dv, DataGridView gridView, Dictionary<KeyValuePair<string, bool>, DifferPropertiesDictionary> diffElems)
		{
			//Color cells, - element(attribute|field) properties, - which are differ by values             
			foreach (KeyValuePair<string, bool> diffElemKey in diffElems.Keys)
			{
				DifferPropertiesDictionary props = diffElems[diffElemKey];
				int diffRowIndex = dv.Find(new object[2] { diffElemKey.Key, diffElemKey.Value });

				if ((diffRowIndex > -1) && (props != null))
				{
					IEnumerator ien = props.Items.Keys.GetEnumerator();
					while (ien.MoveNext())
					{
						string propName = ien.Current.ToString();
						//
						if (gridView.Columns.Contains(propName))
						{
							//Collorize differ property cell
							if (gridView[propName, diffRowIndex] != null)
								gridView[propName, diffRowIndex].Style.BackColor = Settings.Default.DifferColor;
						}
					}
				}
			}
		}

		private void ReColorDataGridView(DataGridView gridView, string keyProperty, Dictionary<string, DifferPropertiesDictionary> diffElems, List<string> originalElems)
		{
			DataGridViewRowCollection rows = gridView.Rows;
			foreach (DataGridViewRow row in rows)
			{
				string elemname = row.Cells[keyProperty].Value.ToString();
				if (originalElems.Contains(elemname))
				{
					row.DefaultCellStyle.BackColor = Settings.Default.AbsentColor;
				}
				else if (diffElems.ContainsKey(elemname.ToLower()))
				{
					int diffRowIndex = row.Index;
					DifferPropertiesDictionary props = diffElems[elemname.ToLower()];
					IEnumerator ien = props.Items.Keys.GetEnumerator();
					while (ien.MoveNext())
					{
						string propName = ien.Current.ToString();
						if (gridView.Columns.Contains(propName))
						{
							//Collorize differ property cell
							if (gridView[propName, diffRowIndex] != null)
								gridView[propName, diffRowIndex].Style.BackColor = Settings.Default.DifferColor;
						}
					}
				}
			}
		}

		#endregion

		#endregion

		#region Event handlers

		private void viewsElemsTabControl_Selected(object sender, TabControlEventArgs e)
		{
			bool showOnlyUpdated = showDifferRadioButton.Checked;
			if (e.TabPage.Tag != null)
				ChangeComparatorView(e.TabPage.Tag.ToString(), showOnlyUpdated);
		}

		private void ChangeComparatorView(ComparatorView pagetag, bool showOnlyUpdated)
		{
			if (pagetag == ComparatorView.analyze)
			{
				bool isAnalyzed = AnalyzeCustomizations(showOnlyUpdated);
				if (isAnalyzed)
				{
					viewsElemsTabControl.SelectedIndex = 1;
					diffButton.Enabled = true;
					reportButton.Enabled = true;
				}
				else
					viewsElemsTabControl.SelectedIndex = 0;
			}
			else
			{
				viewsElemsTabControl.SelectedIndex = 0;
				SetStatusMessage(String.Empty);

				diffButton.Enabled = false;
				reportButton.Enabled = false;
			}
		}

		private void ChangeComparatorView(string pageTag, bool showOnlyUpdated)
		{
			ComparatorView comparatorView;
			if (Enum.TryParse(pageTag, true, out comparatorView))
				ChangeComparatorView(comparatorView, showOnlyUpdated);
		}

		#region Analyzing 1<=>2 metastore controls (tabControls, grids) event handlers

		private void analyzeTabControl_SelectedIndexChanged(object sender, EventArgs e)
		{
			bool showDiff = showDifferRadioButton.Checked;
			if (analyzeTabControl.SelectedTab.Tag != null)
				SwitchTableAnalyzingGrids(showDiff);
		}

		/// <summary>
		/// Show all or only different elements\tags radio-button changed
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void showAllDifferRadioButton_CheckedChanged(object sender, EventArgs e)
		{
			SwitchAnalyzingGrids(showDifferRadioButton.Checked);
		}

		#endregion

		#region Analyzed GridViews event handlers

		private void firstAttributesDataGridView_SelectionChanged(object sender, EventArgs e)
		{
			DataGridView grid = (DataGridView)sender;
			ShowCurrentCellContent(grid);

			AutoNavigateCrid(TAG_NAME, grid, secondAttributesDataGridView);
		}

		private void firstFieldsDataGridView_SelectionChanged(object sender, EventArgs e)
		{
			DataGridView grid = (DataGridView)sender;
			ShowCurrentCellContent(grid);

			AutoNavigateCrid(TAG_NAME, grid, secondFieldsDataGridView);
		}

		private void firstFormDataGridView_SelectionChanged(object sender, EventArgs e)
		{
			DataGridView grid = (DataGridView)sender;
			ShowCurrentCellContent(grid);

			AutoNavigateCrid(TAG_NAME, grid, gridSecondForms);
		}
		/// <summary>
		/// sub-tabControl for analizing FORMs
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void formsTabPage_AnalizingPartChanged(object sender, EventArgs e)
		{
			SwitchFormAnalizingPart(showDifferRadioButton.Checked);
		}

		private void currentFormCombo_SelectedIndexChanged(object sender, EventArgs e)
		{
			SwitchFormAnalizingPart(showDifferRadioButton.Checked);
		}

		private void firstFormDataGridView_Scroll(object sender, ScrollEventArgs e)
		{
			gridSecondForms.HorizontalScrollingOffset = e.NewValue;
		}

		private void secondFormDataGridView_Scroll(object sender, ScrollEventArgs e)
		{
			if (e.ScrollOrientation == ScrollOrientation.HorizontalScroll)
				gridFirstForms.HorizontalScrollingOffset = e.NewValue;
		}

		private void firstAttributesDataGridView_Scroll(object sender, ScrollEventArgs e)
		{
			if (e.ScrollOrientation == ScrollOrientation.HorizontalScroll)
				secondAttributesDataGridView.HorizontalScrollingOffset = e.NewValue;
		}

		private void secondAttributesDataGridView_Scroll(object sender, ScrollEventArgs e)
		{
			if (e.ScrollOrientation == ScrollOrientation.HorizontalScroll)
				firstAttributesDataGridView.HorizontalScrollingOffset = e.NewValue;
		}

		private void firstFieldsDataGridView_Scroll(object sender, ScrollEventArgs e)
		{
			if (e.ScrollOrientation == ScrollOrientation.HorizontalScroll)
				secondFieldsDataGridView.HorizontalScrollingOffset = e.NewValue;
		}

		private void secondFieldsDataGridView_Scroll(object sender, ScrollEventArgs e)
		{
			if (e.ScrollOrientation == ScrollOrientation.HorizontalScroll)
				firstFieldsDataGridView.HorizontalScrollingOffset = e.NewValue;
		}

		private void firstAttributesDataGridView_ColumnSortModeChanged(object sender, DataGridViewColumnEventArgs e)
		{
			DataView dv = ((DataView)firstAttributesDataGridView.DataSource);

			//ColorCells(dv, firstAttributesDataGridView, diffAttrs, firstOriginalAttrsNames);
		}

		private void firstAttributesDataGridView_Sorted(object sender, EventArgs e)
		{
			//ReColorDataGridView((DataGridView)sender, attributeKeyProperty, diffAttrs, firstOriginalAttrsNames);
		}

		private void secondAttributesDataGridView_Sorted(object sender, EventArgs e)
		{
			//ReColorDataGridView((DataGridView)sender, attributeKeyProperty, diffAttrs, secondOriginalAttrsNames);
		}

		private void firstFieldsDataGridView_Sorted(object sender, EventArgs e)
		{
			//ReColorDataGridView((DataGridView)sender, fieldKeyProperty, diffFields, firstOriginalFieldsNames);
		}

		private void secondFieldsDataGridView_Sorted(object sender, EventArgs e)
		{
			//ReColorDataGridView((DataGridView)sender, fieldKeyProperty, diffFields, secondOriginalFieldsNames);
		}

		private void firstFormDataGridView_Sorted(object sender, EventArgs e)
		{
			//if (controlsRadioButton.Checked)
			//	ReColorDataGridView((DataGridView)sender, cellControlKeyProperty, diffFormsCells, firstFormOriginalControlsNames);
			//else if (dataRadioButton.Checked)
			//	ReColorDataGridView((DataGridView)sender, dataFormKeyProperty, diffFormsData, firstFormOriginalDataIds);
		}

		private void secondFormDataGridView_Sorted(object sender, EventArgs e)
		{
			//if (controlsRadioButton.Checked)
			//	ReColorDataGridView((DataGridView)sender, cellControlKeyProperty, diffFormsCells, secondFormOriginalControlsNames);
			//else if (dataRadioButton.Checked)
			//	ReColorDataGridView((DataGridView)sender, dataFormKeyProperty, diffFormsData, secondFormOriginalDataIds);
		}

		private void grid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			if (Settings.Default.LoadCellToWordPad)
				LoadWordPad(sender);
		}

		private void DataGridView_SelectionChanged(object sender, EventArgs e)
		{
			ShowCurrentCellContent((DataGridView)sender);
		}

		#endregion

		#endregion

		#region Reporting

		private void ShowPathToolTips(TreeView owner, string filepath)
		{
			customizationFilePathToolTip.Active = true;
			customizationFilePathToolTip.Show(filepath, owner);
		}

		private void LoadWordPad(object sender)
		{
			try
			{
				DataGridView currentGrid = sender as DataGridView;

				if (currentGrid != null)
				{
					string path = Path.GetFullPath("currCell.txt");
					if (!File.Exists(path))
					{
						FileStream fs = File.Create(path);
						fs.Close();
					}

					using (StreamWriter file = new StreamWriter(path))
					{
						file.WriteLine(currentGrid.CurrentCell.Value.ToString());
					}

					//2. Start wordpad
					System.Diagnostics.Process wordPad = new System.Diagnostics.Process();
					if (!((wordpadpath != null) && (wordpadpath.Length > 0)))
					{
						string[] localDirs = Directory.GetLogicalDrives();
						foreach (string drive in localDirs)
						{
							wordpadpath = Directory.GetFiles(drive + "Program Files", "WORDPAD.EXE", SearchOption.AllDirectories);
							if (wordpadpath.Length > 0)
								break;
						}
					}

					if (wordpadpath.Length > 0)
					{
						wordPad.StartInfo.FileName = wordpadpath[0];
						//@"C:\Program Files\Windows NT\Accessories\WORDPAD.EXE";
						wordPad.StartInfo.Arguments = path;
						wordPad.StartInfo.UseShellExecute = false;
						wordPad.Start();
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		public void SetStatusMessage(string statusString)
		{
			statusLabel.Text = statusString;
		}

		private void reportButton_Click(object sender, EventArgs e)
		{
			TableAnalyzingType reportType;
			if (Enum.TryParse(analyzeTabControl.SelectedTab.Tag.ToString(), out reportType))
				OpenReport(reportType);
		}

		/// <summary>
		///  Load different Table elemements(attributes|fields|forms) data to report
		/// </summary>
		/// <param name="reportType"></param>
		private void OpenReport(TableAnalyzingType reportType)
		{
			try
			{
				ReportDialog reportForm = null;
				DialogResult dlgResult;

				switch (reportType)
				{
					case TableAnalyzingType.attributes:
						//1.1. Generate DataSet
						DataSet diffAttrsDS = GenerateReportDataSet(_diffAttrs);

						//1.2. Fill report data
						if ((diffAttrsDS != null) || (_firstOriginalAttrsNames.Count > 0) || (_secondOriginalAttrsNames.Count > 0))
						{
							reportForm = new ReportDialog(ReportHeader.Attributes, diffAttrsDS, _firstOriginalAttrsNames,
								_secondOriginalAttrsNames);
						}
						break;

					case TableAnalyzingType.fields:
						//2.1 Generate DataSet 
						DataSet diffFieldsDS = GenerateReportDataSet(_diffFields);
						//2.2. Fill report data
						if ((diffFieldsDS != null) || (_firstOriginalFieldsNames.Count > 0) || (_secondOriginalFieldsNames.Count > 0))
						{
							reportForm = new ReportDialog(ReportHeader.Fields, diffFieldsDS, _firstOriginalFieldsNames,
								_secondOriginalFieldsNames);
						}
						break;

					case TableAnalyzingType.actions:
						//3.1. Generate DataSet
						DataSet diffActionsDS = GenerateReportDataSet(_diffActions);

						//3.2. Fill report data
						if ((diffActionsDS != null) || (_firstOriginalActionsNames.Count > 0) || (_secondOriginalActionsNames.Count > 0))
						{
							reportForm = new ReportDialog(ReportHeader.Actions, diffActionsDS, _firstOriginalActionsNames,_secondOriginalActionsNames);
						}
						break;

					case TableAnalyzingType.forms:
						//4.1 Forms attributes
						DataSet diffAttrDS = GenerateReportDataSet(_diffFormsAttributes);

						//4.2 Forms securities
						DataSet diffFormsSecuritiesDS = GenerateReportDataSet(_diffFormsSecurities);

						//4.3 Forms fields
						DataSet diffFormsFieldsDS = GenerateReportDataSet(_diffFormsFields);

						//4.4 Forms actions
						DataSet diffFormsActionsDS = GenerateReportDataSet(_diffFormsActions);

						//4.5. Fill report data   
						switch (CurrentFormAnalyzingPart)
						{
							case FormAnalyzingType.attributes:
								reportForm = new ReportDialog(ReportHeader.FormAttributes, diffAttrDS, _firstFormOriginalAttrsNames, _secondFormOriginalAttrsNames);
								break;
							case FormAnalyzingType.security:
								reportForm = new ReportDialog(ReportHeader.FormSecurities, diffFormsSecuritiesDS, _firstFormOriginalSecurities, _secondFormOriginalSecurities);
								break;
							case FormAnalyzingType.fields:
								reportForm = new ReportDialog(ReportHeader.FormFields, diffFormsFieldsDS, _firstFormOriginalFieldsNames, _secondFormOriginalFieldsNames);
								break;
							case FormAnalyzingType.actions:
								reportForm = new ReportDialog(ReportHeader.FormActions, diffFormsActionsDS, _firstFormOriginalActionsNames, _secondFormOriginalActionsNames);
								break;
						}
						break;

					case TableAnalyzingType.searches:
						//5.1. Generate DataSet
						DataSet diffSearchesDS = GenerateReportDataSet(_diffSearches);

						//5.2. Fill report data
						if ((diffSearchesDS != null) || (_firstOriginalSearchesNames.Count > 0) || (_secondOriginalSearchesNames.Count > 0))
						{
							reportForm = new ReportDialog(ReportHeader.Searches, diffSearchesDS, _firstOriginalSearchesNames,
								_secondOriginalSearchesNames);
						}
						break;
				}
				
				if (reportForm != null)
				{
					if (reportForm.ShowDialog() == DialogResult.OK)
						System.Diagnostics.Process.Start(@reportForm.FilePath);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private DataSet GenerateReportDataSet(Dictionary<string, DifferPropertiesDictionary> diffElems)
		{
			DataSet diffDS = null;
			if (diffElems != null)
			{
				diffDS = new DataSet();
				//2.1 Generate columns
				diffDS.Tables.Add(RepDataSet.DataTable.Name);
				diffDS.Tables[0].Columns.Add(RepDataSet.DataTable.ColumnElementName);
				diffDS.Tables[0].Columns.Add(RepDataSet.DataTable.ColumnPropertyName);
				diffDS.Tables[0].Columns.Add(RepDataSet.DataTable.ColumnFirstValue);
				diffDS.Tables[0].Columns.Add(RepDataSet.DataTable.ColumnSecondValue);

				//2.2 Fill rows
				foreach (string elemName in diffElems.Keys)
				{
					Dictionary<string, CompareItem> diffProps = diffElems[elemName].Items;
					foreach (string propName in diffProps.Keys)
					{
						DataRow row = diffDS.Tables[RepDataSet.DataTable.Name].NewRow();
						//row["Element Name"]
						row[RepDataSet.DataTable.ColumnElementName] = elemName;
						row[RepDataSet.DataTable.ColumnPropertyName] = propName;
						row[RepDataSet.DataTable.ColumnFirstValue] = (diffProps[propName].Firstvalue != null) ? CompareHelper.ToString(diffProps[propName].Firstvalue) : "null";
						row[RepDataSet.DataTable.ColumnSecondValue] = (diffProps[propName].Secondvalue != null) ? CompareHelper.ToString(diffProps[propName].Secondvalue) : "null";
						diffDS.Tables[0].Rows.Add(row);
					}
				}
			}
			return diffDS;
		}

		#endregion
	}
}