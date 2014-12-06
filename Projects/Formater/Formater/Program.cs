using System;
using System.IO;
using System.Text;

namespace Formater
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string path;
            if (args.Length == 0)
            {
                path = Environment.CurrentDirectory;
            }
            else if (args.Length == 1 && Directory.Exists(args[0]))
            {
                path = args[0];
            }
            else
            {
                Console.WriteLine("Please give a valid diractory path");
                return;
            }

            var files = Directory.GetFiles(path, "*.srt");
            var length = files.Length;

            Console.WriteLine("Converting {0} files:" , length);
            for (int index = 0; index < files.Length; index++)
            {
                var filePath = files[index];
                var fileName = Path.GetFileName(filePath);

                try
                {
                    var sr = new StreamReader(filePath, Encoding.Default, true);

                    var readToEnd = sr.ReadToEnd();

                    sr.Close();

                    var sw = new StreamWriter(filePath, false, Encoding.UTF8);

                    sw.WriteLine(readToEnd);

                    sw.Close();

                    Console.WriteLine("{0}/{1} - {2}", index + 1, length, fileName);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error converting file {0} - {1}", fileName, e.Message);
                }
            }

            Console.WriteLine("Conversion ended");

            Console.ReadKey();
        }
    }
}