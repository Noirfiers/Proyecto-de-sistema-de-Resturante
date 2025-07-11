using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Proyecto_de_sistema_de_invetario.clases
{
    public class Plato : EntidadBase, IMenu
    {
        private string nombre;
        private decimal precio;
        private string categoria;
        private int cantidadDisponible;
        private string descripcion;
        private int tiempoPreparacion;

        public string Nombre
        {
            get => nombre;
            set => nombre = string.IsNullOrWhiteSpace(value) ? throw new ArgumentException("Nombre inválido") : value;
        }

        public decimal Precio
        {
            get => precio;
            set => precio = (value <= 0) ? throw new ArgumentException("Precio debe ser mayor a cero") : value;
        }

        public string Categoria
        {
            get => categoria;
            set => categoria = string.IsNullOrWhiteSpace(value) ? throw new ArgumentException("Categoría inválida") : value;
        }

        public int CantidadDisponible
        {
            get => cantidadDisponible;
            set => cantidadDisponible = (value < 0) ? throw new ArgumentException("Cantidad inválida") : value;
        }

        public string Descripcion
        {
            get => descripcion;
            set => descripcion = string.IsNullOrWhiteSpace(value) ? "-" : value;
        }

        public int TiempoPreparacion
        {
            get => tiempoPreparacion;
            set => tiempoPreparacion = (value <= 0) ? throw new ArgumentException("Tiempo inválido") : value;
        }

        private static List<Plato> listaPlatos = new List<Plato>();
        private const string archivo = "platos.json";
        private const int maxRegistros = 100;

        public Plato() { }

        public Plato(string id, string nombre, decimal precio, string categoria, int cantidadDisponible, string descripcion, int tiempoPreparacion)
        {
            Id = id;
            Nombre = nombre;
            Precio = precio;
            Categoria = categoria;
            CantidadDisponible = cantidadDisponible;
            Descripcion = descripcion;
            TiempoPreparacion = tiempoPreparacion;
            Estado = "Activo";
        }

        public void Mostrar()
        {
            Console.WriteLine($"{Id}\t{Nombre}\tL.{Precio:F2}\t{Categoria}\t{CantidadDisponible}\t{Descripcion}\t{TiempoPreparacion} min\t{Estado}");
        }

        public void MostrarMenu()
        {
            Cargar();

            bool salir = false;
            while (!salir)
            {
                Console.WriteLine("\n-- Menú Platos --");
                Console.WriteLine("1. Agregar Plato");
                Console.WriteLine("2. Listar Platos");
                Console.WriteLine("3. Buscar Plato");
                Console.WriteLine("4. Actualizar Plato");
                Console.WriteLine("5. Eliminar Plato");
                Console.WriteLine("6. Guardar y Salir");
                Console.Write("Opción: ");

                string opcion = Console.ReadLine();
                switch (opcion)
                {
                    case "1": Agregar(); break;
                    case "2": Listar(); break;
                    case "3": Buscar(); break;
                    case "4": Actualizar(); break;
                    case "5": Eliminar(); break;
                    case "6": Guardar(); salir = true; break;
                    default: Console.WriteLine("Opción inválida."); break;
                }
            }
        }

        private void Agregar()
        {
            if (listaPlatos.Count >= maxRegistros)
            {
                Console.WriteLine("Límite de platos alcanzado.");
                return;
            }
            try
            {
                Console.Write("ID: ");
                string id = Console.ReadLine();
                if (listaPlatos.Any(p => p.Id == id))
                {
                    Console.WriteLine("ID ya existe.");
                    return;
                }

                Console.Write("Nombre: ");
                string nombre = Console.ReadLine();

                Console.Write("Precio: ");
                decimal precio = decimal.Parse(Console.ReadLine());

                Console.Write("Categoría: ");
                string categoria = Console.ReadLine();

                Console.Write("Cantidad Disponible: ");
                int cantidad = int.Parse(Console.ReadLine());

                Console.Write("Descripción: ");
                string descripcion = Console.ReadLine();

                Console.Write("Tiempo de Preparación (minutos): ");
                int tiempo = int.Parse(Console.ReadLine());

                Plato nuevo = new Plato(id, nombre, precio, categoria, cantidad, descripcion, tiempo);
                listaPlatos.Add(nuevo);

                Console.WriteLine("Plato agregado.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        private void Listar()
        {
            if (listaPlatos.Count == 0)
            {
                Console.WriteLine("No hay platos.");
                return;
            }
            Console.WriteLine("ID\tNombre\tPrecio\tCategoría\tCantidad\tDescripción\tTiempoPrep\tEstado");
            foreach (var p in listaPlatos)
            {
                p.Mostrar();
            }
        }

        private void Buscar()
        {
            Console.Write("ID a buscar: ");
            string id = Console.ReadLine();
            var plato = listaPlatos.FirstOrDefault(p => p.Id == id);
            if (plato != null)
                plato.Mostrar();
            else
                Console.WriteLine("Plato no encontrado.");
        }

        private void Actualizar()
        {
            Console.Write("ID a actualizar: ");
            string id = Console.ReadLine();
            var plato = listaPlatos.FirstOrDefault(p => p.Id == id);
            if (plato == null)
            {
                Console.WriteLine("Plato no encontrado.");
                return;
            }
            try
            {
                Console.Write("Nuevo Nombre: ");
                plato.Nombre = Console.ReadLine();

                Console.Write("Nuevo Precio: ");
                plato.Precio = decimal.Parse(Console.ReadLine());

                Console.Write("Nueva Categoría: ");
                plato.Categoria = Console.ReadLine();

                Console.Write("Nueva Cantidad Disponible: ");
                plato.CantidadDisponible = int.Parse(Console.ReadLine());

                Console.Write("Nueva Descripción: ");
                plato.Descripcion = Console.ReadLine();

                Console.Write("Nuevo Tiempo de Preparación (minutos): ");
                plato.TiempoPreparacion = int.Parse(Console.ReadLine());

                Console.WriteLine("Plato actualizado.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        private void Eliminar()
        {
            Console.Write("ID a eliminar: ");
            string id = Console.ReadLine();
            var plato = listaPlatos.FirstOrDefault(p => p.Id == id);
            if (plato != null)
            {
                listaPlatos.Remove(plato);
                Console.WriteLine("Plato eliminado.");
            }
            else
                Console.WriteLine("Plato no encontrado.");
        }

        private void Guardar()
        {
            string json = JsonSerializer.Serialize(listaPlatos, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(archivo, json);
            Console.WriteLine("Datos guardados.");
        }

        private void Cargar()
        {
            if (File.Exists(archivo))
            {
                string json = File.ReadAllText(archivo);
                listaPlatos = JsonSerializer.Deserialize<List<Plato>>(json) ?? new List<Plato>();
            }
            else
            {
                listaPlatos = new List<Plato>();
            }
        }
    }
}
