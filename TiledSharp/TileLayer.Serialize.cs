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
using System.Drawing;
using System.Runtime.Serialization;

namespace TheWarrentTeam.TiledSharp
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
