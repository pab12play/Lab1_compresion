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
            if (args.Length == 1 && args[0].ToLower().Equals("help") )
            {
                Console.WriteLine("Hola");
                Console.WriteLine("Uso: .\\Lab1_compresion.exe [-d] [-c] [-hd] [-hc] [-f <archivo>]");
                Console.WriteLine("");
                Console.WriteLine("\t-d\tDescomprime un archivo");
                Console.WriteLine("\t-c\tComprime un archivo");
                Console.WriteLine("\t-hd\tDescomprime un archivo usando huffman");
                Console.WriteLine("\t-hc\tComprime un archivo usando huffman");
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
                    case "-hc":
                        if(args[1].ToLower().Equals("-f"))
                        {
                            huffman_compresion(args[2]);
                        }
                        else
                        {
                            Console.WriteLine("Por favor ingrese una opción correcta. Consulte la opción 'help' para ayuda");
                        }
                        break;
                    case "-hd":
                        if (args[1].ToLower().Equals("-f"))
                        {
                            huffman_descompresion(args[2]);
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
        
        static void preorder(nodoH nodo1,ref string file)
        {
            if (nodo1.esHoja())
            {
                file = file + "0" + nodo1.simbolo + nodo1.Frecuencias+(char)4;
            }
            else
            {
                file = file + "1" + nodo1.simbolo + nodo1.Frecuencias+(char)4;
                preorder(nodo1.izquierda,ref file);
                preorder(nodo1.derecha,ref file);
            }
        }
        
        static void huffman_compresion(string archivo)
        {
            arbolHuffman arbol = new arbolHuffman();

            //leer el archivo
            string bytes = File.ReadAllText(archivo);
            sizeBefore = bytes.Length;
            arbol.construir(bytes);
            // Compresion
            BitArray bitComprimido = arbol.comprimir(bytes);
            string bitaCadena = devolver(bitComprimido);

            List<byte> escribir = Encoding.ASCII.GetBytes(Path.GetFileName(archivo)+(char)3+bitComprimido.Length+(char)3).ToList();
            string arbol1 = "";
            preorder(arbol.Root,ref arbol1);
            escribir.AddRange(Encoding.ASCII.GetBytes(arbol1+(char)5).ToList());
            List<byte> huffman = convertir_bytes(bitaCadena);
            escribir.AddRange(huffman);
            sizeAfter = huffman.Count;

            mostrar();
            File.WriteAllBytes(Path.GetFileNameWithoutExtension(archivo) + ".comp", escribir.ToArray());
        }

        static void huffman_descompresion(string archivo)
        {
            byte[] bytes = File.ReadAllBytes(archivo);
            int contador = 0;
            string nombre = "";
            string size = "";
            List<string> arbolString = new List<string>();
            List<byte> mensaje = new List<byte>();
            for(int i = 0; i<bytes.Length; i++)
            {
                switch (contador)
                {
                    case 0:
                        if (bytes[i] == (char)3)
                        {
                            contador++;
                        }
                        else
                        {
                            nombre = nombre + Convert.ToChar(bytes[i]);
                        }
                        break;
                    case 1:
                        if (bytes[i] == (char)3)
                        {
                            contador++;
                        }
                        else
                        {
                            size = size + Convert.ToChar(bytes[i]);
                        }
                        break;
                    case 2:
                        if (bytes[i] == (char)5)
                        {
                            contador++;
                        }
                        else
                        {
                            string nodo = ""+ Convert.ToChar(bytes[i]) + Convert.ToChar(bytes[++i]);
                            i++;
                            while (bytes[i] != (char)4)
                            {
                                nodo = nodo + Convert.ToChar(bytes[i]);
                                i++;
                            }
                            arbolString.Add(nodo);
                        }
                        break;
                    case 3:
                        mensaje.Add(bytes[i]);
                        break;
                }
            }
            arbolHuffman arbol = new arbolHuffman();
            arbol.Root = arbol.cargar(arbolString);
            
            // Descompresion
            string decoded = arbol.descomprimir(convertir_string(mensaje.ToArray(),int.Parse(size)));

            File.WriteAllText(nombre, decoded);
        }

        static BitArray convertir_string(byte[] bt,int size)
        {
            List<bool> bit = new List<bool>();
            foreach (byte b in bt)
            {
                string byte1 = Convert.ToString(b, 2).PadLeft(8, '0');
                foreach(char c in byte1)
                {
                    if (c == '0')
                    {
                        bit.Add(false);
                    }
                    else
                    {
                        bit.Add(true);
                    }
                }
            }
            int cantidad = bit.Count-size;
            bit.RemoveRange(size, cantidad);
            return new BitArray(bit.ToArray());
        }

        static List<byte> convertir_bytes(string cadena)
        {
            int contador = 0;
            List<byte> huffman = new List<byte>();
            string byte1 = "";
            foreach (char caracter in cadena)
            {
                if (contador < 8)
                {
                    byte1 = byte1 + caracter;
                    contador++;
                }
                else
                {
                    huffman.Add(Convert.ToByte(byte1, 2));
                    byte1 = caracter.ToString();
                    contador = 1;
                }
            }
            if (!byte1.Equals(""))
            {
                for (int i = 8 - byte1.Length; i > 0; i--)
                {
                    byte1 = byte1 + "0";
                }
            }
            huffman.Add(Convert.ToByte(byte1, 2));
            return huffman;
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
