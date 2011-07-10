using System;
using System.Drawing;

namespace TiledSharp
{
    /// <summary>
    /// Represents an image in a tiled map.
    /// </summary>
    public partial class ImageInfo
    {
        public string Source { get; set; }
        public Color TransColor { get; set; }
        public bool UseTransColor { get; set; }
		public Size Size;
		
		public ImageInfo ()
		{
			this.Source = string.Empty;
			this.TransColor = Color.White;
			this.UseTransColor = false;
			this.Size = new Size(0, 0);
		}
		
    }
}
