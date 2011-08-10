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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Drawing;

namespace TheWarrentTeam.TiledSharp
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
