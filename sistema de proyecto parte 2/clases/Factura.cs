using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Proyecto_de_sistema_de_invetario.clases
{
    public class Factura : EntidadBase, IMenu
    {
        public string IdCliente { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Impuesto { get; set; }
        public decimal Total { get; set; }
        public new string Estado { get; set; }

        private static List<Factura> listaFacturas = new List<Factura>();
        private const string archivo = "facturas.json";
        private const int maxRegistros = 100;

        public Factura() { }

        public Factura(string id, string idCliente, decimal subtotal, decimal impuesto, decimal total)
        {
            Id = id;
            IdCliente = idCliente;
            Fecha = DateTime.Now;
            Subtotal = subtotal;
            Impuesto = impuesto;
            Total = total;
            Estado = "Activa";
        }

        public void Mostrar()
        {
            Console.WriteLine($"{Id}\t{IdCliente}\t{Fecha:yyyy-MM-dd}\tL.{Subtotal:F2}\tL.{Impuesto:F2}\tL.{Total:F2}\t{Estado}");
        }

        public void MostrarMenu()
        {
            Cargar();
            bool salir = false;
            while (!salir)
            {
                Console.WriteLine("\n-- Menú Facturas --");
                Console.WriteLine("1. Generar Factura por Cliente");
                Console.WriteLine("2. Listar Facturas");
                Console.WriteLine("3. Guardar y Salir");
                Console.Write("Opción: ");
                string op = Console.ReadLine();
                switch (op)
                {
                    case "1": GenerarFactura(); break;
                    case "2": Listar(); break;
                    case "3": Guardar(); salir = true; break;
                    default: Console.WriteLine("Opción inválida."); break;
                }
            }
        }

        private void GenerarFactura()
        {
            if (listaFacturas.Count >= maxRegistros)
            {
                Console.WriteLine("Límite alcanzado.");
                return;
            }

            Console.Write("ID Cliente: ");
            string idCliente = Console.ReadLine();

            var pedidosCliente = Pedido.ObtenerPedidosPorCliente(idCliente);

            if (pedidosCliente.Count == 0)
            {
                Console.WriteLine("El cliente no tiene pedidos registrados.");
                return;
            }

            decimal subtotal = pedidosCliente.Sum(p => p.Total);
            decimal impuesto = subtotal * 0.15M;
            decimal total = subtotal + impuesto;

            string id = "F" + (listaFacturas.Count + 1).ToString("000");

            Factura nueva = new Factura(id, idCliente, subtotal, impuesto, total);
            listaFacturas.Add(nueva);

            Console.WriteLine("Factura generada:");
            nueva.Mostrar();
        }

        private void Listar()
        {
            if (listaFacturas.Count == 0)
            {
                Console.WriteLine("No hay facturas.");
                return;
            }

            Console.WriteLine("ID\tCliente\tFecha\tSubtotal\tImpuesto\tTotal\tEstado");
            foreach (var f in listaFacturas)
                f.Mostrar();
        }

        private void Guardar()
        {
            string json = JsonSerializer.Serialize(listaFacturas, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(archivo, json);
            Console.WriteLine("Datos guardados.");
        }

        private void Cargar()
        {
            if (File.Exists(archivo))
            {
                string json = File.ReadAllText(archivo);
                listaFacturas = JsonSerializer.Deserialize<List<Factura>>(json) ?? new List<Factura>();
            }
        }
    }
}
