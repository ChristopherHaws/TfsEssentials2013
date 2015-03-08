using System;
using System.ComponentModel.Composition;
using System.Diagnostics;
using Microsoft.TeamFoundation.VersionControl.Client;
using Microsoft.VisualStudio.Shell;
using Microsoft.Win32;
using Spiral.TfsEssentials.Extentions;

namespace Spiral.TfsEssentials.Providers
{
	[Export]
	internal class PackageConfigurationManager
	{
		private readonly Package package;
		private const string RootKey = "TfsEssentials2013";

		[ImportingConstructor]
		public PackageConfigurationManager([Import(typeof(SVsServiceProvider))] IServiceProvider serviceProvider)
		{
			package = serviceProvider.GetService<Package>();
		}

		public string GetValue(string key)
		{
			return GetValue(RootKey, key);
		}

		public string GetValue(TeamProject teamProject, string key)
		{
			var teamProjectKey = String.Format(@"{0}\Servers\{1}\Collections\{2}\{3}",
				RootKey,
				teamProject.TeamProjectCollection.DisplayName,
				teamProject.TeamProjectCollection.CatalogNode.Resource.DisplayName,
				teamProject.Name
			);

			return GetValue(teamProjectKey, key);
		}

		private string GetValue(string location, string key)
		{
			using (var registryKey = package.UserRegistryRoot.OpenSubKey(location, false))
			{
				if (registryKey == null)
				{
					Debug.Write("Could not open registry key.");
					return null;
				}

				return registryKey.GetValue(key) as string;
			}
		}

		public void SetValue(string key, string value)
		{
			SetValue(RootKey, key, value);
		}

		public void SetValue(TeamProject teamProject, string key, string value)
		{
			var teamProjectKey = String.Format(@"{0}\Servers\{1}\Collections\{2}\{3}",
				RootKey,
				teamProject.TeamProjectCollection.DisplayName,
				teamProject.TeamProjectCollection.CatalogNode.Resource.DisplayName,
				teamProject.Name
			);

			SetValue(teamProjectKey, key, value);
		}

		private void SetValue(string location, string key, string value)
		{
			EnsureKeyExists(location);

			using (var registryKey = package.UserRegistryRoot.OpenSubKey(location, true))
			{
				if (registryKey == null)
				{
					Debug.Write("Could not open registry key.");
					return;
				}

				if (value == null)
				{
					registryKey.DeleteValue(key, false);
				}
				else
				{
					registryKey.SetValue(key, value, RegistryValueKind.String);
				}
			}
		}

		private void EnsureKeyExists(string key)
		{
			using (var registryKey = package.UserRegistryRoot.OpenSubKey(key))
			{
				if (registryKey == null)
				{
					package.UserRegistryRoot.CreateSubKey(key);
				}
			}
		}
	}
}