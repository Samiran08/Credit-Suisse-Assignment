using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
	public partial class CreateClient : Form
	{
		private static int clientCount = 0;
		public CreateClient()
		{
			InitializeComponent();
		}

		private void createclient1_Click(object sender, EventArgs e)
		{
			clientCount++;
			Client client = new Client(clientCount);
			client.Show();
		}

		private void orderHistory_TextChanged(object sender, EventArgs e)
		{
			foreach (Form frm in Application.OpenForms)
			{
				if(frm.Name == "Client")
				{
					frm.Update();
				}
			}
		}
	}
}
