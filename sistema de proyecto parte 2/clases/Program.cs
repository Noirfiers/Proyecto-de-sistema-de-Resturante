using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proyecto_de_sistema_de_invetario.clases;

namespace Proyecto_de_sistema_de_invetario
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool salir = false;
            while (!salir)
            {
                Console.WriteLine("\n=== Sistema Restaurante Los Pollos Hermanos ===");
                Console.WriteLine("Seleccione el módulo:");
                Console.WriteLine("1. Plato");
                Console.WriteLine("2. Cliente");
                Console.WriteLine("3. Pedido");
                Console.WriteLine("4. Factura");
                Console.WriteLine("5. Detalle Factura");
                Console.WriteLine("6. Reporte de Ventas");
                Console.WriteLine("7. Salir");
                Console.Write("Opción: ");

                string opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        new Plato().MostrarMenu();
                        break;
                    case "2":
                        new Cliente().MostrarMenu();
                        break;
                    case "3":
                        new Pedido().MostrarMenu();
                        break;
                    case "4":
                        new Factura().MostrarMenu();
                        break;
                    case "5":
                        new DetalleFactura().MostrarMenu();
                        break;
                    case "6":
                        new ReporteVentas().MostrarMenu();
                        break;
                    case "7":
                        salir = true;
                        Console.WriteLine("Gracias por usar el sistema.");
                        break;
                    default:
                        Console.WriteLine("Opción inválida. Intente de nuevo.");
                        break;
                }
            }
        }
    }
}
