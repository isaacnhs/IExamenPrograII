using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

/* I examen parcial de programación II */

class Program
{
    static List<Paciente> pacientes = new List<Paciente>();
    static List<Medicamento> catalogoMedicamentos = new List<Medicamento>();

    static void Main()
    {
        int opcion;
        do
        {
            MostrarMenu();
            Console.WriteLine("");
            opcion = LeerEntero("Seleccione una opción del 1-4 para continuar o 5 para salir: ");
            Console.WriteLine("");

            switch (opcion)
            {
                case 1:
                    AgregarPaciente();
                    break;
                case 2:
                    AgregarMedicamento();
                    break;
                case 3:
                    AsignarTratamiento();
                    break;
                case 4:
                    MostrarConsultas();
                    break;
                case 5:
                    Console.WriteLine("");
                    Console.WriteLine("Usted está saliendo del sistema...");
                    Console.WriteLine("");
                    break;
                default:
                    Console.WriteLine("");
                    Console.WriteLine("La opción digitada es inválida. Por favor, intente de nuevo con un número entero del 1-5.");
                    Console.WriteLine("");
                    break;
            }

        } while (opcion != 5);
    }

    static void MostrarMenu()
    {
        Console.WriteLine("");
        Console.WriteLine("==========================================================================");
        Console.WriteLine("* ¡Bienvenido(a) al sistema de gestión de pacientes y consultas médicas! * ");
        Console.WriteLine("==========================================================================");
        Console.WriteLine("");
        Console.WriteLine("---Menú principal--");
        Console.WriteLine("");
        Console.WriteLine("1- Agregar paciente");
        Console.WriteLine("2- Agregar medicamento al catálogo");
        Console.WriteLine("3- Asignar tratamiento médico a un paciente");
        Console.WriteLine("4- Consultas");
        Console.WriteLine("5- Salir");
        Console.WriteLine("");
    }

    static void AgregarPaciente()
    {
        try
        {
            Console.WriteLine("");
            Console.WriteLine("Agregar nuevo paciente:");
            Console.WriteLine("");
            string nombre = LeerCadena("Nombre: ");
            string cedula = LeerCedula("Número de cédula: ");
            string telefono = LeerTelefono("Número telefónico: ");
            string tipoSangre = LeerTipoSangre("Tipo de sangre: ");
            string direccion = LeerDireccion("Dirección: ");
            DateTime fechaNacimiento = LeerFecha("Fecha de nacimiento (formato: dd-mm-aaaa): ");

            Paciente nuevoPaciente = new Paciente(nombre, cedula, telefono, tipoSangre, direccion, fechaNacimiento);
            pacientes.Add(nuevoPaciente);
            Console.WriteLine("");
            Console.WriteLine("El paciente fue agregado correctamente.");
            Console.WriteLine("");
        }
        catch (Exception ex)
        {
            Console.WriteLine("");
            Console.WriteLine("Error al agregar paciente: " + ex.Message);
            Console.WriteLine("");
        }
    }

    static void AgregarMedicamento()
    {
        try
        {
            Console.WriteLine("");
            Console.WriteLine("Agregar nuevo medicamento:");
            Console.WriteLine("");
            int codigo = LeerEntero("Código del medicamento: ", true);
            if (catalogoMedicamentos.Any(m => m.Codigo == codigo))
            {
                throw new InvalidOperationException("Ese medicamento ya ha sido agregado anteriormente al catálogo.\nPor favor, intente de nuevo con un código diferente.");
            }

            string nombre = LeerCadena("Nombre del medicamento: ");
            int cantidad = LeerEntero("Cantidad: ", true);

            Medicamento nuevoMedicamento = new Medicamento(codigo, nombre, cantidad);
            catalogoMedicamentos.Add(nuevoMedicamento);
            Console.WriteLine("");
            Console.WriteLine("El medicamento fue agregado correctamente.");
            Console.WriteLine("");
        }
        catch (Exception ex)
        {
            Console.WriteLine("");
            Console.WriteLine("Error al agregar el medicamento: " + ex.Message);
            Console.WriteLine("");
        }
    }

    static void AsignarTratamiento()
    {
        try
        {
            Console.WriteLine("");
            Console.WriteLine("Asignar tratamiento médico a un paciente:");
            Console.WriteLine("");
            MostrarPacientes();
            Console.WriteLine("");
            int indicePaciente = LeerEntero("Seleccione el índice del paciente: ");
            Console.WriteLine("");

            indicePaciente--; // Se le resta 1 al índice digitado por el usuario para que coincida con el índice en la consola

            if (indicePaciente < 0 || indicePaciente >= pacientes.Count)
            {
                Console.WriteLine("");
                throw new IndexOutOfRangeException("El índice del paciente está fuera del rango.");
                Console.WriteLine("");
            }

            Paciente paciente = pacientes[indicePaciente];

            if (paciente.Tratamientos.Count >= 3) // Permite que el paciente reciba máximo 3 tratamientos
            {
                Console.WriteLine("");
                Console.WriteLine("Este paciente ya ha alcanzdo el límite de 3 tratamientos asignados y no se le pueden asignar más.");
                Console.WriteLine("Usted está regresando al menú principal...");
                Console.WriteLine("");
                return;
            }

            Console.WriteLine("");
            Console.WriteLine("Medicamentos disponibles:");
            Console.WriteLine("");
            MostrarMedicamentos();

            List<Medicamento> tratamiento = new List<Medicamento>();
            while (true)
            {
                Console.WriteLine("");
                int indiceMedicamento = LeerEntero("Seleccione el índice del medicamento o digite 0 para regresar al menú principal: ");
                Console.WriteLine("");

                if (indiceMedicamento == 0)
                {
                    if (tratamiento.Count == 0)
                    {
                        Console.WriteLine("");
                        Console.WriteLine("No se le asignó ningún medicamento al tratamiento del paciente.\nRegresando al menú principal...");
                        Console.WriteLine("");
                    }
                    break;
                }

                // Se le resta 1 al índice ingresado por el usuario para que coincida con el número en la consola
                indiceMedicamento--;

                Console.WriteLine("");
                if (indiceMedicamento < 0 || indiceMedicamento >= catalogoMedicamentos.Count)
                {
                    Console.WriteLine("");
                    Console.WriteLine("El índice del medicamento está fuera del rango. Por favor, intente de nuevo con otro diferente.");
                    Console.WriteLine("");
                    continue;
                }

                // Verifica si el medicamento ya se asignó al tratamiento del paciente, para que no hayan duplicados
                Medicamento medicamento = catalogoMedicamentos[indiceMedicamento];
                if (tratamiento.Contains(medicamento))
                {
                    Console.WriteLine("");
                    Console.WriteLine("Este medicamento ya ha sido agregado al tratamiento del paciente.\nPor favor, intente de nuevo con otro diferente.");
                    Console.WriteLine("");
                    continue;
                }

                // Verifica que haya suficiente inventario del medicamento para asignarlo al tratamiento del paciente
                Console.WriteLine("");
                int cantidad = LeerEntero("Cantidad a asignar: ");
                Console.WriteLine("");

                if (cantidad == 0)
                {
                    Console.WriteLine("");
                    Console.WriteLine("No se le asignó ningún medicamento al tratamiento del paciente.\nUsted está regresando al menú principal...");
                    Console.WriteLine("");
                    break;
                }

                else if (cantidad > medicamento.Cantidad)
                {
                    Console.WriteLine("");
                    Console.WriteLine("No hay suficiente inventario de este medicamento. Por favor, intente con otra cantidad.");
                    Console.WriteLine("");
                    continue;
                }

                medicamento.Cantidad -= cantidad;
                tratamiento.Add(new Medicamento(medicamento.Codigo, medicamento.Nombre, cantidad));
                Console.WriteLine("");
                Console.WriteLine("El medicamento fue agregado al tratamiento correctamente.");
                Console.WriteLine("");
            }

            if (tratamiento.Count == 0)
            {
                Console.WriteLine("");
                Console.WriteLine("No se le asignó ningún medicamento al tratamiento del paciente.\nUsted está regresando al menú principal...");
                Console.WriteLine("");
            }
            else
            {
                paciente.Tratamientos.Add(tratamiento);
                Console.WriteLine("");
                Console.WriteLine("El tratamiento fue asignado correctamente.");
                Console.WriteLine("");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("");
            Console.WriteLine("Error al asignar tratamiento: " + ex.Message);
            Console.WriteLine("");
        }
    }
    static void MostrarConsultas()
    {
        try
        {
            Console.WriteLine("");
            Console.WriteLine("Consultas:");
            Console.WriteLine("");
            Console.WriteLine("1- Cantidad total de pacientes registrados.");
            Console.WriteLine("2- Reporte de todos los medicamentos recetados.");
            Console.WriteLine("3- Reporte de cantidad de pacientes agrupados según su edad.");
            Console.WriteLine("4- Reporte de pacientes y consultas ordenado según su nombre.");
            Console.WriteLine("");
            int opcionConsulta = LeerEntero("Seleccione el tipo de consulta: ");
            Console.WriteLine("");

            switch (opcionConsulta)
            {
                case 1:
                    Console.WriteLine($"Cantidad total de pacientes registrados: {pacientes.Count}");
                    break;
                case 2:
                    var medicamentosAsignados = pacientes
                        .SelectMany(p => p.Tratamientos.SelectMany(t => t))
                        .GroupBy(m => m.Nombre);

                    foreach (var medicamento in medicamentosAsignados)
                    {
                        Console.WriteLine($"{medicamento.Key} - Cantidad total recetada: {medicamento.Sum(m => m.Cantidad)}");
                    }
                    break;
                case 3:
                    Console.WriteLine("");
                    Console.WriteLine("Reporte de cantidad de pacientes agrupado según su edad (en orden ascendente):");
                    Console.WriteLine("");

                    var pacientesPorEdad = pacientes.OrderBy(p => (DateTime.Now.DayOfYear < p.FechaNacimiento.DayOfYear) ? DateTime.Now.Year - p.FechaNacimiento.Year - 1 : DateTime.Now.Year - p.FechaNacimiento.Year)
                                                    .GroupBy(p =>
                                                    {
                                                        int edad = (DateTime.Now.DayOfYear < p.FechaNacimiento.DayOfYear) ? DateTime.Now.Year - p.FechaNacimiento.Year - 1 : DateTime.Now.Year - p.FechaNacimiento.Year;
                                                        return edad <= 10 ? "0-10 años" :
                                                               edad <= 30 ? "11-30 años" :
                                                               edad <= 50 ? "31-50 años" :
                                                               "Mayores de 51 años";
                                                    });

                    foreach (var grupo in pacientesPorEdad)
                    {
                        Console.WriteLine($"{grupo.Key}: {grupo.Count()} paciente(s)");
                        foreach (var paciente in grupo)
                        {
                            int edad = (DateTime.Now.DayOfYear < paciente.FechaNacimiento.DayOfYear) ? DateTime.Now.Year - paciente.FechaNacimiento.Year - 1 : DateTime.Now.Year - paciente.FechaNacimiento.Year;
                            Console.WriteLine("");
                            Console.WriteLine("");
                            Console.WriteLine($"{paciente.Nombre}, edad: {edad} año(s)");
                        }
                    }
                    break;
                case 4:
                    Console.WriteLine("");
                    Console.WriteLine("Reporte de pacientes y consultas ordenado según su nombre (en orden alfabético):");
                    Console.WriteLine("");

                    var pacientesOrdenados = pacientes.OrderBy(p => p.Nombre);
                    foreach (var paciente in pacientesOrdenados)
                    {
                        Console.WriteLine($"Nombre: {paciente.Nombre}, cédula: {paciente.Cedula}, número telefónico: +506 {paciente.Telefono}, tipo de sangre: {paciente.TipoSangre}, dirección: {paciente.Direccion}, fecha de nacimiento: {paciente.FechaNacimiento.ToString("dd-MM-yyyy")}");
                    }
                    break;
                default:
                    Console.WriteLine("");
                    Console.WriteLine("La opción es inválida. Por favor, intente de nuevo.");
                    Console.WriteLine("");
                    break;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("");
            Console.WriteLine("Error al mostrar consultas: " + ex.Message);
            Console.WriteLine("");
        }
    }

    static void MostrarPacientes()
    {
        for (int i = 0; i < pacientes.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {pacientes[i].Nombre}"); //para que el número i en la función corresponda con el índice del paciente que ve el usuario//
        }
    }

    static void MostrarMedicamentos()
    {
        for (int i = 0; i < catalogoMedicamentos.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {catalogoMedicamentos[i].Nombre} - Cantidad: {catalogoMedicamentos[i].Cantidad}"); //para que el número i en la función corresponda con el índice del medicamento que ve el usuario//
        }
    }

    static string LeerCedula(string mensaje)
    {
        string cedula;
        while (true)
        {
            try
            {
                Console.Write(mensaje);
                cedula = Console.ReadLine();
                if (cedula.Length != 9)
                {
                    Console.WriteLine("");
                    throw new FormatException("El número de cédula debe ser un número entero de 9 dígitos. Además, debe digitarse sin espacios ni guiones.");
                    Console.WriteLine("");
                }
                return cedula;
            }
            catch (FormatException ex)
            {
                Console.WriteLine("");
                Console.WriteLine("Error: " + ex.Message);
                Console.WriteLine("");
            }
        }
    }

    static string LeerCadena(string mensaje)
    {
        while (true)
        {
            try
            {
                Console.Write(mensaje);
                string input = Console.ReadLine();
                if (!input.All(char.IsLetter))
                {
                    Console.WriteLine("");
                    throw new FormatException("el nombre es inválido.\nEl nombre debe contener únicamente letras.");
                    Console.WriteLine("");
                }
                return input;
            }
            catch (FormatException ex)
            {
                Console.WriteLine("");
                Console.WriteLine("Error: " + ex.Message);
                Console.WriteLine("");
            }
        }
    }
    static string LeerTelefono(string mensaje)
    {
        while (true)
        {
            try
            {
                Console.Write(mensaje);
                string input = Console.ReadLine();
                if (!int.TryParse(input, out _) || input.Length != 8)
                {
                    Console.WriteLine("");
                    throw new FormatException("valor inválido.\nEl número telefónico debe ser un número entero positivo de 8 dígitos. Además, debe digitarse sin espacios ni guiones.");
                    Console.WriteLine("");
                }
                return input;
            }
            catch (FormatException ex)
            {
                Console.WriteLine("");
                Console.WriteLine("Error: " + ex.Message);
                Console.WriteLine("");
            }
        }
    }
    static string LeerTipoSangre(string mensaje)
    {
        while (true)
        {
            try
            {
                Console.Write(mensaje);
                string input = Console.ReadLine();
                if (input.Any(char.IsDigit))
                {
                    Console.WriteLine("");
                    throw new FormatException("el tipo de sangre es inválido.\nEl tipo de sangre no puede contener números, pero puede contener letras y símbolos combinados (+/-).");
                    Console.WriteLine("");
                }
                return input;
            }
            catch (FormatException ex)
            {
                Console.WriteLine("");
                Console.WriteLine("Error: " + ex.Message);
                Console.WriteLine("");
            }
        }
    }
    static string LeerDireccion(string mensaje)
    {
        while (true)
        {
            try
            {
                Console.Write(mensaje);
                string input = Console.ReadLine();
                if (!input.All(char.IsLetter))
                {
                    Console.WriteLine("");
                    throw new FormatException("el nombre del lugar es inválido, debe contener únicamente letras.");
                    Console.WriteLine("");
                }
                return input;
            }
            catch (FormatException ex)
            {
                Console.WriteLine("");
                Console.WriteLine("Error: " + ex.Message);
                Console.WriteLine("");
            }
        }
    }
    static DateTime LeerFecha(string mensaje)
    {
        while (true)
        {
            try
            {
                Console.Write(mensaje);
                string input = Console.ReadLine();
                return DateTime.ParseExact(input, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            }
            catch (FormatException)
            {
                Console.WriteLine("");
                Console.WriteLine("Error: el formato de la fecha es inválido. Por favor, intente de nuevo.");
                Console.WriteLine("");
            }
        }
    }

    static int LeerEntero(string mensaje, bool positivo = false)
    {
        while (true)
        {
            try
            {
                Console.Write(mensaje);
                string input = Console.ReadLine();
                if (!int.TryParse(input, out int valor) || (positivo && valor <= 0))
                {
                    Console.WriteLine("");
                    throw new FormatException("El valor ingresado no es un número entero válido.");
                    Console.WriteLine("");
                }
                return valor;
            }
            catch (FormatException ex)
            {
                Console.WriteLine("");
                Console.WriteLine("Error: " + ex.Message);
                Console.WriteLine("");
            }
        }
    }
    class Paciente
    {
        public string Nombre { get; set; }
        public string Cedula { get; set; }
        public string Telefono { get; set; }
        public string TipoSangre { get; set; }
        public string Direccion { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public List<List<Medicamento>> Tratamientos { get; set; }

        public Paciente(string nombre, string cedula, string telefono, string tipoSangre, string direccion, DateTime fechaNacimiento)
        {
            Nombre = nombre;
            Cedula = cedula;
            Telefono = telefono;
            TipoSangre = tipoSangre;
            Direccion = direccion;
            FechaNacimiento = fechaNacimiento;
            Tratamientos = new List<List<Medicamento>>();
        }
        public override string ToString()
        {
            return $"Nombre: {Nombre}, cédula: {Cedula}, número telefónico: {Telefono}, tipo de sangre: {TipoSangre}, dirección: {Direccion}, fecha de nacimiento: {FechaNacimiento.ToString("dd-MM-yyyy")}";
        }
    }
    class Medicamento
    {
        public int Codigo { get; set; }
        public string Nombre { get; set; }
        public int Cantidad { get; set; }

        public Medicamento(int codigo, string nombre, int cantidad)
        {
            Codigo = codigo;
            Nombre = nombre;
            Cantidad = cantidad;
        }
    }
}
