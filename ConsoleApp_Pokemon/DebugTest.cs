using System;

namespace ConsoleApp_Pokemon;

public class DebugTest
{
    public static void TestCase3()
    {
        // Case 3: level=5, power=50, attackStat=100, defenseStat=50, modifier=2, expected=16, isSpecial=true

        int level = 5;
        int power = 50;
        int spAtk = 100;
        int spDef = 50;

        // Paso 1: levelFactor
        int levelFactor = 2 * (level / 5) + 2;
        Console.WriteLine($"Level={level}, levelFactor = 2 * ({level} / 5) + 2 = 2 * {level/5} + 2 = {levelFactor}");

        // Nueva fórmula: (levelFactor * (PWR * SpATK + 2 * SpDEF)) / (SpDEF * 50)
        int numerator = levelFactor * (power * spAtk + 2 * spDef);
        int denominator = spDef * 50;
        int baseDamage = numerator / denominator;
        Console.WriteLine($"numerator = {levelFactor} * ({power} * {spAtk} + 2 * {spDef}) = {levelFactor} * {power * spAtk + 2 * spDef} = {numerator}");
        Console.WriteLine($"denominator = {spDef} * 50 = {denominator}");
        Console.WriteLine($"baseDamage = {numerator} / {denominator} = {baseDamage}");

        // Paso 5: modifier=2 (super efectivo)
        int finalDamage = baseDamage * 2;
        Console.WriteLine($"finalDamage = {baseDamage} * 2 = {finalDamage}");

        Console.WriteLine($"\nEsperado: 16");
        Console.WriteLine($"Obtenido: {finalDamage}");
    }

    public static void TestCase4()
    {
        // Case 4: level=5, power=50, attackStat=100, defenseStat=50, modifier=1, expected=5, isSpecial=false (Physical)

        int level = 5;
        int power = 50;
        int atk = 100;
        int def = 50;

        // Paso 1: levelFactor
        int levelFactor = 2 * (level / 5) + 2;
        Console.WriteLine($"\n\n=== CASE 4 (Physical) ===");
        Console.WriteLine($"Level={level}, levelFactor = 2 * ({level} / 5) + 2 = 2 * {level/5} + 2 = {levelFactor}");

        // Nueva fórmula: (levelFactor * (PWR * ATK + 2 * DEF)) / (DEF * 50)
        int numerator = levelFactor * (power * atk + 2 * def);
        int denominator = def * 50;
        int baseDamage = numerator / denominator;
        Console.WriteLine($"numerator = {levelFactor} * ({power} * {atk} + 2 * {def}) = {levelFactor} * {power * atk + 2 * def} = {numerator}");
        Console.WriteLine($"denominator = {def} * 50 = {denominator}");
        Console.WriteLine($"baseDamage = {numerator} / {denominator} = {baseDamage}");

        // Paso 5: modifier=1 (normal)
        int finalDamage = baseDamage * 1;
        Console.WriteLine($"finalDamage = {baseDamage} * 1 = {finalDamage}");

        Console.WriteLine($"\nEsperado: 5");
        Console.WriteLine($"Obtenido: {finalDamage}");
    }
}
