using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Lab1_compresion
{
    public class arbolHuffman
    {
        private List<nodoH> nodos = new List<nodoH>();
        public nodoH Root { get; set; }
        public Dictionary<char, int> frecuencias = new Dictionary<char, int>();

        public void construir(string ruta)
        {
            for (int i = 0; i < ruta.Length; i++)
            {
                if (!frecuencias.ContainsKey(ruta[i]))
                {
                    frecuencias.Add(ruta[i], 0);
                }

                frecuencias[ruta[i]]++;
            }

            foreach (KeyValuePair<char, int> simbolo in frecuencias)
            {
                nodos.Add(new nodoH() { simbolo = simbolo.Key, Frecuencias = simbolo.Value });
            }

            while (nodos.Count > 1)
            {
                List<nodoH> nodosOrdenados = nodos.OrderBy(node => node.Frecuencias).ToList<nodoH>();

                if (nodosOrdenados.Count >= 2)
                {
                    // primeros dos datos
                    List<nodoH> Ntomados = nodosOrdenados.Take(2).ToList<nodoH>();

                    // crear padre combinando las frecuencias
                    nodoH padre = new nodoH()
                    {
                        simbolo = '*',
                        Frecuencias = Ntomados[0].Frecuencias + Ntomados[1].Frecuencias,
                        izquierda = Ntomados[0],
                        derecha = Ntomados[1]
                    };

                    nodos.Remove(Ntomados[0]);
                    nodos.Remove(Ntomados[1]);
                    nodos.Add(padre);
                }

                this.Root = nodos.FirstOrDefault();

            }

        }

        public BitArray comprimir(string source)
        {
            List<bool> ruta = new List<bool>();

            for (int i = 0; i < source.Length; i++)
            {
                List<bool> comSimbolo = this.Root.atravesar(source[i], new List<bool>());
                ruta.AddRange(comSimbolo);
            }

            BitArray bits = new BitArray(ruta.ToArray());

            return bits;
        }

        public string descomprimir(BitArray bits)
        {
            nodoH actual = this.Root;
            string comprimido = "";

            foreach (bool bit in bits)
            {
                if (bit)
                {
                    if (actual.derecha != null)
                    {
                        actual = actual.derecha;
                    }
                }
                else
                {
                    if (actual.izquierda != null)
                    {
                        actual = actual.izquierda;
                    }
                }

                if (eshoja(actual))
                {
                    comprimido += actual.simbolo;
                    actual = this.Root;
                }
            }

            return comprimido;
        }

        public bool eshoja(nodoH nodo)
        {
            return (nodo.izquierda == null && nodo.derecha == null);
        }

    }
}
