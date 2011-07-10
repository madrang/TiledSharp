using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;

namespace TiledSharp
{
    public partial class ObjectGroup : Collection<MapObject>, iLayer
	{
		/// <summary>
		/// The name of the object group.
		/// </summary>
		public string Name { get; set; }
		
		/// <summary>
		/// The coordinate of the object group in tiles.
		/// </summary>
		public Rectangle Coordinate { get; set; }
		
		public Dictionary<string, string> Properties { get; private set; }
		
		public ObjectGroup ()
		{
			this.Properties = new Dictionary<string, string> ();
		}
		
	}
}
