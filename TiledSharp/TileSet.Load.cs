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
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Xml;

namespace TiledSharp
{
    public partial class TileSet
    {

		public static TileSet Load (XmlReader TileReader)
		{
			return Load(TileReader, "");
		}
		
		public static TileSet Load (XmlReader TileReader, string base_path)
		{
			TileSet LoadingTileSet = new TileSet(null);
			
			if(TileReader.NodeType != XmlNodeType.Element || !TileReader.HasAttributes ||
			   TileReader.IsEmptyElement || TileReader.Name != "tileset")
				throw new ArgumentException("Element is not of type tileset.");
			
			int TmpIntParse = 0;
			while(TileReader.MoveToNextAttribute()) {
				switch (TileReader.Name) {
					
				case "tilewidth":
					LoadingTileSet.TileSize.Width = int.Parse(TileReader.Value);
					break;
					
				case "tileheight":
					LoadingTileSet.TileSize.Height = int.Parse(TileReader.Value);
					break;
					
					/* Optional attributes */
				case "spacing":
					LoadingTileSet.Spacing = int.TryParse(TileReader.Value, out TmpIntParse) ? TmpIntParse : 0;
					break;
					
				case "margin":
					LoadingTileSet.Margin = int.TryParse(TileReader.Value, out TmpIntParse) ? TmpIntParse : 0;
					break;
					
				case "name":
					LoadingTileSet.Name = TileReader.Value;
					break;
					
				}
			}
			
			TileReader.MoveToElement();
			using (XmlReader TileChildReader = TileReader.ReadSubtree()) {
				bool ReadingChilds = true;
				while(ReadingChilds) {
					
					if(TileChildReader.NodeType != XmlNodeType.Element) {
						ReadingChilds = TileChildReader.Read ();
						continue;
					}
					
					switch (TileChildReader.Name) {
					
					case "image":
						if(LoadingTileSet.ImageInfomation != null)
							throw new ArgumentException("Element tileset has two image element and can only have one.");
						
						LoadingTileSet.ImageInfomation = ImageInfo.Load(TileChildReader);
						break;
						
						//TODO load tiles properties.
						
					default:
						ReadingChilds = TileChildReader.Read ();
						break;
					}
				}
			}
			
			if (LoadingTileSet.ImageInfomation == null)
				throw new Exception("Element tileset does not contain an image element.");
			
			if(base_path != null || base_path != "")
				LoadingTileSet.ImageInfomation.Source = Path.Combine(base_path, LoadingTileSet.ImageInfomation.Source);
			
			return LoadingTileSet;
		}
		
		public static TileSet Load(string fileurl)
		{
			using(XmlReader XmlRead = Helpers.CreateCustomXmlReader(fileurl)) {
				bool FoundTileSet = false;
				while (!FoundTileSet && XmlRead.ReadToFollowing ("tileset")) {
					if (XmlRead.NodeType == XmlNodeType.Element && XmlRead.HasAttributes &&
					    !XmlRead.IsEmptyElement)
						FoundTileSet = true;
				}
				
				if (!FoundTileSet)
					throw new ArgumentException (string.Format ("TileSet Element can't be found in {0}", fileurl));
				
				return TileSet.Load(XmlRead, Path.GetDirectoryName(fileurl));
			}
		}
		
	}
}

