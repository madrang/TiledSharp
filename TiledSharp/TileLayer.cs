using System;
using System.Collections.Generic;
using System.Drawing;

namespace TiledSharp
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
