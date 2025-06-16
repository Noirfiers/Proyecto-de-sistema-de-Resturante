using System;
using System.Linq;

namespace Proyecto_de_sistema_de_resturante
{
    internal class Program
    {
        static string[,] menu;
        static int menuCount = 0;
        static string[,] inventario;
        static int inventarioCount = 0;
        static string[,] pedidos;
        static int pedidosCount = 0;
        static string[,] facturas;
        static int facturasCount = 0;
        static string[,] detalleFactura;
        static int detalleCount = 0;
        static string[,] reporteVentas;
        static int reporteCount = 0;

        static void Main(string[] args)
        {
            int maxFilas = 50;
            int columnas = 7;
            Console.Write("¿Cuántos menús desea ingresar? (máx. 50): ");
            int filasMenu;
            while (!int.TryParse(Console.ReadLine(), out filasMenu) || filasMenu <= 0 || filasMenu > 50)
            {
                Console.Write("Ingrese un número válido entre 1 y 50: ");
            }
            menu = new string[filasMenu, columnas];

            menu = new string[maxFilas, columnas];
            inventario = new string[maxFilas, columnas];
            pedidos = new string[maxFilas, columnas];
            facturas = new string[maxFilas, columnas];
            detalleFactura = new string[maxFilas, columnas];
            reporteVentas = new string[maxFilas, columnas];

            Console.WriteLine("Sistema Restaurante Los Pollos Hermanos");
            bool salir = false;
            while (!salir)
            {
                Console.WriteLine("\nMenú Principal:");
                Console.WriteLine("1. Gestionar Menú");
                Console.WriteLine("2. Tomar Pedido");
                Console.WriteLine("3. Reporte de Ventas");
                Console.WriteLine("4. Salir");
                Console.Write("Seleccione opción: ");

                int opcion = LeerEntero(1, 4);

                switch (opcion)
                {
                    case 1:
                        GestionarMenu();
                        break;
                    case 2:
                        TomarPedido();
                        break;
                    case 3:
                        MostrarReporteVentas();
                        break;
                    case 4:
                        salir = true;
                        break;
                }
            }
            Console.WriteLine("Gracias por usar el sistema.");
        }

        static int LeerEntero(int min, int max)
        {
            int num;
            while (true)
            {
                try
                {
                    num = int.Parse(Console.ReadLine());
                    if (num < min || num > max)
                    {
                        Console.Write($"Error: Ingrese un número entre {min} y {max}: ");
                        continue;
                    }
                    return num;
                }
                catch
                {
                    Console.Write("Error: Entrada inválida. Intente de nuevo: ");
                }
            }
        }

        static decimal LeerDecimalPositivo()
        {
            decimal val;
            while (true)
            {
                try
                {
                    val = decimal.Parse(Console.ReadLine());
                    if (val <= 0)
                    {
                        Console.Write("Error: Debe ingresar un número mayor a 0: ");
                        continue;
                    }
                    return val;
                }
                catch
                {
                    Console.Write("Error: Entrada inválida. Intente de nuevo: ");
                }
            }
        }

        static void GestionarMenu()
        {
            bool volver = false;
            while (!volver)
            {
                Console.WriteLine("\nGestionar Menú:");
                Console.WriteLine("1. Agregar Plato");
                Console.WriteLine("2. Visualizar Menú");
                Console.WriteLine("3. Imprimir Plato (por índice)");
                Console.WriteLine("4. Volver");
                Console.Write("Seleccione opción: ");
                int op = LeerEntero(1, 4);
                switch (op)
                {
                    case 1:
                        AgregarPlato();
                        break;
                    case 2:
                        VisualizarMenu();
                        break;
                    case 3:
                        ImprimirPlato();
                        break;
                    case 4:
                        volver = true;
                        break;
                }
            }
        }

        static void AgregarPlato()
        {
            if (menuCount >= menu.GetLength(0))
            {
                Console.WriteLine("Error: Menú lleno.");
                return;
            }
            Console.Write("Ingrese ID del Plato (único): ");
            string id = Console.ReadLine();

            if (BuscarIndicePorId(menu, menuCount, id) != -1)
            {
                Console.WriteLine("Error: Ya existe un plato con ese ID.");
                return;
            }
            Console.Write("Ingrese Nombre del Plato: ");
            string nombre = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(nombre))
            {
                Console.WriteLine("Error: Nombre inválido.");
                return;
            }
            Console.Write("Ingrese Precio: ");
            decimal precio = LeerDecimalPositivo();
            Console.Write("Ingrese Categoría: ");
            string categoria = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(categoria))
            {
                Console.WriteLine("Error: Categoría inválida.");
                return;
            }
            Console.Write("Ingrese Cantidad Disponible: ");
            int cantidadDisponible = LeerEntero(0, 1000);
            Console.Write("Ingrese Descripción: ");
            string descripcion = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(descripcion))
            {
                descripcion = "-";
            }
            Console.Write("Ingrese Tiempo de Preparación (minutos): ");
            int tiempoPrep = LeerEntero(1, 180);

            menu[menuCount, 0] = id;
            menu[menuCount, 1] = nombre;
            menu[menuCount, 2] = precio.ToString("F2");
            menu[menuCount, 3] = categoria;
            menu[menuCount, 4] = cantidadDisponible.ToString();
            menu[menuCount, 5] = descripcion;
            menu[menuCount, 6] = tiempoPrep.ToString();

            menuCount++;
            Console.WriteLine("Plato agregado correctamente.");
        }

        static void VisualizarMenu()
        {
            if (menuCount == 0)
            {
                Console.WriteLine("Menú vacío.");
                return;
            }
            Console.WriteLine("\nMenú actual:");
            Console.WriteLine("ID\tNombre\tPrecio\tCategoría\tCantidad\tDescripción\tTiempoPrep");
            for (int i = 0; i < menuCount; i++)
            {
                Console.WriteLine($"{menu[i, 0]}\t{menu[i, 1]}\t{menu[i, 2]}\t{menu[i, 3]}\t\t{menu[i, 4]}\t\t{menu[i, 5]}\t\t{menu[i, 6]}");
            }
        }

        static void ImprimirPlato()
        {
            if (menuCount == 0)
            {
                Console.WriteLine("Menú vacío.");
                return;
            }
            Console.Write($"Ingrese índice del plato (0 a {menuCount - 1}): ");
            int idx = LeerEntero(0, menuCount - 1);

            Console.WriteLine("Detalles del plato:");
            Console.WriteLine($"ID: {menu[idx, 0]}");
            Console.WriteLine($"Nombre: {menu[idx, 1]}");
            Console.WriteLine($"Precio: {menu[idx, 2]}");
            Console.WriteLine($"Categoría: {menu[idx, 3]}");
            Console.WriteLine($"Cantidad Disponible: {menu[idx, 4]}");
            Console.WriteLine($"Descripción: {menu[idx, 5]}");
            Console.WriteLine($"Tiempo Preparación: {menu[idx, 6]} minutos");
        }

        static void TomarPedido()
        {
            if (menuCount == 0)
            {
                Console.WriteLine("Error: Menú vacío, no puede tomar pedido.");
                return;
            }
            Console.Write("Ingrese nombre del cliente: ");
            string cliente = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(cliente))
            {
                Console.WriteLine("Error: Nombre inválido.");
                return;
            }


            if (facturasCount >= facturas.GetLength(0))
            {
                Console.WriteLine("Error: Límite de facturas alcanzado.");
                return;
            }
            string idFactura = "F" + (facturasCount + 1).ToString("000");

            decimal totalSinImpuesto = 0M;
            decimal impuestoTotal = 0M;
            decimal totalConImpuesto = 0M;


            int itemsPedidos = 0;


            var pedidoTemporal = new System.Collections.Generic.List<(int indiceMenu, int cantidad)>();

            bool seguir = true;
            while (seguir)
            {
                Console.WriteLine("\nMenú para pedido:");
                VisualizarMenu();
                Console.Write("Ingrese índice del plato a pedir (o -1 para terminar): ");
                int idx = LeerEntero(-1, menuCount - 1);
                if (idx == -1)
                {
                    seguir = false;
                    break;
                }

                int cantDisponible = int.Parse(menu[idx, 4]);
                if (cantDisponible == 0)
                {
                    Console.WriteLine("No hay existencia de este plato.");
                    continue;
                }
                Console.Write($"Ingrese cantidad (disponible {cantDisponible}): ");
                int cantidad = LeerEntero(1, cantDisponible);

                pedidoTemporal.Add((idx, cantidad));
            }

            if (pedidoTemporal.Count == 0)
            {
                Console.WriteLine("No se realizó pedido.");
                return;
            }


            foreach (var item in pedidoTemporal)
            {
                int idxMenu = item.indiceMenu;
                int cant = item.cantidad;

                decimal precioUnit = decimal.Parse(menu[idxMenu, 2]);
                decimal subtotal = precioUnit * cant;
                decimal impuesto = subtotal * 0.15M;
                decimal total = subtotal + impuesto;


                if (pedidosCount >= pedidos.GetLength(0))
                {
                    Console.WriteLine("Error: Límite de pedidos alcanzado.");
                    break;
                }

                pedidos[pedidosCount, 0] = "P" + (pedidosCount + 1).ToString("000");
                pedidos[pedidosCount, 1] = menu[idxMenu, 0];
                pedidos[pedidosCount, 2] = cant.ToString();
                pedidos[pedidosCount, 3] = precioUnit.ToString("F2");
                pedidos[pedidosCount, 4] = subtotal.ToString("F2");
                pedidos[pedidosCount, 5] = impuesto.ToString("F2");
                pedidos[pedidosCount, 6] = total.ToString("F2");
                pedidosCount++;

                int actualDisponible = int.Parse(menu[idxMenu, 4]);
                menu[idxMenu, 4] = (actualDisponible - cant).ToString();

                if (detalleCount >= detalleFactura.GetLength(0))
                {
                    Console.WriteLine("Error: Límite de detalles alcanzado.");
                    break;
                }
                detalleFactura[detalleCount, 0] = "D" + (detalleCount + 1).ToString("000");
                detalleFactura[detalleCount, 1] = idFactura;
                detalleFactura[detalleCount, 2] = menu[idxMenu, 0];
                detalleFactura[detalleCount, 3] = cant.ToString();
                detalleFactura[detalleCount, 4] = precioUnit.ToString("F2");
                detalleFactura[detalleCount, 5] = subtotal.ToString("F2");
                detalleFactura[detalleCount, 6] = "Activo";
                detalleCount++;

                totalSinImpuesto += subtotal;
                impuestoTotal += impuesto;
                totalConImpuesto += total;
                itemsPedidos++;
            }

            facturas[facturasCount, 0] = idFactura;
            facturas[facturasCount, 1] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            facturas[facturasCount, 2] = cliente;
            facturas[facturasCount, 3] = totalSinImpuesto.ToString("F2");
            facturas[facturasCount, 4] = impuestoTotal.ToString("F2");
            facturas[facturasCount, 5] = totalConImpuesto.ToString("F2");
            facturas[facturasCount, 6] = "Activa";
            facturasCount++;

            Console.WriteLine("\nFactura generada:");
            MostrarFactura(idFactura);
        }

        static void MostrarFactura(string idFactura)
        {
            int idxFact = BuscarIndicePorId(facturas, facturasCount, idFactura);
            if (idxFact == -1)
            {
                Console.WriteLine("Factura no encontrada.");
                return;
            }

            Console.WriteLine($"Factura ID: {facturas[idxFact, 0]}");
            Console.WriteLine($"Fecha: {facturas[idxFact, 1]}");
            Console.WriteLine($"Cliente: {facturas[idxFact, 2]}");
            Console.WriteLine("Detalles:");

            Console.WriteLine("IDDetalle\tIDPlato\tCantidad\tPrecioUnit\tSubtotal");
            for (int i = 0; i < detalleCount; i++)
            {
                if (detalleFactura[i, 1] == idFactura)
                {
                    Console.WriteLine($"{detalleFactura[i, 0]}\t{detalleFactura[i, 2]}\t{detalleFactura[i, 3]}\t\t{detalleFactura[i, 4]}\t\t{detalleFactura[i, 5]}");
                }
            }

            Console.WriteLine($"Total sin impuesto: {facturas[idxFact, 3]}");
            Console.WriteLine($"Impuesto: {facturas[idxFact, 4]}");
            Console.WriteLine($"Total con impuesto: {facturas[idxFact, 5]}");
        }

        static void MostrarReporteVentas()
        {
            if (facturasCount == 0)
            {
                Console.WriteLine("No hay facturas registradas.");
                return;
            }

            Console.WriteLine("\nReporte de todas las facturas:");
            Console.WriteLine("IDFactura\tFecha\t\tCliente\tTotal");
            for (int i = 0; i < facturasCount; i++)
            {
                Console.WriteLine($"{facturas[i, 0]}\t{facturas[i, 1]}\t{facturas[i, 2]}\t{facturas[i, 5]}");
            }

            var totales = new System.Collections.Generic.List<decimal>();
            for (int i = 0; i < facturasCount; i++)
            {
                totales.Add(decimal.Parse(facturas[i, 5]));
            }

            decimal totalGeneral = totales.Sum(x => x); 
            Console.WriteLine($"\nTotal general de ventas: L. {totalGeneral:F2}");
        }
        static int BuscarIndicePorId(string[,] matriz, int filas, string id)
        {
            for (int i = 0; i < filas; i++)
            {
                if (matriz[i, 0] == id)
                    return i;
            }
            return -1;
        }
    }
}
