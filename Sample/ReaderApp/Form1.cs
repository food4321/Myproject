using Provider;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReaderApp
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}
		Reader reader;
		private void btnStart_Click(object sender, EventArgs e)
		{
			reader = new BarCodePro();

			reader.ReadData += reader_ReadData;
			reader.Start();
		}

		void reader_ReadData(ReaderEvertArgs e)
		{
			var str = Convert.ToString(e.Data);

			txtContent.Text = str.Substring(3);
		  
		}

		private void btnStop_Click(object sender, EventArgs e)
		{
			reader.Stop();
		}
	}
}
