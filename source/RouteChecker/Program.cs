/*
 * Created by SharpDevelop.
 * User: Gudbrandr
 * Date: 15/05/2009
 * Time: 3:14 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Windows.Forms;

namespace OpenBve
{
	/// <summary>
	/// Class with program entry point.
	/// </summary>
	internal sealed class Program
	{
		// system
		internal static FileSystem FileSystem = null;
		
		internal static double MinimumJumpToPositionValue =  0;
		internal static bool CurrentlyLoading = false;
		internal static int CurrentStation = -1;
		internal static bool JumpToPositionEnabled = false;
		internal static string JumpToPositionValue = "";
		
		
		/// <summary>
		/// Program entry point.
		/// </summary>
		[STAThread]
		private static void Main(string[] args)
		{
			Interface.CurrentOptions.UseSound = true;
			Interface.CurrentOptions.ObjectOptimizationBasicThreshold = 1000;
			Interface.CurrentOptions.ObjectOptimizationFullThreshold = 250;
			// file system
			FileSystem = FileSystem.FromCommandLineArgs(args);
			FileSystem.CreateFileSystem();
			SetPackageLookupDirectories();
			//Unload
			TextureManager.UnuseAllTextures();
//			SoundManager.Deinitialize();
			
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MainForm());
		}
		
		/// <summary>The object that serves as an authentication for the SetPackageLookupDirectories call.</summary>
		private static object SetPackageLookupDirectoriesAuthentication = null;

		/// <summary>Provides the API with lookup directories for all installed packages.</summary>
		internal static void SetPackageLookupDirectories() {
			int size = 16;
			string[] names = new string[size];
			string[] directories = new string[size];
			int count = 0;
			foreach (string lookupDirectory in FileSystem.ManagedContentFolders) {
				string[] packageDirectories = System.IO.Directory.GetDirectories(lookupDirectory);
				foreach (string packageDirectory in packageDirectories) {
					string package = System.IO.Path.GetFileName(packageDirectory);
					if (count == size) {
						size <<= 1;
						Array.Resize<string>(ref names, size);
						Array.Resize<string>(ref directories, size);
					}
					names[count] = package;
					directories[count] = packageDirectory;
					count++;
				}
			}
			Array.Resize<string>(ref names, count);
			Array.Resize<string>(ref directories, count);
			SetPackageLookupDirectoriesAuthentication = OpenBveApi.Path.SetPackageLookupDirectories(names, directories, SetPackageLookupDirectoriesAuthentication);
		}
	}
}
