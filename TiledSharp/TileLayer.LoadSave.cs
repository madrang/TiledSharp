//Author:
//      Marc-Andre Ferland <madrang@gmail.com>
//
//Copyright (c) 2011 Linsft
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
using System.IO;
using System.IO.Compression;
using System.Xml;
using System.Drawing;

namespace Linsft.TiledSharp
{
	public partial class TileLayer
    {
		public static TileLayer Load (XmlReader LayerReader)
		{
			if(LayerReader.NodeType != XmlNodeType.Element || !LayerReader.HasAttributes ||
			   LayerReader.IsEmptyElement || LayerReader.Name != "layer")
				throw new ArgumentException("Found no Element of type layer.");
			
			TileLayer LoadingLayer = new TileLayer ();
			
			int TmpIntParse;
			double TmpDoubleParse;
			Rectangle RectCoord = new Rectangle();
			while(LayerReader.MoveToNextAttribute()) {
				switch (LayerReader.Name) {
					
				case "width":
					RectCoord.Width = int.Parse(LayerReader.Value);
					break;
					
				case "height":
					RectCoord.Height = int.Parse(LayerReader.Value);
					break;
					
				case "x":
					RectCoord.X = int.Parse(LayerReader.Value);
					break;
					
				case "y":
					RectCoord.Y = int.Parse(LayerReader.Value);
					break;
					
					/* Optional attributes */
				case "name":
					LoadingLayer.Name = LayerReader.Value;
					break;
					
				case "opacity":
					LoadingLayer.Opacity = double.TryParse(LayerReader.Value, out TmpDoubleParse) ? TmpDoubleParse : 1.0d;
					break;
					
					//FIXME "", "false", "0" to == false
					// "true", Value != 0 to == true
				case "visible":
					LoadingLayer.Visible = int.TryParse(LayerReader.Value, out TmpIntParse) ? TmpIntParse != 0 : true;
					break;
					
				}
			}
			LoadingLayer.Rect_Coordinate = RectCoord;
			
			bool FoundData = false;
			LayerReader.MoveToElement();
			using (XmlReader LayerChildReader = LayerReader.ReadSubtree()) {
				bool ReadingChilds = true;
				while(ReadingChilds) {
					
					if(LayerChildReader.NodeType != XmlNodeType.Element) {
						ReadingChilds = LayerChildReader.Read ();
						continue;
					}
					
					switch (LayerChildReader.Name) {
					case "data":
						if(FoundData)
							throw new ArgumentException("Element layer has two data element and can only have one.");
						LoadingLayer.Data = LoadData(LayerChildReader, LoadingLayer.Coordinate.Width, LoadingLayer.Coordinate.Height);
						FoundData = true;
						break;
						
					case "properties":
						Helpers.ReadProperties (LayerChildReader, LoadingLayer.Properties);
						break;
						
					default:
						ReadingChilds = LayerChildReader.Read ();
						break;
					}
				}
			}
			
			if(!FoundData)
				throw new ArgumentException("Found no Element of type data.");
			
			return LoadingLayer;
		}
		
		private static int[,] LoadData(XmlReader DataReader, int width, int height)
		{
			using (MemoryStream CompressedData = new MemoryStream()) {
				
				string compression = DataReader.GetAttribute("compression");
				string encoding = DataReader.GetAttribute("encoding");
				
				if (encoding == "base64") {
					ReadBase64(DataReader, CompressedData);
				} else if (encoding == "csv") {
					return ReadCSV(DataReader, width, height);
				} else throw new NotSupportedException("Layer is using an unsupported data format.");
				CompressedData.Position = 0;
				
				if (compression == "gzip")
					return DecompressLayerGzip(CompressedData, width, height);
				else if (compression == "zlib")
					return DecompressLayerZlib(CompressedData, width, height);
				else if (compression == null)
					return DecompressedLayer(CompressedData, width, height);
				else throw new NotSupportedException("Layer is using an unsupported compression.");
			}
		}
		
		private static void ReadBase64(XmlReader DataReader, Stream Output)
		{
			/*
			int ReadCount = 0;
			byte[] DataBuffer = new byte[1024];
			do {
				ReadCount = LayerReader.ReadElementContentAsBase64(DataBuffer, 0, DataBuffer.Length);
				CompressedData.Write(DataBuffer, 0, ReadCount);
			} while(ReadCount != 0);
			*/ 
			
			//FIXME For some reason ReadElementContentAsBase64 misses some bytes.
			//fixed temporarely with FromBase64String.
			byte[] data = Convert.FromBase64String(DataReader.ReadElementString());
			Output.Write(data, 0, data.Length);
		}
		
		private static int[,] ReadCSV(XmlReader DataReader, int width, int height)
		{
			string[] ValueArray = DataReader.ReadElementString().Replace ("\n", "").Split(new char[] {',', ';'});
			int[,] Data = new int[width, height];
			
			for (int y = 0; y < height; y++) {
				for (int x = 0; x < width; x++) {
					Data[x, y] = int.Parse(ValueArray[x + (y * width)]);
				}
			}
			return Data;
		}
		
		private static int[,] DecompressedLayer(Stream Input, int width, int height)
		{
			int[,] Data = new int[width, height];
			using(BinaryReader br = new BinaryReader (Input)) {
				for (int y = 0; y < height; y++) {
					for (int x = 0; x < width; x++) {
						Data[x, y] = br.ReadInt32 ();
					}
				}
			}
			return Data;
		}
		
		private static int[,] DecompressLayerGzip(Stream Input, int width, int height)
		{
			Stream GZipReader = new GZipStream(Input, CompressionMode.Decompress);
			return DecompressedLayer(GZipReader, width, height);
		}
		
		private static int[,] DecompressLayerZlib(Stream Input, int width, int height)
		{
			Ionic.Zlib.ZlibStream ZlibDecompressed = new Ionic.Zlib.ZlibStream(Input, Ionic.Zlib.CompressionMode.Decompress);
			return DecompressedLayer(ZlibDecompressed, width, height);
		}

	}
}
