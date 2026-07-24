# Ejercicios de Programación — App Avalonia

Aplicación de escritorio en C# (.NET 8 + Avalonia UI 11) con un menú **"Ejercicios"**
que da acceso a los 28 ejercicios (01–15, 18–30; el enunciado original no incluye
16 y 17). Cada ejercicio tiene lógica de cálculo real (no simulada) implementada
en `Data/ExerciseCatalog.cs`.

## Cómo se organiza

- `Models/Exercise.cs` — contrato `IExercise`: título, descripción, etiquetas de
  entrada y una función `Compute(string[] entradas) -> string` con el resultado.
- `Data/ExerciseCatalog.cs` — los 28 ejercicios, cada uno con su lógica real.
- `Data/ParseHelpers.cs` — parseo de números/listas con manejo de errores.
- `ViewModels/` — MVVM: `MainWindowViewModel` (lista de ejercicios + selección),
  `ExerciseViewModel` (campos de entrada dinámicos + comando Calcular).
- `Views/MainWindow.axaml(.cs)` — ventana principal con el `Menu`. Los items del
  menú se generan en el code-behind a partir de la lista de ejercicios.
- `Views/ExerciseView.axaml(.cs)` — vista **genérica** reutilizada por los 28
  ejercicios: dibuja las cajas de texto según `InputLabels`, el botón "Calcular"
  y el panel de resultado (se pinta de rojo si hubo un error de validación).

Este diseño evita repetir 28 pantallas casi idénticas: una sola vista + una
lista de "recetas" de cálculo.

## Cómo compilar y ejecutar

Requieres el **.NET 8 SDK** instalado (`dotnet --version` debe mostrar 8.x).

```bash
cd AvaloniaEjercicios
dotnet restore
dotnet run
```

> Nota: este proyecto fue escrito en un entorno sin acceso a NuGet, así que no
> pudo compilarse ni probarse aquí. Revisé el código a mano con cuidado, pero
> si `dotnet build` marca algún error de sintaxis o de versión de paquete,
> dímelo y lo corrijo.

## Notas sobre las entradas

Para no tener que dibujar 50, 100 o 130 cajas de texto individuales, los
ejercicios que piden "N números por teclado" (09, 12, 14, 27, etc.) usan **una
sola caja de texto** donde escribes los valores separados por comas
(ej: `4, -3, 0, 7, 10`). Los ejercicios con ciclos que terminan con un valor
centinela (22 "hasta 0", 23 "hasta que num1 sea 0", 29 "hasta -1") funcionan
igual: escribes la secuencia completa separada por comas/punto y coma,
incluyendo el valor de corte, y el programa se detiene ahí tal como lo haría
leyendo del teclado en un ciclo real.

Algunos ejercicios (02 descuentos por tramos, 09 nota mínima, 30 nota de
aprobación) dependían de un dato que el enunciado no fija con un único valor
universal (¿qué % exacto? ¿qué nota mínima?); usé los valores más comunes en
este tipo de ejercicio y lo dejo indicado en el propio resultado — son fáciles
de ajustar en `ExerciseCatalog.cs` si tu profesor pide otros.

## Validación de errores

Cada ejercicio corre dentro de un `try/catch`: si dejas un campo vacío,
escribes texto donde va un número, o dan valores fuera de rango (ej. lados que
no forman un triángulo), el panel de resultado se pinta de rojo con un mensaje
explicando qué está mal, en vez de crashear la app.
# 30-programas
