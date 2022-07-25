using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Win32;
using System.Diagnostics;
using System.Security.Principal;
namespace EdgeUninstaller
{
	internal class Program
	{
		static void Main(string[] args)
		{
			bool isElevated;
			using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
			{
				WindowsPrincipal principal = new WindowsPrincipal(identity);
				isElevated = principal.IsInRole(WindowsBuiltInRole.Administrator);
			}
			if (isElevated == false)
			{
				Console.Clear();
				Console.WriteLine("Run as administrator!");
				Console.WriteLine("Press enter to exit");
				Console.ReadLine();
				Environment.Exit(1);
			}
			Console.Clear();
			Console.WriteLine("Removing restrictive registry key");
			Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall\Microsoft Edge", "NoRemove", 0);
			Console.WriteLine("Removed registry key");
			Console.WriteLine("Locating installer executable");
			RegistryKey key = Registry.LocalMachine.OpenSubKey("SOFTWARE\\WOW6432Node\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\Microsoft Edge");
			//get the registry value of HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall\Microsoft Edge\\DisplayVersion");
			
			string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "Microsoft\\Edge\\Application\\" + key.GetValue("DisplayVersion") + "\\Installer");
			Console.WriteLine("Found installer! At: " + path);
			Console.WriteLine("Running installer in uninstaller mode");
			var process = Process.Start(path + "\\setup.exe" ,"--force-uninstall --uninstall --system-level");
			process.WaitForExit();
			Console.WriteLine("Edge uninstalled!");
			Console.WriteLine("Press enter to exit");
			Console.ReadLine();
			Environment.Exit(0);
		}
	}
}
