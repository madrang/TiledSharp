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
using TheWarrentTeam.TiledSharp;
using NUnit.Framework;

namespace TiledSharpNUnit
{
	[TestFixture()]
	public class TileSetNUnit
	{
		[Test()]
		public void TileCount ()
		{
			TileSet TestSet = new TileSet();
			
			for (int SizeWidth = 1; SizeWidth <= 32; SizeWidth++) {
				TestSet.ImageInfomation.Size.Width = SizeWidth;
				for (int SizeHeight = 1; SizeHeight <= 32; SizeHeight++) {
					TestSet.ImageInfomation.Size.Height = SizeHeight;
					for (int TileSizeWidth = 1; TileSizeWidth <= 16; TileSizeWidth++) {
						TestSet.TileSize.Width = TileSizeWidth;
						for (int TileSizeHeight = 1; TileSizeHeight <= 16; TileSizeHeight++) {
							TestSet.TileSize.Height = TileSizeHeight;
							for (int Spacing = 0; Spacing <= 8; Spacing++) {
								TestSet.Spacing = Spacing;
								for (int Margin = 0; Margin <= 8; Margin++) {
									TestSet.Margin = Margin;
									AssertCount(TestSet);
								}
							}
						}
					}
				}
			}
		}
		
		public void AssertCount(TileSet TestSet)
		{
			Assert.AreEqual(IterCalcTileCount(TestSet), TestSet.Count,
			                "Source [Width {0}, Heigth {1}], Tile [Width {2}, Heigth {3}], Spacing {4}, Margin {5}",
			                TestSet.ImageInfomation.Size.Width,
			                TestSet.ImageInfomation.Size.Height,
			                TestSet.TileSize.Width,
			                TestSet.TileSize.Height,
			                TestSet.Spacing,
			                TestSet.Margin);
		}
		
		public int IterCalcTileCount(TileSet tset)
		{
			System.Drawing.Size CropImage = new System.Drawing.Size(tset.ImageInfomation.Size.Width - (tset.Margin * 2),
			                                                        tset.ImageInfomation.Size.Height - (tset.Margin * 2));
			System.Drawing.Rectangle ScanRect = new System.Drawing.Rectangle(0, 0,
			                                                                 tset.TileSize.Width, tset.TileSize.Height);
			
			if(ScanRect.Width > CropImage.Width || ScanRect.Height > CropImage.Height)
				return 0;
			
			int Count = 0;
			while(ScanRect.Y + ScanRect.Height <= CropImage.Height) {
				ScanRect.X += tset.TileSize.Width + tset.Spacing;
				Count++;
				
				// If scan is outside the bitmap width.
				// Start on the next line.
				if (ScanRect.X + ScanRect.Width > CropImage.Width) {
					ScanRect.X = 0;
					ScanRect.Y += tset.TileSize.Height + tset.Spacing;
				}
			}
			return Count;
		}
		
		[Test()]
		public void TilePosition ()
		{
			TileSet TestSet = new TileSet();
			

			for (int TileSizeWidth = 1; TileSizeWidth <= 16; TileSizeWidth++) {
				TestSet.TileSize.Width = TileSizeWidth;
				for (int TileSizeHeight = 1; TileSizeHeight <= 16; TileSizeHeight++) {
					TestSet.TileSize.Height = TileSizeHeight;
					for (int SizeWidth = TileSizeWidth; SizeWidth <= 32; SizeWidth++) {
						TestSet.ImageInfomation.Size.Width = SizeWidth;
						for (int SizeHeight = TileSizeHeight; SizeHeight <= 32; SizeHeight++) {
							TestSet.ImageInfomation.Size.Height = SizeHeight;
							for (int Spacing = 0; Spacing <= 8; Spacing++) {
								TestSet.Spacing = Spacing;
								for (int Margin = 0; Margin <= 8; Margin++) {
									TestSet.Margin = Margin;
									AssertPosition(TestSet);
								}
							}
						}
					}
				}
			}
		}
		
		public void AssertPosition(TileSet TestSet)
		{
			for (int Id = 1; Id < TestSet.Count; Id++) {
				int IterX, IterY;
				System.Drawing.Point TestPos = TestSet.GetTilePos(Id);
				IterCalcTilePos(TestSet, Id, out IterX, out IterY);
				
				Assert.AreEqual(IterX, TestPos.X,
				                "Error X, Id {6}, Source [Width {0}, Heigth {1}], Tile [Width {2}, Heigth {3}], Spacing {4}, Margin {5}",
				                TestSet.ImageInfomation.Size.Width,
				                TestSet.ImageInfomation.Size.Height,
				                TestSet.TileSize.Width,
				                TestSet.TileSize.Height,
				                TestSet.Spacing,
				                TestSet.Margin,
				                Id);
				
				Assert.AreEqual(IterY, TestPos.Y,
				                "Error Y, Id {6}, Source [Width {0}, Heigth {1}], Tile [Width {2}, Heigth {3}], Spacing {4}, Margin {5}",
				                TestSet.ImageInfomation.Size.Width,
				                TestSet.ImageInfomation.Size.Height,
				                TestSet.TileSize.Width,
				                TestSet.TileSize.Height,
				                TestSet.Spacing,
				                TestSet.Margin,
				                Id);
			}
		}
		
		public void IterCalcTilePos(TileSet tset, int Id, out int X, out int Y)
		{
			System.Drawing.Rectangle ScanRect = new System.Drawing.Rectangle(tset.Margin, tset.Margin,
			                                                                 tset.TileSize.Width, tset.TileSize.Height);
			if(Id <= 0)
				throw new Exception();
			
			X = ScanRect.X;
			Y = ScanRect.Y;
			
			int Count = 0;
			while(ScanRect.Y + ScanRect.Height <= tset.ImageInfomation.Size.Height - tset.Margin) {
				
				Count++;
				if(Count == Id)
					return;
				
				ScanRect.X += tset.TileSize.Width + tset.Spacing;
				X = ScanRect.X;
				
				// If scan is outside the bitmap width.
				// Start on the next line.
				if (ScanRect.X + ScanRect.Width > tset.ImageInfomation.Size.Width - tset.Margin) {
					ScanRect.X = tset.Margin;
					X = ScanRect.X;
					ScanRect.Y += tset.TileSize.Height + tset.Spacing;
					Y = ScanRect.Y;
				}
			}
		}
		
		
		
	}
}

