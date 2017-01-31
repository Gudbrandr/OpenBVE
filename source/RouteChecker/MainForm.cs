/*
 * Created by SharpDevelop.
 * User: Gudbrandr
 * Date: 15/05/2009
 * Time: 3:14 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using OpenBve;


namespace OpenBve
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		private string strFilePath;
	    private string RailwayFolder;
		private string ObjectFolder;
		private string SoundFolder;
		private string DataFolder;
		

		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			this.DataFolder = Program.FileSystem.GetDataFolder("Menu");
			
			// Register to receive custom progress events.
			CsvRwRouteParser.onProgress += this.HandleProgressEvent;
		}
		
		private void BtnBrowseClick(object sender, EventArgs e)
		{
		    OpenFileDialog ofdRoute = new OpenFileDialog();
		
		    ofdRoute.InitialDirectory = this.RailwayFolder + "/Route" ;
		    ofdRoute.Filter = "BVE route files (*.csv;*.rw)|*.csv;*.rw|All files (*.*)|*.*" ;
		    ofdRoute.RestoreDirectory = true ;
		
		    if(ofdRoute.ShowDialog() == DialogResult.OK){
				this.strFilePath = ofdRoute.FileName;
			    // Get folders.
			    string rf = OpenBve.Loading.GetRailwayFolder(this.strFilePath);
			    if(rf != null){
			    	txtRoute.Text = ofdRoute.FileName;
					this.RailwayFolder = rf;
					this.ObjectFolder = OpenBveApi.Path.CombineDirectory(RailwayFolder, "Object");
					this.SoundFolder = OpenBveApi.Path.CombineDirectory(RailwayFolder, "Sound");

					if(!btnProcessRoute.Enabled){
						btnProcessRoute.Enabled = true;
					}
			    }else{
			    	MessageBox.Show("Error getting railway folder from route file path.", "Route Checker Error", MessageBoxButtons.OK , MessageBoxIcon.Error);
			    }
				// Disable button, clear list.
				btnExportCsv.Enabled = false;
				btnClipboard.Enabled = false;
				progRoute.Value = 0;
				
				ListViewItem item1 = new ListViewItem("Type",0);
				ListViewItem item2 = new ListViewItem("Description",1);
				lvMessages.Clear();
				lvMessages.Columns.Add("Type", 90, HorizontalAlignment.Left);
				lvMessages.Columns.Add("Description", 700, HorizontalAlignment.Left);
		    }
		}
		
		private void BtnExitClick(object sender, EventArgs e)
		{
			this.Close();
		}
		
		private void BtnProcessRouteClick(object sender, EventArgs e)
		{
			// Start wait cursor.
			this.Cursor = Cursors.WaitCursor;
			
			ListViewItem item1 = new ListViewItem("Type",0);
			ListViewItem item2 = new ListViewItem("Description",1);
			lvMessages.Clear();
			lvMessages.Columns.Add("Type", 90, HorizontalAlignment.Left);
			lvMessages.Columns.Add("Description", 700, HorizontalAlignment.Left);

			bool bDike = cbDikeReport.Checked;
			bool bPole = cbPoleReport.Checked;
			bool bRail = cbRailReport.Checked;
			bool bWall = cbWallReport.Checked;
			try{
				Game.Reset();
				bool IsRW = string.Equals(System.IO.Path.GetExtension(strFilePath), ".rw", StringComparison.OrdinalIgnoreCase);
				CsvRwRouteParser.ParseRoute(strFilePath, IsRW, System.Text.Encoding.UTF8, Application.StartupPath, this.ObjectFolder, this.SoundFolder, false, bDike, bPole, bRail, bWall);
			}catch(Exception ex){
				switch(ex.GetType().ToString())
				{
					case "System.TypeLoadException":
						//Normally caused by attempting to use this in an older openBVE version
						var ec = ex as System.TypeLoadException;
						if (ec.TypeName.StartsWith("OpenBveApi"))
						{
							MessageBox.Show("Route Checker has failed to load the openBVE API. \n\nPlease make sure that your openBVE version is 1.5.0 or above.", "Route Checker Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
						}
						else
						{
							MessageBox.Show("Route Checker has failed to load the following library: \n\n" + ec.TypeName, "Route Checker Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
						}
						break;
					default:
						MessageBox.Show("Route Checker has experienced the following error:\n\n" + ex.Message +
								"\n\nPlease make sure you are running Route Checker in the OpenBVE directory.",
								"Route Checker Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
						break;
				}
			}
	        
			lvMessages.SmallImageList = new ImageList();
			try {
				lvMessages.SmallImageList.Images.Add("information", Image.FromFile(OpenBveApi.Path.CombineDirectory(this.DataFolder, "icon_information.png")));
			} catch { }
			try {
				lvMessages.SmallImageList.Images.Add("warning", Image.FromFile(OpenBveApi.Path.CombineDirectory(this.DataFolder, "icon_warning.png")));
			} catch { }
			try {
				lvMessages.SmallImageList.Images.Add("error", Image.FromFile(OpenBveApi.Path.CombineDirectory(this.DataFolder, "icon_error.png")));
			} catch { }
			try {
				lvMessages.SmallImageList.Images.Add("critical", Image.FromFile(OpenBveApi.Path.CombineDirectory(this.DataFolder, "icon_critical.png")));
			} catch { }
			Trace.WriteLine(Interface.MessageCount);
			for(int i = 0; i < Interface.MessageCount; i++){
				string t = "Unknown";
				string g = "information";
				switch (Interface.Messages[i].Type) {
					case Interface.MessageType.Information:
						t = "Information";
						g = "information";
						break;
					case Interface.MessageType.Warning:
						t = "Warning";
						g = "warning";
						break;
					case Interface.MessageType.Error:
						t = "Error";
						g = "error";
						break;
					case Interface.MessageType.Critical:
						t = "Critical";
						g = "critical";
						break;
				}
				ListViewItem a = lvMessages.Items.Add(t, g);
				a.SubItems.Add(Interface.Messages[i].Text);
			}
			// Avoid column resizing when no text is present.
			if(Interface.MessageCount > 0){
				lvMessages.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
				btnClipboard.Enabled = true;
			}else{
				ListViewItem a = lvMessages.Items.Add("Information", "Information");
				a.SubItems.Add("Nothing to report.");
			}

			int j = 0;
			Color shaded = Color.FromArgb(240, 240, 240);
			foreach(ListViewItem item in lvMessages.Items){
				if(j++ % 2 == 1){
					item.BackColor = shaded;
					item.UseItemStyleForSubItems = true;
				}
			}
			// Toggle the CSV export button.
			if(Path.GetExtension(strFilePath).ToLower() == ".rw"){
				btnExportCsv.Enabled = true;
			}
	        
			// Stop wait cursor.
			this.Cursor = Cursors.Default;
		}		
		
		private void btnClipboardClick(object sender, EventArgs e)
		{
       		string lines = "";
        	for(int i = 0;i <  lvMessages.Items.Count;i++ ){
        		lines += lvMessages.Items[i].SubItems[0].Text + "\t\t" + lvMessages.Items[i].SubItems[1].Text + "\n";
        	}
       		Clipboard.SetDataObject(lines, true);
        }

		// Custom progress event handler.
		void HandleProgressEvent(object sender, CsvRwRouteParser.ProgressEventArgs e)
		{
			progRoute.Value = Convert.ToInt32(e.ProgValue * 100);
		}
	
		void BtnExportCsvClick(object sender, EventArgs e)
		{
	        string path = "";

	        SaveFileDialog sfdRoute = new SaveFileDialog();
			sfdRoute.Filter = "CSV files (*.csv)|*.csv" ;
			sfdRoute.FileName = Path.GetFileNameWithoutExtension(strFilePath);
			if(sfdRoute.ShowDialog() == DialogResult.OK){
				path = sfdRoute.FileName;
				this.ConvertToCVS(CsvRwRouteParser.rwExpressions, path);
			}
		}
		
		void BtnNextErrorClick(object sender, EventArgs e)
		{
			int iSel = 0;
			if(lvMessages.SelectedIndices.Count > 0){
				iSel = lvMessages.SelectedIndices[0] + 1;
			}
			for(int i = iSel;i < lvMessages.Items.Count;i++){
				if(lvMessages.Items[i].SubItems[0].Text != "Information"){
					lvMessages.Items[i].Selected = true;
					lvMessages.Focus();
					lvMessages.Items[i].EnsureVisible();
					break;
				}
				
			}
		}

		void BtnLastErrorClick(object sender, EventArgs e)
		{
			int iSel = lvMessages.Items.Count - 1;
			if(lvMessages.SelectedIndices.Count > 0){
				iSel = lvMessages.SelectedIndices[0] - 1;
			}
			for(int i = iSel;i >= 0;i--){
				if(lvMessages.Items[i].SubItems[0].Text != "Information"){
					lvMessages.Items[i].Selected = true;
					lvMessages.Focus();
					lvMessages.Items[i].EnsureVisible();
					break;
				}
				
			}
		}

		//=================================================================================
		// CONVERSION TO CSV
		
		private enum With{Route, Train, Options, Structure, Texture, Cycle, Signal, Track};
		private static MainForm.With WithSection;
			
		private void ConvertToCVS(CsvRwRouteParser.Expression[] Expressions, string path)
		{
			int iTrack, iTrackCnt = 0;
			string strTabs = "";
			bool bNumLine = false;
			
	        // Section stringbuilders.
        	System.Text.StringBuilder sbRoute = new System.Text.StringBuilder();
        	System.Text.StringBuilder sbTrain = new System.Text.StringBuilder();
//        	System.Text.StringBuilder sbOptions = new System.Text.StringBuilder();
        	System.Text.StringBuilder sbStructure = new System.Text.StringBuilder();
//        	System.Text.StringBuilder sbTexture = new System.Text.StringBuilder();
        	System.Text.StringBuilder sbCycle = new System.Text.StringBuilder();
//        	System.Text.StringBuilder sbSignal = new System.Text.StringBuilder();
        	System.Text.StringBuilder sbTrack = new System.Text.StringBuilder();
        	for(int i = 0;i < Expressions.Length;i++){
        		OpenBve.CsvRwRouteParser.Expression exp = Expressions[i];
        		
        		switch (exp.Text){
        			case "[Route]":
			        	// Route section.
			        	sbRoute.Append("With Route" + Environment.NewLine);
			        	sbRoute.Append("\t.Comment " + InsertChr(Game.RouteComment) + Environment.NewLine);
			        	WithSection = With.Route;
		        		continue;
        			case "[Train]":
			        	sbTrain.Append(Environment.NewLine + "With Train" + Environment.NewLine);
        				WithSection = With.Train;
		        		continue;
//        			case "[Options]":
//			        	sbOptions.Append(Environment.NewLine + "With Options" + Environment.NewLine);
//        				WithSection = With.Options;
//		        		continue;
        			case "[Object]":
			        	sbStructure.Append(Environment.NewLine + "With Structure" + Environment.NewLine);
        				WithSection = With.Structure;
		        		continue;
//        			case "[Texture]":
//			        	sbTexture.Append(Environment.NewLine + "With Texture" + Environment.NewLine);
//        				WithSection = With.Texture;
//		        		continue;
        			case "[Cycle]":
			        	sbCycle.Append(Environment.NewLine + "With Cycle" + Environment.NewLine);
        				WithSection = With.Cycle;
		        		continue;
//        			case "[Signal]":
////			        	sbRoute.Append(Environment.NewLine + "With Signal" + Environment.NewLine);
//        				
//		        		continue;
        			case "[Railway]":
			        	sbTrack.Append(Environment.NewLine + "With Track" + Environment.NewLine);
        				WithSection = With.Track;
		        		continue;
        		}
        	
        		// Write commands:
        		// -Commas are replaced by semi-colons.
        		// -Unused commands are skipped.
        		// -The latest constructs are used.
        		switch (WithSection){
        			case With.Route:
        				// "DeveloperID" is ignored but retain it so that the author is credited.
        				sbRoute.Append("\t." + exp.Text + Environment.NewLine);
	        			break;
        			case With.Train:
						// Replace "File" with "Folder".
	        			if(exp.Text.StartsWith("File")){
	        			   	sbTrain.Append("\t.Folder (" + exp.Text.Split(' ')[1] + ")" + Environment.NewLine);
        				// Reformat "Flange".
	        			}else if(exp.Text.StartsWith("Flange")){
	        				string[] strFlange = exp.Text.Split(' ');
	        				sbTrain.Append("\t." + strFlange[0] + ".Set " + strFlange[1] + Environment.NewLine);
	        				break;
        				// Replace "Rail" with "Run".
	        			}else if(exp.Text.StartsWith("Rail")){
	        				string[] strRun = exp.Text.Split(' ');
	        				sbTrain.Append("\t." + strRun[0].Replace("Rail", "Run") + ".Set " + strRun[1] + Environment.NewLine);
	        				break;
	        			// Dump unused commands.
	        			}else if(exp.Text.StartsWith("Acceleration")){
	        				break;
	        			}else if(exp.Text.StartsWith("Station")){
	        				break;
						}else{
	        				sbTrain.Append("\t." + exp.Text + Environment.NewLine);
						}
	        			break;
        			case With.Structure:
        				sbStructure.Append("\t." + exp.Text + Environment.NewLine);
	        			break;
//        			case With.Texture:
//
//	        			break;
        			case With.Cycle:
	        			// Cycle commands already have the dot prefix.
	        			sbCycle.Append("\t" + exp.Text.Replace(',', ';') + Environment.NewLine);
	        			break;
//        			case With.Signal:
//
//	        			break;
        			case With.Track:
        				if(int.TryParse(exp.Text, out iTrack)){
		        			iTrackCnt = iTrack.ToString().Length;
		        			sbTrack.Append(exp.Text + ",");
		        			strTabs = "";
		        			// Assume tab width = 4.
		        			for (int j = 1;j <= iTrackCnt / 4 + 1;j++ ) {
		        				strTabs += "\t";
		        			}
		        			bNumLine = true;
        				}else{
	        				// Convert -9,9 to L,R in .form
		        			if(exp.Text.StartsWith("form")){
								string[] arrSplit = exp.Text.Split(',');
								arrSplit[1] = arrSplit[1].Replace("-9", "L");
								arrSplit[1] = arrSplit[1].Replace("9", "R");
		        			   	exp.Text = arrSplit[0] + "," + arrSplit[1] + "," + arrSplit[2];
							}

							if(bNumLine){
								int iRes;
								// Allow for the comma.
								Math.DivRem(iTrackCnt + 1, 4, out iRes);
								sbTrack.Append((iRes == 0 ? "." : "\t.") + exp.Text.Replace(',', ';') + Environment.NewLine);
								bNumLine = false;
							}else{
		        				sbTrack.Append(strTabs + "." + exp.Text.Replace(',', ';') + Environment.NewLine);
							}
        				}
	        			break;
	        			
	    		}
			}
        	System.Text.StringBuilder sbCSV = new System.Text.StringBuilder();
        	sbCSV.Append(sbRoute.ToString());
        	sbCSV.Append(sbTrain.ToString());
        	sbCSV.Append(sbStructure.ToString());
//        	sbCSV.Append(sbTexture.ToString());
        	sbCSV.Append(sbCycle.ToString());
//        	sbCSV.Append(sbSignal.ToString());
        	sbCSV.Append(sbTrack.ToString());
        	
        	System.IO.File.WriteAllText(path, sbCSV.ToString());
			
		}
		
		// Insert $Chr() statements
		private string InsertChr(string str)
		{
			string strNew;
			// Do parentheses first to avoid catching Chr()s.
			strNew = str.Replace(")", "@");
			strNew = strNew.Replace("(", "$Chr(40)");
			strNew = strNew.Replace("@", "$Chr(41)");
			strNew = strNew.Replace("\n", "$Chr(13)");
			//strNew = strNew.Replace(" ", "$Chr(20)");
			strNew = strNew.Replace(",", "$Chr(44)");
			strNew = strNew.Replace(";", "$Chr(59)");
			return strNew;
		}

	}
}
