//Author:
//      Marc-Andre Ferland <madrang@gmail.com>
//
//Copyright (c) 2011 Linsft
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
using System.Collections.ObjectModel;
using System.Xml;

namespace Linsft.TiledSharp
{
    public partial class ObjectGroup : Collection<MapObject>
	{
		
		public static ObjectGroup Load (XmlReader ObjectGroupReader)
		{
			using (XmlReader ObjectGroupChildReader = ObjectGroupReader.ReadSubtree ()) {
				ObjectGroupChildReader.Read();
				ObjectGroupChildReader.Read();
			}
			ObjectGroupReader.Read();
			
			return new ObjectGroup ();
			
			/*
			ObjectGroup objgrp = new ObjectGroup ();
			objgrp.Name = node.ReadTag ("name");
			objgrp.X = node.ReadInt ("x");
			objgrp.Y = node.ReadInt ("y");
			objgrp.Width = node.ReadInt ("width");
			objgrp.Height = node.ReadInt ("height");
			
			if (node.HasChildNodes) {
				//ColorConverter cc = new ColorConverter ();
				foreach (XmlNode NodeItem in node.ChildNodes) {
					MapObject mapobj = new MapObject ();
					mapobj.Name = NodeItem.ReadTag ("name");
					mapobj.Type = NodeItem.ReadTag ("type");
					mapobj.X = NodeItem.ReadInt ("x");
					mapobj.Y = NodeItem.ReadInt ("y");
					mapobj.Width = NodeItem.ReadInt ("width");
					mapobj.Height = NodeItem.ReadInt ("height");
					mapobj.Gid = NodeItem.ReadInt ("gid", -1);
					
					if (NodeItem.HasChildNodes) {
						foreach (XmlNode ChildNodeItem in NodeItem.ChildNodes) {
							switch (ChildNodeItem.Name) {
							case "properties":
								//Helpers.ReadProperties(ChildNodeItem, mapobj.Properties);
								break;
								
							case "image":
								mapobj.Images.Add(new ImageInfo(ChildNodeItem));
								break;
							}
						}
					}
					objgrp.Add (mapobj);
				}
			}
			return objgrp;
			*/
		}
		
	}
}
