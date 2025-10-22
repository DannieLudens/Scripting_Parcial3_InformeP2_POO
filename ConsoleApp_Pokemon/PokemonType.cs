using System;

namespace ConsoleApp_Pokemon;

/// <summary>
/// Tipos de Pokémon según la tabla del enunciado
///
/// IMPORTANTE: Este enum contiene SOLO los 10 tipos especificados en la tabla
/// de efectividad del enunciado. NO se incluyen tipos adicionales como Normal
/// que existen en los juegos Pokémon pero no fueron solicitados.
///
/// CORRECCIÓN: Se eliminó "Normal" que fue agregado por error.
/// El profesor señaló que no debemos agregar tipos que no están en el ejercicio,
/// aunque existan en otras generaciones de Pokémon.
///
/// Los índices 0-9 corresponden directamente a las filas/columnas de la tabla
/// de efectividad en CombatCalculator.
/// </summary>
public enum PokemonType
{
    Rock = 0,      // Índice 0 en tabla
    Ground = 1,    // Índice 1 en tabla
    Water = 2,     // Índice 2 en tabla
    Electric = 3,  // Índice 3 en tabla
    Fire = 4,      // Índice 4 en tabla
    Grass = 5,     // Índice 5 en tabla
    Ghost = 6,     // Índice 6 en tabla
    Poison = 7,    // Índice 7 en tabla
    Psychic = 8,   // Índice 8 en tabla
    Bug = 9        // Índice 9 en tabla
}
