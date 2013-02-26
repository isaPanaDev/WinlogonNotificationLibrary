namespace eventsample
{
	partial class Form1
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
			this.listBoxEvents = new System.Windows.Forms.ListBox();
			this.SuspendLayout();
			// 
			// listBoxEvents
			// 
			this.listBoxEvents.FormattingEnabled = true;
			this.listBoxEvents.ItemHeight = 16;
			this.listBoxEvents.Location = new System.Drawing.Point(12, 58);
			this.listBoxEvents.Name = "listBoxEvents";
			this.listBoxEvents.Size = new System.Drawing.Size(394, 276);
			this.listBoxEvents.TabIndex = 0;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(438, 353);
			this.Controls.Add(this.listBoxEvents);
			this.Name = "Form1";
			this.Text = "Form1";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListBox listBoxEvents;
	}
}

