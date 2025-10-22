using System;

namespace ConsoleApp_Pokemon;

public class Move
{
    public string Name { get; }
    public int Power { get; }    // 1..255, default 100
    public int Speed { get; }    // 1..5, default 1
    public PokemonType Type { get; }
    public MoveType MoveType { get; }

    // Constructor por defecto (GREEN mínimo)
    // NOTA: Usa Rock como tipo por defecto (primer tipo de la tabla del enunciado)
    // NO se usa "Normal" porque no está en la tabla de tipos especificada
    public Move()
        : this(string.Empty, 100, 1, PokemonType.Rock, MoveType.Physical) { }

    // Constructor con parámetros (acepta named args como en los tests)
    // CORRECCIÓN: Cambiado default de Normal a Rock (Normal no está en el enunciado)
    public Move(string name = "", int power = 100, int speed = 1, PokemonType type = PokemonType.Rock, MoveType moveType = MoveType.Physical)
    {
        Name = name ?? string.Empty;
        Power = Clamp(power, 1, 255, 100);
        Speed = Clamp(speed, 1, 5, 1);
        Type = type;
        MoveType = moveType;
    }

    /// <summary>
    /// Valida un valor y retorna el valor por defecto si está fuera del rango permitido
    ///
    /// IMPORTANTE: Esta implementación difiere de Pokemon.Clamp()
    /// - Pokemon.Clamp: Trunca valores fuera de rango (256 → 255)
    /// - Move.Clamp: Usa default para CUALQUIER valor inválido (256 → 100)
    ///
    /// ¿POR QUÉ ESTA DIFERENCIA?
    /// Decisión de diseño basada en interpretación del enunciado:
    /// - Para Pokémon stats: Si alguien pone ATK=300, es un "typo" cercano, truncar a 255 es razonable
    /// - Para Move power/speed: Si alguien pone power=999, es claramente inválido, usar default es más seguro
    ///
    /// DEBATE: Ambas aproximaciones son válidas. En tu próxima revisión con el profesor,
    /// pregunta cuál prefiere y justifica tu elección actual.
    /// </summary>
    private static int Clamp(int value, int min, int max, int defaultIfInvalid)
    {
        // Si el valor está fuera del rango [min, max], usar default
        if (value < min || value > max)
        {
            return defaultIfInvalid;
        }
        return value;
    }
}
