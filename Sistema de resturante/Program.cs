using System;
using System.Collections.Generic;

namespace Proyecto_de_sistema_de_resturante
{
    internal class Program
    {
        static List<string> dishNames = new List<string> { "Cupeta de Pollo Frito 9 PZS", "Big Box", "Crispy SandWich", "Papas Fritas", "Strips" };
        static List<decimal> dishPrices = new List<decimal> { 369m, 249m, 73.5m, 51.75m, 150.89m };
        static List<string> dishDescriptions = new List<string>
        {
            "9 piezas de pollo crujiente",
            "1 Pieza de pollo, papas fritas, un refresco",
            "Pan suave, pollo empanizado, lechuga y salsa",
            "Porción de papas fritas clásicas",
            "Chicken Finger de 6 piezas"
        };
        static List<int> inventory = new List<int> { 10, 25, 20, 50, 30 };
        static List<int> salesCounts = new List<int> { 0, 0, 0, 0, 0 };
        static decimal totalRevenue = 0;
        static string[,] clientes = new string[10, 7];
        static string[,] evaluaciones = new string[10, 7];

        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Restaurante Los Pollos Hermanos ===");
                Console.WriteLine("1. Gestionar Menú");
                Console.WriteLine("2. Tomar Pedido");
                Console.WriteLine("3. Ver Inventario");
                Console.WriteLine("4. Ver Reporte de Ventas");
                Console.WriteLine("5. Gestión de Clientes");
                Console.WriteLine("6. Gestión de Evaluaciones");
                Console.WriteLine("8. Salir");
                Console.Write("Seleccione una opción: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": ManageMenu(); break;
                    case "2": TakeOrder(); break;
                    case "3": ShowInventory(); break;
                    case "4": ShowSalesReport(); break;
                    case "5": GestionarClientes(); break;
                    case "6": GestionarEvaluaciones(); break;
                    case "8": return;
                    default:
                        Console.WriteLine("Opción inválida. Presione Enter para continuar.");
                        Console.ReadLine();
                        break;
                }
            }
        }

        static void ManageMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("--- Gestionar Menú ---");
                Console.WriteLine("1. Agregar Plato");
                Console.WriteLine("2. Editar Plato");
                Console.WriteLine("3. Eliminar Plato");
                Console.WriteLine("4. Volver");
                Console.Write("Seleccione: ");
                string opt = Console.ReadLine();

                if (opt == "4") break;

                switch (opt)
                {
                    case "1":
                        Console.Write("Nombre: ");
                        string name = Console.ReadLine();
                        Console.Write("Precio: ");
                        if (!decimal.TryParse(Console.ReadLine(), out decimal price))
                        {
                            Console.WriteLine("Precio inválido.");
                            break;
                        }
                        Console.Write("Descripción: ");
                        string desc = Console.ReadLine();

                        dishNames.Add(name);
                        dishPrices.Add(price);
                        dishDescriptions.Add(desc);
                        inventory.Add(0);
                        salesCounts.Add(0);
                        Console.WriteLine("Plato agregado.");
                        break;

                    case "2":
                        ShowMenuItems();
                        Console.Write("Índice a editar: ");
                        if (int.TryParse(Console.ReadLine(), out int idx) && idx >= 0 && idx < dishNames.Count)
                        {
                            Console.Write("Nuevo nombre (Enter para omitir): ");
                            string newName = Console.ReadLine();
                            if (!string.IsNullOrEmpty(newName)) dishNames[idx] = newName;

                            Console.Write("Nuevo precio (Enter para omitir): ");
                            string newPrice = Console.ReadLine();
                            if (!string.IsNullOrEmpty(newPrice)) dishPrices[idx] = decimal.Parse(newPrice);

                            Console.Write("Nueva descripción (Enter para omitir): ");
                            string newDesc = Console.ReadLine();
                            if (!string.IsNullOrEmpty(newDesc)) dishDescriptions[idx] = newDesc;

                            Console.WriteLine("Plato actualizado.");
                        }
                        else Console.WriteLine("Índice inválido.");
                        break;

                    case "3":
                        ShowMenuItems();
                        Console.Write("Índice a eliminar: ");
                        if (int.TryParse(Console.ReadLine(), out int del) && del >= 0 && del < dishNames.Count)
                        {
                            dishNames.RemoveAt(del);
                            dishPrices.RemoveAt(del);
                            dishDescriptions.RemoveAt(del);
                            inventory.RemoveAt(del);
                            salesCounts.RemoveAt(del);
                            Console.WriteLine("Plato eliminado.");
                        }
                        else Console.WriteLine("Índice inválido.");
                        break;

                    default:
                        Console.WriteLine("Opción inválida.");
                        break;
                }
                Console.WriteLine("Presione Enter para continuar.");
                Console.ReadLine();
            }
        }

        static void TakeOrder()
        {
            Console.Clear();
            Console.WriteLine("--- Tomar Pedido ---");
            decimal orderTotal = 0;
            List<int> quantities = new List<int>(new int[dishNames.Count]);

            while (true)
            {
                ShowMenuItems();
                Console.WriteLine("Ingrese índice del plato (o -1 para finalizar): ");
                if (!int.TryParse(Console.ReadLine(), out int sel)) continue;
                if (sel == -1) break;
                if (sel < 0 || sel >= dishNames.Count)
                {
                    Console.WriteLine("Índice inválido.");
                    continue;
                }

                Console.Write("Cantidad: ");
                if (!int.TryParse(Console.ReadLine(), out int qty)) continue;
                if (qty <= inventory[sel])
                {
                    inventory[sel] -= qty;
                    quantities[sel] += qty;
                    decimal lineTotal = dishPrices[sel] * qty;
                    orderTotal += lineTotal;
                    totalRevenue += lineTotal;
                    salesCounts[sel] += qty;
                    Console.WriteLine($"Añadido {qty} x {dishNames[sel]} - Subtotal línea: {lineTotal:C}");
                }
                else Console.WriteLine("Inventario insuficiente.");
            }

            Console.WriteLine($"Total a pagar: {orderTotal:C}");
            Console.WriteLine("Presione Enter para finalizar pedido.");
            Console.ReadLine();
        }

        static void ShowInventory()
        {
            Console.Clear();
            Console.WriteLine("--- Inventario ---");
            for (int i = 0; i < dishNames.Count; i++)
            {
                Console.WriteLine($"{i}: {dishNames[i]} - Cantidad disponible: {inventory[i]}");
            }
            Console.WriteLine("Presione Enter para volver.");
            Console.ReadLine();
        }

        static void ShowSalesReport()
        {
            Console.Clear();
            Console.WriteLine("--- Reporte de Ventas ---");
            Console.WriteLine($"Ingresos totales: {totalRevenue:C}");
            for (int i = 0; i < dishNames.Count; i++)
            {
                Console.WriteLine($"{dishNames[i]}: Vendidos {salesCounts[i]} unidades");
            }
            Console.WriteLine("Presione Enter para volver.");
            Console.ReadLine();
        }

        static void ShowMenuItems()
        {
            Console.WriteLine("Índice | Plato - Precio");
            for (int i = 0; i < dishNames.Count; i++)
            {
                Console.WriteLine($"{i}: {dishNames[i]} - {dishPrices[i]:C}");
            }
        }

        static void GestionarClientes()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("--- Gestión de Clientes ---");
                Console.WriteLine("1. Agregar Cliente");
                Console.WriteLine("2. Ver Todos");
                Console.WriteLine("3. Ver Cliente por Índice");
                Console.WriteLine("4. Volver");
                Console.Write("Seleccione una opción: ");
                string opc = Console.ReadLine();

                switch (opc)
                {
                    case "1":
                        for (int i = 0; i < clientes.GetLength(0); i++)
                        {
                            if (clientes[i, 0] == null)
                            {
                                Console.Write("ID: ");                  clientes[i, 0] = Console.ReadLine();
                                Console.Write("Nombre: ");              clientes[i, 1] = Console.ReadLine();
                                Console.Write("Teléfono: ");            clientes[i, 2] = Console.ReadLine();
                                Console.Write("Dirección: ");           clientes[i, 3] = Console.ReadLine();
                                Console.Write("Email: ");               clientes[i, 4] = Console.ReadLine();
                                Console.Write("Edad: ");                clientes[i, 5] = Console.ReadLine();
                                Console.Write("Género: ");              clientes[i, 6] = Console.ReadLine();
                                Console.WriteLine("Cliente registrado.");
                                break;
                            }
                        }
                        break;
                    case "2":
                        MostrarClientes();
                        break;
                    case "3":
                        Console.Write("Índice: ");
                        if (int.TryParse(Console.ReadLine(), out int idx) && idx >= 0 && idx < 10 && clientes[idx, 0] != null)
                        {
                            Console.WriteLine("=== Detalles del Cliente ===");
                            Console.WriteLine($"• ID:        {clientes[idx, 0]}");
                            Console.WriteLine($"• Nombre:    {clientes[idx, 1]}");
                            Console.WriteLine($"• Teléfono:  {clientes[idx, 2]}");
                            Console.WriteLine($"• Dirección: {clientes[idx, 3]}");
                            Console.WriteLine($"• Email:     {clientes[idx, 4]}");
                            Console.WriteLine($"• Edad:      {clientes[idx, 5]}");
                            Console.WriteLine($"• Género:    {clientes[idx, 6]}");
                        }
                        else Console.WriteLine("Índice inválido o cliente no registrado.");
                        Console.ReadLine();
                        break;
                    case "4":
                        return;
                }
            }
        }

        static void MostrarClientes()
        {
            Console.Clear();
            for (int i = 0; i < clientes.GetLength(0); i++)
            {
                if (clientes[i, 0] != null)
                {
                    Console.WriteLine($"ID: {clientes[i, 0]}, " +
                        $"Nombre: {clientes[i, 1]}, " +
                        $"Tel: {clientes[i, 2]}, " +
                        $"Direcion: {clientes[i, 3]}, " +
                        $"Email: {clientes[i, 4]}, " +
                        $"Edad: {clientes[i, 5]}, " +
                        $"Género: {clientes[i, 6]}");
                }
            }
            Console.WriteLine("Presione Enter para continuar.");
            Console.ReadLine();
        }

        static void GestionarEvaluaciones()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("--- Gestión de Evaluaciones ---");
                Console.WriteLine("1. Agregar Evaluación");
                Console.WriteLine("2. Ver Todas");
                Console.WriteLine("3. Ver por Índice");
                Console.WriteLine("4. Volver");
                string opc = Console.ReadLine();

                switch (opc)
                {
                    case "1":
                        for (int i = 0; i < evaluaciones.GetLength(0); i++)
                        {
                            if (evaluaciones[i, 0] == null)
                            {
                                Console.Write("ID: "); evaluaciones[i, 0] = Console.ReadLine();
                                Console.Write("Nombre: "); evaluaciones[i, 1] = Console.ReadLine();
                                Console.Write("Servicio (0-10): "); int s = int.Parse(Console.ReadLine());
                                Console.Write("Limpieza (0-10): "); int l = int.Parse(Console.ReadLine());
                                Console.Write("Presentación (0-10): "); int p = int.Parse(Console.ReadLine());
                                evaluaciones[i, 2] = s.ToString();
                                evaluaciones[i, 3] = l.ToString();
                                evaluaciones[i, 4] = p.ToString();
                                evaluaciones[i, 5] = DateTime.Now.ToString("yyyy-MM-dd");
                                evaluaciones[i, 6] = ((s + l + p) / 3.0).ToString("0.0");
                                Console.WriteLine("Evaluación registrada.");
                                break;
                            }
                        }
                        break;
                    case "2":
                        MostrarEvaluaciones();
                        break;
                    case "3":
                        Console.Write("Índice: ");
                        if (int.TryParse(Console.ReadLine(), out int idx) && idx >= 0 && idx < 10 && evaluaciones[idx, 0] != null)
                        {
                            Console.WriteLine($"ID: {evaluaciones[idx, 0]}, Nombre: {evaluaciones[idx, 1]}, Servicio: {evaluaciones[idx, 2]}, Limpieza: {evaluaciones[idx, 3]}, Presentación: {evaluaciones[idx, 4]}, Fecha: {evaluaciones[idx, 5]}, Promedio: {evaluaciones[idx, 6]}");
                        }
                        else Console.WriteLine("Índice inválido o evaluación no registrada.");
                        Console.ReadLine();
                        break;
                    case "4":
                        return;
                }
            }
        }

        static void MostrarEvaluaciones()
        {
            Console.Clear();
            for (int i = 0; i < evaluaciones.GetLength(0); i++)
            {
                if (evaluaciones[i, 0] != null)
                {
                    Console.WriteLine($"ID: {evaluaciones[i, 0]}, Nombre: {evaluaciones[i, 1]}, Servicio: {evaluaciones[i, 2]}, Limpieza: {evaluaciones[i, 3]}, Presentación: {evaluaciones[i, 4]}, Fecha: {evaluaciones[i, 5]}, Promedio: {evaluaciones[i, 6]}");
                }
            }
            Console.WriteLine("Presione Enter para continuar.");
            Console.ReadLine();
        }
    }
}
