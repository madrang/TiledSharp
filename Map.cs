﻿//Author:
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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;

namespace Linsft.TiledSharp
{
	public partial class Map
	{
		public Map ()
		{
			this.Version = "1.0";
			this.Orientation = Orientation.Orthogonal;
			this.Size.Width = 32;
			this.Size.Height = 32;
			this.TileSize.Width = 32;
			this.TileSize.Height = 32;
			
			this.TileSets = new Collection<TileSet>();
			this.Layers = new Collection<iLayer> ();
			this.Properties = new Dictionary<string, string>();
		}
		
		#region Properties
		
		/// <summary>
		/// The TMX format version, generally 1.0.
		/// </summary>
		public string Version { get; set; }
		
		/// <summary>
		/// Map orientation.
		/// Tiled supports "orthogonal" and "isometric" at the moment.
		/// </summary>
		public Orientation Orientation { get; set; }

		/// <summary>
		/// The map size in tiles.
		/// </summary>
		public Size Size;
		
		/// <summary>
		/// The size of a tile. 
		/// </summary>
		public Size TileSize;

		public Dictionary<string, string> Properties { get; private set; }
		public Collection<iLayer> Layers { get; private set; }
		public Collection<TileSet> TileSets { get; private set; }
		
		#endregion
		
		public void GetTileSetIndex(int Gid, out int SetCount, out int TileId)
		{
			TileId = Gid;
			SetCount = 0;
			foreach (TileSet tSetItem in this.TileSets) {
				if(TileId - tSetItem.Count <= 0) {
					return;
				}
				TileId -= tSetItem.Count;
				SetCount++;
			}
			
			throw new ArgumentOutOfRangeException("Gid");
		}

		/// <summary>
		/// Draws a preview of a map.
		/// </summary>
		public Bitmap Draw ()
		{
			Bitmap canvas = new Bitmap (this.TileSize.Width * this.Size.Width,
			                            this.TileSize.Height * this.Size.Height);
			Collection<Bitmap> TilesBitmaps = new Collection<Bitmap>();
			
			foreach (TileSet SetItem in this.TileSets) {
				SetItem.LoadBitmaps(TilesBitmaps);
			}
			
			using (Graphics g = Graphics.FromImage (canvas)) {
				foreach (TileLayer LayerItem in this.Layers) {
					if(!LayerItem.Visible)
						continue;
					
					int MaxWidth = Math.Min(LayerItem.Coordinate.Right, this.Size.Width);
					int MaxHeight = Math.Min(LayerItem.Coordinate.Bottom, this.Size.Height);
					
					for (int y = LayerItem.Coordinate.Y; y < MaxHeight; y++) {
						for (int x = LayerItem.Coordinate.X; x < MaxWidth; x++) {
							int tile_index = LayerItem.Data[x, y];
							if (tile_index < 1)
								continue;
							
							Bitmap TileBmp = TilesBitmaps[tile_index - 1];
							int realX = ((x + 1) * this.TileSize.Width) - TileBmp.Width;
							int realY = ((y + 1) * this.TileSize.Height) - TileBmp.Height;
							
							g.DrawImage (TileBmp, realX, realY);
						}
					}
				}
			}
			return canvas;
		}
		
	}
	
}
