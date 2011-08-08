using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;

namespace TiledSharp
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
