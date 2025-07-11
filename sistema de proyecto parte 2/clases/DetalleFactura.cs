using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Proyecto_de_sistema_de_invetario.clases
{
    public class DetalleFactura : EntidadBase, IMenu
    {
        private string idFactura;
        private string idPlato;
        private int cantidad;
        private decimal precioUnitario;
        private decimal subtotal;
        private string estado;
        private decimal impuesto;

        public string IdFactura
        {
            get => idFactura;
            set => idFactura = string.IsNullOrWhiteSpace(value) ? throw new ArgumentException("ID Factura inválido") : value;
        }

        public string IdPlato
        {
            get => idPlato;
            set => idPlato = string.IsNullOrWhiteSpace(value) ? throw new ArgumentException("ID Plato inválido") : value;
        }

        public int Cantidad
        {
            get => cantidad;
            set => cantidad = (value <= 0) ? throw new ArgumentException("Cantidad inválida") : value;
        }

        public decimal PrecioUnitario
        {
            get => precioUnitario;
            set => precioUnitario = (value < 0) ? throw new ArgumentException("Precio inválido") : value;
        }

        public decimal Subtotal
        {
            get => subtotal;
            set => subtotal = (value < 0) ? throw new ArgumentException("Subtotal inválido") : value;
        }

        public new string Estado
        {
            get => estado;
            set => estado = string.IsNullOrWhiteSpace(value) ? "Activo" : value;
        }

        public decimal Impuesto
        {
            get => impuesto;
            set => impuesto = (value < 0) ? throw new ArgumentException("Impuesto inválido") : value;
        }

        private static List<DetalleFactura> listaDetalles = new List<DetalleFactura>();
        private const string archivo = "detalles.json";
        private const int maxRegistros = 100;

        public DetalleFactura() { }

        public DetalleFactura(string id, string idFactura, string idPlato, int cantidad, decimal precioUnitario, decimal subtotal, decimal impuesto)
        {
            Id = id;
            IdFactura = idFactura;
            IdPlato = idPlato;
            Cantidad = cantidad;
            PrecioUnitario = precioUnitario;
            Subtotal = subtotal;
            Impuesto = impuesto;
            Estado = "Activo";
        }

        public void Mostrar()
        {
            Console.WriteLine($"{Id}\t{IdFactura}\t{IdPlato}\t{Cantidad}\tL.{PrecioUnitario:F2}\tL.{Subtotal:F2}\tL.{Impuesto:F2}\t{Estado}");
        }

        public void MostrarMenu()
        {
            Cargar();
            bool salir = false;
            while (!salir)
            {
                Console.WriteLine("\n-- Menú Detalle Factura --");
                Console.WriteLine("1. Agregar Detalle");
                Console.WriteLine("2. Listar Detalles");
                Console.WriteLine("3. Buscar Detalle");
                Console.WriteLine("4. Actualizar Detalle");
                Console.WriteLine("5. Eliminar Detalle");
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
            if (listaDetalles.Count >= maxRegistros)
            {
                Console.WriteLine("Límite de detalles alcanzado.");
                return;
            }
            try
            {
                Console.Write("ID Detalle: ");
                string id = Console.ReadLine();
                if (listaDetalles.Any(d => d.Id == id))
                {
                    Console.WriteLine("ID ya existe.");
                    return;
                }
                Console.Write("ID Factura: ");
                string idFactura = Console.ReadLine();
                Console.Write("ID Plato: ");
                string idPlato = Console.ReadLine();
                Console.Write("Cantidad: ");
                int cantidad = int.Parse(Console.ReadLine());
                Console.Write("Precio Unitario: ");
                decimal precio = decimal.Parse(Console.ReadLine());
                Console.Write("Subtotal: ");
                decimal subtotal = decimal.Parse(Console.ReadLine());
                Console.Write("Impuesto: ");
                decimal impuesto = decimal.Parse(Console.ReadLine());

                DetalleFactura nuevo = new DetalleFactura(id, idFactura, idPlato, cantidad, precio, subtotal, impuesto);
                listaDetalles.Add(nuevo);

                Console.WriteLine("Detalle agregado.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        private void Listar()
        {
            if (listaDetalles.Count == 0)
            {
                Console.WriteLine("No hay detalles.");
                return;
            }
            Console.WriteLine("ID\tIDFactura\tIDPlato\tCantidad\tPrecioUnit\tSubtotal\tImpuesto\tEstado");
            foreach (var d in listaDetalles)
            {
                d.Mostrar();
            }
        }

        private void Buscar()
        {
            Console.Write("ID a buscar: ");
            string id = Console.ReadLine();
            var detalle = listaDetalles.FirstOrDefault(d => d.Id == id);
            if (detalle != null)
                detalle.Mostrar();
            else
                Console.WriteLine("Detalle no encontrado.");
        }

        private void Actualizar()
        {
            Console.Write("ID a actualizar: ");
            string id = Console.ReadLine();
            var detalle = listaDetalles.FirstOrDefault(d => d.Id == id);
            if (detalle == null)
            {
                Console.WriteLine("Detalle no encontrado.");
                return;
            }
            try
            {
                Console.Write("Nuevo ID Factura: ");
                detalle.IdFactura = Console.ReadLine();
                Console.Write("Nuevo ID Plato: ");
                detalle.IdPlato = Console.ReadLine();
                Console.Write("Nueva Cantidad: ");
                detalle.Cantidad = int.Parse(Console.ReadLine());
                Console.Write("Nuevo Precio Unitario: ");
                detalle.PrecioUnitario = decimal.Parse(Console.ReadLine());
                Console.Write("Nuevo Subtotal: ");
                detalle.Subtotal = decimal.Parse(Console.ReadLine());
                Console.Write("Nuevo Impuesto: ");
                detalle.Impuesto = decimal.Parse(Console.ReadLine());

                Console.WriteLine("Detalle actualizado.");
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
            var detalle = listaDetalles.FirstOrDefault(d => d.Id == id);
            if (detalle != null)
            {
                listaDetalles.Remove(detalle);
                Console.WriteLine("Detalle eliminado.");
            }
            else
                Console.WriteLine("Detalle no encontrado.");
        }

        private void Guardar()
        {
            string json = JsonSerializer.Serialize(listaDetalles, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(archivo, json);
            Console.WriteLine("Datos guardados.");
        }

        private void Cargar()
        {
            if (File.Exists(archivo))
            {
                string json = File.ReadAllText(archivo);
                listaDetalles = JsonSerializer.Deserialize<List<DetalleFactura>>(json) ?? new List<DetalleFactura>();
            }
            else
            {
                listaDetalles = new List<DetalleFactura>();
            }
        }
    }
}
