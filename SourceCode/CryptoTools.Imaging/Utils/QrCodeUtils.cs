using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoTools.Imaging.Utils
{
	public class QrCodeUtils
	{
		public static Bitmap GenerateBitmap(string inputString)
		{
			QRCodeGenerator generator = new QRCodeGenerator();
			QRCodeData data = generator.CreateQrCode(inputString, QRCodeGenerator.ECCLevel.H);
			QRCode code = new QRCode(data);
			Bitmap bitmap = code.GetGraphic(21, Color.Black, Color.White, null, 15, 0, false);
			return bitmap;

		}
	}
}
