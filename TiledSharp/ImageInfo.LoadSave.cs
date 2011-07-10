using System;
using System.Drawing;
using System.Xml;

namespace TiledSharp
{
    public partial class ImageInfo
	{
		//TODO check with and height to see if it is the good file.
		public static ImageInfo Load (XmlReader ImageReader)
		{
			ImageInfo LoadingImageInfo = new ImageInfo();
			
			if(ImageReader.NodeType != XmlNodeType.Element || !ImageReader.HasAttributes ||
			   ImageReader.Name != "image")
				throw new ArgumentException("Element is not of type image.");
			
			while(ImageReader.MoveToNextAttribute()) {
				switch (ImageReader.Name) {
					
				case "trans":
					if(ImageReader.Value == "")
						break;
					
					//TODO use regex to identify and convert.
					string col = ImageReader.Value;
					if (col.Length == 6 && !col.StartsWith ("#"))
						col = "#" + col;
					
					LoadingImageInfo.TransColor = ColorTranslator.FromHtml (col);
					LoadingImageInfo.UseTransColor = true;
					break;
					
				case "source":
					LoadingImageInfo.Source = ImageReader.Value;
					break;
					
				case "width":
					LoadingImageInfo.Size.Width = int.Parse(ImageReader.Value);
					break;
					
				case "height":
					LoadingImageInfo.Size.Height = int.Parse(ImageReader.Value);
					break;
					
				}
			}
			
			//Go to next Element.
			ImageReader.Read();
			
			return LoadingImageInfo;
		}
	}
}
