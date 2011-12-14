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

namespace TheWarrentTeam.TiledSharp
{
    /// <summary>
    /// A tileset that provides methods for reading tiles associated with this map.
    /// </summary>
    public partial class TileSet
    {
        public string Name { get; set; }
		public ImageInfo ImageInfomation { get; set; }
        
		/// <summary>
		/// The size of a tile. 
		/// </summary>
		public Size TileSize;
		
        public int Spacing { get; set; }
		public int Margin { get; set; }
		
		public Dictionary<int, Dictionary<string, string>> TilesProperties { get; private set; }
		
		public TileSet()
		{
			this.ImageInfomation = new ImageInfo();
		}
		internal TileSet(ImageInfo ImgInf)
		{
			this.ImageInfomation = ImgInf;
		}
		
		#region LoadBitmap
		
		/// <summary>
		/// Loads the tiles bitmap.
		/// </summary>
		public Bitmap LoadBitmap ()
		{
			Bitmap SourceImage = new Bitmap(this.ImageInfomation.Source);
			if (this.ImageInfomation.UseTransColor)
				SourceImage.MakeTransparent(this.ImageInfomation.TransColor);
			return SourceImage;
		}
		
		/// <summary>
		/// Load and cuts the image into tiles.
		/// </summary>
		public Collection<Bitmap> LoadBitmaps()
		{
			Collection<Bitmap> Tiles = new Collection<Bitmap>();
			this.LoadBitmaps(Tiles);
			return Tiles;
		}
		
		/// <summary>
		/// Load and cuts the image into tiles.
		/// </summary>
		public void LoadBitmaps(Collection<Bitmap> Tiles)
		{
			if (Tiles == null)
				throw new ArgumentNullException("Tiles");
			
			using(Bitmap SourceImage = this.LoadBitmap())
				this.LoadBitmaps(SourceImage, Tiles);
		}
		
		/// <summary>
        /// Cuts the image into tiles.
		/// </summary>
		public Collection<Bitmap> LoadBitmaps(Bitmap SourceImage)
        {
			Collection<Bitmap> Tiles = new Collection<Bitmap>();
			this.LoadBitmaps(SourceImage, Tiles);
			return Tiles;
		}

        /// <summary>
        /// Cuts the image into tiles.
		/// </summary>
        public void LoadBitmaps(Bitmap SourceImage, Collection<Bitmap> Tiles)
        {
			if(Tiles == null)
				throw new ArgumentNullException("Tiles");
			
			if(SourceImage == null)
				throw new ArgumentNullException("SourceImage");
			
			Rectangle SourceCrop = new Rectangle(this.Margin, this.Margin,
			                                     SourceImage.Width - this.Margin * 2,
			                                     SourceImage.Height - this.Margin * 2);
			Rectangle DestCrop = new Rectangle(0, 0, SourceCrop.Width, SourceCrop.Height);
			
			using(Bitmap CropImage = new Bitmap(SourceCrop.Width, SourceCrop.Height)) {
				
				/* Crop the image margin. */
				using (Graphics g = Graphics.FromImage(CropImage))
					g.DrawImage(SourceImage, DestCrop, SourceCrop, GraphicsUnit.Pixel);
				
				/* Load all the tiles */
				// Size of the destination tile.
				Rectangle DestRect = new Rectangle(0, 0, this.TileSize.Width, this.TileSize.Height);
				// Scan the image line by line to load all the tiles.
				Rectangle ScanRect = new Rectangle(0, 0, this.TileSize.Width, this.TileSize.Height);
				while(ScanRect.Y + ScanRect.Height <= CropImage.Height) {
					Bitmap newTile = new Bitmap(this.TileSize.Width, this.TileSize.Height);
					
					using (Graphics g = Graphics.FromImage(newTile))
						g.DrawImage(CropImage, DestRect, ScanRect, GraphicsUnit.Pixel);
					
					Tiles.Add(newTile);
					
					ScanRect.X += this.TileSize.Width + this.Spacing;
					
					// If scan is outside the bitmap width.
					// Start on the next line.
					if (ScanRect.X + ScanRect.Width > CropImage.Width) {
						ScanRect.X = 0;
						ScanRect.Y += this.TileSize.Height + this.Spacing;
					}
				}
			}
		}
		
		#endregion
		
		public Point GetTilePos(int Id)
		{
			if(Id <= 0 || Id > this.Count)
				throw new ArgumentOutOfRangeException("Id");
			
			//Id is 1 based. For math to work need 0 based.
			Id--;
			int Line = (Id / this.Rows);
			int y = this.Margin + (Line * this.TileSize.Height) + (Line * this.Spacing);
			int Row = Id - (Line * this.Rows);
			int x = this.Margin + (Row * this.TileSize.Width) + (Row * this.Spacing);
			
			return new System.Drawing.Point(x, y);
		}
		
		public int Count {
			get { return this.Rows * this.Columns; }
		}
		
		public int Rows {
			get {
				int x = this.ImageInfomation.Size.Width - (this.Margin * 2);
				
				if(x < this.TileSize.Width)
					return 0;
				
				int SpaceLeft = x % (this.TileSize.Width + this.Spacing);
				x /= (this.TileSize.Width + this.Spacing);
				x += (SpaceLeft >= this.TileSize.Width) ? 1 : 0;
				return x;
			}
		}
		
		public int Columns {
			get {
				int y = this.ImageInfomation.Size.Height - (this.Margin * 2);
				
				if(y < this.TileSize.Height)
					return 0;
				
				int SpaceLeft = y % (this.TileSize.Height + this.Spacing);
				y /= (this.TileSize.Height + this.Spacing);
				y += (SpaceLeft >= this.TileSize.Height) ? 1 : 0;
				return y;
			}
		}
		
	}
}
