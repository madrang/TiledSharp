using System;
using System.Reflection;
using System.IO;
using System.Xml;

namespace TiledSharp
{
	internal class LocalDTDResolver : XmlUrlResolver
	{
		private static string AssemblyDirectory
		{
			get {
				string codeBase = Assembly.GetExecutingAssembly().CodeBase;
				UriBuilder uri = new UriBuilder(codeBase);
				string path = Uri.UnescapeDataString(uri.Path);
				return Path.GetDirectoryName(path);
			}
		}
		
		public override Uri ResolveUri(Uri baseUri, string relativeUri)
		{
			if (relativeUri == @"http://mapeditor.org/dtd/1.0/map.dtd") {
				string LocalDTD = Path.Combine("file://" + AssemblyDirectory + "/DTD", "Map1.0.dtd");
				return base.ResolveUri(baseUri, LocalDTD);
			} else return base.ResolveUri(baseUri, relativeUri);
		}
		
	}
}
