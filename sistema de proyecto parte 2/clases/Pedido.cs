using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Proyecto_de_sistema_de_invetario.clases
{
    public class Pedido : EntidadBase, IMenu
    {
        public string IdCliente { get; set; }
        public string IdPlato { get; set; }
        public int Cantidad { get; set; }
        public DateTime FechaPedido { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Total => Cantidad * PrecioUnitario;
        public new string Estado { get; set; }

        private static List<Pedido> listaPedidos = new List<Pedido>();
        private const string archivo = "pedidos.json";
        private const int maxRegistros = 100;

        public Pedido() { }

        public Pedido(string id, string idCliente, string idPlato, int cantidad, decimal precio)
        {
            Id = id;
            IdCliente = idCliente;
            IdPlato = idPlato;
            Cantidad = cantidad;
            PrecioUnitario = precio;
            FechaPedido = DateTime.Now;
            Estado = "Pendiente";
        }

        public void Mostrar()
        {
            Console.WriteLine($"{Id}\t{IdCliente}\t{IdPlato}\t{Cantidad}\tL.{PrecioUnitario:F2}\tL.{Total:F2}\t{FechaPedido:yyyy-MM-dd}\t{Estado}");
        }

        public void MostrarMenu()
        {
            Cargar();
            bool salir = false;
            while (!salir)
            {
                Console.WriteLine("\n-- Menú Pedidos --");
                Console.WriteLine("1. Agregar Pedido");
                Console.WriteLine("2. Listar Pedidos");
                Console.WriteLine("3. Buscar Pedido");
                Console.WriteLine("4. Eliminar Pedido");
                Console.WriteLine("5. Guardar y Salir");
                Console.Write("Opción: ");
                string opcion = Console.ReadLine();
                switch (opcion)
                {
                    case "1": Agregar(); break;
                    case "2": Listar(); break;
                    case "3": Buscar(); break;
                    case "4": Eliminar(); break;
                    case "5": Guardar(); salir = true; break;
                    default: Console.WriteLine("Opción inválida."); break;
                }
            }
        }

        private void Agregar()
        {
            if (listaPedidos.Count >= maxRegistros)
            {
                Console.WriteLine("Límite alcanzado.");
                return;
            }

            try
            {
                Console.Write("ID Pedido: ");
                string id = Console.ReadLine();
                if (listaPedidos.Any(p => p.Id == id))
                {
                    Console.WriteLine("ID ya existe.");
                    return;
                }

                Console.Write("ID Cliente: ");
                string idCliente = Console.ReadLine();
                Console.Write("ID Plato: ");
                string idPlato = Console.ReadLine();
                Console.Write("Cantidad: ");
                int cantidad = int.Parse(Console.ReadLine());
                Console.Write("Precio Unitario: ");
                decimal precio = decimal.Parse(Console.ReadLine());

                Pedido nuevo = new Pedido(id, idCliente, idPlato, cantidad, precio);
                listaPedidos.Add(nuevo);
                Console.WriteLine("Pedido agregado.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        private void Listar()
        {
            if (listaPedidos.Count == 0)
            {
                Console.WriteLine("No hay pedidos.");
                return;
            }

            Console.WriteLine("ID\tCliente\tPlato\tCant\tPrecio\tTotal\tFecha\tEstado");
            foreach (var p in listaPedidos)
                p.Mostrar();
        }

        private void Buscar()
        {
            Console.Write("ID a buscar: ");
            string id = Console.ReadLine();
            var pedidoEncontrado = listaPedidos.FirstOrDefault(p => p.Id == id);
            if (pedidoEncontrado != null)
                pedidoEncontrado.Mostrar();
            else
                Console.WriteLine("Pedido no encontrado.");
        }

        private void Eliminar()
        {
            Console.Write("ID a eliminar: ");
            string id = Console.ReadLine();
            var pedidoEliminar = listaPedidos.FirstOrDefault(p => p.Id == id);
            if (pedidoEliminar != null)
            {
                listaPedidos.Remove(pedidoEliminar);
                Console.WriteLine("Pedido eliminado.");
            }
            else Console.WriteLine("No encontrado.");
        }

        private void Guardar()
        {
            string json = JsonSerializer.Serialize(listaPedidos, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(archivo, json);
            Console.WriteLine("Datos guardados.");
        }

        private void Cargar()
        {
            if (File.Exists(archivo))
            {
                string json = File.ReadAllText(archivo);
                listaPedidos = JsonSerializer.Deserialize<List<Pedido>>(json) ?? new List<Pedido>();
            }
        }

        public static List<Pedido> ObtenerPedidosPorCliente(string idCliente)
        {
            return listaPedidos.Where(p => p.IdCliente == idCliente).ToList();
        }
    }
}
