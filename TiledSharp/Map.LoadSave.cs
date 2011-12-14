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
using System.IO;
using System.Xml;

namespace TheWarrentTeam.TiledSharp
{
	public partial class Map
	{
		public static Map Load (string fileurl)
		{
			using(XmlReader XmlRead = Helpers.CreateCustomXmlReader (fileurl)) {
				
				string base_path = Path.GetDirectoryName (fileurl);
				bool FoundMap = false;
				while (!FoundMap && XmlRead.ReadToFollowing ("map")) {
					if (XmlRead.NodeType == XmlNodeType.Element && XmlRead.HasAttributes && !XmlRead.IsEmptyElement)
						FoundMap = true;
				}
				
				if (!FoundMap)
					throw new ArgumentException (string.Format ("Map Element can't be found in {0}", fileurl));
				
				return Load (XmlRead, base_path);
			}
		}

		public static Map Load (XmlReader MapReader, string base_path)
		{
			if (MapReader.NodeType != XmlNodeType.Element || !MapReader.HasAttributes ||
			    MapReader.IsEmptyElement || MapReader.Name != "map")
				throw new ArgumentException ("Element is not of type map.");
			
			Map LoadingMap = new Map ();
			
			string orientation = string.Empty;
			while (MapReader.MoveToNextAttribute ()) {
				switch (MapReader.Name) {
				
				case "version":
					LoadingMap.Version = MapReader.Value;
					break;
				
				case "orientation":
					orientation = MapReader.Value;
					break;
				
				case "width":
					LoadingMap.Size.Width = int.Parse (MapReader.Value);
					break;
				
				case "height":
					LoadingMap.Size.Height = int.Parse (MapReader.Value);
					break;
				
				case "tilewidth":
					LoadingMap.TileSize.Width = int.Parse (MapReader.Value);
					break;
				
				case "tileheight":
					LoadingMap.TileSize.Height = int.Parse (MapReader.Value);
					break;
					
				}
			}
			
			switch (orientation) {
			case "isometric":
				LoadingMap.Orientation = Orientation.Isometric;
				break;
			
			case "orthogonal":
				LoadingMap.Orientation = Orientation.Orthogonal;
				break;
			
			case "":
				throw new ArgumentException ("Map dosen't have a orientation attribute.");
			default:
				throw new NotImplementedException (string.Format ("Map orientation: {0} isn't implemented.", orientation));
			}
			
			MapReader.MoveToElement ();
			using (XmlReader MapChildReader = MapReader.ReadSubtree ()) {
				bool ReadingChilds = true;
				while (ReadingChilds) {
					
					if (MapChildReader.NodeType != XmlNodeType.Element) {
						ReadingChilds = MapChildReader.Read ();
						continue;
					}
					
					switch (MapChildReader.Name) {
					
					case "properties":
						Helpers.ReadProperties (MapChildReader, LoadingMap.Properties);
						break;
					
						//TODO ensure that the TileSet are added in Gid order to the collection.
					case "tileset":
						TileSet tset;
						int firstGid = int.Parse (MapChildReader.GetAttribute ("firstgid"));
						
						// If it is a tsx tile set, Load the file instead.
						string Source = MapChildReader.GetAttribute ("source");
						if (Source != null) {
							tset = TileSet.Load (Path.Combine (base_path, Source));
							MapChildReader.Read();
						} else {
							tset = TileSet.Load (MapChildReader, base_path);
						}
						LoadingMap.TileSets.Add(tset);
						break;
					
					case "layer":
						LoadingMap.Layers.Add (TileLayer.Load (MapChildReader));
						break;
					
					case "objectgroup":
						LoadingMap.Layers.Add (ObjectGroup.Load (MapChildReader));
						break;
					
					default:
						ReadingChilds = MapChildReader.Read ();
						break;
					}
					
				}
			}
			return LoadingMap;
		}
		
	}
}
