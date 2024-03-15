using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CreateImage
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
           
        }
        private void Form1_Load(object sender, EventArgs e)
        {
             CreateImage();
        }
        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CreateImage();
        }
        public void CreateImage()
        {
            // Width and height of the image
            int width = 392; // Adjusted width to fit vertical text
            int height = 380; // Adjusted height to fit vertical text

            // Create a bitmap with white background
            Bitmap bmp = new Bitmap(width, height);
            using (Graphics graph = Graphics.FromImage(bmp))
            {
                Rectangle rect = new Rectangle(0, 0, width, height);
                graph.FillRectangle(Brushes.White, rect);
            }

            // Draw cardNumber and validity on the bitmap with custom positions and font styles
            using (Graphics graph = Graphics.FromImage(bmp))
            {
                
                string cardNumber = string.IsNullOrEmpty(txtCardNo.Text) == true ? "4890 9105 9330 4905" : txtCardNo.Text;
                string AccountNo = string.IsNullOrEmpty(txtAccountNo.Text) == true ? "01011393123001" : FormatAccNo(txtAccountNo.Text);
                string validityDate = string.IsNullOrEmpty(txtExpiarydate.Text) == true ? "09/23" : txtExpiarydate.Text;
                string cvvTxt = string.IsNullOrEmpty(txtCVV.Text) == true ? "883" : txtCVV.Text;
              
                // Font style for cardNumber
                Font fontStyle = new Font("Arial", 17, FontStyle.Regular);
                Brush brush = Brushes.Black;
                StringFormat stringFormat = new StringFormat();
                // Rotate text 90 degrees
                stringFormat.FormatFlags = StringFormatFlags.DirectionVertical;
                // Draw cardNumber
                float X = 180; // Adjust as needed
                float Y = 140; // Adjust as needed
                graph.DrawString(AccountNo, fontStyle, brush, new RectangleF(X, 70, width, height), stringFormat);
                float _validityX = X + fontStyle.GetHeight()+20;
                graph.DrawString(validityDate, fontStyle, brush, new RectangleF(_validityX, Y, width, height), stringFormat);
                Y = Y + 120;
                graph.DrawString(cvvTxt, fontStyle, brush, new RectangleF(_validityX, Y, width, height), stringFormat);
                // Draw Card
                float cardNumberX = X + 50 + fontStyle.GetHeight(); // Position to the right of cardNumber
                graph.DrawString(cardNumber, fontStyle, brush, new RectangleF(cardNumberX, 70, width, height), stringFormat);
            }

            // Save the bitmap as a BMP file
            bmp.Save("LogoBack.bmp");


            int _width = 691; // Adjusted width to fit vertical text
            int _height = 596; // Adjusted height to fit vertical text

            Bitmap bmpTitle = new Bitmap(_width, _height);
            using (Graphics graph = Graphics.FromImage(bmpTitle))
            {
                Rectangle rect = new Rectangle(0, 0, _width, _height);
                graph.FillRectangle(Brushes.White, rect);
            }
            using (Graphics graph = Graphics.FromImage(bmpTitle))
            {
                string customerName = string.IsNullOrEmpty(txtName.Text) == true ? "Test Account" : txtName.Text;
                Font fontStyle = new Font("Arial", 27, FontStyle.Regular);
                Brush brush = Brushes.Black;
                StringFormat stringFormat = new StringFormat();
                // Rotate text 90 degrees
                stringFormat.FormatFlags = StringFormatFlags.DirectionVertical;
                // Draw cardNumber
                float X = 180; // Adjust as needed
                float Y = 140; // Adjust as needed
                graph.DrawString(customerName, fontStyle, brush, new RectangleF(X, Y, width, height), stringFormat);
            }
            bmpTitle.RotateFlip(RotateFlipType.Rotate180FlipNone);
            bmpTitle.Save("LogoFront3.bmp");

        }







        public static string FormatAccNo(string acctNo)
        {
            if (!string.IsNullOrEmpty(acctNo) && acctNo.Length > 15)
            {
                return acctNo.Substring(0, 4) + " " + acctNo.Substring(4, 4) + " " + acctNo.Substring(8, 4) + " " +
                     acctNo.Substring(12, 4);
            }
            // If the account number is invalid, return the original string
            return acctNo;
        }




        public void CreateImage_Back()
        {
            // Width and height of the image
            int width = 392; // Adjusted width to fit vertical text
            int height = 380; // Adjusted height to fit vertical text

            // Create a bitmap with white background
            Bitmap bmp = new Bitmap(width, height);
            using (Graphics graph = Graphics.FromImage(bmp))
            {
                Rectangle rect = new Rectangle(0, 0, width, height);
                graph.FillRectangle(Brushes.White, rect);
            }

            // Draw cardNumber and validity on the bitmap with custom positions and font styles
            using (Graphics graph = Graphics.FromImage(bmp))
            {
                string cardNumber = "1234 5678 9012 3456";
                // string validityTxt = "VALID";
                string validityDate = "09/23";

                // Font style for cardNumber
                Font cardNumberFont = new Font("Arial", 20, FontStyle.Regular);

                // Font style for validity
                Font validityTxtFont = new Font("Arial", 16, FontStyle.Regular);
                Font validityDateFont = new Font("Arial", 20, FontStyle.Regular);

                Brush brush = Brushes.Black;
                StringFormat stringFormat = new StringFormat();

                // Rotate text 90 degrees
                stringFormat.FormatFlags = StringFormatFlags.DirectionVertical;

                // Draw cardNumber
                float cardNumberX = 250; // Adjust as needed
                float cardNumberY = 60; // Adjust as needed
                                        //graph.DrawString(validityDate, validityTxtFont, brush, new RectangleF(cardNumberX, cardNumberY, width, height), stringFormat);
                                        // cardNumberY = cardNumberY + 80;
                graph.DrawString(validityDate, validityDateFont, brush, new RectangleF(cardNumberX, cardNumberY, width, height), stringFormat);
                cardNumberY = cardNumberY + 90;
                graph.DrawString("883", validityDateFont, brush, new RectangleF(cardNumberX, cardNumberY, width, height), stringFormat);

                // Draw validity
                float validityX = cardNumberX + validityTxtFont.GetHeight(); // Position to the right of cardNumber
                float validityY = 10; // Align with cardNumber
                graph.DrawString(cardNumber, cardNumberFont, brush, new RectangleF(validityX, validityY, width, height), stringFormat);
            }

            // Save the bitmap as a BMP file
            bmp.Save("output.bmp");
        }
    }
}
