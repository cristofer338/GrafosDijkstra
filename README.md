# GrafosDijkstra
# Tarea 4.3: Modelado y recorrido de Grafos (Algoritmo de Dijkstra)

**Alumno:** Cristofer Alejandro Barboza Chávez  
**Matrícula:** I24050398  
**Carrera:** Informática  
**Institución:** Tecnológico Nacional de México  
**Materia:** Estructuras no lineales (Unidad 4)  
**Entorno:** Aplicación de consola en C# (.NET / Visual Studio)

---

## Descripción del proyecto

Este proyecto implementa un **grafo no dirigido ponderado** que modela una red de ciudades y rutas de distribución con distancias en kilómetros (`Monterrey`, `Saltillo`, `Monclova`, `Torreón`, `Piedras Negras` y `Sabinas`). Utiliza el **algoritmo de Dijkstra** para calcular las rutas más cortas desde un vértice de origen, mostrando tanto las estructuras de representación como la traza paso a paso de su ejecución.

---

## Funcionalidades implementadas

* **Modelado de grafo ponderado (`GrafoPonderado`):** Registra vértices (ciudades) y aristas bidireccionales con pesos positivos en kilómetros.
* **Matriz de adyacencia (`MostrarMatrizAdyacencia`):** Muestra los pesos directos entre pares de ciudades (`0` en la diagonal principal e `INF` cuando no existe conexión directa).
* **Lista de adyacencia (`MostrarListaAdyacencia`):** Lista para cada ciudad sus conexiones directas y el peso en kilómetros de cada tramo.
* **Algoritmo de Dijkstra con traza (`EjecutarDijkstraConTraza`):** Imprime el estado del algoritmo en cada paso (nodo `u` seleccionado, nodos visitados, distancia mínima acumulada y vértice previo) y reconstruye la ruta óptima hacia cada destino.
* **Menú interactivo en consola:** Permite consultar la matriz, la lista, ejecutar la traza desde cualquier ciudad origen o mostrar todo el recorrido desde `Monterrey`.

---

## Ejecución en Visual Studio

1. Abre **Visual Studio** y abre el proyecto o solución de consola en C# (`Tarea4_3_GrafosDijkstra`).
2. Verifica que el código del grafo y del menú esté en `Program.cs`.
3. Ejecuta el programa con **Ctrl + F5** (*Iniciar sin depurar*) y selecciona la opción deseada en el menú de consola.
