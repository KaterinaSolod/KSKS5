using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.IO;

class UdpClientExample
{
    static void Main(string[] args)
    {
        string serverIP = "127.0.0.1";
        int serverPort = 12345;

        using (UdpClient udpClient = new UdpClient())
        {
            IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse(serverIP), serverPort);

            while (true)
            {
                Console.WriteLine("Выберите команду:");
                Console.WriteLine("1. Clear Display");
                Console.WriteLine("2. Draw Pixel");
                Console.WriteLine("3. Draw Line");
                Console.WriteLine("4. Draw Rectangle");
                Console.WriteLine("5. Fill Rectangle");
                Console.WriteLine("6. Draw Ellipse");
                Console.WriteLine("7. Fill Ellipse");
                Console.WriteLine("8. Draw Circle");
                Console.WriteLine("9. Fill Circle");
                Console.WriteLine("10. Draw text");
                Console.WriteLine("11. Draw Image");
                Console.WriteLine("12. Выход");
                Console.Write("Введите номер команды: ");

                string userInput = Console.ReadLine();
                if (userInput == "12")
                {
                    break;
                }

                string command = "";
                string parameters = "";

                switch (userInput)
                {
                    case "1":
                        command = "clear display";
                        Console.Write("Введите цвет: ");
                        parameters = Console.ReadLine();
                        break;
                    case "2":
                        command = "draw pixel";
                        Console.Write("Введите x0: ");
                        parameters = Console.ReadLine();
                        Console.Write("Введите y0: ");
                        parameters += "," + Console.ReadLine();
                        Console.Write("Введите цвет: ");
                        parameters += "," + Console.ReadLine();
                        break;
                    case "3":
                        command = "draw line";
                        Console.Write("Введите x0: ");
                        parameters = Console.ReadLine();
                        Console.Write("Введите y0: ");
                        parameters += "," + Console.ReadLine();
                        Console.Write("Введите x1: ");
                        parameters += "," + Console.ReadLine();
                        Console.Write("Введите y1: ");
                        parameters += "," + Console.ReadLine();
                        Console.Write("Введите цвет: ");
                        parameters += "," + Console.ReadLine();
                        break;
                    case "4":
                        command = "draw rectangle";
                        Console.Write("Введите x0: ");
                        parameters = Console.ReadLine();
                        Console.Write("Введите y0: ");
                        parameters += "," + Console.ReadLine();
                        Console.Write("Введите ширину (w): ");
                        parameters += "," + Console.ReadLine();
                        Console.Write("Введите высоту (h): ");
                        parameters += "," + Console.ReadLine();
                        Console.Write("Введите цвет: ");
                        parameters += "," + Console.ReadLine();
                        break;
                    case "5":
                        command = "fill rectangle";
                        Console.Write("Введите x0: ");
                        parameters = Console.ReadLine();
                        Console.Write("Введите y0: ");
                        parameters += "," + Console.ReadLine();
                        Console.Write("Введите ширину (w): ");
                        parameters += "," + Console.ReadLine();
                        Console.Write("Введите высоту (h): ");
                        parameters += "," + Console.ReadLine();
                        Console.Write("Введите цвет: ");
                        parameters += "," + Console.ReadLine();
                        break;
                    case "6":
                        command = "draw ellipse";
                        Console.Write("Введите x0: ");
                        parameters = Console.ReadLine();
                        Console.Write("Введите y0: ");
                        parameters += "," + Console.ReadLine();
                        Console.Write("Введите радиус_x: ");
                        parameters += "," + Console.ReadLine();
                        Console.Write("Введите радиус_y: ");
                        parameters += "," + Console.ReadLine();
                        Console.Write("Введите цвет: ");
                        parameters += "," + Console.ReadLine();
                        break;
                    case "7":
                        command = "fill ellipse";
                        Console.Write("Введите x0: ");
                        parameters = Console.ReadLine();
                        Console.Write("Введите y0: ");
                        parameters += "," + Console.ReadLine();
                        Console.Write("Введите радиус_x: ");
                        parameters += "," + Console.ReadLine();
                        Console.Write("Введите радиус_y: ");
                        parameters += "," + Console.ReadLine();
                        Console.Write("Введите цвет: ");
                        parameters += "," + Console.ReadLine();
                        break;
                    case "8":
                        command = "draw circle";
                        Console.Write("Введите x0: ");
                        parameters = Console.ReadLine();
                        Console.Write("Введите y0: ");
                        parameters += "," + Console.ReadLine();
                        Console.Write("Введите радиус: ");
                        parameters += "," + Console.ReadLine();
                        Console.Write("Введите цвет: ");
                        parameters += "," + Console.ReadLine();
                        break;
                    case "9":
                        command = "fill circle";
                        Console.Write("Введите x0: ");
                        parameters = Console.ReadLine();
                        Console.Write("Введите y0: ");
                        parameters += "," + Console.ReadLine();
                        Console.Write("Введите радиус: ");
                        parameters += "," + Console.ReadLine();
                        Console.Write("Введите цвет: ");
                        parameters += "," + Console.ReadLine();
                        break;
                    case "10":
                        command = "draw text";
                        Console.Write("Введите x0: ");
                        parameters = Console.ReadLine();
                        Console.Write("Введите y0: ");
                        parameters += "," + Console.ReadLine();
                        Console.Write("Введите текст: ");
                        parameters += "," + Console.ReadLine();
                        Console.Write("Введите цвет тексту: ");
                        parameters += "," + Console.ReadLine();
                        break;
                    case "11":
                        command = "draw image";
                        Console.Write("Введите путь к изображению: ");
                        parameters = Console.ReadLine();
                        Console.Write("Введите X-позицию: ");
                        parameters += "," + Console.ReadLine();
                        Console.Write("Введите Y-позицию: ");
                        parameters += "," + Console.ReadLine();
                        break;


                    default:
                        Console.WriteLine("Неверная команда. Пожалуйста, выберите существующую команду.");
                        continue;
                }

                string message = command + ":" + parameters;
                byte[] data = Encoding.UTF8.GetBytes(message);

                try
                {
                    udpClient.Send(data, data.Length, serverEndPoint);
                    Console.WriteLine("Команда отправлена на сервер.");
                }
                catch (Exception e)
                {
                    Console.WriteLine("Ошибка при отправке команды: " + e.Message);
                }
            }
        }
    }
}
