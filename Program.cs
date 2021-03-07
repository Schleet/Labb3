using System;
using System.IO;
using System.Linq;
using System.Text;


namespace Labb3
{
    class Program
    {
        static byte[] catchPNG = { 137, 80, 78, 71, 13, 10, 26, 10 };
        static byte[] catchBMP = { 66, 77 };
        static void Main (string[] args)
        {
            Console.WriteLine("Skriv in filsökvägen för din fil:");
            string inputFile = Console.ReadLine();
            FileStream file;
            try
            {
                file = new FileStream(inputFile, FileMode.Open);
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("Error!: {0}", e);
                Console.ReadLine();
                return;
            }
            byte[] buffer = new byte[8];
            byte[] sizeBuffer = new byte[8];

            using (file)
            {
                file.Read(buffer, 0, buffer.Length);

                bool bufferIsPNG = buffer.SequenceEqual(catchPNG);
                bool bufferIsBMP = (buffer[0]) == 66 && (buffer[1] == 77);

                if (bufferIsPNG)
                {
                    file.Position = 16;
                    file.Read(sizeBuffer, 0, sizeBuffer.Length);
                    var width = BitConverter.ToInt32(new byte[] { sizeBuffer[3], sizeBuffer[2], sizeBuffer[1], sizeBuffer[0] });
                    var height = BitConverter.ToInt32(new byte[] { sizeBuffer[7], sizeBuffer[6], sizeBuffer[5], sizeBuffer[4] });
                    Console.WriteLine($"Detta är en PNG-fil. Bredd = {width}, Höjd = {height}");
                }
                else if (bufferIsBMP)
                {
                    file.Position = 0x12;
                    file.Read(sizeBuffer, 0, sizeBuffer.Length);
                    var width = BitConverter.ToInt32(new byte[] { sizeBuffer[0], sizeBuffer[1], sizeBuffer[2], sizeBuffer[3] });
                    var height = BitConverter.ToInt32(new byte[] { sizeBuffer[4], sizeBuffer[5], sizeBuffer[6], sizeBuffer[7] });
                    Console.WriteLine($"Detta är en BMP-fil. Bredd = {width}, Höjd = {height}");
                }
                else
                {
                    Console.WriteLine("Detta är varken en .png eller en .bmp -fil!");
                }
            }
            Console.ReadLine();
        }
    }
}
