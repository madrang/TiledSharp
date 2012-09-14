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
using System.Collections.Generic;
using System.Drawing;

namespace Linsft.TiledSharp
{
    public partial class TileLayer : iLayer
    {
		private Rectangle Rect_Coordinate;
		
		public TileLayer(int width, int height) : this()
        {
			this.Rect_Coordinate.Width = width;
			this.Rect_Coordinate.Height = height;
			this.Data = new int[width, height];
		}
		
		private TileLayer()
		{
			this.Data = new int[0,0];
			this.Properties = new Dictionary<string, string>();
			this.Visible = true;
			this.Opacity = 1.0d;
			this.Rect_Coordinate = new Rectangle(0, 0, 0, 0);
			this.Name = string.Empty;
		}
		
		/// <summary>
		/// The name of the layer.
		/// </summary>
        public string Name { get; set; }
		
		/// <summary>
		/// The opacity of the layer as a value from 0 to 1.
		/// Defaults to 1.
		/// </summary>
		public double Opacity { get; set; }
		
		/// <summary>
		/// Whether the layer is shown or hidden.
		/// </summary>
		public bool Visible { get; set; }
		
		public Dictionary<string, string> Properties { get; private set; }
        
		public int[,] Data { get; private set; }
		
		/// <summary>
		/// The coordinate of the layer in tiles.
		/// </summary>
		public Rectangle Coordinate {
			get { return this.Rect_Coordinate; }
			set {
				//TODO resize Data to new Values
				throw new NotImplementedException();
				//this.Rect_Coordinate = value;
			}
		}
		
    }
}
