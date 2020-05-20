namespace Server_UI
{
	partial class Server
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle18 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle19 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle25 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle20 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle21 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle22 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle23 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle24 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle26 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle27 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle33 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle34 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle28 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle29 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle30 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle31 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle32 = new System.Windows.Forms.DataGridViewCellStyle();
			this.gvOrderBook = new System.Windows.Forms.DataGridView();
			this.Client = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.MyBidQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.MktBidQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.PriceOB = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.MyAskQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.MktAskQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.gvOrderHistory = new System.Windows.Forms.DataGridView();
			this.Buyer = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Seller = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.SymbolH = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.QtyH = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.PriceH = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.loginInfo = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.gvOrderBook)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.gvOrderHistory)).BeginInit();
			this.SuspendLayout();
			// 
			// gvOrderBook
			// 
			this.gvOrderBook.AllowUserToAddRows = false;
			this.gvOrderBook.AllowUserToDeleteRows = false;
			dataGridViewCellStyle18.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
			dataGridViewCellStyle18.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewCellStyle18.ForeColor = System.Drawing.Color.Black;
			dataGridViewCellStyle18.SelectionBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
			dataGridViewCellStyle18.SelectionForeColor = System.Drawing.Color.Black;
			this.gvOrderBook.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle18;
			this.gvOrderBook.BackgroundColor = System.Drawing.Color.White;
			this.gvOrderBook.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			dataGridViewCellStyle19.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle19.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(123)))), ((int)(((byte)(196)))));
			dataGridViewCellStyle19.Font = new System.Drawing.Font("Times New Roman", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewCellStyle19.ForeColor = System.Drawing.Color.White;
			dataGridViewCellStyle19.SelectionBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
			dataGridViewCellStyle19.SelectionForeColor = System.Drawing.Color.Black;
			dataGridViewCellStyle19.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.gvOrderBook.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle19;
			this.gvOrderBook.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.gvOrderBook.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Client,
            this.MyBidQty,
            this.MktBidQty,
            this.PriceOB,
            this.MyAskQty,
            this.MktAskQty});
			this.gvOrderBook.EnableHeadersVisualStyles = false;
			this.gvOrderBook.Location = new System.Drawing.Point(12, 62);
			this.gvOrderBook.Name = "gvOrderBook";
			this.gvOrderBook.ReadOnly = true;
			this.gvOrderBook.RowHeadersVisible = false;
			dataGridViewCellStyle25.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle25.BackColor = System.Drawing.Color.Silver;
			dataGridViewCellStyle25.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewCellStyle25.SelectionBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
			dataGridViewCellStyle25.SelectionForeColor = System.Drawing.Color.Black;
			this.gvOrderBook.RowsDefaultCellStyle = dataGridViewCellStyle25;
			this.gvOrderBook.Size = new System.Drawing.Size(394, 254);
			this.gvOrderBook.TabIndex = 5;
			// 
			// Client
			// 
			this.Client.DataPropertyName = "Client";
			this.Client.HeaderText = "Client";
			this.Client.Name = "Client";
			this.Client.ReadOnly = true;
			this.Client.Width = 50;
			// 
			// MyBidQty
			// 
			this.MyBidQty.DataPropertyName = "MyBidQty";
			dataGridViewCellStyle20.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			this.MyBidQty.DefaultCellStyle = dataGridViewCellStyle20;
			this.MyBidQty.HeaderText = "My Bid Qty";
			this.MyBidQty.Name = "MyBidQty";
			this.MyBidQty.ReadOnly = true;
			this.MyBidQty.Width = 75;
			// 
			// MktBidQty
			// 
			this.MktBidQty.DataPropertyName = "MktBidQty";
			dataGridViewCellStyle21.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			this.MktBidQty.DefaultCellStyle = dataGridViewCellStyle21;
			this.MktBidQty.HeaderText = "Mkt Bid Qty";
			this.MktBidQty.Name = "MktBidQty";
			this.MktBidQty.ReadOnly = true;
			this.MktBidQty.Width = 73;
			// 
			// PriceOB
			// 
			this.PriceOB.DataPropertyName = "PriceOB";
			dataGridViewCellStyle22.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			this.PriceOB.DefaultCellStyle = dataGridViewCellStyle22;
			this.PriceOB.HeaderText = "Price";
			this.PriceOB.Name = "PriceOB";
			this.PriceOB.ReadOnly = true;
			this.PriceOB.Width = 50;
			// 
			// MyAskQty
			// 
			this.MyAskQty.DataPropertyName = "MyAskQty";
			dataGridViewCellStyle23.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			this.MyAskQty.DefaultCellStyle = dataGridViewCellStyle23;
			this.MyAskQty.HeaderText = "My Ask Qty";
			this.MyAskQty.Name = "MyAskQty";
			this.MyAskQty.ReadOnly = true;
			this.MyAskQty.Width = 70;
			// 
			// MktAskQty
			// 
			this.MktAskQty.DataPropertyName = "MktAskQty";
			dataGridViewCellStyle24.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			this.MktAskQty.DefaultCellStyle = dataGridViewCellStyle24;
			this.MktAskQty.HeaderText = "Mkt Ask Qty";
			this.MktAskQty.Name = "MktAskQty";
			this.MktAskQty.ReadOnly = true;
			this.MktAskQty.Width = 75;
			// 
			// gvOrderHistory
			// 
			this.gvOrderHistory.AllowUserToAddRows = false;
			this.gvOrderHistory.AllowUserToDeleteRows = false;
			dataGridViewCellStyle26.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
			dataGridViewCellStyle26.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewCellStyle26.ForeColor = System.Drawing.Color.Black;
			this.gvOrderHistory.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle26;
			this.gvOrderHistory.BackgroundColor = System.Drawing.Color.White;
			this.gvOrderHistory.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			dataGridViewCellStyle27.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle27.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(123)))), ((int)(((byte)(196)))));
			dataGridViewCellStyle27.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewCellStyle27.ForeColor = System.Drawing.Color.White;
			dataGridViewCellStyle27.SelectionBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
			dataGridViewCellStyle27.SelectionForeColor = System.Drawing.Color.Black;
			dataGridViewCellStyle27.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.gvOrderHistory.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle27;
			this.gvOrderHistory.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Buyer,
            this.Seller,
            this.SymbolH,
            this.QtyH,
            this.PriceH});
			this.gvOrderHistory.EnableHeadersVisualStyles = false;
			this.gvOrderHistory.Location = new System.Drawing.Point(419, 62);
			this.gvOrderHistory.Name = "gvOrderHistory";
			dataGridViewCellStyle33.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle33.BackColor = System.Drawing.Color.White;
			dataGridViewCellStyle33.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewCellStyle33.ForeColor = System.Drawing.Color.Black;
			dataGridViewCellStyle33.SelectionBackColor = System.Drawing.Color.LightGray;
			dataGridViewCellStyle33.SelectionForeColor = System.Drawing.Color.Black;
			dataGridViewCellStyle33.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.gvOrderHistory.RowHeadersDefaultCellStyle = dataGridViewCellStyle33;
			this.gvOrderHistory.RowHeadersVisible = false;
			dataGridViewCellStyle34.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle34.BackColor = System.Drawing.Color.Silver;
			dataGridViewCellStyle34.SelectionBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
			dataGridViewCellStyle34.SelectionForeColor = System.Drawing.Color.Black;
			this.gvOrderHistory.RowsDefaultCellStyle = dataGridViewCellStyle34;
			this.gvOrderHistory.Size = new System.Drawing.Size(251, 254);
			this.gvOrderHistory.TabIndex = 6;
			// 
			// Buyer
			// 
			this.Buyer.DataPropertyName = "BuyerH";
			dataGridViewCellStyle28.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			this.Buyer.DefaultCellStyle = dataGridViewCellStyle28;
			this.Buyer.HeaderText = "Buyer";
			this.Buyer.Name = "Buyer";
			this.Buyer.ReadOnly = true;
			this.Buyer.Width = 50;
			// 
			// Seller
			// 
			this.Seller.DataPropertyName = "SellerH";
			dataGridViewCellStyle29.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			this.Seller.DefaultCellStyle = dataGridViewCellStyle29;
			this.Seller.HeaderText = "Seller";
			this.Seller.Name = "Seller";
			this.Seller.ReadOnly = true;
			this.Seller.Width = 50;
			// 
			// SymbolH
			// 
			this.SymbolH.DataPropertyName = "SymbolH";
			dataGridViewCellStyle30.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			this.SymbolH.DefaultCellStyle = dataGridViewCellStyle30;
			this.SymbolH.HeaderText = "Symbol";
			this.SymbolH.Name = "SymbolH";
			this.SymbolH.ReadOnly = true;
			this.SymbolH.Width = 50;
			// 
			// QtyH
			// 
			this.QtyH.DataPropertyName = "QtyH";
			dataGridViewCellStyle31.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			this.QtyH.DefaultCellStyle = dataGridViewCellStyle31;
			this.QtyH.HeaderText = "Qty";
			this.QtyH.Name = "QtyH";
			this.QtyH.ReadOnly = true;
			this.QtyH.Width = 50;
			// 
			// PriceH
			// 
			this.PriceH.DataPropertyName = "PriceH";
			dataGridViewCellStyle32.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			this.PriceH.DefaultCellStyle = dataGridViewCellStyle32;
			this.PriceH.HeaderText = "Price";
			this.PriceH.Name = "PriceH";
			this.PriceH.ReadOnly = true;
			this.PriceH.Width = 50;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(123)))), ((int)(((byte)(196)))));
			this.label2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.ForeColor = System.Drawing.Color.White;
			this.label2.Location = new System.Drawing.Point(12, 34);
			this.label2.MinimumSize = new System.Drawing.Size(394, 25);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(394, 25);
			this.label2.TabIndex = 11;
			this.label2.Text = "Market Order Book";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(123)))), ((int)(((byte)(196)))));
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3.ForeColor = System.Drawing.Color.White;
			this.label3.Location = new System.Drawing.Point(419, 34);
			this.label3.MinimumSize = new System.Drawing.Size(251, 25);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(251, 25);
			this.label3.TabIndex = 12;
			this.label3.Text = "Market Trades";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// loginInfo
			// 
			this.loginInfo.AutoSize = true;
			this.loginInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.loginInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(123)))), ((int)(((byte)(196)))));
			this.loginInfo.Location = new System.Drawing.Point(559, 9);
			this.loginInfo.Name = "loginInfo";
			this.loginInfo.Size = new System.Drawing.Size(0, 13);
			this.loginInfo.TabIndex = 13;
			// 
			// Server
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(686, 406);
			this.Controls.Add(this.loginInfo);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.gvOrderHistory);
			this.Controls.Add(this.gvOrderBook);
			this.Name = "Server";
			this.Text = "Server";
			((System.ComponentModel.ISupportInitialize)(this.gvOrderBook)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.gvOrderHistory)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.DataGridView gvOrderBook;
		private System.Windows.Forms.DataGridView gvOrderHistory;
		private System.Windows.Forms.DataGridViewTextBoxColumn Buyer;
		private System.Windows.Forms.DataGridViewTextBoxColumn Seller;
		private System.Windows.Forms.DataGridViewTextBoxColumn SymbolH;
		private System.Windows.Forms.DataGridViewTextBoxColumn QtyH;
		private System.Windows.Forms.DataGridViewTextBoxColumn PriceH;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label loginInfo;
		private System.Windows.Forms.DataGridViewTextBoxColumn Client;
		private System.Windows.Forms.DataGridViewTextBoxColumn MyBidQty;
		private System.Windows.Forms.DataGridViewTextBoxColumn MktBidQty;
		private System.Windows.Forms.DataGridViewTextBoxColumn PriceOB;
		private System.Windows.Forms.DataGridViewTextBoxColumn MyAskQty;
		private System.Windows.Forms.DataGridViewTextBoxColumn MktAskQty;
	}
}

