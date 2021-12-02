

using System.Drawing;
using System.Xml;
using Svg;
/**
* MetroFramework - Modern UI for WinForms
* 
* The MIT License (MIT)
* Copyright (c) 2021 Edwin Zimmerman, https://github.com/9cb14c1ec0
* 
* Permission is hereby granted, free of charge, to any person obtaining a copy of 
* this software and associated documentation files (the "Software"), to deal in the 
* Software without restriction, including without limitation the rights to use, copy, 
* modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, 
* and to permit persons to whom the Software is furnished to do so, subject to the 
* following conditions:
* 
* The above copyright notice and this permission notice shall be included in 
* all copies or substantial portions of the Software.
* 
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
* INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A 
* PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
* HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
* CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE 
* OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/
namespace MetroFramework.Drawing
 {
     public enum MetroIcons
     {
         None,
         Eye,
         MobilePhone
     }

     public class MetroIconUtils
     {
         public static Bitmap GetMetroIconBitmap(MetroIcons icon, Color color)
         {
             var resource_name = "MetroFramework.Drawing.Icons.";

             switch (icon)
             {
                 case MetroIcons.Eye:
                    resource_name += "eye.svg";
                    break;
                case MetroIcons.MobilePhone:
                    resource_name += "mobile.svg";
                    break;
             }

            SvgDocument svg;
            var assembly = System.Reflection.Assembly.GetAssembly(typeof(MetroIcons));
			using (System.IO.Stream stream = assembly.GetManifestResourceStream(resource_name))
			{
                svg = SvgDocument.Open<SvgDocument>(stream);
			}
            processNodes(svg.Descendants(), new SvgColourServer(color));
            return svg.Draw(30, 0);
         }

        private static void processNodes(IEnumerable<SvgElement> nodes, SvgPaintServer colorServer)
        {
            foreach (var  node in nodes)
            {
                if(node.Fill != SvgPaintServer.None) node.Fill = colorServer;
                if(node.Color != SvgPaintServer.None) node.Color = colorServer;
                if(node.Stroke != SvgPaintServer.None) node.Stroke = colorServer;

                processNodes(node.Descendants(), colorServer);
            }
        }
     }
 }