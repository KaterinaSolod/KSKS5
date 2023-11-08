using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Labk2
{
    public partial class Form1 : Form
    {
        private UdpClient udpListener;
        private Thread listenThread;
        private Bitmap drawingBitmap;
        private Graphics drawingGraphics;

        public Form1()
        {
            InitializeComponent();
            InitializeUdpListener();
            InitializeDrawingCanvas();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void InitializeUdpListener()
        {
            udpListener = new UdpClient(12345);

            listenThread = new Thread(new ThreadStart(ListenForData));
            listenThread.IsBackground = true;
            listenThread.Start();
        }

        private void InitializeDrawingCanvas()
        {
            drawingBitmap = new Bitmap(pictureBox.Width, pictureBox.Height);
            pictureBox.Image = drawingBitmap;
            drawingGraphics = Graphics.FromImage(drawingBitmap);
            drawingGraphics.Clear(Color.White);
        }

        private void ListenForData()
        {
            while (true)
            {
                try
                {
                    IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 12345);
                    byte[] data = udpListener.Receive(ref endPoint);
                    string message = Encoding.UTF8.GetString(data);

                    string[] parts = message.Split(':');
                    string command = parts[0].Trim().ToLower();
                    string[] parameters = parts[1].Split(',');

                    switch (command)
                    {
                        case "draw pixel":
                            int x = int.Parse(parameters[0]);
                            int y = int.Parse(parameters[1]);
                            Color pixelColor = ColorTranslator.FromHtml(parameters[2]);
                            drawingBitmap.SetPixel(x, y, pixelColor);
                            pictureBox.Invalidate();
                            break;

                        case "draw line":
                            int x0 = int.Parse(parameters[0]);
                            int y0 = int.Parse(parameters[1]);
                            int x1 = int.Parse(parameters[2]);
                            int y1 = int.Parse(parameters[3]);
                            Color lineColor = ColorTranslator.FromHtml(parameters[4]);
                            using (Pen pen = new Pen(lineColor))
                            {
                                drawingGraphics.DrawLine(pen, x0, y0, x1, y1);
                                pictureBox.Invalidate();
                            }
                            break;

                        case "draw rectangle":
                            int rectX = int.Parse(parameters[0]);
                            int rectY = int.Parse(parameters[1]);
                            int rectWidth = int.Parse(parameters[2]);
                            int rectHeight = int.Parse(parameters[3]);
                            Color rectColor = ColorTranslator.FromHtml(parameters[4]);
                            using (Pen pen = new Pen(rectColor))
                            {
                                drawingGraphics.DrawRectangle(pen, rectX, rectY, rectWidth, rectHeight);
                                pictureBox.Invalidate();
                            }
                            break;

                        case "fill rectangle":
                            int fillRectX = int.Parse(parameters[0]);
                            int fillRectY = int.Parse(parameters[1]);
                            int fillRectWidth = int.Parse(parameters[2]);
                            int fillRectHeight = int.Parse(parameters[3]);
                            Color fillRectColor = ColorTranslator.FromHtml(parameters[4]);
                            using (Brush brush = new SolidBrush(fillRectColor))
                            {
                                drawingGraphics.FillRectangle(brush, fillRectX, fillRectY, fillRectWidth, fillRectHeight);
                                pictureBox.Invalidate();
                            }
                            break;

                        case "draw ellipse":
                            int ellipseX = int.Parse(parameters[0]);
                            int ellipseY = int.Parse(parameters[1]);
                            int ellipseRadiusX = int.Parse(parameters[2]);
                            int ellipseRadiusY = int.Parse(parameters[3]);
                            Color ellipseColor = ColorTranslator.FromHtml(parameters[4]);
                            using (Pen pen = new Pen(ellipseColor))
                            {
                                drawingGraphics.DrawEllipse(pen, ellipseX, ellipseY, ellipseRadiusX * 2, ellipseRadiusY * 2);
                                pictureBox.Invalidate();
                            }
                            break;

                        case "fill ellipse":
                            int fillEllipseX = int.Parse(parameters[0]);
                            int fillEllipseY = int.Parse(parameters[1]);
                            int fillEllipseRadiusX = int.Parse(parameters[2]);
                            int fillEllipseRadiusY = int.Parse(parameters[3]);
                            Color fillEllipseColor = ColorTranslator.FromHtml(parameters[4]);
                            using (Brush brush = new SolidBrush(fillEllipseColor))
                            {
                                drawingGraphics.FillEllipse(brush, fillEllipseX, fillEllipseY, fillEllipseRadiusX * 2, fillEllipseRadiusY * 2);
                                pictureBox.Invalidate();
                            }
                            break;

                        case "draw circle":
                            int circleX = int.Parse(parameters[0]);
                            int circleY = int.Parse(parameters[1]);
                            int circleRadius = int.Parse(parameters[2]);
                            Color circleColor = ColorTranslator.FromHtml(parameters[3]);
                            using (Pen pen = new Pen(circleColor))
                            {
                                int circleDiameter = circleRadius * 2;
                                drawingGraphics.DrawEllipse(pen, circleX - circleRadius, circleY - circleRadius, circleDiameter, circleDiameter);
                                pictureBox.Invalidate();
                            }
                            break;

                        case "fill circle":
                            int fillCircleX = int.Parse(parameters[0]);
                            int fillCircleY = int.Parse(parameters[1]);
                            int fillCircleRadius = int.Parse(parameters[2]);
                            Color fillCircleColor = ColorTranslator.FromHtml(parameters[3]);
                            using (Brush brush = new SolidBrush(fillCircleColor))
                            {
                                int fillCircleDiameter = fillCircleRadius * 2;
                                drawingGraphics.FillEllipse(brush, fillCircleX - fillCircleRadius, fillCircleY - fillCircleRadius, fillCircleDiameter, fillCircleDiameter);
                                pictureBox.Invalidate();
                            }
                            break;

                        case "draw text":
                            int textX = int.Parse(parameters[0]);
                            int textY = int.Parse(parameters[1]);
                            string text = parameters[2].ToLower();
                            Color textColor = ColorTranslator.FromHtml(parameters[3]);

                            int letterWidth = 10;
                            int letterSpacing = 4;

                            foreach (char letter in text)
                            {
                                using (Pen pen = new Pen(textColor, 2))
                                {
                                    switch (letter)
                                    {
                                        case 'a':
                                            drawingGraphics.DrawLine(pen, textX, textY, textX + letterWidth, textY);
                                            drawingGraphics.DrawLine(pen, textX + letterWidth / 2, textY, textX + letterWidth / 2, textY + 30);
                                            drawingGraphics.DrawLine(pen, textX, textY + 30, textX + letterWidth, textY + 30);
                                            textX += letterWidth + letterSpacing;
                                            break;
                                        case 'd':
                                            drawingGraphics.DrawLine(pen, textX, textY, textX, textY + 30);
                                            drawingGraphics.DrawLine(pen, textX, textY, textX + letterWidth, textY);
                                            drawingGraphics.DrawLine(pen, textX + letterWidth, textY, textX + letterWidth, textY + 30);
                                            drawingGraphics.DrawLine(pen, textX, textY + 30, textX + letterWidth, textY + 30);
                                            textX += letterWidth + letterSpacing;
                                            break;
                                        case 'g':
                                            drawingGraphics.DrawLine(pen, textX + letterWidth, textY, textX + letterWidth, textY + 30);
                                            drawingGraphics.DrawLine(pen, textX + letterWidth, textY + 30, textX, textY + 30);
                                            drawingGraphics.DrawLine(pen, textX, textY + 30, textX, textY + 15);
                                            drawingGraphics.DrawLine(pen, textX, textY + 15, textX + letterWidth / 2, textY + 15);
                                            drawingGraphics.DrawLine(pen, textX + letterWidth / 2, textY + 15, textX + letterWidth / 2, textY + 30);
                                            textX += letterWidth + letterSpacing;
                                            break;
                                        case 'o':
                                            drawingGraphics.DrawEllipse(pen, textX, textY, letterWidth, 30);
                                            textX += letterWidth + letterSpacing;
                                            break;
                                        case 's':
                                            drawingGraphics.DrawLine(pen, textX, textY, textX + letterWidth, textY);
                                            drawingGraphics.DrawLine(pen, textX, textY, textX, textY + 15);
                                            drawingGraphics.DrawLine(pen, textX + letterWidth, textY + 15, textX, textY + 15);
                                            drawingGraphics.DrawLine(pen, textX + letterWidth, textY + 30, textX, textY + 30);
                                            drawingGraphics.DrawLine(pen, textX + letterWidth, textY + 15, textX + letterWidth, textY + 30);
                                            textX += letterWidth + letterSpacing;
                                            break;
                                        case 'h':
                                            drawingGraphics.DrawLine(pen, textX, textY, textX, textY + 30);
                                            drawingGraphics.DrawLine(pen, textX, textY + 15, textX + letterWidth, textY + 15);
                                            drawingGraphics.DrawLine(pen, textX + letterWidth, textY, textX + letterWidth, textY + 30);
                                            textX += letterWidth + letterSpacing;
                                            break;
                                        case 't':
                                            drawingGraphics.DrawLine(pen, textX + letterWidth / 2, textY, textX + letterWidth / 2, textY + 30);
                                            drawingGraphics.DrawLine(pen, textX, textY, textX + letterWidth, textY);
                                            textX += letterWidth + letterSpacing;
                                            break;
                                        default:
                                            textX += letterWidth + letterSpacing;
                                            break;
                                    }
                                }
                            }
                            pictureBox.Invalidate();
                            break;


                        case "draw image":
                            string imagePath = parameters[0];
                            int imageX = int.Parse(parameters[1]);
                            int imageY = int.Parse(parameters[2]);

                            try
                            {
                                Image image = Image.FromFile(imagePath);
                                drawingGraphics.DrawImage(image, imageX, imageY);
                                pictureBox.Invalidate();
                                image.Dispose();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Ошибка при загрузке и рисовании изображения: " + ex.Message);
                            }
                            break;


                        default:
                            Console.WriteLine("Неизвестная команда: " + command);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ошибка при обработке данных: " + ex.Message);
                }
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            udpListener.Close();
            listenThread.Abort();
        }
    }
}
