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
            args = new string[] { "-d","-f","test.comp" };
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
                byte[] bytes = System.IO.File.ReadAllBytes(archivo);
                List<byte> RLE = new List<byte>();
                byte anterior=bytes[0];
                int contador=1;
                for (int i = 1; i < bytes.Length; i++)
                {
                    if (bytes[i] == anterior)
                    {
                        if (contador >= 256)
                        {
                            RLE.Add(anterior);
                            RLE.Add(Convert.ToByte(contador));
                            contador = 1;
                            anterior = bytes[i];
                        }
                        else
                        {
                            contador++;
                        }
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
                System.IO.File.WriteAllBytes("test.comp", RLE.ToArray());
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
                byte[] bytes = System.IO.File.ReadAllBytes(archivo);
                List<byte> RLE = new List<byte>();
                for (int i = 0; i < bytes.Length; i++)
                {
                    byte caracter = bytes[i];
                    int contador = Convert.ToInt32(bytes[++i]);
                    for(int j = 0; j < contador; j++)
                    {
                        RLE.Add(caracter);
                    }
                }
                System.IO.File.WriteAllBytes("test.txt", RLE.ToArray());
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
