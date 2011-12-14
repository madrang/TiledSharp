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
using System.Drawing;
using System.Xml;

namespace TheWarrentTeam.TiledSharp
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
