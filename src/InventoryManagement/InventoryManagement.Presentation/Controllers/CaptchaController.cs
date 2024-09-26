using Microsoft.AspNetCore.Mvc;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing;


namespace InventoryManagement.Presentation.Controllers
{
    public class CaptchaController : Controller
    {
        [HttpGet]
        public IActionResult GenerateCaptchaImage()
        {
            string captchaText = GenerateRandomText();
            Image image = GenerateCaptchaImage(captchaText);
            HttpContext.Session.SetString("CaptchaCode", captchaText);

            MemoryStream ms = new MemoryStream();
            image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            //Session["CaptchaCode"] = captchaText; // Store the CAPTCHA code in session for validation
            var img = ms.ToArray();
            return base.File(img, "image/jpeg");
        }

        private string GenerateRandomText()
        {
            // Generate random string (e.g., alphanumeric characters)
            Random rand = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, 6)
              .Select(s => s[rand.Next(s.Length)]).ToArray());
        }

        public Image GenerateCaptchaImage(string text)
        {
            Image img = new Bitmap(1, 1);
            Graphics drawing = Graphics.FromImage(img);
            SizeF textSize = drawing.MeasureString(text, new Font(FontFamily.GenericMonospace,14));
            img.Dispose();
            drawing.Dispose();
            img = new Bitmap(110, 35);
            drawing = Graphics.FromImage(img);
            drawing.Clear(Color.WhiteSmoke);
            Brush textBrush = new SolidBrush(Color.Purple);
            drawing.DrawString(text, new Font(FontFamily.GenericMonospace, 14), textBrush, 0, 0);
            drawing.Save();
            textBrush.Dispose();
            drawing.Dispose();
            return img;
        }
    }
}
