using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using AvaloniaEjercicios.Models;
using static AvaloniaEjercicios.Data.ParseHelpers;

namespace AvaloniaEjercicios.Data;

public static class ExerciseCatalog
{
    public static readonly IReadOnlyList<IExercise> Exercises = new List<IExercise>
    {
        // 01 -----------------------------------------------------------
        new Exercise(
            "01", "Salario y horas extra",
            "Calcula el salario según horas trabajadas y tarifa; las horas que exceden 40 se pagan con 50% de recargo.",
            new[] { "Horas trabajadas", "Tarifa por hora" },
            args =>
            {
                double horas = D(args[0], "horas trabajadas");
                double tarifa = D(args[1], "tarifa por hora");
                double horasNormales = Math.Min(horas, 40);
                double horasExtra = Math.Max(0, horas - 40);
                double pagoNormal = horasNormales * tarifa;
                double pagoExtra = horasExtra * tarifa * 1.5;
                double total = pagoNormal + pagoExtra;
                var sb = new StringBuilder();
                sb.AppendLine($"Horas normales: {Num(horasNormales)} -> {Money(pagoNormal)}");
                sb.AppendLine($"Horas extra: {Num(horasExtra)} -> {Money(pagoExtra)} (tarifa x1.5)");
                sb.AppendLine($"Salario total: {Money(total)}");
                return sb.ToString();
            },
            new[] { "ej: 45", "ej: 100" }),

        // 02 -----------------------------------------------------------
        new Exercise(
            "02", "Descuentos escalonados",
            "Aplica un descuento por tramos: hasta 1000 (0%), entre 1000 y 2000 (10%), más de 2000 (20%).",
            new[] { "Sueldo bruto" },
            args =>
            {
                double sueldo = D(args[0], "sueldo");
                double pct = sueldo <= 1000 ? 0.0 : sueldo <= 2000 ? 0.10 : 0.20;
                double descuento = sueldo * pct;
                double neto = sueldo - descuento;
                return $"Tramo aplicado: {(pct == 0 ? "hasta 1000 (0%)" : pct == 0.10 ? "1000-2000 (10%)" : "mayor a 2000 (20%)")}\n" +
                       $"Descuento: {Money(descuento)}\n" +
                       $"Sueldo neto: {Money(neto)}\n" +
                       "(Los tramos son un supuesto habitual del ejercicio; ajústalos si tu enunciado da otros %.)";
            },
            new[] { "ej: 1500" }),

        // 03 -----------------------------------------------------------
        new Exercise(
            "03", "Descuento según monto",
            "10% de descuento si el monto es mayor a 100; 2% si es menor o igual a 100.",
            new[] { "Monto" },
            args =>
            {
                double monto = D(args[0], "monto");
                double pct = monto > 100 ? 0.10 : 0.02;
                double descuento = monto * pct;
                double total = monto - descuento;
                return $"Porcentaje aplicado: {pct * 100:0}%\nDescuento: {Money(descuento)}\nTotal a pagar: {Money(total)}";
            },
            new[] { "ej: 250" }),

        // 04 -----------------------------------------------------------
        new Exercise(
            "04", "Segundos hasta el próximo minuto",
            "Dado un tiempo en segundos, indica cuántos segundos faltan para completar el próximo minuto exacto.",
            new[] { "Segundos" },
            args =>
            {
                int segundos = I(args[0], "segundos");
                if (segundos < 0) throw new FormatException("Los segundos no pueden ser negativos.");
                int minutosCompletos = segundos / 60;
                int segundosRestantes = segundos % 60;
                int faltan = segundosRestantes == 0 ? 0 : 60 - segundosRestantes;
                return $"Minutos completos: {minutosCompletos}\nSegundos ya transcurridos del minuto actual: {segundosRestantes}\nSegundos que faltan para el próximo minuto: {faltan}";
            },
            new[] { "ej: 133" }),

        // 05 -----------------------------------------------------------
        new Exercise(
            "05", "Minutos a días, horas y minutos",
            "Convierte una cantidad total de minutos a su equivalente en días, horas y minutos.",
            new[] { "Minutos totales" },
            args =>
            {
                long minutos = (long)D(args[0], "minutos");
                if (minutos < 0) throw new FormatException("Los minutos no pueden ser negativos.");
                long dias = minutos / 1440;
                long resto = minutos % 1440;
                long horas = resto / 60;
                long minsRestantes = resto % 60;
                return $"{minutos} minutos = {dias} día(s), {horas} hora(s), {minsRestantes} minuto(s)";
            },
            new[] { "ej: 4321" }),

        // 06 -----------------------------------------------------------
        new Exercise(
            "06", "Suma de los N primeros naturales",
            "Suma acumulada de la serie 1 + 2 + ... + N usando un bucle.",
            new[] { "N" },
            args =>
            {
                int n = I(args[0], "N");
                if (n < 1) throw new FormatException("N debe ser mayor o igual a 1.");
                long suma = 0;
                for (int i = 1; i <= n; i++) suma += i;
                return $"Suma de 1 a {n} = {suma}";
            },
            new[] { "ej: 100" }),

        // 07 -----------------------------------------------------------
        new Exercise(
            "07", "Suma acumulada de salarios (varios trabajadores)",
            "Igual que el Ejercicio 01 pero para varios trabajadores. Ingresa pares 'horas,tarifa' separados por punto y coma.",
            new[] { "Trabajadores (horas,tarifa; horas,tarifa; ...)" },
            args =>
            {
                var pares = args[0].Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                if (pares.Length == 0) throw new FormatException("Ingresa al menos un trabajador.");
                var sb = new StringBuilder();
                double totalGeneral = 0;
                int idx = 1;
                foreach (var par in pares)
                {
                    var partes = par.Split(',', StringSplitOptions.TrimEntries);
                    if (partes.Length != 2)
                        throw new FormatException($"'{par}' debe tener el formato horas,tarifa.");
                    double horas = D(partes[0], "horas");
                    double tarifa = D(partes[1], "tarifa");
                    double normales = Math.Min(horas, 40) * tarifa;
                    double extra = Math.Max(0, horas - 40) * tarifa * 1.5;
                    double salario = normales + extra;
                    totalGeneral += salario;
                    sb.AppendLine($"Trabajador {idx}: {Num(horas)} h @ {Money(tarifa)} -> {Money(salario)}");
                    idx++;
                }
                sb.AppendLine($"---\nSuma total de salarios: {Money(totalGeneral)}");
                return sb.ToString();
            },
            new[] { "ej: 45,10; 38,12; 50,8" }),

        // 08 -----------------------------------------------------------
        new Exercise(
            "08", "Salario simple",
            "Multiplica directamente las horas trabajadas por la tarifa por hora.",
            new[] { "Horas trabajadas", "Tarifa por hora" },
            args =>
            {
                double horas = D(args[0], "horas");
                double tarifa = D(args[1], "tarifa");
                return $"Salario = {Num(horas)} x {Money(tarifa)} = {Money(horas * tarifa)}";
            },
            new[] { "ej: 40", "ej: 15" }),

        // 09 -----------------------------------------------------------
        new Exercise(
            "09", "Estadística y promedio de notas",
            "Cuenta aprobados/desaprobados y calcula promedio general, de aprobados y de desaprobados.",
            new[] { "Notas (separadas por coma)", "Nota mínima para aprobar" },
            args =>
            {
                double[] notas = DList(args[0], "notas");
                double minima = D(args[1], "nota mínima");
                var aprobados = notas.Where(n => n >= minima).ToArray();
                var desaprobados = notas.Where(n => n < minima).ToArray();
                var sb = new StringBuilder();
                sb.AppendLine($"Total de notas: {notas.Length}");
                sb.AppendLine($"Aprobados: {aprobados.Length}  |  Desaprobados: {desaprobados.Length}");
                sb.AppendLine($"Promedio general: {Num(notas.Average())}");
                sb.AppendLine(aprobados.Length > 0
                    ? $"Promedio de aprobados: {Num(aprobados.Average())}"
                    : "Promedio de aprobados: N/A (no hay aprobados)");
                sb.AppendLine(desaprobados.Length > 0
                    ? $"Promedio de desaprobados: {Num(desaprobados.Average())}"
                    : "Promedio de desaprobados: N/A (no hay desaprobados)");
                return sb.ToString();
            },
            new[] { "ej: 12, 8, 15, 20, 5, 11", "ej: 11" }),

        // 10 -----------------------------------------------------------
        new Exercise(
            "10", "Suma de dígitos de un número",
            "Determina la suma de los dígitos que componen un número entero.",
            new[] { "Número" },
            args =>
            {
                long n = (long)D(args[0], "número");
                long original = n;
                n = Math.Abs(n);
                long suma = 0;
                if (n == 0) suma = 0;
                while (n > 0)
                {
                    suma += n % 10;
                    n /= 10;
                }
                return $"La suma de los dígitos de {original} es {suma}";
            },
            new[] { "ej: 48293" }),

        // 11 -----------------------------------------------------------
        new Exercise(
            "11", "Factura con IVA y descuento",
            "Calcula el IVA (15%) sobre el precio de venta y aplica un 5% de descuento si el total bruto supera 50.",
            new[] { "Precio de venta (neto)" },
            args =>
            {
                double precio = D(args[0], "precio de venta");
                double iva = precio * 0.15;
                double bruto = precio + iva;
                bool aplicaDescuento = bruto > 50;
                double descuento = aplicaDescuento ? bruto * 0.05 : 0;
                double totalFinal = bruto - descuento;
                var sb = new StringBuilder();
                sb.AppendLine($"Precio neto: {Money(precio)}");
                sb.AppendLine($"IVA (15%): {Money(iva)}");
                sb.AppendLine($"Precio bruto: {Money(bruto)}");
                sb.AppendLine(aplicaDescuento
                    ? $"Descuento (5%, bruto > 50): {Money(descuento)}"
                    : "Sin descuento (bruto no supera 50)");
                sb.AppendLine($"Total a pagar: {Money(totalFinal)}");
                return sb.ToString();
            },
            new[] { "ej: 60" }),

        // 12 -----------------------------------------------------------
        new Exercise(
            "12", "Clasificación de números",
            "Analiza una lista de números (pensada para 50) e informa cuántos son pares, impares, positivos y negativos.",
            new[] { "Números (separados por coma)" },
            args =>
            {
                double[] nums = DList(args[0], "números");
                int pares = nums.Count(x => x % 2 == 0);
                int impares = nums.Length - pares;
                int positivos = nums.Count(x => x > 0);
                int negativos = nums.Count(x => x < 0);
                int ceros = nums.Count(x => x == 0);
                return $"Cantidad analizada: {nums.Length}\nPares: {pares}\nImpares: {impares}\nPositivos: {positivos}\nNegativos: {negativos}\nCeros: {ceros}";
            },
            new[] { "ej: 4,-3,0,7,10,-8,15" }),

        // 13 -----------------------------------------------------------
        new Exercise(
            "13", "Factorial de un número",
            "Calcula N! usando un ciclo iterativo.",
            new[] { "N" },
            args =>
            {
                int n = I(args[0], "N");
                if (n < 0) throw new FormatException("N debe ser mayor o igual a 0.");
                if (n > 20) throw new FormatException("Usa N <= 20 para evitar desbordamiento de long.");
                long resultado = 1;
                for (int i = 2; i <= n; i++) resultado *= i;
                return $"{n}! = {resultado}";
            },
            new[] { "ej: 6" }),

        // 14 -----------------------------------------------------------
        new Exercise(
            "14", "Promedio de números",
            "Calcula la media aritmética de una lista de números (pensada para 100).",
            new[] { "Números (separados por coma)" },
            args =>
            {
                double[] nums = DList(args[0], "números");
                return $"Cantidad de números: {nums.Length}\nSuma: {Num(nums.Sum())}\nPromedio: {Num(nums.Average())}";
            },
            new[] { "ej: 8,9,7,10,6" }),

        // 15 -----------------------------------------------------------
        new Exercise(
            "15", "Suma y producto de pares en un rango",
            "Calcula la suma y el producto de los números pares comprendidos entre dos límites (ej. 20 y 400).",
            new[] { "Inicio del rango", "Fin del rango" },
            args =>
            {
                int inicio = I(args[0], "inicio");
                int fin = I(args[1], "fin");
                if (fin < inicio) throw new FormatException("El fin del rango debe ser mayor o igual al inicio.");
                long suma = 0;
                double producto = 1;
                int cantidad = 0;
                for (int i = inicio; i <= fin; i++)
                {
                    if (i % 2 == 0)
                    {
                        suma += i;
                        producto *= i;
                        cantidad++;
                    }
                }
                string prodTexto = double.IsInfinity(producto) ? "demasiado grande para mostrar (overflow)" : Num(producto);
                return $"Pares encontrados: {cantidad}\nSuma de pares: {suma}\nProducto de pares: {prodTexto}";
            },
            new[] { "ej: 20", "ej: 40" }),

        // 18 -----------------------------------------------------------
        new Exercise(
            "18", "Detección de la primera vocal",
            "Recorre una cadena de caracteres e identifica la primera vocal (a, e, i, o, u) ingresada.",
            new[] { "Texto" },
            args =>
            {
                string texto = args[0];
                if (string.IsNullOrEmpty(texto)) throw new FormatException("Ingresa un texto.");
                string vocales = "aeiouAEIOU";
                for (int i = 0; i < texto.Length; i++)
                {
                    if (vocales.IndexOf(texto[i]) >= 0)
                        return $"Primera vocal encontrada: '{texto[i]}' (posición {i + 1})";
                }
                return "No se encontró ninguna vocal en el texto.";
            },
            new[] { "ej: xyz Wonder" }),

        // 19 -----------------------------------------------------------
        new Exercise(
            "19", "Verificación de parte fraccionaria",
            "Evalúa si un número flotante ingresado contiene o no decimales.",
            new[] { "Número" },
            args =>
            {
                double n = D(args[0], "número");
                double parteFraccionaria = n - Math.Truncate(n);
                bool tieneDecimales = Math.Abs(parteFraccionaria) > 1e-9;
                return tieneDecimales
                    ? $"{Num(n)} sí tiene parte fraccionaria ({Num(Math.Abs(parteFraccionaria))})."
                    : $"{Num(n)} es un número entero (sin decimales).";
            },
            new[] { "ej: 12.5" }),

        // 20 -----------------------------------------------------------
        new Exercise(
            "20", "Ecuación cuadrática",
            "Resuelve aX² + bX + c = 0, contemplando discriminante positivo, nulo o negativo (raíces complejas).",
            new[] { "a", "b", "c" },
            args =>
            {
                double a = D(args[0], "a");
                double b = D(args[1], "b");
                double c = D(args[2], "c");
                if (a == 0) throw new FormatException("'a' no puede ser 0 (no sería una ecuación cuadrática).");
                double disc = b * b - 4 * a * c;
                if (disc > 0)
                {
                    double x1 = (-b + Math.Sqrt(disc)) / (2 * a);
                    double x2 = (-b - Math.Sqrt(disc)) / (2 * a);
                    return $"Discriminante = {Num(disc)} (positivo, dos raíces reales)\nX1 = {Num(x1)}\nX2 = {Num(x2)}";
                }
                if (disc == 0)
                {
                    double x = -b / (2 * a);
                    return $"Discriminante = 0 (raíz doble)\nX = {Num(x)}";
                }
                double real = -b / (2 * a);
                double imag = Math.Sqrt(-disc) / (2 * a);
                return $"Discriminante = {Num(disc)} (negativo, raíces complejas)\nX1 = {Num(real)} + {Num(imag)}i\nX2 = {Num(real)} - {Num(imag)}i";
            },
            new[] { "ej: 1", "ej: -3", "ej: 2" }),

        // 21 -----------------------------------------------------------
        new Exercise(
            "21", "Operaciones aritméticas iterativas (x10)",
            "Suma, resta, multiplicación y división de dos números, repitiendo el ciclo 10 veces tal como pide el enunciado.",
            new[] { "Número 1", "Número 2" },
            args =>
            {
                double n1 = D(args[0], "número 1");
                double n2 = D(args[1], "número 2");
                var sb = new StringBuilder();
                for (int i = 1; i <= 10; i++)
                {
                    string div = n2 == 0 ? "indefinida (división por 0)" : Num(n1 / n2);
                    sb.AppendLine($"Iteración {i,2}: suma={Num(n1 + n2)}  resta={Num(n1 - n2)}  mult={Num(n1 * n2)}  div={div}");
                }
                return sb.ToString();
            },
            new[] { "ej: 8", "ej: 3" }),

        // 22 -----------------------------------------------------------
        new Exercise(
            "22", "Cubo y raíz cuadrada hasta ingresar 0",
            "Procesa una secuencia de números calculando su cubo y raíz cuadrada, deteniéndose al llegar a 0. Ingresa la secuencia separada por comas.",
            new[] { "Secuencia de números (termina al llegar a 0)" },
            args =>
            {
                double[] nums = DList(args[0], "secuencia");
                var sb = new StringBuilder();
                foreach (var n in nums)
                {
                    if (n == 0)
                    {
                        sb.AppendLine("Se ingresó 0: fin del ciclo.");
                        break;
                    }
                    double cubo = n * n * n;
                    string raiz = n < 0 ? "no definida en reales (número negativo)" : Num(Math.Sqrt(n));
                    sb.AppendLine($"n={Num(n)} -> cubo={Num(cubo)}, raíz cuadrada={raiz}");
                }
                if (!nums.Contains(0))
                    sb.AppendLine("(No se incluyó un 0 en la secuencia; en un programa interactivo real el ciclo seguiría pidiendo datos.)");
                return sb.ToString();
            },
            new[] { "ej: 4,9,16,-2,0,99" }),

        // 23 -----------------------------------------------------------
        new Exercise(
            "23", "Calculadora básica con parada en 0",
            "Realiza las 4 operaciones básicas entre pares de números hasta que el primer número de un par sea 0. Ingresa pares 'n1,n2' separados por punto y coma.",
            new[] { "Pares num1,num2 (termina cuando num1 = 0)" },
            args =>
            {
                var pares = args[0].Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                var sb = new StringBuilder();
                bool detenido = false;
                foreach (var par in pares)
                {
                    var partes = par.Split(',', StringSplitOptions.TrimEntries);
                    if (partes.Length != 2) throw new FormatException($"'{par}' debe tener el formato num1,num2.");
                    double n1 = D(partes[0], "num1");
                    double n2 = D(partes[1], "num2");
                    if (n1 == 0)
                    {
                        sb.AppendLine("num1 = 0: fin del ciclo.");
                        detenido = true;
                        break;
                    }
                    string div = n2 == 0 ? "indefinida (división por 0)" : Num(n1 / n2);
                    sb.AppendLine($"{Num(n1)}, {Num(n2)} -> suma={Num(n1 + n2)} resta={Num(n1 - n2)} mult={Num(n1 * n2)} div={div}");
                }
                if (!detenido)
                    sb.AppendLine("(No se incluyó un par con num1 = 0; agrega uno para ver dónde se detiene el ciclo.)");
                return sb.ToString();
            },
            new[] { "ej: 8,2; 5,0; 0,9" }),

        // 24 -----------------------------------------------------------
        new Exercise(
            "24", "Área de un triángulo (fórmula de Herón)",
            "Calcula el área de un triángulo dados sus tres lados, usando el semiperímetro.",
            new[] { "Lado A", "Lado B", "Lado C" },
            args =>
            {
                double a = D(args[0], "lado A");
                double b = D(args[1], "lado B");
                double c = D(args[2], "lado C");
                if (a <= 0 || b <= 0 || c <= 0) throw new FormatException("Los lados deben ser mayores a 0.");
                if (a + b <= c || a + c <= b || b + c <= a)
                    throw new FormatException("Esos lados no forman un triángulo válido (desigualdad triangular).");
                double s = (a + b + c) / 2;
                double area = Math.Sqrt(s * (s - a) * (s - b) * (s - c));
                return $"Semiperímetro (s): {Num(s)}\nÁrea: {Num(area)}";
            },
            new[] { "ej: 3", "ej: 4", "ej: 5" }),

        // 25 -----------------------------------------------------------
        new Exercise(
            "25", "Hipotenusa (Teorema de Pitágoras)",
            "Calcula la hipotenusa de un triángulo rectángulo a partir de los dos catetos.",
            new[] { "Cateto A", "Cateto B" },
            args =>
            {
                double a = D(args[0], "cateto A");
                double b = D(args[1], "cateto B");
                if (a <= 0 || b <= 0) throw new FormatException("Los catetos deben ser mayores a 0.");
                double hip = Math.Sqrt(a * a + b * b);
                return $"Hipotenusa = √({Num(a)}² + {Num(b)}²) = {Num(hip)}";
            },
            new[] { "ej: 3", "ej: 4" }),

        // 26 -----------------------------------------------------------
        new Exercise(
            "26", "Geometría del círculo y la esfera",
            "Calcula la circunferencia, el área del círculo y el volumen de la esfera para un radio dado.",
            new[] { "Radio" },
            args =>
            {
                double r = D(args[0], "radio");
                if (r <= 0) throw new FormatException("El radio debe ser mayor a 0.");
                double circunferencia = 2 * Math.PI * r;
                double areaCirculo = Math.PI * r * r;
                double volumenEsfera = (4.0 / 3.0) * Math.PI * r * r * r;
                return $"Circunferencia: {Num(circunferencia)}\nÁrea del círculo: {Num(areaCirculo)}\nVolumen de la esfera: {Num(volumenEsfera)}";
            },
            new[] { "ej: 5" }),

        // 27 -----------------------------------------------------------
        new Exercise(
            "27", "Consumos en restaurante",
            "Procesa una lista de consumos (pensada para 130), aplicando 15% de descuento a los que superen $130, y suma el total general.",
            new[] { "Consumos (separados por coma)" },
            args =>
            {
                double[] consumos = DList(args[0], "consumos");
                var sb = new StringBuilder();
                double total = 0;
                int conDescuento = 0;
                for (int i = 0; i < consumos.Length; i++)
                {
                    double c = consumos[i];
                    bool aplica = c > 130;
                    double final = aplica ? c * 0.85 : c;
                    if (aplica) conDescuento++;
                    total += final;
                    sb.AppendLine($"Consumo {i + 1}: {Money(c)}{(aplica ? $" -> con 15% desc. = {Money(final)}" : "")}");
                }
                sb.AppendLine($"---\nConsumos con descuento: {conDescuento} de {consumos.Length}");
                sb.AppendLine($"Total general acumulado: {Money(total)}");
                return sb.ToString();
            },
            new[] { "ej: 120,150,200,80" }),

        // 28 -----------------------------------------------------------
        new Exercise(
            "28", "Suma de serie desde 8 hasta N",
            "Suma los enteros consecutivos S = 8 + 9 + 10 + ... + N.",
            new[] { "N" },
            args =>
            {
                int n = I(args[0], "N");
                if (n < 8) throw new FormatException("N debe ser mayor o igual a 8.");
                long suma = 0;
                for (int i = 8; i <= n; i++) suma += i;
                return $"S = 8 + 9 + ... + {n} = {suma}";
            },
            new[] { "ej: 20" }),

        // 29 -----------------------------------------------------------
        new Exercise(
            "29", "Balance de egresos de caja",
            "Resta egresos diarios a una caja inicial de 371, acumulando hasta que se ingrese -1 como valor de corte.",
            new[] { "Egresos (separados por coma, termina en -1)" },
            args =>
            {
                const double cajaInicial = 371;
                double[] egresos = DList(args[0], "egresos");
                var sb = new StringBuilder();
                double caja = cajaInicial;
                double totalEgresos = 0;
                bool corteEncontrado = false;
                sb.AppendLine($"Caja inicial: {Money(cajaInicial)}");
                foreach (var e in egresos)
                {
                    if (e == -1)
                    {
                        sb.AppendLine("Se ingresó -1: fin del registro de egresos.");
                        corteEncontrado = true;
                        break;
                    }
                    caja -= e;
                    totalEgresos += e;
                    sb.AppendLine($"Egreso: {Money(e)}  ->  caja restante: {Money(caja)}");
                }
                if (!corteEncontrado)
                    sb.AppendLine("(No se incluyó -1 en la lista; agrégalo para marcar el corte del día.)");
                sb.AppendLine($"---\nTotal egresado: {Money(totalEgresos)}\nCaja final: {Money(caja)}");
                return sb.ToString();
            },
            new[] { "ej: 50,30,15,-1" }),

        // 30 -----------------------------------------------------------
        new Exercise(
            "30", "Evaluación académica de dos notas",
            "Evalúa el promedio de dos notas en escala vigesimal (0-20) e indica si el estado es aprobado o desaprobado.",
            new[] { "Nota 1 (0-20)", "Nota 2 (0-20)" },
            args =>
            {
                double n1 = D(args[0], "nota 1");
                double n2 = D(args[1], "nota 2");
                if (n1 < 0 || n1 > 20 || n2 < 0 || n2 > 20)
                    throw new FormatException("Las notas deben estar entre 0 y 20.");
                double promedio = (n1 + n2) / 2;
                bool aprobado = promedio >= 11;
                return $"Promedio: {Num(promedio)} / 20\nEstado: {(aprobado ? "APROBADO" : "DESAPROBADO")}\n(Se usa 11 como nota mínima de aprobación, escala vigesimal habitual.)";
            },
            new[] { "ej: 14", "ej: 9" }),
    };
}
