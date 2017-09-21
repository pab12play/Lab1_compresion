using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;

namespace Lab1_compresion
{
    class Program
    {
        
        static void Main(string[] args)
        {
            //args = new string[] { "-c","-f","image.bmp" };
            if (args.Length == 1 && args[0].ToLower().Equals("help") )
            {
                Console.WriteLine("Hola");
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
                    case "-h":
                        if(args[1].ToLower().Equals("-f"))
                        {
                            huffman(args[2]);
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
            nodo prueba1 = new nodo('a');
            prueba(prueba1);
        }


        static void prueba(nodo nodo1)
        {
            using (StreamWriter file = new StreamWriter("prueba.txt"))
            {
                preorder(nodo1, file);
            }
        }

        static void preorder(nodo nodo1, StreamWriter file)
        {
            if (nodo1.esHoja())
            {
                //escribir 0
                file.Write(nodo1.Caracter);
                file.Write(nodo1.Izq);
                file.Write(nodo1.Der);
            }
            else
            {
                //escribir 1
                preorder(nodo1.Izq,file);
                preorder(nodo1.Der, file);
            }
        }



        static double sizeAfter = 0;
        static double sizeBefore = 0;

        static void comprimir(string archivo)
        {
            try
            {
                byte[] bytes = File.ReadAllBytes(archivo);
                sizeBefore = bytes.Length;
                string nombre = Path.GetFileName(archivo);
                List<byte> RLE = new List<byte>(Encoding.ASCII.GetBytes(nombre));
                RLE.Add(Convert.ToByte((char)3));
                byte anterior=bytes[0];
                int contador=1;
                for (int i = 1; i < bytes.Length; i++)
                {
                    if (bytes[i] == anterior && contador < 255)
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
                sizeAfter = RLE.Count();
                mostrar();
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
                mostrar();
                File.WriteAllBytes(file, RLE.ToArray());

            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        static void mostrar()
        {
            double ratio = sizeAfter / sizeBefore;
            double factor = sizeBefore / sizeAfter;
            Console.WriteLine("Tamaño antes: " + sizeBefore);
            Console.WriteLine("Tamaño despues: " + sizeAfter);
            Console.WriteLine("Ratio de compresion: " + Math.Round(ratio, 2));
            Console.WriteLine("Factor de compresion: " + Math.Round(factor, 2));
            double porcentaje = ((sizeBefore - sizeAfter) / sizeBefore) * 100;
            Console.WriteLine("Porcentaje de compresion: " + Math.Round(porcentaje, 2));
        }

        static void huffman(string archivo)
        {
            //string input = "jkljakldjlajlajldkjalkjljijuwijiwjowjojlannadjaijaonannnniwiiiiiiwwwweweeeee";
            arbolHuffman arbol = new arbolHuffman();

            //leer el archivo
            string bytes = File.ReadAllText(archivo);

            arbol.construir(bytes);
            // Compresion
            BitArray bitComprimido = arbol.comprimir(bytes);
            string bitaCadena = devolver(bitComprimido);
            Console.Write("Comprimido: ");

            foreach (bool bit in bitComprimido)
            {
                Console.Write((bit ? 1 : 0) + "");
            }
            File.WriteAllText(Path.GetFileNameWithoutExtension(archivo) + ".comp", bitaCadena);
            Console.WriteLine();
            // Descompresion
            string decoded = arbol.descomprimir(bitComprimido);

            Console.WriteLine("Decomprimido: " + decoded);

            Console.ReadLine();
        }

        static string devolver(BitArray ba)
        {
            string final = "";
            foreach(bool b in ba)
            {
                if(b)
                {
                    final += 1.ToString();
                }
                else
                {
                    final += 0.ToString();
                }
            }
            return final;
        }
        
    }
}
