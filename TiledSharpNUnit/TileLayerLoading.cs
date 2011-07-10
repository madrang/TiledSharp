using System;
using System.Xml;
using TiledSharp;
using NUnit.Framework;

namespace TiledSharpNUnit
{
	[TestFixture()]
	public class TileLayerLoading
	{
		private void AssertLayer (TileLayer TestLayer)
		{
			Assert.AreEqual("Tile Layer 1", TestLayer.Name);
			
			Assert.AreEqual(0, TestLayer.Coordinate.X, "X");
			Assert.AreEqual(0, TestLayer.Coordinate.Y, "Y");
			Assert.AreEqual(9, TestLayer.Coordinate.Width, "Width");
			Assert.AreEqual(9, TestLayer.Coordinate.Height, "Height");
			Assert.AreEqual(1d, TestLayer.Opacity, "Opacity");
			Assert.AreEqual(true, TestLayer.Visible, "Visible");
			
			//Alternating 2,3,4
			for (int y = 0; y < TestLayer.Coordinate.Height; y++) {
				for (int x = 0; x < TestLayer.Coordinate.Width; x++) {
					Assert.AreEqual((y % 3) + 2, TestLayer.Data[x, y], "Error in layer data.");
				}
			}
			
			string str;
			
			Assert.AreEqual(true, TestLayer.Properties.TryGetValue("PropTest 1", out str), "Missing Property \"PropTest 1\".");
			Assert.AreEqual("Value 1", str, "PropTest 1");
			
			Assert.AreEqual(true, TestLayer.Properties.TryGetValue("PropTest 2", out str), "Missing Property \"PropTest 2\".");
			Assert.AreEqual("Value 2", str, "PropTest 2");
			
			Assert.AreEqual(true, TestLayer.Properties.TryGetValue("PropTest 3", out str), "Missing Property \"PropTest 3\".");
			Assert.AreEqual("Value 3", str, "PropTest 3");
		}
				
		
		[Test()]
		public void Base64 ()
		{
			string Base64Layer = " <layer name=\"Tile Layer 1\" width=\"9\" height=\"9\">\n"
				+ "  <data encoding=\"base64\">\n"
					+ "   AgAAAAIAAAACAAAAAgAAAAIAAAACAAAAAgAAAAIAAAACAAAA"
					+ "AwAAAAMAAAADAAAAAwAAAAMAAAADAAAAAwAAAAMAAAADAAAABAA"
					+ "AAAQAAAAEAAAABAAAAAQAAAAEAAAABAAAAAQAAAAEAAAAAgAAAA"
					+ "IAAAACAAAAAgAAAAIAAAACAAAAAgAAAAIAAAACAAAAAwAAAAMAA"
					+ "AADAAAAAwAAAAMAAAADAAAAAwAAAAMAAAADAAAABAAAAAQAAAAE"
					+ "AAAABAAAAAQAAAAEAAAABAAAAAQAAAAEAAAAAgAAAAIAAAACAAA"
					+ "AAgAAAAIAAAACAAAAAgAAAAIAAAACAAAAAwAAAAMAAAADAAAAAw"
					+ "AAAAMAAAADAAAAAwAAAAMAAAADAAAABAAAAAQAAAAEAAAABAAAA"
					+ "AQAAAAEAAAABAAAAAQAAAAEAAAA\n"
					+ "  </data>\n"
					+ "  <properties>"
					+ "   <property name=\"PropTest 1\" value=\"Value 1\"/>\n"
					+ "   <property name=\"PropTest 2\" value=\"Value 2\"/>\n"
					+ "   <property name=\"PropTest 3\" value=\"Value 3\"/>\n"
					+ "  </properties>\n"
					+ " </layer>\n";
			
			System.IO.StringReader Reader = new System.IO.StringReader(Base64Layer);
			XmlReader NodeReader = XmlReader.Create(Reader);
			
			NodeReader.ReadToFollowing("layer");
			
			AssertLayer(TileLayer.Load(NodeReader));
		}
		
		[Test()]
		public void Base64Gzip ()
		{
			string Base64GzipLayer = " <layer name=\"Tile Layer 1\" width=\"9\" height=\"9\">\n"
				+ "  <data encoding=\"base64\" compression=\"gzip\">\n"
					+ "   H4sIAAAAAAAAA2NiYGBgIoCZicAsRGBC9gxVuwAU6aN8RAEAAA==\n"
					+ "  </data>\n"
					+ "  <properties>"
					+ "   <property name=\"PropTest 1\" value=\"Value 1\"/>\n"
					+ "   <property name=\"PropTest 2\" value=\"Value 2\"/>\n"
					+ "   <property name=\"PropTest 3\" value=\"Value 3\"/>\n"
					+ "  </properties>\n"
					+ " </layer>\n";
			
			System.IO.StringReader Reader = new System.IO.StringReader(Base64GzipLayer);
			XmlReader NodeReader = XmlReader.Create(Reader);
			
			NodeReader.ReadToFollowing("layer");
			
			AssertLayer(TileLayer.Load(NodeReader));
		}
		
		[Test()]
		public void Base64Zlib ()
		{
			string Base64ZlibLayer = " <layer name=\"Tile Layer 1\" width=\"9\" height=\"9\">\n"
				+ "  <data encoding=\"base64\" compression=\"zlib\">\n"
					+ "   eJxjYmBgYCKAmYnALERgQvYMVbsAlVgA9A==\n"
					+ "  </data>\n"
					+ "  <properties>"
					+ "   <property name=\"PropTest 1\" value=\"Value 1\"/>\n"
					+ "   <property name=\"PropTest 2\" value=\"Value 2\"/>\n"
					+ "   <property name=\"PropTest 3\" value=\"Value 3\"/>\n"
					+ "  </properties>\n"
					+ " </layer>\n";
			
			System.IO.StringReader Reader = new System.IO.StringReader(Base64ZlibLayer);
			XmlReader NodeReader = XmlReader.Create(Reader);
			
			NodeReader.ReadToFollowing("layer");
			
			AssertLayer(TileLayer.Load(NodeReader));
		}
		
		[Test()]
		public void CSV ()
		{
			string CSVLayer = " <layer name=\"Tile Layer 1\" width=\"9\" height=\"9\">\n"
				+ "  <data encoding=\"csv\">\n"
					+ "2,2,2,2,2,2,2,2,2,\n"
					+ "3,3,3,3,3,3,3,3,3,\n"
					+ "4,4,4,4,4,4,4,4,4,\n"
					+ "2,2,2,2,2,2,2,2,2,\n"
					+ "3,3,3,3,3,3,3,3,3,\n"
					+ "4,4,4,4,4,4,4,4,4,\n"
					+ "2,2,2,2,2,2,2,2,2,\n"
					+ "3,3,3,3,3,3,3,3,3,\n"
					+ "4,4,4,4,4,4,4,4,4\n"
					+ "  </data>\n"
					+ "  <properties>"
					+ "   <property name=\"PropTest 1\" value=\"Value 1\"/>\n"
					+ "   <property name=\"PropTest 2\" value=\"Value 2\"/>\n"
					+ "   <property name=\"PropTest 3\" value=\"Value 3\"/>\n"
					+ "  </properties>\n"
					+ " </layer>\n";
			
			System.IO.StringReader Reader = new System.IO.StringReader(CSVLayer);
			XmlReader NodeReader = XmlReader.Create(Reader);
			
			NodeReader.ReadToFollowing("layer");
			
			AssertLayer(TileLayer.Load(NodeReader));
		}
		
		[Test()]
		public void XML ()
		{
			string XMLLayer = " <layer name=\"Tile Layer 1\" width=\"9\" height=\"9\">"
				+ "  <data>\n"
					+ "   <tile gid=\"2\"/>\n"
					+ "   <tile gid=\"2\"/>\n"
					#region XmlData
					+ "   <tile gid=\"2\"/>\n"
					+ "   <tile gid=\"2\"/>\n"
					+ "   <tile gid=\"2\"/>\n"
					+ "   <tile gid=\"2\"/>\n"
					+ "   <tile gid=\"2\"/>\n"
					+ "   <tile gid=\"2\"/>\n"
					+ "   <tile gid=\"2\"/>\n"
					+ "   <tile gid=\"3\"/>\n"
					+ "   <tile gid=\"3\"/>\n"
					+ "   <tile gid=\"3\"/>\n"
					+ "   <tile gid=\"3\"/>\n"
					+ "   <tile gid=\"3\"/>\n"
					+ "   <tile gid=\"3\"/>\n"
					+ "   <tile gid=\"3\"/>\n"
					+ "   <tile gid=\"3\"/>\n"
					+ "   <tile gid=\"3\"/>\n"
					+ "   <tile gid=\"4\"/>\n"
					+ "   <tile gid=\"4\"/>\n"
					+ "   <tile gid=\"4\"/>\n"
					+ "   <tile gid=\"4\"/>\n"
					+ "   <tile gid=\"4\"/>\n"
					+ "   <tile gid=\"4\"/>\n"
					+ "   <tile gid=\"4\"/>\n"
					+ "   <tile gid=\"4\"/>\n"
					+ "   <tile gid=\"4\"/>\n"
					+ "   <tile gid=\"2\"/>\n"
					+ "   <tile gid=\"2\"/>\n"
					+ "   <tile gid=\"2\"/>\n"
					+ "   <tile gid=\"2\"/>\n"
					+ "   <tile gid=\"2\"/>\n"
					+ "   <tile gid=\"2\"/>\n"
					+ "   <tile gid=\"2\"/>\n"
					+ "   <tile gid=\"2\"/>\n"
					+ "   <tile gid=\"2\"/>\n"
					+ "   <tile gid=\"3\"/>\n"
					+ "   <tile gid=\"3\"/>\n"
					+ "   <tile gid=\"3\"/>\n"
					+ "   <tile gid=\"3\"/>\n"
					+ "   <tile gid=\"3\"/>\n"
					+ "   <tile gid=\"3\"/>\n"
					+ "   <tile gid=\"3\"/>\n"
					+ "   <tile gid=\"3\"/>\n"
					+ "   <tile gid=\"3\"/>\n"
					+ "   <tile gid=\"4\"/>\n"
					+ "   <tile gid=\"4\"/>\n"
					+ "   <tile gid=\"4\"/>\n"
					+ "   <tile gid=\"4\"/>\n"
					+ "   <tile gid=\"4\"/>\n"
					+ "   <tile gid=\"4\"/>\n"
					+ "   <tile gid=\"4\"/>\n"
					+ "   <tile gid=\"4\"/>\n"
					+ "   <tile gid=\"4\"/>\n"
					+ "   <tile gid=\"2\"/>\n"
					+ "   <tile gid=\"2\"/>\n"
					+ "   <tile gid=\"2\"/>\n"
					+ "   <tile gid=\"2\"/>\n"
					+ "   <tile gid=\"2\"/>\n"
					+ "   <tile gid=\"2\"/>\n"
					+ "   <tile gid=\"2\"/>\n"
					+ "   <tile gid=\"2\"/>\n"
					+ "   <tile gid=\"2\"/>\n"
					+ "   <tile gid=\"3\"/>\n"
					+ "   <tile gid=\"3\"/>\n"
					+ "   <tile gid=\"3\"/>\n"
					+ "   <tile gid=\"3\"/>\n"
					+ "   <tile gid=\"3\"/>\n"
					+ "   <tile gid=\"3\"/>\n"
					+ "   <tile gid=\"3\"/>\n"
					+ "   <tile gid=\"3\"/>\n"
					+ "   <tile gid=\"3\"/>\n"
					+ "   <tile gid=\"4\"/>\n"
					+ "   <tile gid=\"4\"/>\n"
					+ "   <tile gid=\"4\"/>\n"
					+ "   <tile gid=\"4\"/>\n"
					+ "   <tile gid=\"4\"/>\n"
					+ "   <tile gid=\"4\"/>\n"
					+ "   <tile gid=\"4\"/>\n"
					#endregion
					+ "   <tile gid=\"4\"/>\n"
					+ "   <tile gid=\"4\"/>\n"
					+ "  </data>\n"
					+ "  <properties>"
					+ "   <property name=\"PropTest 1\" value=\"Value 1\"/>\n"
					+ "   <property name=\"PropTest 2\" value=\"Value 2\"/>\n"
					+ "   <property name=\"PropTest 3\" value=\"Value 3\"/>\n"
					+ "  </properties>\n"
					+ " </layer>\n";
			
			System.IO.StringReader Reader = new System.IO.StringReader(XMLLayer);
			XmlReader NodeReader = XmlReader.Create(Reader);
			
			NodeReader.ReadToFollowing("layer");
			
			AssertLayer(TileLayer.Load(NodeReader));
		}
	}
}
