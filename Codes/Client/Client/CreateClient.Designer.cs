namespace Client
{
	partial class CreateClient
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
			this.createclient1 = new System.Windows.Forms.Button();
			this.orderHistory = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// createclient1
			// 
			this.createclient1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(51)))), ((int)(((byte)(102)))));
			this.createclient1.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(51)))), ((int)(((byte)(102)))));
			this.createclient1.FlatAppearance.BorderSize = 0;
			this.createclient1.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(51)))), ((int)(((byte)(102)))));
			this.createclient1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(51)))), ((int)(((byte)(102)))));
			this.createclient1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.createclient1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.createclient1.ForeColor = System.Drawing.Color.White;
			this.createclient1.Location = new System.Drawing.Point(25, 12);
			this.createclient1.Name = "createclient1";
			this.createclient1.Size = new System.Drawing.Size(109, 34);
			this.createclient1.TabIndex = 0;
			this.createclient1.Text = "Create New Client";
			this.createclient1.UseVisualStyleBackColor = false;
			this.createclient1.Click += new System.EventHandler(this.createclient1_Click);
			// 
			// orderHistory
			// 
			this.orderHistory.AutoSize = true;
			this.orderHistory.Location = new System.Drawing.Point(58, 6);
			this.orderHistory.Name = "orderHistory";
			this.orderHistory.Size = new System.Drawing.Size(10, 13);
			this.orderHistory.TabIndex = 1;
			this.orderHistory.Text = " ";
			this.orderHistory.Visible = false;
			this.orderHistory.TextChanged += new System.EventHandler(this.orderHistory_TextChanged);
			// 
			// CreateClient
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(175, 60);
			this.Controls.Add(this.orderHistory);
			this.Controls.Add(this.createclient1);
			this.Name = "CreateClient";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button createclient1;
		private System.Windows.Forms.Label orderHistory;
	}
}