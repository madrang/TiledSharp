using System;
using System.Collections.Generic;
using System.Xml;

namespace TiledSharp
{
	internal static class Helpers
	{
		public static void ReadProperties(XmlReader XmlRead, Dictionary<string, string> Props)
		{
			if(XmlRead.NodeType != XmlNodeType.Element || XmlRead.IsEmptyElement ||
			   XmlRead.Name != "properties")
				throw new ArgumentException("Element is not of type properties.");
			
			using (XmlReader PropReader = XmlRead.ReadSubtree ()) {
				while(PropReader.Read()) {
					
					if(PropReader.NodeType != XmlNodeType.Element ||
					   !PropReader.HasAttributes || PropReader.Name != "property")
						continue;
					
					Props.Add (PropReader.GetAttribute("name"), PropReader.GetAttribute("value"));
				}
			}
		}
		
		private static XmlReaderSettings CustomSettings {
			get {
				XmlReaderSettings ReadSettings = new XmlReaderSettings();
				
				//Allow DTD
				ReadSettings.ProhibitDtd = false;
				
				//Use a local copy of DTD to load faster.
				//Resolve some problems when offline.
				ReadSettings.XmlResolver = new LocalDTDResolver();
				
				return ReadSettings;
			}
		}
		public static XmlReader CreateCustomXmlReader(string fileurl)
		{
			return XmlReader.Create(fileurl, CustomSettings);
		}
		public static XmlReader CreateCustomXmlReader(System.IO.Stream dataStream)
		{
			return XmlReader.Create(dataStream, CustomSettings);
		}
		
	}
}

