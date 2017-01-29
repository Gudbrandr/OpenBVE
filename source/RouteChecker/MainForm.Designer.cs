/*
 * Created by SharpDevelop.
 * User: Gudbrandr
 * Date: 15/05/2009
 * Time: 3:14 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

namespace OpenBve
{
	partial class MainForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.txtRoute = new System.Windows.Forms.TextBox();
			this.lblRoute = new System.Windows.Forms.Label();
			this.btnBrowse = new System.Windows.Forms.Button();
			this.lvMessages = new System.Windows.Forms.ListView();
			this.columnHeaderType = new System.Windows.Forms.ColumnHeader();
			this.columnHeaderDescription = new System.Windows.Forms.ColumnHeader();
			this.btnExit = new System.Windows.Forms.Button();
			this.btnProcessRoute = new System.Windows.Forms.Button();
			this.btnClipboard = new System.Windows.Forms.Button();
			this.progRoute = new System.Windows.Forms.ProgressBar();
			this.btnExportCsv = new System.Windows.Forms.Button();
			this.cbWallReport = new System.Windows.Forms.CheckBox();
			this.cbRailReport = new System.Windows.Forms.CheckBox();
			this.cbPoleReport = new System.Windows.Forms.CheckBox();
			this.cbDikeReport = new System.Windows.Forms.CheckBox();
			this.BottomToolStripPanel = new System.Windows.Forms.ToolStripPanel();
			this.TopToolStripPanel = new System.Windows.Forms.ToolStripPanel();
			this.RightToolStripPanel = new System.Windows.Forms.ToolStripPanel();
			this.LeftToolStripPanel = new System.Windows.Forms.ToolStripPanel();
			this.ContentPanel = new System.Windows.Forms.ToolStripContentPanel();
			this.btnNextError = new System.Windows.Forms.Button();
			this.btnLastError = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// txtRoute
			// 
			this.txtRoute.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.txtRoute.Enabled = false;
			this.txtRoute.Location = new System.Drawing.Point(76, 13);
			this.txtRoute.Name = "txtRoute";
			this.txtRoute.Size = new System.Drawing.Size(675, 20);
			this.txtRoute.TabIndex = 0;
			// 
			// lblRoute
			// 
			this.lblRoute.Location = new System.Drawing.Point(12, 12);
			this.lblRoute.Name = "lblRoute";
			this.lblRoute.Size = new System.Drawing.Size(58, 22);
			this.lblRoute.TabIndex = 1;
			this.lblRoute.Text = "Route File";
			this.lblRoute.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// btnBrowse
			// 
			this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnBrowse.Location = new System.Drawing.Point(757, 12);
			this.btnBrowse.Name = "btnBrowse";
			this.btnBrowse.Size = new System.Drawing.Size(75, 23);
			this.btnBrowse.TabIndex = 2;
			this.btnBrowse.Text = "Browse";
			this.btnBrowse.UseVisualStyleBackColor = true;
			this.btnBrowse.Click += new System.EventHandler(this.BtnBrowseClick);
			// 
			// lvMessages
			// 
			this.lvMessages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
			| System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.lvMessages.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
			this.columnHeaderType,
			this.columnHeaderDescription});
			this.lvMessages.FullRowSelect = true;
			this.lvMessages.GridLines = true;
			this.lvMessages.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.lvMessages.Location = new System.Drawing.Point(12, 71);
			this.lvMessages.MultiSelect = false;
			this.lvMessages.Name = "lvMessages";
			this.lvMessages.Size = new System.Drawing.Size(819, 492);
			this.lvMessages.TabIndex = 3;
			this.lvMessages.UseCompatibleStateImageBehavior = false;
			this.lvMessages.View = System.Windows.Forms.View.Details;
			// 
			// columnHeaderType
			// 
			this.columnHeaderType.Text = "Type";
			this.columnHeaderType.Width = 90;
			// 
			// columnHeaderDescription
			// 
			this.columnHeaderDescription.Text = "Description";
			this.columnHeaderDescription.Width = 700;
			// 
			// btnExit
			// 
			this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnExit.Location = new System.Drawing.Point(757, 569);
			this.btnExit.Name = "btnExit";
			this.btnExit.Size = new System.Drawing.Size(75, 23);
			this.btnExit.TabIndex = 4;
			this.btnExit.Text = "Exit";
			this.btnExit.UseVisualStyleBackColor = true;
			this.btnExit.Click += new System.EventHandler(this.BtnExitClick);
			// 
			// btnProcessRoute
			// 
			this.btnProcessRoute.Enabled = false;
			this.btnProcessRoute.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.btnProcessRoute.Location = new System.Drawing.Point(416, 42);
			this.btnProcessRoute.Name = "btnProcessRoute";
			this.btnProcessRoute.Size = new System.Drawing.Size(88, 23);
			this.btnProcessRoute.TabIndex = 5;
			this.btnProcessRoute.Text = "Process Route";
			this.btnProcessRoute.UseVisualStyleBackColor = true;
			this.btnProcessRoute.Click += new System.EventHandler(this.BtnProcessRouteClick);
			// 
			// btnClipboard
			// 
			this.btnClipboard.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnClipboard.Enabled = false;
			this.btnClipboard.Location = new System.Drawing.Point(640, 569);
			this.btnClipboard.Name = "btnClipboard";
			this.btnClipboard.Size = new System.Drawing.Size(112, 23);
			this.btnClipboard.TabIndex = 6;
			this.btnClipboard.Text = "Copy To Clipboard";
			this.btnClipboard.UseVisualStyleBackColor = true;
			this.btnClipboard.Click += new System.EventHandler(this.btnClipboardClick);
			// 
			// progRoute
			// 
			this.progRoute.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.progRoute.Location = new System.Drawing.Point(12, 569);
			this.progRoute.Name = "progRoute";
			this.progRoute.Size = new System.Drawing.Size(540, 23);
			this.progRoute.TabIndex = 7;
			// 
			// btnExportCsv
			// 
			this.btnExportCsv.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnExportCsv.Enabled = false;
			this.btnExportCsv.Location = new System.Drawing.Point(560, 569);
			this.btnExportCsv.Name = "btnExportCsv";
			this.btnExportCsv.Size = new System.Drawing.Size(75, 23);
			this.btnExportCsv.TabIndex = 8;
			this.btnExportCsv.Text = "Export CSV";
			this.btnExportCsv.UseVisualStyleBackColor = true;
			this.btnExportCsv.Click += new System.EventHandler(this.BtnExportCsvClick);
			// 
			// cbWallReport
			// 
			this.cbWallReport.Location = new System.Drawing.Point(331, 42);
			this.cbWallReport.MaximumSize = new System.Drawing.Size(85, 23);
			this.cbWallReport.Name = "cbWallReport";
			this.cbWallReport.Size = new System.Drawing.Size(85, 23);
			this.cbWallReport.TabIndex = 9;
			this.cbWallReport.Text = "Wall Report";
			this.cbWallReport.UseVisualStyleBackColor = true;
			// 
			// cbRailReport
			// 
			this.cbRailReport.Location = new System.Drawing.Point(246, 42);
			this.cbRailReport.MinimumSize = new System.Drawing.Size(85, 23);
			this.cbRailReport.Name = "cbRailReport";
			this.cbRailReport.Size = new System.Drawing.Size(85, 23);
			this.cbRailReport.TabIndex = 10;
			this.cbRailReport.Text = "Rail Report";
			this.cbRailReport.UseVisualStyleBackColor = true;
			// 
			// cbPoleReport
			// 
			this.cbPoleReport.Location = new System.Drawing.Point(161, 42);
			this.cbPoleReport.MaximumSize = new System.Drawing.Size(85, 23);
			this.cbPoleReport.Name = "cbPoleReport";
			this.cbPoleReport.Size = new System.Drawing.Size(85, 23);
			this.cbPoleReport.TabIndex = 11;
			this.cbPoleReport.Text = "Pole Report";
			this.cbPoleReport.UseVisualStyleBackColor = true;
			// 
			// cbDikeReport
			// 
			this.cbDikeReport.Location = new System.Drawing.Point(76, 42);
			this.cbDikeReport.MaximumSize = new System.Drawing.Size(85, 23);
			this.cbDikeReport.Name = "cbDikeReport";
			this.cbDikeReport.Size = new System.Drawing.Size(85, 23);
			this.cbDikeReport.TabIndex = 12;
			this.cbDikeReport.Text = "Dike Report";
			this.cbDikeReport.UseVisualStyleBackColor = true;
			// 
			// BottomToolStripPanel
			// 
			this.BottomToolStripPanel.Location = new System.Drawing.Point(0, 0);
			this.BottomToolStripPanel.Name = "BottomToolStripPanel";
			this.BottomToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
			this.BottomToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
			this.BottomToolStripPanel.Size = new System.Drawing.Size(0, 0);
			// 
			// TopToolStripPanel
			// 
			this.TopToolStripPanel.Location = new System.Drawing.Point(0, 0);
			this.TopToolStripPanel.Name = "TopToolStripPanel";
			this.TopToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
			this.TopToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
			this.TopToolStripPanel.Size = new System.Drawing.Size(0, 0);
			// 
			// RightToolStripPanel
			// 
			this.RightToolStripPanel.Location = new System.Drawing.Point(0, 0);
			this.RightToolStripPanel.Name = "RightToolStripPanel";
			this.RightToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
			this.RightToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
			this.RightToolStripPanel.Size = new System.Drawing.Size(0, 0);
			// 
			// LeftToolStripPanel
			// 
			this.LeftToolStripPanel.Location = new System.Drawing.Point(0, 0);
			this.LeftToolStripPanel.Name = "LeftToolStripPanel";
			this.LeftToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
			this.LeftToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
			this.LeftToolStripPanel.Size = new System.Drawing.Size(0, 0);
			// 
			// ContentPanel
			// 
			this.ContentPanel.Size = new System.Drawing.Size(576, 128);
			// 
			// btnNextError
			// 
			this.btnNextError.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnNextError.Image = ((System.Drawing.Image)(resources.GetObject("btnNextError.Image")));
			this.btnNextError.Location = new System.Drawing.Point(757, 42);
			this.btnNextError.Name = "btnNextError";
			this.btnNextError.Size = new System.Drawing.Size(35, 23);
			this.btnNextError.TabIndex = 13;
			this.btnNextError.UseVisualStyleBackColor = true;
			this.btnNextError.Click += new System.EventHandler(this.BtnNextErrorClick);
			// 
			// btnLastError
			// 
			this.btnLastError.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnLastError.Image = ((System.Drawing.Image)(resources.GetObject("btnLastError.Image")));
			this.btnLastError.Location = new System.Drawing.Point(797, 42);
			this.btnLastError.Name = "btnLastError";
			this.btnLastError.Size = new System.Drawing.Size(35, 23);
			this.btnLastError.TabIndex = 14;
			this.btnLastError.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnLastError.UseVisualStyleBackColor = true;
			this.btnLastError.Click += new System.EventHandler(this.BtnLastErrorClick);
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label1.Location = new System.Drawing.Point(694, 42);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(58, 23);
			this.label1.TabIndex = 15;
			this.label1.Text = "Next Error:";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(843, 598);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.btnProcessRoute);
			this.Controls.Add(this.cbWallReport);
			this.Controls.Add(this.btnLastError);
			this.Controls.Add(this.cbDikeReport);
			this.Controls.Add(this.cbRailReport);
			this.Controls.Add(this.btnNextError);
			this.Controls.Add(this.cbPoleReport);
			this.Controls.Add(this.btnExportCsv);
			this.Controls.Add(this.progRoute);
			this.Controls.Add(this.btnClipboard);
			this.Controls.Add(this.btnExit);
			this.Controls.Add(this.lvMessages);
			this.Controls.Add(this.btnBrowse);
			this.Controls.Add(this.lblRoute);
			this.Controls.Add(this.txtRoute);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(680, 300);
			this.Name = "MainForm";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
			this.Text = "Route Checker";
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btnLastError;
		private System.Windows.Forms.ToolStripContentPanel ContentPanel;
		private System.Windows.Forms.ToolStripPanel LeftToolStripPanel;
		private System.Windows.Forms.ToolStripPanel RightToolStripPanel;
		private System.Windows.Forms.ToolStripPanel TopToolStripPanel;
		private System.Windows.Forms.ToolStripPanel BottomToolStripPanel;
		private System.Windows.Forms.Button btnNextError;
		private System.Windows.Forms.CheckBox cbWallReport;
		private System.Windows.Forms.CheckBox cbRailReport;
		private System.Windows.Forms.CheckBox cbPoleReport;
		private System.Windows.Forms.CheckBox cbDikeReport;
		private System.Windows.Forms.Button btnExportCsv;
		public System.Windows.Forms.ProgressBar progRoute;
		private System.Windows.Forms.Button btnClipboard;
		private System.Windows.Forms.ListView lvMessages;
		private System.Windows.Forms.Button btnProcessRoute;
		private System.Windows.Forms.ColumnHeader columnHeaderDescription;
		private System.Windows.Forms.ColumnHeader columnHeaderType;
		private System.Windows.Forms.Button btnExit;
		private System.Windows.Forms.Button btnBrowse;
		private System.Windows.Forms.Label lblRoute;
		private System.Windows.Forms.TextBox txtRoute;
	}
}
