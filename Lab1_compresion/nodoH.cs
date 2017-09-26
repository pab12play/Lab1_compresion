using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1_compresion
{
    public class nodoH
    {
        public char simbolo { get; set; }
        public int Frecuencias { get; set; }
        public nodoH derecha { get; set; }
        public nodoH izquierda { get; set; }

        public bool esHoja()
        {
            if (izquierda != null)
            {
                return false;
            }
            else if (derecha != null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public List<bool> atravesar(char symbol, List<bool> data)
        {
            // hoja
            if (derecha == null && izquierda == null)
            {
                if (symbol.Equals(this.simbolo))
                {
                    return data;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                List<bool> left = null;
                List<bool> right = null;

                if (izquierda != null)
                {
                    List<bool> leftPath = new List<bool>();
                    leftPath.AddRange(data);
                    leftPath.Add(false);

                    left = izquierda.atravesar(symbol, leftPath);
                }

                if (derecha != null)
                {
                    List<bool> rightPath = new List<bool>();
                    rightPath.AddRange(data);
                    rightPath.Add(true);
                    right = derecha.atravesar(symbol, rightPath);
                }

                if (left != null)
                {
                    return left;
                }
                else
                {
                    return right;
                }
            }
        }
    }
}

