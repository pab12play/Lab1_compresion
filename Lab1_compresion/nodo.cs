using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1_compresion
{
    class nodo
    {
        nodo izq;
        nodo der;
        char caracter;
        double probabilidad;

        public nodo(nodo izq, nodo der, char caracter, double probabilidad)
        {
            this.izq = izq;
            this.der = der;
            this.caracter = caracter;
            this.probabilidad = probabilidad;
        }

        public nodo(nodo izq, nodo der, double probabilidad)
        {
            this.izq = izq;
            this.der = der;
            this.probabilidad = probabilidad;
        }

        public nodo(char caracter, double probabilidad)
        {
            this.caracter = caracter;
            this.probabilidad = probabilidad;
        }

        public bool esHoja()
        {
            if (izq != null)
            {
                return false;
            }
            else if (der != null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public char Caracter { get => caracter; set => caracter = value; }
        internal nodo Izq { get => izq; set => izq = value; }
        internal nodo Der { get => der; set => der = value; }
        public double Probabilidad { get => probabilidad; set => probabilidad = value; }
    }
}
