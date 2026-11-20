using System;
using System.Collections.Generic;
using System.Linq;

namespace Tarea4_3_GrafosDijkstra
{
    public class GrafoPonderado
    {
        private readonly int _numVertices;
        private readonly string[] _nombres;
        private readonly int[,] _matrizAdyacencia;
        private readonly Dictionary<int, List<(int Destino, int Peso)>> _listaAdyacencia;

        public GrafoPonderado(string[] nombres)
        {
            _numVertices = nombres.Length;
            _nombres = nombres;
            _matrizAdyacencia = new int[_numVertices, _numVertices];
            _listaAdyacencia = new Dictionary<int, List<(int, int)>>();

            for (int i = 0; i < _numVertices; i++)
            {
                _listaAdyacencia[i] = new List<(int, int)>();
                for (int j = 0; j < _numVertices; j++)
                {
                    _matrizAdyacencia[i, j] = (i == j) ? 0 : int.MaxValue;
                }
            }
        }

        public void AgregarAristaNoDirigida(int origen, int destino, int peso)
        {
            if (origen < 0 || origen >= _numVertices || destino < 0 || destino >= _numVertices || peso < 0)
                throw new ArgumentException("Índices o peso inválidos.");

            _matrizAdyacencia[origen, destino] = peso;
            _matrizAdyacencia[destino, origen] = peso;

            _listaAdyacencia[origen].Add((destino, peso));
            _listaAdyacencia[destino].Add((origen, peso));
        }

        public void MostrarMatrizAdyacencia()
        {
            Console.WriteLine("\n=== MATRIZ DE ADYACENCIA (Pesos en km | INF = sin conexión directa) ===");
            Console.Write($"{"Ciudad",-16}");
            for (int j = 0; j < _numVertices; j++)
                Console.Write($"{$"[{j}] {_nombres[j][..Math.Min(7, _nombres[j].Length)]}",12}");
            Console.WriteLine();

            for (int i = 0; i < _numVertices; i++)
            {
                Console.Write($"{$"[{i}] {_nombres[i]}",-16}");
                for (int j = 0; j < _numVertices; j++)
                {
                    string celda = _matrizAdyacencia[i, j] == int.MaxValue
                        ? "INF"
                        : _matrizAdyacencia[i, j].ToString();
                    Console.Write($"{celda,12}");
                }
                Console.WriteLine();
            }
        }

        public void MostrarListaAdyacencia()
        {
            Console.WriteLine("\n=== LISTA DE ADYACENCIA ===");
            for (int i = 0; i < _numVertices; i++)
            {
                var conexiones = _listaAdyacencia[i]
                    .Select(arista => $"{_nombres[arista.Destino]} [{arista.Destino}] ({arista.Peso} km)");
                Console.WriteLine($"[{i}] {_nombres[i]} -> {string.Join(" | ", conexiones)}");
            }
        }

        public void EjecutarDijkstraConTraza(int origen)
        {
            if (origen < 0 || origen >= _numVertices)
            {
                Console.WriteLine("Vértice de origen inválido.");
                return;
            }

            int[] dist = new int[_numVertices];
            int[] previo = new int[_numVertices];
            bool[] visitado = new bool[_numVertices];

            for (int i = 0; i < _numVertices; i++)
            {
                dist[i] = int.MaxValue;
                previo[i] = -1;
                visitado[i] = false;
            }

            dist[origen] = 0;

            Console.WriteLine($"\n=== TRAZA DE EJECUCIÓN DE DIJKSTRA (Origen: [{origen}] {_nombres[origen]}) ===");
            ImprimirEstadoTabla("Inicio", -1, dist, previo, visitado);

            for (int iter = 1; iter <= _numVertices; iter++)
            {
                int u = -1;
                int menorDist = int.MaxValue;

                for (int i = 0; i < _numVertices; i++)
                {
                    if (!visitado[i] && dist[i] < menorDist)
                    {
                        menorDist = dist[i];
                        u = i;
                    }
                }

                if (u == -1 || dist[u] == int.MaxValue)
                    break;

                visitado[u] = true;

                foreach (var (v, peso) in _listaAdyacencia[u])
                {
                    if (!visitado[v] && dist[u] != int.MaxValue && dist[u] + peso < dist[v])
                    {
                        dist[v] = dist[u] + peso;
                        previo[v] = u;
                    }
                }

                ImprimirEstadoTabla($"Paso {iter}", u, dist, previo, visitado);
            }

            Console.WriteLine("\n=== RUTAS MÁS CORTAS RESULTANTES ===");
            for (int destino = 0; destino < _numVertices; destino++)
            {
                if (destino == origen) continue;

                if (dist[destino] == int.MaxValue)
                {
                    Console.WriteLine($"-> {_nombres[origen]} a {_nombres[destino]}: Sin ruta disponible.");
                }
                else
                {
                    var ruta = ReconstruirRuta(origen, destino, previo);
                    string rutaFormateada = string.Join(" -> ", ruta.Select(idx => $"{_nombres[idx]} [{idx}]"));
                    Console.WriteLine($"-> {_nombres[origen]} a {_nombres[destino]}: {dist[destino]} km | Ruta: {rutaFormateada}");
                }
            }
        }

        private List<int> ReconstruirRuta(int origen, int destino, int[] previo)
        {
            var ruta = new List<int>();
            for (int actual = destino; actual != -1; actual = previo[actual])
            {
                ruta.Add(actual);
                if (actual == origen) break;
            }
            ruta.Reverse();
            return ruta;
        }

        private void ImprimirEstadoTabla(string etiqueta, int actualU, int[] dist, int[] previo, bool[] visitado)
        {
            string actualTexto = actualU == -1 ? "Ninguno" : $"[{actualU}] {_nombres[actualU]}";
            Console.WriteLine($"\n[{etiqueta}] Nodo seleccionado (u): {actualTexto}");
            Console.WriteLine($"{"Vértice",-22} {"Visitado",-10} {"Dist. Min",-12} {"Previo",-18}");
            for (int i = 0; i < _numVertices; i++)
            {
                string dStr = dist[i] == int.MaxValue ? "INF" : $"{dist[i]} km";
                string pStr = previo[i] == -1 ? "-" : $"[{previo[i]}] {_nombres[previo[i]]}";
                string vStr = visitado[i] ? "Sí" : "No";
                Console.WriteLine($"{$"[{i}] {_nombres[i]}",-22} {vStr,-10} {dStr,-12} {pStr,-18}");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            string[] ciudades =
            {
                "Monterrey",
                "Saltillo",
                "Monclova",
                "Torreón",
                "Piedras Negras",
                "Sabinas"
            };

            var grafo = new GrafoPonderado(ciudades);
            grafo.AgregarAristaNoDirigida(0, 1, 85);   // Monterrey - Saltillo
            grafo.AgregarAristaNoDirigida(0, 5, 210);  // Monterrey - Sabinas
            grafo.AgregarAristaNoDirigida(1, 2, 195);  // Saltillo - Monclova
            grafo.AgregarAristaNoDirigida(1, 3, 270);  // Saltillo - Torreón
            grafo.AgregarAristaNoDirigida(2, 5, 120);  // Monclova - Sabinas
            grafo.AgregarAristaNoDirigida(2, 3, 235);  // Monclova - Torreón
            grafo.AgregarAristaNoDirigida(5, 4, 185);  // Sabinas - Piedras Negras
            grafo.AgregarAristaNoDirigida(2, 4, 245);  // Monclova - Piedras Negras

            int opcion;
            do
            {
                Console.WriteLine("\n=== MODELADO DE GRAFOS Y DIJKSTRA (C#) ===");
                Console.WriteLine("1. Mostrar Matriz de Adyacencia");
                Console.WriteLine("2. Mostrar Lista de Adyacencia");
                Console.WriteLine("3. Ejecutar Dijkstra con traza paso a paso");
                Console.WriteLine("4. Mostrar todo (Matriz, Lista y Traza desde Monterrey)");
                Console.WriteLine("0. Salir");
                Console.Write("Elige una opción: ");

                if (!int.TryParse(Console.ReadLine(), out opcion))
                {
                    opcion = -1;
                    Console.WriteLine("Ingresa un número válido.");
                    continue;
                }

                switch (opcion)
                {
                    case 1:
                        grafo.MostrarMatrizAdyacencia();
                        break;
                    case 2:
                        grafo.MostrarListaAdyacencia();
                        break;
                    case 3:
                        Console.WriteLine("\nCiudades disponibles:");
                        for (int i = 0; i < ciudades.Length; i++)
                            Console.WriteLine($"  {i}: {ciudades[i]}");
                        Console.Write("Elige el índice de la ciudad origen: ");
                        if (int.TryParse(Console.ReadLine(), out int origen))
                            grafo.EjecutarDijkstraConTraza(origen);
                        break;
                    case 4:
                        grafo.MostrarMatrizAdyacencia();
                        grafo.MostrarListaAdyacencia();
                        grafo.EjecutarDijkstraConTraza(0);
                        break;
                    case 0:
                        Console.WriteLine("Saliendo...");
                        break;
                    default:
                        Console.WriteLine("Opción no válida.");
                        break;
                }
            } while (opcion != 0);
        }
    }
}