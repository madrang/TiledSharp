using System;
using System.Collections.Generic;
using System.Drawing;

namespace TiledSharp
{
	public interface iLayer
	{
		/// <summary>
		/// The name of the layer.
		/// </summary>
		string Name { get; set; }
		
		/// <summary>
		/// The coordinate of the layer in tiles.
		/// </summary>
		Rectangle Coordinate { get; set; }
		
		Dictionary<string, string> Properties { get; }
	}
}
