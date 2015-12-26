using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace WAFMetastoreComparator.Report
{
	public partial class ReportDialog : System.Windows.Forms.Form
	{
		public Export.ExportFormat ExportFormat { get; private set; }
		public string FilePath { get; private set; }

		#region variables

		string headerText;
		DataSet diffElemsData;
		List<string> originalElems;
		List<string> newElems;

		#endregion

		#region Ctor

		public ReportDialog()
		{
			InitializeComponent();
		}

		public ReportDialog(string headerText, DataSet diffElemsData, List<string> originalElems, List<string> newElems)
		{
			InitializeComponent();

			this.headerText = headerText;
			this.diffElemsData = diffElemsData;
			this.originalElems = originalElems;
			this.newElems = newElems;
		}

		#endregion

		private void exportButton_Click(object sender, EventArgs e)
		{
			ExportFormat = Export.ExportFormat.Text;
			if (formatComboBox.Text.Equals(Export.ExportFormat.CSV.ToString()))
				ExportFormat = Export.ExportFormat.CSV;
			else if (formatComboBox.Text.Equals(Export.ExportFormat.Excel.ToString()))
				ExportFormat = Export.ExportFormat.Excel;

			var report = new Report(ExportFormat, headerText, originalElemsPrefixTextBox.Text, diffElemsData, originalElems, newElems);
			FilePath = report.FilePath;
		}

		private void ReportDialog_Load(object sender, EventArgs e)
		{
			formatComboBox.Items.Add(Export.ExportFormat.Text.ToString());
			formatComboBox.Items.Add(Export.ExportFormat.CSV.ToString());
			formatComboBox.Items.Add(Export.ExportFormat.Excel.ToString());
			formatComboBox.SelectedItem = formatComboBox.Items[0];
		}
	}

	internal class Report
	{
		List<OriginalElement> originalElems = new List<OriginalElement>();
		List<OriginalElement> newElems = new List<OriginalElement>();

		public DataSet DataSource { get; set; }

		public string FilePath { get; private set; }

		public Report(Export.ExportFormat reportFormat, string headerText, string originalElemsPrefix, DataSet diffElemsData, List<string> originalElems, List<string> newElems)
		{
			// Export           
			using (var saveFileDlg = new System.Windows.Forms.SaveFileDialog())
			{
				saveFileDlg.FileName = headerText;

				switch (reportFormat)
				{
					case Export.ExportFormat.Text:
						saveFileDlg.Filter = "Text file(*.txt)|*.txt";
						reportFormat = Export.ExportFormat.Text;
						break;
					case Export.ExportFormat.Excel:
						saveFileDlg.Filter = "Excel file(*.xls)|*.xls";
						reportFormat = Export.ExportFormat.Excel;
						break;
					case Export.ExportFormat.CSV:
						saveFileDlg.Filter = "Extended Text file(*.csv)|*.csv";
						reportFormat = Export.ExportFormat.CSV;
						break;
				}

				if (saveFileDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
				{
					 FilePath = saveFileDlg.FileName;
					// Generate columns
					string[] hColumns =
					{
						RepDataSet.DataTable.ColumnElementName,
						RepDataSet.DataTable.ColumnPropertyName,
						RepDataSet.DataTable.ColumnFirstValue,
						RepDataSet.DataTable.ColumnSecondValue
					};

					// Export the details of specified columns
					DataTable dt = diffElemsData.Tables[0].Copy();

					//_________Delete similar elems names to indication groups only - for REPORT ONLY
					List<string> groupElems = new List<string>();
					string elementColName = RepDataSet.DataTable.ColumnElementName;
					foreach (DataRow row in dt.Rows)
					{
						string propertyName = row[elementColName].ToString();
						if (!groupElems.Contains(propertyName))
							groupElems.Add(propertyName);
						else
							row[elementColName] = "";
					}

					//Export differ properties and original elements to 3 files of folder
					SetHeader(headerText);

					// differ properties
					Export export = new Export();
					export.ExportDetails(dt, hColumns, reportFormat, FilePath);
					export.ExportDetails(originalElems, reportFormat, originalElemsPrefix + "_1_" + Path.GetFileName(FilePath));
					export.ExportDetails(newElems, reportFormat, originalElemsPrefix + "_2_" + Path.GetFileName(FilePath));
				}

				this.DataSource = diffElemsData;

				foreach (string name in originalElems)
					this.originalElems.Add(new OriginalElement(name));
				foreach (string name in newElems)
					this.newElems.Add(new OriginalElement(name));
			}
		}

		private void SetHeader(string caption)
		{
			//FieldHeadingObject header = ((FieldHeadingObject)(DifferenceInPropsReport.Section1.ReportObjects["HeaderText"]));
			//header.Text = caption;
		}
	}
}
