using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Proyecto_de_sistema_de_invetario.clases
{
    public class Cliente : EntidadBase, IMenu
    {
        private string nombre;
        private string telefono;
        private string direccion;
        private string correo;
        private string identidad;
        private int visitas;

        public string Nombre
        {
            get => nombre;
            set => nombre = string.IsNullOrWhiteSpace(value) ? throw new ArgumentException("Nombre inválido") : value;
        }

        public string Telefono
        {
            get => telefono;
            set => telefono = string.IsNullOrWhiteSpace(value) ? throw new ArgumentException("Teléfono inválido") : value;
        }

        public string Direccion
        {
            get => direccion;
            set => direccion = string.IsNullOrWhiteSpace(value) ? throw new ArgumentException("Dirección inválida") : value;
        }

        public string Correo
        {
            get => correo;
            set => correo = (string.IsNullOrWhiteSpace(value) || !value.Contains("@")) ? throw new ArgumentException("Correo inválido") : value;
        }

        public string Identidad
        {
            get => identidad;
            set => identidad = string.IsNullOrWhiteSpace(value) ? throw new ArgumentException("Identidad inválida") : value;
        }

        public int Visitas
        {
            get => visitas;
            set => visitas = (value < 0) ? throw new ArgumentException("Visitas inválidas") : value;
        }

        public DateTime FechaRegistro { get; set; }

        private static List<Cliente> listaClientes = new List<Cliente>();
        private const string archivo = "clientes.json";
        private const int maxRegistros = 100;

        public Cliente() { }

        public Cliente(string id, string nombre, string telefono, string direccion, string correo, string identidad)
        {
            Id = id;
            Nombre = nombre;
            Telefono = telefono;
            Direccion = direccion;
            Correo = correo;
            Identidad = identidad;
            Visitas = 1;
            FechaRegistro = DateTime.Now;
            Estado = "Activo";
        }

        public void Mostrar()
        {
            Console.WriteLine($"{Id}\t{Nombre}\t{Telefono}\t{Correo}\t{Visitas}\t{Estado}");
        }

        public void MostrarMenu()
        {
            Cargar();

            bool salir = false;
            while (!salir)
            {
                Console.WriteLine("\n-- Menú Clientes --");
                Console.WriteLine("1. Agregar Cliente");
                Console.WriteLine("2. Listar Clientes");
                Console.WriteLine("3. Buscar Cliente");
                Console.WriteLine("4. Actualizar Cliente");
                Console.WriteLine("5. Eliminar Cliente");
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
            if (listaClientes.Count >= maxRegistros)
            {
                Console.WriteLine("Límite de clientes alcanzado.");
                return;
            }
            try
            {
                Console.Write("ID: ");
                string id = Console.ReadLine();
                if (listaClientes.Any(c => c.Id == id))
                {
                    Console.WriteLine("ID ya existe.");
                    return;
                }

                Console.Write("Nombre: ");
                string nombre = Console.ReadLine();

                Console.Write("Teléfono: ");
                string telefono = Console.ReadLine();

                Console.Write("Dirección: ");
                string direccion = Console.ReadLine();

                Console.Write("Correo: ");
                string correo = Console.ReadLine();

                Console.Write("Identidad: ");
                string identidad = Console.ReadLine();

                Cliente nuevo = new Cliente(id, nombre, telefono, direccion, correo, identidad);
                listaClientes.Add(nuevo);

                Console.WriteLine("Cliente agregado.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        private void Listar()
        {
            if (listaClientes.Count == 0)
            {
                Console.WriteLine("No hay clientes.");
                return;
            }
            Console.WriteLine("ID\tNombre\tTeléfono\tCorreo\tVisitas\tEstado");
            foreach (var c in listaClientes)
            {
                c.Mostrar();
            }
        }

        private void Buscar()
        {
            Console.Write("ID a buscar: ");
            string id = Console.ReadLine();
            var cliente = listaClientes.FirstOrDefault(c => c.Id == id);
            if (cliente != null)
                cliente.Mostrar();
            else
                Console.WriteLine("Cliente no encontrado.");
        }

        private void Actualizar()
        {
            Console.Write("ID a actualizar: ");
            string id = Console.ReadLine();
            var cliente = listaClientes.FirstOrDefault(c => c.Id == id);
            if (cliente == null)
            {
                Console.WriteLine("Cliente no encontrado.");
                return;
            }
            try
            {
                Console.Write("Nuevo Nombre: ");
                cliente.Nombre = Console.ReadLine();

                Console.Write("Nuevo Teléfono: ");
                cliente.Telefono = Console.ReadLine();

                Console.Write("Nueva Dirección: ");
                cliente.Direccion = Console.ReadLine();

                Console.Write("Nuevo Correo: ");
                cliente.Correo = Console.ReadLine();

                Console.Write("Nueva Identidad: ");
                cliente.Identidad = Console.ReadLine();

                Console.WriteLine("Cliente actualizado.");
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
            var cliente = listaClientes.FirstOrDefault(c => c.Id == id);
            if (cliente != null)
            {
                listaClientes.Remove(cliente);
                Console.WriteLine("Cliente eliminado.");
            }
            else
                Console.WriteLine("Cliente no encontrado.");
        }

        private void Guardar()
        {
            string json = System.Text.Json.JsonSerializer.Serialize(listaClientes, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(archivo, json);
            Console.WriteLine("Datos guardados.");
        }

        private void Cargar()
        {
            if (File.Exists(archivo))
            {
                string json = File.ReadAllText(archivo);
                listaClientes = System.Text.Json.JsonSerializer.Deserialize<List<Cliente>>(json) ?? new List<Cliente>();
            }
            else
            {
                listaClientes = new List<Cliente>();
            }
        }
    }
}
