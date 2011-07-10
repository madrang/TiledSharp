using System;
using System.Drawing;
using System.Runtime.Serialization;

namespace TiledSharp
{
	[Serializable]
    public partial class ImageInfo : ISerializable
	{
		public ImageInfo(SerializationInfo info, StreamingContext context)
		{
			this.Source = info.GetString("Source");
			this.UseTransColor = info.GetBoolean ("UseTransColor");
			this.TransColor = (Color)info.GetValue("TransColor", typeof(Color));
			this.Size = (Size)info.GetValue("Size", typeof(Size));
		}
		
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("Source", this.Source);
			info.AddValue("UseTransColor", this.UseTransColor);
			info.AddValue("TransColor", this.TransColor);
			info.AddValue("Size", this.Size);
		}
	}
}
