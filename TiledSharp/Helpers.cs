//Author:
//      Marc-Andre Ferland <madrang@gmail.com>
//
//Copyright (c) 2011 TheWarrentTeam
//
//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:
//
//The above copyright notice and this permission notice shall be included in
//all copies or substantial portions of the Software.
//
//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Xml;

namespace TheWarrentTeam.TiledSharp
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

