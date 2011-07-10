using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Drawing;

namespace TiledSharp
{
	[Serializable]
	public partial class Map : ISerializable
	{
		public Map(SerializationInfo info, StreamingContext context)
		{
			this.Version = info.GetString("Version");
			this.Orientation = (Orientation)info.GetByte("Orientation");
			
			this.Size = (Size)info.GetValue("Size", typeof(Size));
			this.TileSize = (Size)info.GetValue("TileSize", typeof(Size));
			
			this.Properties = (Dictionary<string, string>)info.GetValue("Properties", typeof(Dictionary<string, string>));
			this.Layers = (Collection<iLayer>)info.GetValue("Layers", typeof(Collection<iLayer>));
			
			this.TileSets = (Collection<TileSet>)info.GetValue("TileSets", typeof(Collection<TileSet>));
		}
		
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("Version", this.Version);
			info.AddValue("Orientation", (byte)this.Orientation);
			
			info.AddValue("Size", this.Size);
			info.AddValue("TileSize", this.TileSize);
			
			info.AddValue("Properties", this.Properties);
			info.AddValue("Layers", this.Layers);
			
			info.AddValue("TileSets", this.TileSets);
		}
	}
}
