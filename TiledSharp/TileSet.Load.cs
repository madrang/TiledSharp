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

