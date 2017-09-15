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

        public nodo(nodo izq, nodo der, char caracter)
        {
            this.izq = izq;
            this.der = der;
            this.caracter = caracter;
        }

        public nodo(nodo izq, nodo der)
        {
            this.izq = izq;
            this.der = der;
        }

        public char Caracter { get => caracter; set => caracter = value; }
        internal nodo Izq { get => izq; set => izq = value; }
        internal nodo Der { get => der; set => der = value; }
    }
}
