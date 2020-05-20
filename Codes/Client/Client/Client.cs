using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;


namespace Client
{
	public partial class Client : Form
	{
		#region Declaration
		String thisprocessname = Process.GetCurrentProcess().ProcessName;
		int clientnumber;
		private byte[] _buffer = new byte[10240];
		private byte[] _recBuf = new byte[10240];
		public Client client = null;
		private List<Socket> _clientSocketsList = new List<Socket>();
		enum Dealdir
		{
			Buy,
			Sell
		}
		enum ModeToSendMessage
		{
			GetOrderList,
			sendOrder,
			OrderHistory,
			ReceiveNotification
		}
		private Socket _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		#endregion
		public Client()
		{

		}
		public Client(int clientNUmber)
		{
			InitializeComponent();
			clientnumber = clientNUmber;
			this.Name = "Client" + clientnumber;
			client = this;
			LoopConnect();                          //Try to connect to server
		}

		#region Fill corresponding grids
		// Fills Market Order grid of a particular client
		private void fillOrderListGrid(string response)
		{
			try
			{
				DataTable dtOrder = new DataTable();
				DataTable dtOrderNew = new DataTable();
				//Client client = new Client();
				if (response != "[{ }]" || response != "")
				{
					dtOrder = (DataTable)JsonConvert.DeserializeObject(response, (typeof(DataTable)));

					if (dtOrder.Rows.Count > 0)
					{
						DataRow[] dr = dtOrder.Select("Client = 'Client" + client.clientnumber + "'", "Time ASC");          //Select only paticular clients table to display
						if (dr.Count() > 0)
						{
							dtOrder = dr.CopyToDataTable();
							dtOrder = dtOrder.AsEnumerable()
			.GroupBy(r => new { Col1 = r["PriceOB"] })
			.Select(g =>
			{
				var row = dtOrder.NewRow();
				row["Client"] = g.First().Field<string>("Client");
				row["DealDir"] = g.First().Field<Int64>("DealDir");
				row["MyBidQty"] = g.Sum(r => r.Field<Int64>("MyBidQty"));

				row["MktBidQty"] = g.First().Field<Int64>("MktBidQty");

				row["MyAskQty"] = g.Sum(r => r.Field<Int64>("MyAskQty"));
				row["MktAskQty"] = g.First().Field<Int64>("MktAskQty");
				row["PriceOB"] = g.Key.Col1;
				row["Time"] = g.First().Field<DateTime>("Time");

				return row;

			})
			.CopyToDataTable();
							string query = "MyAskQty = 0 And MyBidQty = 0";
							var drTemp = dtOrder.Select(query);
							if (drTemp.Length > 0)
							{
								foreach (var row in drTemp)
									row.Delete();
							}

							dtOrder.AcceptChanges();

							client.Invoke(new MethodInvoker(delegate ()
							{
								client.gvOrderBook.AutoGenerateColumns = false;
								client.gvOrderBook.DataSource = dtOrder;
							}));
						}

					}
					dtOrder.Dispose();
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error in fillOrderListGrid: " + ex.Message);
			}
		}
		// Fills Order History grid
		private void fillOrderHistoryGrid(string response)
		{
			try
			{
				DataTable dtHistory = new DataTable();
				dtHistory = (DataTable)JsonConvert.DeserializeObject(response, (typeof(DataTable)));
				if (dtHistory.Rows.Count > 0)
				{
					client.Invoke(new MethodInvoker(delegate ()
					{
						client.gvOrderHistory.AutoGenerateColumns = false;
						client.gvOrderHistory.DataSource = dtHistory;
					}));
				}

				dtHistory.Dispose();
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error in fillOrderHistoryGrid: " + ex.Message);
			}
		}
		//Sets order status
		private void setOrderStatus(string status)
		{
			try
			{
				client.Invoke(new MethodInvoker(delegate ()
				{
					client.lblInfo.Text = status;
				}));
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error in setOrderStatus: " + ex.Message);
			}
		}
		#endregion

		#region ConnectionAndMessageExchange
		//Sends message to server
		private void SendLoop(string text, int mode)
		{
			try
			{
				string req = text;
				byte[] buffer = Encoding.ASCII.GetBytes(mode + "|" + text);
				_clientSocket.Send(buffer);
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error in SendLoop: " + ex.Message);
			}
		}
		//Parses the received message and does the action accordingly
		private void Received(string text)
		{
			try
			{
				if (text != "")
				{
					string[] response = text.Split('|');
					switch (Convert.ToInt32(response[0]))
					{
						case (int)ModeToSendMessage.GetOrderList:
							fillOrderListGrid(response[1]);
							break;
						case (int)ModeToSendMessage.sendOrder:
							setOrderStatus(response[1]);
							break;
						case (int)ModeToSendMessage.OrderHistory:
							fillOrderHistoryGrid(response[1]);
							break;
						case (int)ModeToSendMessage.ReceiveNotification:
							setOrderStatus(response[1]);
							break;
						case 200:
							setOrderStatus(response[1]);
							break;
						default: break;
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error in Received: " + ex.Message);
			}
		}
		//Tries to make connection to server
		private void LoopConnect()
		{
			int attempts = 0;

			while (!_clientSocket.Connected)
			{
				if (attempts < 4)
				{
					try
					{
						attempts++;
						if (attempts <= 4)
						{
							_clientSocket.Connect(IPAddress.Loopback, 100);
							loginInfo.Text = "Connected: Client" + clientnumber;
							Console.WriteLine("Connected");
							ConnectedCallback();
						}
						else
						{
							loginInfo.Text = "Can't Connect";
							attempts = 0;
							//break;
						}
					}
					catch (SocketException s)
					{
						if (attempts <= 4)
						{
							Console.WriteLine(s.Message + " | Connection attempts: " + attempts.ToString());
						}
						else
						{
							attempts = 0;
							//break;
						}
					}
				}
				else
				{
					Console.WriteLine("Too many attempts");
					break;
				}
			}
		}
		//Waits for a message from server
		private void ConnectedCallback()
		{
			try
			{
				_clientSocket.BeginReceive(_recBuf, 0, _recBuf.Length, SocketFlags.None, new AsyncCallback(ReceivedCallback), _clientSocket);
			}

			catch (Exception ex)
			{
				Console.WriteLine("Error in ConnectedCallback: " + ex.Message);
			}

		}
		//Receive a message from server
		private void ReceivedCallback(IAsyncResult iar)
		{
			try
			{
				Socket s = (Socket)iar.AsyncState;
				if (s.Connected)
				{
					int rec = s.EndReceive(iar);
					byte[] dataBuf = new byte[rec];

					Buffer.BlockCopy(_recBuf, 0, dataBuf, 0, rec);

					string q = Encoding.ASCII.GetString(dataBuf);

					//	client.Invoke(new MethodInvoker(delegate () {
					Received(q);
					//}));

					s.BeginReceive(_recBuf, 0, _recBuf.Length, SocketFlags.None, new AsyncCallback(ReceivedCallback), s);
				}
				else
				{
					lblInfo.Text = "Disconnected due to error";
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error in:" + ex.Message);
			}
		}

		#endregion

		#region Page events
		private void createOrder_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			try
			{
				var senderGrid = (DataGridView)sender;

				if (senderGrid.Columns[createOrder.CurrentCell.ColumnIndex] is DataGridViewButtonColumn && createOrder.CurrentRow.Index >= 0)
				{
					//TODO - Button Clicked - Execute Code Here
					int dealDir;
					int Quantity = Convert.ToInt32(createOrder.CurrentRow.Cells[2].Value);
					double Price = Convert.ToDouble(createOrder.CurrentRow.Cells[3].Value);
					if (createOrder.CurrentRow.Cells[1].Value.ToString().ToUpper() == "BUY")
					{
						dealDir = (int)Dealdir.Buy;
					}
					else
					{
						dealDir = (int)Dealdir.Sell;
					}
					if (Quantity > 0 && Price > 0)
					{
						List<Order> order = new List<Order>{
				   new Order{
					   user = "Client" + clientnumber,
					   symbol = "StockA",
							   quantity = Quantity,
							   price = Price,
					   dealdir =dealDir}
					};
						var json = JsonConvert.SerializeObject(order);
						SendLoop(json, (int)ModeToSendMessage.sendOrder);
					}
					else
					{
						if (Quantity <= 0)
						{
							lblInfo.Text = "Please enter valid quantity";
						}
						else if (Price <= 0)
						{
							lblInfo.Text = "Please enter valid price";
						}
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}
		private void createOrder_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			bool validClick = (e.RowIndex != -1 && e.ColumnIndex != -1); //Make sure the clicked row/column is valid.
			var datagridview = sender as DataGridView;

			// Check to make sure the cell clicked is the cell containing the combobox 
			if (datagridview.Columns[e.ColumnIndex] is DataGridViewComboBoxColumn && validClick)
			{

				datagridview.BeginEdit(true);
				((ComboBox)datagridview.EditingControl).DroppedDown = true;
			}
		}

		private void createOrder_KeyDown(object sender, KeyEventArgs e)
		{
			try
			{
				//createOrder_CellContentClick(createOrder, null);
				try
				{
					if (e.KeyData == Keys.Enter)
					{
						createOrder_CellContentClick(createOrder, null);

						e.SuppressKeyPress = true;
						int iColumn = createOrder.CurrentCell.ColumnIndex;
						int iRow = createOrder.CurrentCell.RowIndex;
						if (iColumn == createOrder.Columns.Count - 1)
						{
							if (createOrder.RowCount > (iRow + 1))
							{
								createOrder.CurrentCell = createOrder[1, iRow + 1];
							}
							else
							{
								//focus next control
							}
						}
						else
							createOrder.CurrentCell = createOrder[iColumn + 1, iRow];
					}

				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.Message);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}
		#endregion
	}

	public class Order
	{
		public string user = string.Empty;
		public string symbol = string.Empty;
		public int quantity;
		public double price;
		public int dealdir;

	}
}
