
using System;
using System.IO;

class Program {
    static void Main() {
        string ruta = "../../../acb2526.csv";
        string[][] partidos = new string[0][];

        try {
            if (!File.Exists(ruta)) {
                Console.WriteLine("No se encuentra el archivo CSV.");
                return;
            }

            partidos = LeerCSV(ruta);
            Console.WriteLine("Partidos cargados: " + partidos.Length);
        }
        catch (Exception e) {
            Console.WriteLine("Error al leer el archivo: " + e.Message);
            return;
        }

        int opcion = 0;

        do {
            Console.WriteLine("\n GESTION ACB 2025-2026 ");
            Console.WriteLine("1. Resultados de un equipo");
            Console.WriteLine("2. Resultados de una jornada");
            Console.WriteLine("3. Top 3 MVP");
            Console.WriteLine("4. Clasificación");
            Console.WriteLine("5. Crear ficheros por equipo");
            Console.WriteLine("6. Salir");
            Console.WriteLine("Elige una opción:");

            try {
                opcion = Convert.ToInt32(Console.ReadLine());

                switch (opcion) {
                    case 1:
                        ResultadosEquipo(partidos);
                        break;

                    case 2:
                        ResultadosJornada(partidos);
                        break;

                    case 3:
                        TopMVP(partidos);
                        break;

                    case 4:
                        Clasificacion(partidos);
                        break;

                    case 5:
                        CrearFicherosEquipos(partidos);
                        break;

                    case 6:
                        Console.WriteLine("Cerrando el programa...");
                        break;

                    default:
                        Console.WriteLine("Opción no válida.");
                        break;
                }
            }
            catch (Exception e) {
                Console.WriteLine("Error: " + e.Message);
                opcion = 0;
            }

            if (opcion != 6) {
                Console.WriteLine("\nPulsa cualquier tecla para continuar...");
                Console.ReadKey();
            }

        } while (opcion != 6);
    }


    static string[][] LeerCSV(string ruta) {
        string[] lineas = File.ReadAllLines(ruta);
        int contador = 0;

        for (int i = 1; i < lineas.Length; i++) {
            string[] campos = lineas[i].Split(',');

            if (campos.Length == 9) {
                contador++;
            }
        }

        string[][] partidos = new string[contador][];
        int posicion = 0;

        for (int i = 1; i < lineas.Length; i++) {
            string[] campos = lineas[i].Split(',');

            if (campos.Length == 9) {
                partidos[posicion] = campos;
                posicion++;
            }
        }

        return partidos;
    }


    static string MostrarPartido(string texto, string[] partido) {
        texto += "Jornada: " + partido[0] + "\n";
        texto += "Equipo local: " + partido[1] + "\n";
        texto += "Equipo visitante: " + partido[2] + "\n";
        texto += "Resultado: " + partido[3] + "\n";
        texto += "MVP: " + partido[4] + "\n";
        texto += "Puntos MVP: " + partido[5] + "\n";
        texto += "Asistencias MVP: " + partido[6] + "\n";
        texto += "Rebotes MVP: " + partido[7] + "\n";
        texto += "Equipo del MVP: " + partido[8] + "\n";
        texto += "--------------------------\n";

        return texto;
    }


    static void ResultadosEquipo(string[][] partidos) {
        Console.WriteLine("Introduce el nombre del equipo:");
        string equipo = Console.ReadLine();

        string texto = "";
        int contador = 0;

        for (int i = 0; i < partidos.Length; i++) {
            if (String.Compare(partidos[i][1], equipo, true) == 0 ||
                String.Compare(partidos[i][2], equipo, true) == 0) {
                texto = MostrarPartido(texto, partidos[i]);
                contador++;
            }
        }

        if (contador == 0) {
            Console.WriteLine("No se han encontrado partidos de ese equipo.");
        }
        else {
            Console.WriteLine(texto);
            MostrarYGuardar(texto);
        }
    }


    static void ResultadosJornada(string[][] partidos) {
        Console.WriteLine("Introduce el número de jornada:");
        int numero = Convert.ToInt32(Console.ReadLine());

        if (numero < 1 || numero > 34) {
            Console.WriteLine("Jornada no valida.");
            return;
        }
        

        string texto = "";
        int contador = 0;

        for (int i = 0; i < partidos.Length; i++) {
            string[] jornada = partidos[i][0].Split(' ');

            if (Convert.ToInt32(jornada[1]) == numero) {
                texto = MostrarPartido(texto, partidos[i]);
                contador++;
            }
        }

        if (contador == 0) {
            Console.WriteLine("No se han encontrado partidos.");
        }
        else {
            Console.WriteLine(texto);
            MostrarYGuardar(texto);
        }
    }


    static void TopMVP(string[][] partidos) {
        string[] jugadores = new string[partidos.Length];
        int[] cantidades = new int[partidos.Length];

        int contador = 0;

        for (int i = 0; i < partidos.Length; i++) {
            string jugador = partidos[i][4];
            int posicion = BuscarJugador(jugadores, contador, jugador);

            if (posicion == -1) {
                jugadores[contador] = jugador;
                cantidades[contador] = 1;
                contador++;
            }
            else {
                cantidades[posicion]++;
            }
        }

        string[] jugadoresFinal = new string[contador];
        int[] cantidadesFinal = new int[contador];

        for (int i = 0; i < contador; i++) {
            jugadoresFinal[i] = jugadores[i];
            cantidadesFinal[i] = cantidades[i];
        }


        for (int i = 0; i < contador - 1; i++) {
            for (int j = 0; j < contador - 1 - i; j++) {
                if (cantidadesFinal[j] < cantidadesFinal[j + 1] ||
                    (cantidadesFinal[j] == cantidadesFinal[j + 1] &&
                    String.Compare(jugadoresFinal[j], jugadoresFinal[j + 1], true) > 0)) {

                    int auxCantidad = cantidadesFinal[j];
                    cantidadesFinal[j] = cantidadesFinal[j + 1];
                    cantidadesFinal[j + 1] = auxCantidad;

                    string auxJugador = jugadoresFinal[j];
                    jugadoresFinal[j] = jugadoresFinal[j + 1];
                    jugadoresFinal[j + 1] = auxJugador;
                }
            }
        }

        string texto = "TOP 3 MVP\n";
        int limite = contador;

        if (limite > 3) {
            limite = 3;
        }

        for (int i = 0; i < limite; i++) {
            texto += (i + 1) + ". " + jugadoresFinal[i];
            texto += " - MVP: " + cantidadesFinal[i] + " veces\n";
        }

        Console.WriteLine(texto);
        MostrarYGuardar(texto);
    }


    static int BuscarJugador(string[] jugadores, int cantidad, string jugador) {
        for (int i = 0; i < cantidad; i++) {
            if (String.Compare(jugadores[i], jugador, true) == 0) {
                return i;
            }
        }

        return -1;
    }


    static string[] ObtenerEquipos(string[][] partidos) {
        string[] equipos = new string[partidos.Length * 2];
        int contador = 0;

        for (int i = 0; i < partidos.Length; i++) {
            if (!ExisteEquipo(equipos, contador, partidos[i][1])) {
                equipos[contador] = partidos[i][1];
                contador++;
            }

            if (!ExisteEquipo(equipos, contador, partidos[i][2])) {
                equipos[contador] = partidos[i][2];
                contador++;
            }
        }

        string[] equiposFinal = new string[contador];

        for (int i = 0; i < contador; i++) {
            equiposFinal[i] = equipos[i];
        }

        return equiposFinal;
    }


    static bool ExisteEquipo(string[] equipos, int cantidad, string equipo) {
        for (int i = 0; i < cantidad; i++) {
            if (String.Compare(equipos[i], equipo, true) == 0) {
                return true;
            }
        }

        return false;
    }


    static int BuscarEquipo(string[] equipos, string equipo) {
        for (int i = 0; i < equipos.Length; i++) {
            if (String.Compare(equipos[i], equipo, true) == 0) {
                return i;
            }
        }

        return -1;
    }


    static void Clasificacion(string[][] partidos) {
        string[] equipos = ObtenerEquipos(partidos);

        int[] victorias = new int[equipos.Length];
        int[] derrotas = new int[equipos.Length];
        int[] diferencias = new int[equipos.Length];

        for (int i = 0; i < partidos.Length; i++) {
            string[] resultado = partidos[i][3].Split('-');

            int puntosLocal = Convert.ToInt32(resultado[0]);
            int puntosVisitante = Convert.ToInt32(resultado[1]);

            int posicionLocal = BuscarEquipo(equipos, partidos[i][1]);
            int posicionVisitante = BuscarEquipo(equipos, partidos[i][2]);

            diferencias[posicionLocal] += puntosLocal - puntosVisitante;
            diferencias[posicionVisitante] += puntosVisitante - puntosLocal;

            if (puntosLocal > puntosVisitante) {
                victorias[posicionLocal]++;
                derrotas[posicionVisitante]++;
            }
            else {
                victorias[posicionVisitante]++;
                derrotas[posicionLocal]++;
            }
        }


        for (int i = 0; i < equipos.Length - 1; i++) {
            for (int j = 0; j < equipos.Length - 1 - i; j++) {
                if (victorias[j] < victorias[j + 1] ||
                    (victorias[j] == victorias[j + 1] &&
                    derrotas[j] > derrotas[j + 1]) ||
                    (victorias[j] == victorias[j + 1] &&
                    derrotas[j] == derrotas[j + 1] &&
                    diferencias[j] < diferencias[j + 1])) {

                    string auxEquipo = equipos[j];
                    equipos[j] = equipos[j + 1];
                    equipos[j + 1] = auxEquipo;

                    int auxVictorias = victorias[j];
                    victorias[j] = victorias[j + 1];
                    victorias[j + 1] = auxVictorias;

                    int auxDerrotas = derrotas[j];
                    derrotas[j] = derrotas[j + 1];
                    derrotas[j + 1] = auxDerrotas;

                    int auxDiferencias = diferencias[j];
                    diferencias[j] = diferencias[j + 1];
                    diferencias[j + 1] = auxDiferencias;
                }
            }
        }

        string texto = "CLASIFICACIÓN ACB\n";

        for (int i = 0; i < equipos.Length; i++) {
            texto += (i + 1) + ". " + equipos[i];
            texto += " | Victorias: " + victorias[i];
            texto += " | Derrotas: " + derrotas[i];
            texto += " | Diferencia de puntos: " + diferencias[i] + "\n";
        }

        Console.WriteLine(texto);
        MostrarYGuardar(texto);
    }


    static void MostrarYGuardar(string texto) {
        Console.WriteLine("¿Quieres guardar los resultados? (s/n)");
        string respuesta = Console.ReadLine();

        if (String.Compare(respuesta, "s", true) == 0) {
            Console.WriteLine("Introduce el nombre del archivo:");
            string nombre = Console.ReadLine();

            if (!nombre.EndsWith(".txt")) {
                nombre += ".txt";
            }

            Directory.CreateDirectory("../../../Archivos");

            File.WriteAllText("../../../Archivos/" + nombre, texto);

            Console.WriteLine("Archivo guardado correctamente.");
        }
    }


    static void CrearFicherosEquipos(string[][] partidos) {
        try {
            string[] equipos = ObtenerEquipos(partidos);

            Directory.CreateDirectory("../../../Archivos");

            for (int i = 0; i < equipos.Length; i++) {
                string texto = "";

                for (int j = 0; j < partidos.Length; j++) {
                    if (String.Compare(partidos[j][1], equipos[i], true) == 0 ||
                        String.Compare(partidos[j][2], equipos[i], true) == 0) {
                        texto = MostrarPartido(texto, partidos[j]);
                    }
                }

                File.WriteAllText("../../../Archivos/" + equipos[i] + ".txt", texto);
            }

            Console.WriteLine("Se han creado los archivos de todos los equipos.");
        }
        catch (Exception e) {
            Console.WriteLine("Error al crear los archivos: " + e.Message);
        }
    }
}
