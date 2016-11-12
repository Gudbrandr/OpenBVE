using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Windows.Forms;
using System.Xml;

namespace OpenBve
{
	internal static partial class PluginManager
	{
		internal struct PluginEntry
		{
			/// <summary>The file length of the blacklisted plugin</summary>
			internal double FileLength;
			/// <summary>The file name of the blacklisted plugin</summary>
			internal string FileName;
			/// <summary>The MD5 sum of the plugin file</summary>
			internal string MD5;
			/// <summary>The train-name if this plugin is blacklisted for one train only</summary>
			internal string Train;
			/// <summary>A textual string describing the reason this plugin is blacklisted</summary>
			internal string Reason;

		}

		/// <summary>The currently blacklisted plugins</summary>
		internal static List<PluginEntry> BlackListedPlugins;

		/// <summary>Checks whether the specified plugin is blacklisted</summary>
		/// <returns>True if blacklisted, false otherwise</returns>
		internal static bool CheckBlackList(string filePath, string trainFolder)
		{
			if (BlackListedPlugins.Count == 0)
			{
				return false;
			}
			var n = System.IO.Path.GetFileName(filePath);
			for (int i = 0; i < BlackListedPlugins.Count; i++)
			{
				if (BlackListedPlugins[i].FileName == n)
				{
					var fi = new FileInfo(filePath);
					if (fi.Length == BlackListedPlugins[i].FileLength && (BlackListedPlugins[i].Train == null || trainFolder.ToLowerInvariant() == BlackListedPlugins[i].Train.ToLowerInvariant()))
					{
						var md5 = MD5.Create();
						using (var stream = File.OpenRead(filePath))
						{
							md5.ComputeHash(stream);
						}
						string s = Convert.ToBase64String(md5.Hash);
						if (s.ToLowerInvariant() == BlackListedPlugins[i].MD5.ToLowerInvariant())
						{
							string pluginTitle = System.IO.Path.GetFileName(filePath);
							Interface.AddMessage(Interface.MessageType.Error, true, "The train plugin " + pluginTitle + " is blacklisted for the following reason:");
							if (BlackListedPlugins[i].Reason == String.Empty)
							{
								Interface.AddMessage(Interface.MessageType.Error, true, "No reason specified.");
							}
							else
							{
								Interface.AddMessage(Interface.MessageType.Error, true, BlackListedPlugins[i].Reason);
							}
							return true;
						}
					}
				}
			}
			return false;
		}

		/// <summary>Loads the database of blacklisted plugins from disk</summary>
		/// <param name="databasePath">The database path</param>
		internal static void LoadBlackListDatabase(string databasePath)
		{
			if (!System.IO.File.Exists(databasePath))
			{
				return;
			}
			BlackListedPlugins = new List<PluginEntry>();
			XmlDocument currentXML = new XmlDocument();
			//Load the object's XML file 
			currentXML.Load(databasePath);
			if (currentXML.DocumentElement != null)
			{
				XmlNodeList DocumentNodes = currentXML.DocumentElement.SelectNodes("/openBVE/TrainPlugins/Blacklist");
				//Check this file actually contains an openBVE plugin blacklist
				if (DocumentNodes != null)
				{
					foreach (XmlNode n in DocumentNodes)
					{
						if (n.HasChildNodes)
						{
							PluginEntry p = new PluginEntry();
							bool ch = false;
							foreach (XmlNode c in n.ChildNodes)
							{
								switch (c.Name.ToLowerInvariant())
								{
									case "filelength":
										Double.TryParse(c.InnerText, out p.FileLength);
										ch = true;
										break;
									case "filename":
										p.FileName = c.InnerText;
										ch = true;
										break;
									case "md5":
										p.MD5 = c.InnerText;
										ch = true;
										break;
									case "train":
										p.Train = c.InnerText;
										ch = true;
										break;
									case "reason":
										p.Reason = c.InnerText;
										ch = true;
										break;
								}
							}
							if (ch && p.MD5 != string.Empty)
							{
								BlackListedPlugins.Add(p);
							}
						}
					}
				}
			}
		}
	}
}
