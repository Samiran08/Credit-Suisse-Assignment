using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace Server_UI
{

	public partial class Server : Form
	{
		#region Declarations
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
			SendNotification
		}
		public static Server server = null;
		private static byte[] _buffer = new byte[1024];
		private static Socket _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		//private static List<Socket> _clientSockets = new List<Socket>();                //Lists of cliens to serve
		private static Dictionary<int, Socket> _clientSockets = new Dictionary<int, Socket>();
		private static DataTable orderBook = new DataTable();
		private static DataTable orderHistory = new DataTable();
		public static bool tradeBooked = false;
		public static Dictionary<int, string> clientsToNotify = new Dictionary<int, string>();
		#endregion
		
		public Server()
		{
			InitializeComponent();
			createOrderBookAndOrderHistory();
			setupServer();
			loginInfo.Text = "Connected: Server";
			server = this;
			gvOrderBook.AutoGenerateColumns = false;
			gvOrderBook.DataSource = orderBook;
		}
		#region OrderAndTradeBooking
		//Creates MarketOrder and Order History table
		public static void createOrderBookAndOrderHistory()
		{
			try
			{
				orderBook.Columns.Add("Client", typeof(string));
				orderBook.Columns.Add("DealDir", typeof(int));
				orderBook.Columns.Add("MyBidQty", typeof(int));
				orderBook.Columns.Add("MktBidQty", typeof(int));
				orderBook.Columns.Add("PriceOB", typeof(double));
				orderBook.Columns.Add("MyAskQty", typeof(int));
				orderBook.Columns.Add("MktAskQty", typeof(int));
				orderBook.Columns.Add("Time", typeof(DateTime));
				//orderBook.Rows.Add("Client2", 3, 30, 2.5, 2, 10);
				//orderBook.Rows.Add("Client3", 4, 30, 2.5, 4, 10);
				//orderBook.Rows.Add("Client4", 3, 30, 2.5, 4, 10);
				//orderBook.Rows.Add("Client1", 10, 5, 2.6, 0, 0);
				//orderBook.Rows.Add("Client1", 10, 0, 2.7, 0, 0);
				//orderBook.Rows.Add("Client1", 0, 0, 2.8, 0, 10);
				//orderBook.Rows.Add("Client1", 0, 8, 2.9, 5, 10);
				//orderBook.Rows.Add("Client2", 5, 8, 2.9, 5, 10);
				orderHistory.Columns.Add("BuyerH", typeof(string));
				orderHistory.Columns.Add("SellerH", typeof(string));
				orderHistory.Columns.Add("SymbolH", typeof(string));
				orderHistory.Columns.Add("QtyH", typeof(int));
				orderHistory.Columns.Add("PriceH", typeof(double));
			}
			catch (Exception ex)
			{
				Console.Write("Error in createOrderBookAndOrderHistory: " + ex.Message);
			}

		}
		//Converts a datatable to JSON
		public static string dtToJSON(DataTable dt)
		{
			try
			{
				string JSONString = string.Empty;
				JSONString = JsonConvert.SerializeObject(dt);
				return JSONString;
			}
			catch (Exception ex)
			{
				Console.Write("Error in dtToJSON: " + ex.Message);
				return "";
			}

		}
		//Perform actions when new order received
		private static void processIncomingOrder(string request)
		{
			try
			{
				List<Order> order = new List<Order>();
				order = (List<Order>)JsonConvert.DeserializeObject(request, (typeof(List<Order>)));
				string client = order[0].user;
				string symbol = order[0].symbol;
				double price = order[0].price;
				int Qty = order[0].quantity;
				int dealDir = order[0].dealdir;
				insertToOrderTable(order[0].user, order[0].symbol, order[0].price, order[0].quantity, order[0].dealdir);
				int noOfOrderExecuted = tryexecuteOrder(order[0].user, order[0].symbol, order[0].price, order[0].quantity, order[0].dealdir);
				//IEnumerable<DataRow> rows = orderBook.Rows.Cast<DataRow>().Where(r => Convert.ToInt32(r["MyBidQty"]) != 0 && Convert.ToInt32(r["MyAskQty"]) != 0);
				//orderBook = rows.CopyToDataTable();
				if (noOfOrderExecuted > 0)
				{
					server.Invoke(new MethodInvoker(delegate ()
					{
						server.Dock = DockStyle.Fill;
						server.gvOrderHistory.AutoGenerateColumns = false;
						server.gvOrderHistory.DataSource = orderHistory;
						server.gvOrderHistory.Refresh();
						//server.gvOrderBook_ColumnHeaderMouseClick(null, null);
					}));
					tradeBooked = true;
					broadCastMessage((int)ModeToSendMessage.GetOrderList);
					System.Threading.Thread.Sleep(350);
					broadCastMessage((int)ModeToSendMessage.OrderHistory);
					System.Threading.Thread.Sleep(350);
					broadCastMessage((int)ModeToSendMessage.SendNotification);
					System.Threading.Thread.Sleep(350);
					string query = "MyAskQty = 0 And MyBidQty = 0";
					var drTemp = orderBook.Select(query);
					if (drTemp.Length > 0)
					{
						foreach (var row in drTemp)
							row.Delete();
					}
					orderBook.AcceptChanges();
				}
				else
				{
					tradeBooked = false;
					broadCastMessage((int)ModeToSendMessage.GetOrderList);
				}
			}
			catch (Exception ex)
			{
				Console.Write("Error in processIncomingOrder: " + ex.Message);
			}
		}
		//Try to match for  trade
		private static int tryexecuteOrder(string user, string symbol, double Price, int quantity, int dealdir)
		{
			string client = string.Empty;
			double price;
			int quantityToFill = 0;
			int noOfTransactions = 0;
			client = user;
			price = Price;
			try
			{
				if (quantity > 0)
				{
					if (dealdir == ((int)Dealdir.Buy))                                                  //Try to execute buy order
					{
						quantityToFill = quantity;
						string query = "Client <> '" + client + "' And PriceOB = " + price;
						DataRow[] drTemp = orderBook.Select(query, "Time ASC");
						if (drTemp.Count() > 0)
						{
							for (int j = 0; j < drTemp.Count(); j++)
							{
								if (quantityToFill < Convert.ToInt16(drTemp[j]["MyAskQty"]))
								{
									int MktAskQty = Convert.ToInt16(drTemp[j]["MktAskQty"]) - quantityToFill;
									int MktBidQty = Convert.ToInt16(drTemp[j]["MktBidQty"]) - quantityToFill;
									//int myBidQty = quantity - quantityToFill;
									int myBidQty = Convert.ToInt16(drTemp[j]["MktBidQty"]) - quantityToFill;
									int myAskQty = Convert.ToInt16(drTemp[j]["MyAskQty"]) - quantityToFill;
									IEnumerable<DataRow> rows = orderBook.Rows.Cast<DataRow>().Where(r => r["Client"].ToString() == drTemp[j]["Client"].ToString() && Convert.ToDouble(r["PriceOB"]) == Convert.ToDouble(drTemp[j]["PriceOB"]) && Convert.ToInt32(r["DealDir"])==Convert.ToInt32(drTemp[j]["DealDir"]));
									rows.ToList().ForEach(r => r.SetField("MyAskQty", myAskQty));
									orderBook.AcceptChanges();
									rows = orderBook.Rows.Cast<DataRow>().Where(r => Convert.ToDouble(r["PriceOB"]) == Convert.ToDouble(drTemp[j]["PriceOB"]));
									rows.ToList().ForEach(r =>
									{
										r.SetField("MktAskQty", MktAskQty);
										r.SetField("MktBidQty", MktBidQty);
									});
									orderBook.AcceptChanges();
									rows = orderBook.Rows.Cast<DataRow>().Where(r => r["Client"].ToString() == client && Convert.ToDouble(r["PriceOB"]) == price);
									rows.ToList().ForEach(r => r.SetField("MyBidQty", 0));
									orderBook.AcceptChanges();
									orderHistory.Rows.Add(client, drTemp[j]["Client"].ToString(), "StockA", quantityToFill, Convert.ToDouble(drTemp[j]["PriceOB"].ToString()));
									if (!clientsToNotify.ContainsKey(Convert.ToInt16(client.Substring(client.Length - 1))))
									{
										clientsToNotify.Add(Convert.ToInt16(client.Substring(client.Length - 1)), "Trade executed with " + drTemp[j]["Client"].ToString());
									}
									else
									{
										string tempMsg = string.Empty;
										clientsToNotify.TryGetValue(Convert.ToInt16(client.Substring(client.Length - 1)), out tempMsg);
										clientsToNotify[Convert.ToInt16(client.Substring(client.Length - 1))] = tempMsg + ", " + drTemp[j]["Client"].ToString();
									}
									if (!clientsToNotify.ContainsKey(Convert.ToInt16(drTemp[j]["Client"].ToString().Substring(drTemp[j]["Client"].ToString().Length - 1))))
									{
										clientsToNotify.Add(Convert.ToInt16(drTemp[j]["Client"].ToString().Substring(drTemp[j]["Client"].ToString().Length - 1)), "Trade executed with " + client);
									}
									else
									{
										string tempMsg = string.Empty;
										clientsToNotify.TryGetValue(Convert.ToInt16(drTemp[j]["Client"].ToString().Substring(drTemp[j]["Client"].ToString().Length - 1)), out tempMsg);
										clientsToNotify[Convert.ToInt16(drTemp[j]["Client"].ToString().Substring(drTemp[j]["Client"].ToString().Length - 1))] = tempMsg + ", " + client;
									}
									noOfTransactions++;
									break;
								}
								else
								{
									if (Convert.ToInt16(drTemp[j]["MyAskQty"]) == 0)
									{
										continue;
									}
									int marketAskQuantity = Convert.ToInt16(drTemp[j]["MktAskQty"]) - Convert.ToInt16(drTemp[j]["MyAskQty"]);
									//int MyBidQty = quantity - Convert.ToInt16(drTemp[j]["MyAskQty"]);
									int MyBidQty = quantityToFill - Convert.ToInt16(drTemp[j]["MyAskQty"]);
									int myAskQty = Convert.ToInt16(drTemp[j]["MyAskQty"]);
									int mktBidQuantity = Convert.ToInt16(drTemp[j]["MktBidQty"]) - Convert.ToInt16(drTemp[j]["MyAskQty"]);
									//int mktBidQuantity = Convert.ToInt16(drTemp[j]["MktBidQty"]) - Convert.ToInt16(dr["MyAskQty"]);
									IEnumerable<DataRow> rows = orderBook.Rows.Cast<DataRow>().Where(r => Convert.ToDouble(r["PriceOB"]) == Convert.ToDouble(drTemp[j]["PriceOB"]));
									rows.ToList().ForEach(r =>
									{
										r.SetField("MktAskQty", marketAskQuantity);
										r.SetField("MktBidQty", mktBidQuantity);
									});
									orderBook.AcceptChanges();
									rows = orderBook.Rows.Cast<DataRow>().Where(r => r["Client"].ToString() == drTemp[j]["Client"].ToString() && Convert.ToDouble(r["PriceOB"]) == Convert.ToDouble(drTemp[j]["PriceOB"]) && Convert.ToInt32(r["DealDir"]) == Convert.ToInt32(drTemp[j]["DealDir"]));
									rows.ToList().ForEach(r => r.SetField("MyAskQty", 0));
									orderBook.AcceptChanges();
									rows = orderBook.Rows.Cast<DataRow>().Where(r => r["Client"].ToString() == client && Convert.ToDouble(r["PriceOB"]) == price);
									rows.ToList().ForEach(r => r.SetField("MyBidQty", MyBidQty));
									orderBook.AcceptChanges();
									orderHistory.Rows.Add(client, drTemp[j]["Client"].ToString(), "StockA", myAskQty, Convert.ToDouble(drTemp[j]["PriceOB"].ToString()));
									if (!clientsToNotify.ContainsKey(Convert.ToInt16(client.Substring(client.Length - 1))))
									{
										clientsToNotify.Add(Convert.ToInt16(client.Substring(client.Length - 1)), "Trade executed with " + drTemp[j]["Client"].ToString());
									}
									else
									{
										string tempMsg = string.Empty;
										clientsToNotify.TryGetValue(Convert.ToInt16(client.Substring(client.Length - 1)), out tempMsg);
										clientsToNotify[Convert.ToInt16(client.Substring(client.Length - 1))] = tempMsg + ", " + drTemp[j]["Client"].ToString();
									}
									if (!clientsToNotify.ContainsKey(Convert.ToInt16(drTemp[j]["Client"].ToString().Substring(drTemp[j]["Client"].ToString().Length - 1))))
									{
										clientsToNotify.Add(Convert.ToInt16(drTemp[j]["Client"].ToString().Substring(drTemp[j]["Client"].ToString().Length - 1)), "Trade executed with " + client);
									}
									else
									{
										string tempMsg = string.Empty;
										clientsToNotify.TryGetValue(Convert.ToInt16(drTemp[j]["Client"].ToString().Substring(drTemp[j]["Client"].ToString().Length - 1)), out tempMsg);
										clientsToNotify[Convert.ToInt16(drTemp[j]["Client"].ToString().Substring(drTemp[j]["Client"].ToString().Length - 1))] = tempMsg + ", " + client;
									}
									noOfTransactions++;
									//if (Convert.ToInt16(drTemp[j]["MktAskQty"]) == 0)
									//{
									//	return 0;
									//}
									//	if ((quantityToFill - myAskQty) > 0)			//Commented by Samirn 18May 10AM
									//{

										if ((quantityToFill - myAskQty) <= 0)
										{
											quantityToFill = 0;
											break;
										}
										else
										{
											quantityToFill = quantityToFill - myAskQty;
										}
									//}

								}
							}
						}

					}
					else                                                                                            //Try to execute sell order
					{
						quantityToFill = quantity;
						string query = "Client <> '" + client + "' And PriceOB = " + price;
						DataRow[] drTemp = orderBook.Select(query, "Time ASC");
						if (drTemp.Count() > 0)
						{
							for (int j = 0; j < drTemp.Count(); j++)
							{
								if (quantityToFill < Convert.ToInt16(drTemp[j]["MyBidQty"]))
								{
									int MktAskQty = Convert.ToInt16(drTemp[j]["MktAskQty"]) - quantityToFill;
									int MktBidQty = Convert.ToInt16(drTemp[j]["MktBidQty"]) - quantityToFill;
									int myBidQty = Convert.ToInt16(drTemp[j]["MyBidQty"]) - quantityToFill;
									//int myAskQty = quantity - quantityToFill;
									int myAskQty = Convert.ToInt16(drTemp[j]["MktAskQty"]) - quantityToFill;
									IEnumerable<DataRow> rows = orderBook.Rows.Cast<DataRow>().Where(r => r["Client"].ToString() == drTemp[j]["Client"].ToString() && Convert.ToDouble(r["PriceOB"]) == Convert.ToDouble(drTemp[j]["PriceOB"]) && Convert.ToInt32(r["DealDir"]) == Convert.ToInt32(drTemp[j]["DealDir"]));
									rows.ToList().ForEach(r => r.SetField("MyBidQty", myBidQty));
									orderBook.AcceptChanges();
									rows = orderBook.Rows.Cast<DataRow>().Where(r => Convert.ToDouble(r["PriceOB"]) == Convert.ToDouble(drTemp[j]["PriceOB"]));
									rows.ToList().ForEach(r =>
									{
										r.SetField("MktAskQty", MktAskQty);
										r.SetField("MktBidQty", MktBidQty);
									});
									orderBook.AcceptChanges();
									rows = orderBook.Rows.Cast<DataRow>().Where(r => r["Client"].ToString() == client && Convert.ToDouble(r["PriceOB"]) == price);
									rows.ToList().ForEach(r => r.SetField("MyAskQty", 0));
									orderBook.AcceptChanges();
									orderHistory.Rows.Add(drTemp[j]["Client"].ToString(), client, "StockA", quantityToFill, Convert.ToDouble(drTemp[j]["PriceOB"].ToString()));
									if (!clientsToNotify.ContainsKey(Convert.ToInt16(client.Substring(client.Length - 1))))
									{
										clientsToNotify.Add(Convert.ToInt16(client.Substring(client.Length - 1)), "Trade executed with " + drTemp[j]["Client"].ToString());
									}
									else
									{
										string tempMsg = string.Empty;
										clientsToNotify.TryGetValue(Convert.ToInt16(client.Substring(client.Length - 1)), out tempMsg);
										clientsToNotify[Convert.ToInt16(client.Substring(client.Length - 1))] = tempMsg + ", " + drTemp[j]["Client"].ToString();
									}
									if (!clientsToNotify.ContainsKey(Convert.ToInt16(drTemp[j]["Client"].ToString().Substring(drTemp[j]["Client"].ToString().Length - 1))))
									{
										clientsToNotify.Add(Convert.ToInt16(drTemp[j]["Client"].ToString().Substring(drTemp[j]["Client"].ToString().Length - 1)), "Trade executed with " + client);
									}
									else
									{
										string tempMsg = string.Empty;
										clientsToNotify.TryGetValue(Convert.ToInt16(drTemp[j]["Client"].ToString().Substring(drTemp[j]["Client"].ToString().Length - 1)), out tempMsg);
										clientsToNotify[Convert.ToInt16(drTemp[j]["Client"].ToString().Substring(drTemp[j]["Client"].ToString().Length - 1))] = tempMsg + ", " + client;
									}
									noOfTransactions++;
									break;
								}
								else
								{
									if (Convert.ToInt16(drTemp[j]["MyBidQty"]) == 0)
									{
										continue;
									}
									if (Convert.ToInt16(drTemp[j]["MyBidQty"]) > 0)
									{
										if (Convert.ToInt16(drTemp[j]["MyBidQty"]) == 0)
										{
											return 0;
										}
										int marketAskQuantity = Convert.ToInt16(drTemp[j]["MktAskQty"]) - Convert.ToInt16(drTemp[j]["MyBidQty"]);
										int MyBidQty = Convert.ToInt16(drTemp[j]["MyBidQty"]);
										//int myAskQty = quantity - Convert.ToInt16(drTemp[j]["MyBidQty"]);
										int myAskQty = quantityToFill - Convert.ToInt16(drTemp[j]["MyBidQty"]);
										int mktBidQuantity = Convert.ToInt16(drTemp[j]["MktBidQty"]) - Convert.ToInt16(drTemp[j]["MyBidQty"]);
										IEnumerable<DataRow> rows = orderBook.Rows.Cast<DataRow>().Where(r => Convert.ToDouble(r["PriceOB"]) == Convert.ToDouble(drTemp[j]["PriceOB"]));
										rows.ToList().ForEach(r =>
										{
											r.SetField("MktAskQty", marketAskQuantity);
											r.SetField("MktBidQty", mktBidQuantity);
										});
										orderBook.AcceptChanges();

										rows = orderBook.Rows.Cast<DataRow>().Where(r => r["Client"].ToString() == client && Convert.ToDouble(r["PriceOB"]) == Convert.ToDouble(drTemp[j]["PriceOB"]));
										rows.ToList().ForEach(r => r.SetField("MyAskQty", myAskQty));
										orderBook.AcceptChanges();
										rows = orderBook.Rows.Cast<DataRow>().Where(r => r["Client"].ToString() == drTemp[j]["Client"].ToString() && Convert.ToDouble(r["PriceOB"]) == price && Convert.ToInt32(r["DealDir"]) == Convert.ToInt32(drTemp[j]["DealDir"]));
										rows.ToList().ForEach(r => r.SetField("MyBidQty", 0));
										orderBook.AcceptChanges();
										orderHistory.Rows.Add(drTemp[j]["Client"].ToString(), client, "StockA", MyBidQty, Convert.ToDouble(drTemp[j]["PriceOB"].ToString()));
										if (!clientsToNotify.ContainsKey(Convert.ToInt16(client.Substring(client.Length - 1))))
										{
											clientsToNotify.Add(Convert.ToInt16(client.Substring(client.Length - 1)), "Trade executed with " + drTemp[j]["Client"].ToString());
										}
										else
										{
											string tempMsg = string.Empty;
											clientsToNotify.TryGetValue(Convert.ToInt16(client.Substring(client.Length - 1)), out tempMsg);
											clientsToNotify[Convert.ToInt16(client.Substring(client.Length - 1))] = tempMsg + ", " + drTemp[j]["Client"].ToString();
										}
										if (!clientsToNotify.ContainsKey(Convert.ToInt16(drTemp[j]["Client"].ToString().Substring(drTemp[j]["Client"].ToString().Length - 1))))
										{
											clientsToNotify.Add(Convert.ToInt16(drTemp[j]["Client"].ToString().Substring(drTemp[j]["Client"].ToString().Length - 1)), "Trade executed with " + client);
										}
										else
										{
											string tempMsg = string.Empty;
											clientsToNotify.TryGetValue(Convert.ToInt16(drTemp[j]["Client"].ToString().Substring(drTemp[j]["Client"].ToString().Length - 1)), out tempMsg);
											clientsToNotify[Convert.ToInt16(drTemp[j]["Client"].ToString().Substring(drTemp[j]["Client"].ToString().Length - 1))] = tempMsg + ", " + client;
										}
										noOfTransactions++;
										if ((quantityToFill - MyBidQty) <= 0)
										{
											quantityToFill = 0;
											break;
										}
										else
										{
											quantityToFill = quantityToFill - MyBidQty;
										}
									}

								}
							}
						}
					}
				}
				else
				{
					return 0;
				}
				return noOfTransactions;
			}
			catch (Exception ex)
			{
				Console.Write("Error in tryexecuteOrder: " + ex.Message);
				return 0;
			}
		}
		//Insert Data to market order table
		private static void insertToOrderTable(string client, string symbol, double price, int quantity, int dealdir)
		{
			string query = string.Empty;
			try
			{
					if (dealdir == ((int)Dealdir.Buy))
					{
					orderBook.Rows.Add(client, (int)Dealdir.Buy ,quantity, quantity, price, 0, 0, DateTime.Now);
						query = "PriceOB=" + price;
						DataRow[] drTemp = orderBook.Select(query);
						if (drTemp.Count() > 1)
						{
							IEnumerable<DataRow> rows = orderBook.Rows.Cast<DataRow>().Where(r => Convert.ToDouble(r["PriceOB"]) == price);
							int newMktBidQty = Convert.ToInt16(rows.ElementAt(0)["MktBidQty"]);
							rows.ToList().ForEach(r =>
							{
								r.SetField("MktBidQty", newMktBidQty + quantity);
							});
						}
					}
					else
					{
					orderBook.Rows.Add(client, (int)Dealdir.Sell,0, 0, price, quantity, quantity, DateTime.Now);
						query = "PriceOB=" + price;
						DataRow[] drTemp = orderBook.Select(query);
						if (drTemp.Count() > 1)
						{
							IEnumerable<DataRow> rows = orderBook.Rows.Cast<DataRow>().Where(r => Convert.ToDouble(r["PriceOB"]) == price);
							int newMktAskQty = Convert.ToInt16(rows.ElementAt(0)["MktAskQty"]);
							rows.ToList().ForEach(r =>
							{
								r.SetField("MktAskQty", newMktAskQty + quantity);
							});
						}

					}
				server.Invoke(new MethodInvoker(delegate ()
				{
					server.Dock = DockStyle.Fill;
					server.gvOrderBook.AutoGenerateColumns = false;
					server.gvOrderBook.DataSource = orderBook;
					server.gvOrderBook.Refresh();
					//server.gvOrderBook_ColumnHeaderMouseClick(null, null);
				}));
			}
			catch (Exception ex)
						{
				Console.Write("Error in insertToOrderTable: " + ex.Message);
			}

		}
		#endregion
		#region ConnectionMessageSend
		//Start Listening
		private static void setupServer()
		{
			try
			{
				//Console.WriteLine("Setting up Server");
				//__ClientSockets = new List<SocketT2h>();
				_serverSocket.Bind(new IPEndPoint(IPAddress.Any, 100));
				_serverSocket.Listen(5);
				_serverSocket.BeginAccept(new AsyncCallback(AcceptCallBack), null);
				Console.WriteLine("Server up");
			}
			catch (Exception ex) {
				Console.Write("Error in setupServer: "+ex.Message);
			}
		}
		//Accept a connection from client
		private static void AcceptCallBack(IAsyncResult ar)
		{
			try
			{
				Socket socket = _serverSocket.EndAccept(ar);
				Console.WriteLine("Client Connected" + socket.LocalEndPoint.ToString());
				_clientSockets.Add(_clientSockets.Count + 1, socket);
				//_listOfClients.Add(_clientSockets.Count,"Client"+ _clientSockets.Count);
				socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallBack), socket);
				_serverSocket.BeginAccept(new AsyncCallback(AcceptCallBack), null);             //Allow to accept more than one connection
			}
			catch (Exception ex)
			{
				Console.Write("Error in AcceptCallBack: " + ex.Message);
			}
		}
		//Receive message from client
		private static void ReceiveCallBack(IAsyncResult ar)
		{
			try
			{
				Socket socket = (Socket)ar.AsyncState;
				System.Threading.Thread.Sleep(300);
				if (socket.Connected == true)
				{
					int received = socket.EndReceive(ar);       //size of data received;
					byte[] dataBuf = new byte[received];
					Array.Copy(_buffer, dataBuf, received);
					string text = Encoding.ASCII.GetString(dataBuf);
					if (text != "")
					{
						string[] modeAndRequest = text.Split('|');
						string response = string.Empty;
						switch (Convert.ToInt16(modeAndRequest[0]))
						{
							case (int)ModeToSendMessage.GetOrderList:
								//response = createOrderBookSendToClient(modeAndRequest[1]);
								//response = (int)ModeToSendMessage.GetOrderList + "|" + response;
								break;
							case (int)ModeToSendMessage.sendOrder:
								processIncomingOrder(modeAndRequest[1]);
								if (tradeBooked)
								{
									//response = "200|trade executed";
									//tradebooked = false;
								}
								else
									response = "200|Order placed successfully";
								//console.write();
								break;
							default:
								break;
						}

						byte[] data = Encoding.ASCII.GetBytes(response);
						socket.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(SendCallBack), socket);
						socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallBack), socket);
					}
				}
					//	_clientSockets.Remove(socket);
				}
			catch (Exception ex)
			{
				Console.Write("Error in ReceiveCallBack: " + ex.Message);
			}
			
		}
		//Send message to client
		private static void SendCallBack(IAsyncResult ar)
		{
			try
			{
				Socket socket = (Socket)ar.AsyncState;
				socket.EndSend(ar);
			}
			catch (Exception ex)
			{
				Console.Write("Error in SendCallBack: " + ex.Message);
			}
		}
		//Broadcast / Multicast depending on a situation
		private static void broadCastMessage(int mode)
		{
			try
			{
				if (mode == (int)ModeToSendMessage.OrderHistory)
				{
					var bytes = Encoding.ASCII.GetBytes((int)ModeToSendMessage.OrderHistory + "|" + dtToJSON(orderHistory));
					foreach (var c in _clientSockets)
					{
						Socket tempSocket = c.Value;
						int tempClient = c.Key;
						if (tempSocket.Connected == false)
						{
							_clientSockets.Remove(tempClient);
						}
						else
						{
							tempSocket.BeginSend(bytes, 0, bytes.Length, SocketFlags.None, SendCallBack, tempSocket);
						}
					}
				}
				else if (mode == (int)ModeToSendMessage.GetOrderList)
				{
					var bytes = Encoding.ASCII.GetBytes((int)ModeToSendMessage.GetOrderList + "|" + dtToJSON(orderBook));
					foreach (var c in _clientSockets)
					{

						Socket tempSocket = c.Value;
						int tempClient = c.Key;
						if (tempSocket.Connected == false)
						{
							_clientSockets.Remove(tempClient);
						}
						else
						{
							tempSocket.BeginSend(bytes, 0, bytes.Length, SocketFlags.None, SendCallBack, tempSocket);
						}
					}
				}
				else if (mode == (int)ModeToSendMessage.SendNotification)
				{
					foreach (var c in _clientSockets)
					{
						Socket tempSocket = c.Value;
						int tempClient = c.Key;
						if (tempSocket.Connected == false)
						{
							_clientSockets.Remove(tempClient);
						}
						else
						{
							if (clientsToNotify.ContainsKey(tempClient))
							{
								string tempMsg = string.Empty;
								clientsToNotify.TryGetValue(tempClient, out tempMsg);
								var bytes = Encoding.ASCII.GetBytes((int)ModeToSendMessage.SendNotification + "|" + tempMsg);
								tempSocket.BeginSend(bytes, 0, bytes.Length, SocketFlags.None, SendCallBack, tempSocket);
							}
						}
					}
					clientsToNotify.Clear();
				}
			}
			catch (Exception ex)
			{
				Console.Write("Error in broadCastMessage: " + ex.Message);
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
