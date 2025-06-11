using System;
using System.Collections.Generic;

namespace Proyecto_de_sistema_de_resturante
{
    internal class Program
    {
        static List<string> dishNames = new List<string> { "Cupeta de Pollo Frito 9 PZS", "Big Box", "Crispy SandWich", "Papas Fritas", "Strips" };
        static List<decimal> dishPrices = new List<decimal> { 369m, 249m, 73.5m, 51.75m, 150.89m };
        static List<string> dishDescriptions = new List<string> {
        "9 piezas de pollo crujiente",
        "1 Pieza de pollo, papa fritas, un refresco",
        "Pan suave, pollo empanizado, lechuga y salsa",
        "Porción de papas fritas clásicas",
        "Chicken Finger de 6 piezas" };
        static List<int> inventory = new List<int> { 10, 25, 20, 50, 30 };
        static decimal totalRevenue = 0;
        static List<int> salesCounts = new List<int> { 0, 0, 0, 0, 0 };

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
                Console.WriteLine("5. Salir");
                Console.Write("Seleccione una opción: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": ManageMenu(); break;
                    case "2": TakeOrder(); break;
                    case "3": ShowInventory(); break;
                    case "4": ShowSalesReport(); break;
                    case "5": return;
                    default:
                        Console.WriteLine("Opción inválida. Presione para intentar de nuevo.");
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
                Console.WriteLine("1. Agregar plato");
                Console.WriteLine("2. Editar plato");
                Console.WriteLine("3. Eliminar plato");
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
                        decimal price = decimal.Parse(Console.ReadLine());
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
                        int idx = int.Parse(Console.ReadLine());
                        if (idx >= 0 && idx < dishNames.Count)
                        {
                            Console.Write("Nuevo nombre (Presione Enter para omitir): ");
                            string newName = Console.ReadLine();
                            if (!string.IsNullOrEmpty(newName)) dishNames[idx] = newName;
                            Console.Write("Nuevo precio (Presione Enter para omitir): ");
                            string newPrice = Console.ReadLine();
                            if (!string.IsNullOrEmpty(newPrice)) dishPrices[idx] = decimal.Parse(newPrice);
                            Console.Write("Nueva descripción (Presione Enter para omitir): ");
                            string newDesc = Console.ReadLine();
                            if (!string.IsNullOrEmpty(newDesc)) dishDescriptions[idx] = newDesc;
                            Console.WriteLine("Plato actualizado.");
                        }
                        else Console.WriteLine("Índice inválido.");
                        break;

                    case "3":
                        ShowMenuItems();
                        Console.Write("Índice a eliminar: ");
                        int del = int.Parse(Console.ReadLine());
                        if (del >= 0 && del < dishNames.Count)
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
                int sel = int.Parse(Console.ReadLine());
                if (sel == -1) break;
                if (sel < 0 || sel >= dishNames.Count)
                {
                    Console.WriteLine("Índice inválido.");
                    continue;
                }
                Console.Write("Cantidad: ");
                int qty = int.Parse(Console.ReadLine());
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
            Console.WriteLine("Presione para volver.");
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
    }

}

