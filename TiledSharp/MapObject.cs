using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;

namespace TiledSharp
{
	public class MapObject
	{
		/// <summary>
		/// The name of the object.
		/// An arbitrary string.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// The type of the object.
		/// An arbitrary string.
		/// </summary>
		public string Type { get; set; }

		/// <summary>
		/// The coordinate of the object in tiles.
		/// </summary>
		public Rectangle Coordinate;
		
		public Dictionary<string, string> Properties { get; set; }
		
		/// <summary>
		/// Used for tile object. 
		/// </summary>
		public int Gid;

		public MapObject ()
		{
			this.Properties = new Dictionary<string, string> ();
		}
	}
}
