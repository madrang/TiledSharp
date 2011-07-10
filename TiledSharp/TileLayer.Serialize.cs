using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.Serialization;

namespace TiledSharp
{
	[Serializable]
	public partial class TileLayer : ISerializable
    {
		public TileLayer(SerializationInfo info, StreamingContext context)
		{
			this.Name = info.GetString("Name");
			this.Rect_Coordinate = (Rectangle)info.GetValue("Coordinate", typeof(Rectangle));
			this.Opacity = info.GetDouble("Opacity");
			this.Visible = info.GetBoolean("Visible");
			
			this.Properties = (Dictionary<string, string>)info.GetValue("TransColor", typeof(Dictionary<string, string>));
			this.Data = (int[,])info.GetValue("TransColor", typeof(int[,]));
		}
		
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("Name", this.Name);
			info.AddValue("Coordinate", this.Rect_Coordinate);
			info.AddValue("Opacity", this.Opacity);
			info.AddValue("Visible", this.Visible);
			
			info.AddValue("Properties", this.Properties);
			info.AddValue("Data", this.Data);
		}
	}
}
