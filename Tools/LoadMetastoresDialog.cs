using System;

namespace  WAFMetastoreComparator
{
    public partial class LoadMetastoresDialog : System.Windows.Forms.Form
    {
        public string firstCustFilePath;
        public string secondCustFilePath;

        public LoadMetastoresDialog()
        {
            InitializeComponent();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void loadButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void firstMetastoreLoadButton_Click(object sender, EventArgs e)
        {
            var fd = new System.Windows.Forms.OpenFileDialog();
            //fd.InitialDirectory = Assembly.GetExecutingAssembly().Location;
            fd.Filter = "XML(*.xml)|*.xml";
            if (fd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                firstCustFilePath = fd.FileName;
                firstCustTextBox.Text = firstCustFilePath;
            }
        }

        private void secondMetastoreLoadButton_Click(object sender, EventArgs e)
        {
            var fd = new System.Windows.Forms.OpenFileDialog();
            //fd.InitialDirectory = Assembly.GetExecutingAssembly().Location;
            fd.Filter = "XML(*.xml)|*.xml";
            if (fd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                secondCustFilePath = fd.FileName;
                secondCustTextBox.Text = secondCustFilePath;
            }
        }
    }
}