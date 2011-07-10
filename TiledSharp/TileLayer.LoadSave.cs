using System;
using System.IO;
using System.IO.Compression;
using System.Xml;
using System.Drawing;

namespace TiledSharp
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
			/*
			int[,] Data = new int[width, height];
			using(zlib.ZInputStream ZReader = new zlib.ZInputStream(Input)) {
				for (int y = 0; y < height; y++) {
					for (int x = 0; x < width; x++) {
						try {
							Data[x, y] = ZReader.ReadInt32 ();
						} catch (Exception) {
							Console.Write("Done [{0}, {1}]", x, y);
							Console.WriteLine(" of [{0}, {1}]", width, height);
							
							Console.Write("Input {0}", Input.Position);
							Console.WriteLine(" of {0}", Input.Length);
							
							Console.WriteLine("In {0}, Out {1}", ZReader.TotalIn, ZReader.TotalOut);
							throw;
						}
					}
				}
			}
			
			return Data;
			*/
			
			//FIXME For some reason ZInputStream deflate has no effect on data.
			//Fixed temporarely by using ZOutputStream.
			
			MemoryStream ZlibDecompressed = new MemoryStream();
			zlib.ZOutputStream Zout = new zlib.ZOutputStream(ZlibDecompressed);
			
			int ReadCount = 0;
			byte[] Buf = new byte[1024];
			do{
				ReadCount = Input.Read(Buf, 0, Buf.Length);
				Zout.Write(Buf, 0, ReadCount);
			} while (ReadCount > 0);
			
			ZlibDecompressed.Position = 0;
			return DecompressedLayer(ZlibDecompressed, width, height);
		}

	}
}
