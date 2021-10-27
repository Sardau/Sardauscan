#region COPYRIGHT
/**
 * This file is part of Sardauscan by Fabio Ferretti, licensed under the CC-BY-NC-SA 4.0 Licence.
 * You can find the original code in this GitHub repository: https://github.com/Sardau/Sardauscan
 */
#endregion
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Sardauscan.Core;
using System.Reflection;
using Sardauscan.Core.IO;
using Sardauscan.Gui;
using System.Threading;
using Sardauscan.Gui.Forms;
using Sardauscan.Gui.Controls;

namespace Sardauscan
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);

			try
			{
				SetDefaultSettings();
				Settings.RegisterInstance(new Settings(ConfigFile));
				Application.EnableVisualStyles();
				Application.SetCompatibleTextRenderingDefault(false);
				Application.Run(new MainForm());
				Settings.DisposeAllRegistred();
			}
			catch (Exception e)
			{
				OnFatalError(e);
			}

		}

		public static void SetDefaultSettings()
		{
			if (File.Exists(ConfigFile))
				return;
			using (StreamWriter w = File.CreateText(ConfigFile))
			{
				w.WriteLine(global::Sardauscan.Properties.Resources.DEFAULT_CONFIG);
				w.Close();
			}
		}

		public static string AppPath
		{
			get { return System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location); }
		}
		private static string GetDirectory(string basepath, string subpath)
		{
			string path = System.IO.Path.Combine(basepath, subpath);
			if (!Directory.Exists(path))
				Directory.CreateDirectory(path);
			return path;
		}
		public static string UserDataPath
		{
			get {
#if DEBUG
				return GetDirectory(AppPath, "User");
#else
				return GetDirectory(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Sardauscan");
#endif
			}
		}
		public static string ConfigFile { get { return System.IO.Path.Combine(UserDataPath, "config.xml"); } }
		public static string TaskPath { get { return GetDirectory(UserDataPath, "Task"); } }
		public static string ProcessPath { get { return GetDirectory(UserDataPath, "Process"); } }
		public static string PluginsPath { get { return GetDirectory(AppPath, "Plugins"); } }
		public static string HardwareConfigPath { get { return GetDirectory(UserDataPath, "Hardware"); } }


		public static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
		{
			OnFatalError(e.Exception);
		}

		public static void OnFatalError(Exception e)
		{
			OkCancelDialog dlg = new OkCancelDialog();
			dlg.Text = "Fatal Error, Restart ?";
			CrashReport view = new CrashReport();
			view.SetException(e);
			if (dlg.ShowDialog(view, null) == System.Windows.Forms.DialogResult.OK)
          Application.Restart();
      else
          System.Environment.Exit(0);
		}
	}
}
