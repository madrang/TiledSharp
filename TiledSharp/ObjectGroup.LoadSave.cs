using System;
using System.Collections.ObjectModel;
using System.Xml;

namespace TiledSharp
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
