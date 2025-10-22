using System;

namespace ConsoleApp_Pokemon;

public class Pokemon
{
    // Estas son propiedades de solo lectura (get only)
    // get only porque ya se asignan en el constructor entonces no hace falta set
    public string Name { get; }
    public int Level { get; }    // 1..99 (default 1)
    public int Atk { get; }      // 1..255 (default 10)
    public int Def { get; }      // 1..255 (default 10)
    public int SpAtk { get; }    // 1..255 (default 10)
    public int SpDef { get; }    // 1..255 (default 10)

    // Una especie puede tener 0, 1 o 2 tipos. (lista para facilidad)
    public List<PokemonType> Types { get; }

    // Movimientos: mínimo 1, máximo 4.
    public List<Move> Moves { get; }

    // Constructor por defecto: valores por defecto según enunciado,
    // y al menos un movimiento por defecto.
    public Pokemon() : this(
                            string.Empty,
                                       1,
                                      10,
                                      10,
                                      10,
                                      10,
                                    null,
                                    null)
    { }
    // Constructor completo (acepta named params como en tests)
    public Pokemon(string name = "",
                     int level = 1,
                       int atk = 10,
                       int def = 10,
                     int spAtk = 10,
                     int spDef = 10,
      List<PokemonType>? types = null,
             List<Move>? moves = null)
    {
        Name = name ?? string.Empty;
        Level = Clamp(level, 1, 99, 1);

        // Clamp estricto 1..255
        Atk = Clamp(atk, 1, 255, 10);
        Def = Clamp(def, 1, 255, 10);
        SpAtk = Clamp(spAtk, 1, 255, 10);
        SpDef = Clamp(spDef, 1, 255, 10);

        // VALIDACIÓN DE TIPOS:
        // 1. Si types es null, crear lista vacía
        // 2. .Distinct() elimina duplicados (Fire/Fire → Fire)
        //    Justificación: En Pokémon no existen especies con tipos duplicados
        //    Ejemplo: No existe un Pokémon Fire/Fire, pero sí Fire/Flying
        // 3. .ToList() convierte el resultado de Distinct() a List<T>
        // 4. Limitar a máximo 2 tipos (según enunciado)
        //
        // ¿POR QUÉ .Distinct()?
        // - LINQ (Language Integrated Query) es una herramienta poderosa de C#
        // - .Distinct() elimina elementos duplicados usando el comparador por defecto
        // - Para enums como PokemonType, compara por valor (Fire == Fire)
        Types = (types ?? new List<PokemonType>()).Distinct().ToList();
        if (Types.Count > 2)
        {
            Types = Types.GetRange(0, 2);
        }

        Moves = moves ?? new List<Move> { new Move() }; // asegurar al menos 1
        if (Moves.Count < 1)
        {
            Moves.Add(new Move());
        }
        else if (Moves.Count > 4)
        {
            Moves = Moves.GetRange(0, 4);
        }
    }

    private static int Clamp(int value, int min, int max, int defaultValue)
    {
        if (value < min) return defaultValue;
        if (value > max) return max;
        return value;
    }
}
