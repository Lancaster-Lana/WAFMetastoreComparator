using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Configuration;
using System.Xml;

namespace WAFMetastoreComparator
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]

		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new WAFMetastoreComparatorForm());
		}
	}
}