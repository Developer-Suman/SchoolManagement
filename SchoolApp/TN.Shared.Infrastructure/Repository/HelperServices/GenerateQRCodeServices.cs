using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Application.ServiceInterface.IHelperServices;
using TN.Shared.Infrastructure.Data;
using TN.Shared.Infrastructure.SignalRHub;
using ZXing;
using ZXing.Common;

namespace TN.Shared.Infrastructure.Repository.HelperServices
{
    public class GenerateQRCodeServices : IGenerateQRCodeServices
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _outputFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "qrcodes");

        public GenerateQRCodeServices(IHubContext<NotificationHub> hubContext, IHttpContextAccessor httpContextAccessor, ApplicationDbContext applicationDbContext)
        {
            _hubContext = hubContext;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<string> GenerateSvgAsync(string values)
        {
            if (!Directory.Exists(_outputFolder))
            {
                Directory.CreateDirectory(_outputFolder);
            }

            // ✅ Interpolated string, not literal
            string value = $"https://location.finnetra.com/api/Purchase/test?abc={values}";
            // ✅ Set default QR value if empty



            int qrCodeSize = 400; // Size of QR code in pixels

            // Generate QR code pixel data using ZXing
            var writer = new BarcodeWriterPixelData
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new EncodingOptions
                {
                    Height = qrCodeSize,
                    Width = qrCodeSize,
                    Margin = 1
                }
            };

            var pixelData = writer.Write(value);

            // Create bitmap from pixel data
            using var qrBitmap = new Bitmap(pixelData.Width, pixelData.Height, PixelFormat.Format32bppArgb);
            var bitmapData = qrBitmap.LockBits(new Rectangle(0, 0, pixelData.Width, pixelData.Height),
                ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

            try
            {
                System.Runtime.InteropServices.Marshal.Copy(pixelData.Pixels, 0, bitmapData.Scan0, pixelData.Pixels.Length);
            }
            finally
            {
                qrBitmap.UnlockBits(bitmapData);
            }

            // Load the logo image "new.png"
            string logoPath = Path.Combine(_outputFolder, "new.png");
            if (!File.Exists(logoPath))
                throw new FileNotFoundException("Logo file 'new.png' not found.", logoPath);

            using Bitmap logo = (Bitmap)Image.FromFile(logoPath);

            int circleDiameter = qrCodeSize / 5;
            int circleX = (qrCodeSize - circleDiameter) / 2;
            int circleY = (qrCodeSize - circleDiameter) / 2;
            int padding = 8;

            int logoDrawSize = circleDiameter - padding * 2;
            int logoX = circleX + padding;
            int logoY = circleY + padding;

            using Graphics graphics = Graphics.FromImage(qrBitmap);
            graphics.CompositingQuality = CompositingQuality.HighQuality;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.SmoothingMode = SmoothingMode.AntiAlias;

            // Draw dark blue border around entire QR code
            using var qrBorderPen = new Pen(Color.FromArgb(0, 51, 102), 6);
            graphics.DrawRectangle(qrBorderPen, 0, 0, qrBitmap.Width - 1, qrBitmap.Height - 1);

            // Draw subtle shadow behind circle
            int shadowOffset = 4;
            for (int i = 0; i < 6; i++)
            {
                int alpha = 80 - i * 12;
                if (alpha < 0) alpha = 0;
                using var shadowBrush = new SolidBrush(Color.FromArgb(alpha, 0, 0, 0));
                graphics.FillEllipse(shadowBrush, circleX - i + shadowOffset, circleY - i + shadowOffset, circleDiameter + i * 2, circleDiameter + i * 2);
            }

            // Draw white circular background with border for logo
            using var path = new GraphicsPath();
            path.AddEllipse(circleX, circleY, circleDiameter, circleDiameter);
            using var whiteBrush = new SolidBrush(Color.White);
            graphics.FillPath(whiteBrush, path);

            using var circleBorderPen = new Pen(Color.FromArgb(0, 51, 102), 4);
            graphics.DrawEllipse(circleBorderPen, circleX, circleY, circleDiameter, circleDiameter);

            // Draw the logo image centered inside the circle with padding
            graphics.DrawImage(logo, logoX, logoY, logoDrawSize, logoDrawSize);

            string fileName = $"{Guid.NewGuid()}.png";
            string outputFile = Path.Combine(_outputFolder, fileName);

            qrBitmap.Save(outputFile, ImageFormat.Png);

            // ✅ Return relative web path
            return $"/qrcodes/{fileName}";
        }
    }
}
