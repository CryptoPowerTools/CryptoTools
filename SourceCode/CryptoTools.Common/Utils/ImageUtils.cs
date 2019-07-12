using System;
using System.Drawing;
//using System.Drawing.Drawing2D;
//using System.Drawing.Imaging;

namespace CryptoTools.Common.Imaging
{
	public class ImageUtils
	{

        //public bool ResizeImage(string fileName, string imgFileName,ImageFormat format, int percent)
        //{
        //	try
        //	{
        //		using (Image img = Image.FromFile(fileName))
        //		{
        //			int width = img.Width * (int)(percent * .01);
        //			int height = img.Height * (int)(percent * .01);
        //			Image thumbNail = new Bitmap(width, height, img.PixelFormat);
        //			Graphics g = Graphics.FromImage(thumbNail);
        //			g.CompositingQuality = CompositingQuality.HighQuality;
        //			g.SmoothingMode = SmoothingMode.HighQuality;
        //			g.InterpolationMode = InterpolationMode.HighQualityBicubic;
        //			Rectangle rect = new Rectangle(0, 0, width, height);
        //			g.DrawImage(img, rect);
        //			thumbNail.Save(imgFileName, format);
        //		}
        //		return true;
        //	}
        //	catch (Exception)
        //	{
        //		return false;
        //	}
        //}


            // REMoVED when converting to .NET Core 2.0
            // TODO I would prefer to not have this dependency in the common library

  //      public static Image ResizeImage(Image image, ImageFormat format, int percent)
		//{
		//	try
		//	{
		//		int width = (int)(((double)image.Width) * ((double)percent) * .01);
		//		int height = (int)(((double)image.Height) * ((double)percent) * .01);
		//		Image resizedImage = new Bitmap(width, height, image.PixelFormat);
		//		Graphics g = Graphics.FromImage(resizedImage);
		//		g.CompositingQuality = CompositingQuality.HighQuality;
		//		g.SmoothingMode = SmoothingMode.HighQuality;
		//		g.InterpolationMode = InterpolationMode.HighQualityBicubic;
		//		Rectangle rect = new Rectangle(0, 0, width, height);
		//		g.DrawImage(image, rect);

		//		return resizedImage;
		//	}
		//	catch (Exception)
		//	{
		//		return null;
		//	}
		//}




	}
}
