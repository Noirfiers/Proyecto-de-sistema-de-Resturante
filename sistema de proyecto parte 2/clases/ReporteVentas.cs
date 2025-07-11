using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Proyecto_de_sistema_de_invetario.clases
{
    public class ReporteVentas : IMenu
    {
        // Para simplificar, el reporte leerá las facturas y sumará totales.

        private static List<Factura> listaFacturas = new List<Factura>();
        private const string archivoFacturas = "facturas.json";

        public void MostrarMenu()
        {
            Cargar();

            bool salir = false;
            while (!salir)
            {
                Console.WriteLine("\n-- Menú Reporte de Ventas --");
                Console.WriteLine("1. Mostrar todas las facturas");
                Console.WriteLine("2. Mostrar total general de ventas");
                Console.WriteLine("3. Salir");
                Console.Write("Opción: ");
                string opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        ListarFacturas();
                        break;
                    case "2":
                        MostrarTotalGeneral();
                        break;
                    case "3":
                        salir = true;
                        break;
                    default:
                        Console.WriteLine("Opción inválida.");
                        break;
                }
            }
        }

        private void Cargar()
        {
            if (File.Exists(archivoFacturas))
            {
                string json = File.ReadAllText(archivoFacturas);
                listaFacturas = JsonSerializer.Deserialize<List<Factura>>(json) ?? new List<Factura>();
            }
            else
            {
                listaFacturas = new List<Factura>();
            }
        }

        private void ListarFacturas()
        {
            if (listaFacturas.Count == 0)
            {
                Console.WriteLine("No hay facturas.");
                return;
            }
            Console.WriteLine("ID\tIDCliente\tFecha\tSubtotal\tImpuesto\tTotal\tEstado");
            foreach (var f in listaFacturas)
            {
                Console.WriteLine($"{f.Id}\t{f.IdCliente}\t{f.Fecha:yyyy-MM-dd}\tL.{f.Subtotal:F2}\tL.{f.Impuesto:F2}\tL.{f.Total:F2}\t{f.Estado}");
            }
        }

        private void MostrarTotalGeneral()
        {
            decimal totalGeneral = listaFacturas.Sum(f => f.Total);
            Console.WriteLine($"Total general de ventas: L. {totalGeneral:F2}");
        }
    }
}
