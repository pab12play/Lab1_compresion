using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Lab1_compresion
{
    class Program
    {
        static void Main(string[] args)
        {
            args = new string[] { "-c","-f","test.txt" };
            if (args.Length == 1 && args[0].ToLower().Equals("help") )
            {
                Console.WriteLine("Uso: .\\Lab1_compresion.exe [-d] [-c] [-f <archivo>]");
                Console.WriteLine("");
                Console.WriteLine("\t-d\tDescomprime un archivo");
                Console.WriteLine("\t-c\tComprime un archivo");
                Console.WriteLine("\t-f\tEspecifica la ruta y nombre del archivo");
            }
            else if (args.Length == 3)
            {
                switch (args[0].ToLower())
                {
                    case "-d":
                        if (args[1].ToLower().Equals("-f"))
                        {
                            descomprimir(args[2]);
                        }
                        else
                        {
                            Console.WriteLine("Por favor ingrese una opción correcta. Consulte la opción 'help' para ayuda");
                        }
                        break;
                    case "-c":
                        if (args[1].ToLower().Equals("-f"))
                        {
                            comprimir(args[2]);
                        }
                        else
                        {
                            Console.WriteLine("Por favor ingrese una opción correcta. Consulte la opción 'help' para ayuda");
                        }
                        break;
                    default:
                        Console.WriteLine("Por favor ingrese una opción correcta. Consulte la opción 'help' para ayuda");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Por favor ingrese una opción correcta. Consulte la opción 'help' para ayuda");
            }
        }

        static void comprimir(string archivo)
        {
            try
            {
                byte[] bytes = File.ReadAllBytes(archivo);
                string nombre = Path.GetFileName(archivo);
                List<byte> RLE = new List<byte>(Encoding.ASCII.GetBytes(nombre));
                RLE.Add(Convert.ToByte((char)3));
                byte anterior=bytes[0];
                int contador=1;
                for (int i = 1; i < bytes.Length; i++)
                {
                    if (bytes[i] == anterior && contador < 256)
                    {
                        contador++;
                    }
                    else
                    {
                        RLE.Add(anterior);
                        RLE.Add(Convert.ToByte(contador));
                        contador = 1;
                        anterior = bytes[i];
                    }
                }
                RLE.Add(anterior);
                RLE.Add(Convert.ToByte(contador));
                File.WriteAllBytes(Path.GetFileNameWithoutExtension(archivo)+".comp", RLE.ToArray());
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        static void descomprimir(string archivo)
        {
            try
            {
                byte[] bytes = File.ReadAllBytes(archivo);
                List<byte> RLE = new List<byte>();
                string file="";
                int i = 0;
                foreach (byte b in bytes)
                {
                    i++;
                    if (b == (char)3)
                    {
                        break;
                    }
                    else
                    {
                        file += Convert.ToChar(b);
                    }
                }
                for (; i < bytes.Length; i++)
                {
                    byte caracter = bytes[i];
                    int contador = Convert.ToInt32(bytes[++i]);
                    for (int j = 0; j < contador; j++)
                    {
                        RLE.Add(caracter);
                    }
                }
                File.WriteAllBytes(file, RLE.ToArray());
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
