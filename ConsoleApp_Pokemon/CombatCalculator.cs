using System;

namespace ConsoleApp_Pokemon;

/// <summary>
/// Calculadora de combate para sistema de Pokémon
///
/// LECCIÓN IMPORTANTE - ARITMÉTICA ENTERA vs PUNTO FLOTANTE:
/// Este código fue refactorizado de float a int para evitar errores de precisión.
///
/// ¿POR QUÉ ENTEROS?
/// 1. DETERMINISMO: 5/2 siempre da 2 (no 2.499999 o 2.500001)
/// 2. PRECISIÓN: No hay acumulación de errores de redondeo
/// 3. VIDEOJUEGOS: Casi todos los sistemas de combate usan enteros por consistencia
/// 4. ENUNCIADO: Todos los valores esperados son enteros, sin decimales
///
/// CONCEPTOS CLAVE:
/// - División entera: 7/2 = 3 (no 3.5)
/// - Multiplicar antes de dividir mantiene precisión: (a*b)/c mejor que a*(b/c)
/// - Modificadores 0.5x se representan como "dividir por 2"
/// </summary>
public class CombatCalculator
{
    /// <summary>
    /// Tabla de efectividad de tipos
    ///
    /// REPRESENTACIÓN:
    /// - 2 = Super efectivo (2x daño)
    /// - 1 = Daño normal (1x daño)
    /// - -1 = No muy efectivo (0.5x daño, representado como división por 2)
    /// - 0 = Inmune (0x daño)
    ///
    /// NOTA: Se mantiene la matriz bidimensional porque es:
    /// ✓ Compacta y visual
    /// ✓ Fácil de modificar valores
    /// ✓ O(1) lookup (búsqueda instantánea)
    ///
    /// El profesor sugiere switch, pero esta aproximación es igualmente válida.
    /// Ver CombatCalculator_SwitchAlternative.md para comparación.
    /// </summary>
    private static readonly int[,] TypeEffectiveness = new int[,]
    {
        // attacking   Defending
        //             Rock  Ground Water Electric Fire  Grass Ghost Poison Psychic Bug
        /* Rock     */ { 1,   -1,   1,    1,      2,    -1,   1,    1,     1,     2 },
        /* Ground   */ { 2,    1,   1,    2,      2,    -1,   1,    2,     1,    -1 },
        /* Water    */ { 2,    2,  -1,    1,      2,    -1,   1,    1,     1,     1 },
        /* Electric */ { 1,    0,   2,   -1,      1,    -1,   1,    1,     1,     1 },
        /* Fire     */ {-1,    1,  -1,    1,     -1,     2,   1,    1,     1,     2 },
        /* Grass    */ { 2,    2,   2,    1,     -1,    -1,   1,   -1,     1,    -1 },
        /* Ghost    */ { 1,    1,   1,    1,      1,     1,   2,    1,     2,     1 },
        /* Poison   */ {-1,   -1,   1,    1,      1,     2,  -1,   -1,     1,     1 },
        /* Psychic  */ { 1,    1,   1,    1,      1,     1,   1,    2,    -1,    -1 },
        /* Bug      */ { 1,    1,   1,    1,     -1,     2,   1,    1,     2,     1 }
    };



    /// <summary>
    /// Calcula el modificador de tipo (MOD) para un ataque
    ///
    /// CAMBIO IMPORTANTE: Ahora retorna un entero que representa el modificador acumulado
    ///
    /// VALORES DE RETORNO:
    /// - 0 = Inmune (0x daño)
    /// - 1 = Daño normal (1x)
    /// - 2 = Super efectivo simple (2x)
    /// - 4 = Super efectivo doble (4x) - ej. Water vs Rock/Ground
    /// - -1 = No muy efectivo (0.5x, se dividirá por 2 en CalculateDamage)
    /// - -2 = Resistencia doble (0.25x, se dividirá por 4)
    ///
    /// LÓGICA:
    /// - Si encuentra un 0 (inmune), retorna 0 inmediatamente
    /// - Acumula multiplicaciones positivas (2 * 2 = 4)
    /// - Acumula "divisiones" como negativos (-1 * -1 = -2 → dividir por 4)
    /// </summary>
    /// <param name="attackingType">Tipo del movimiento atacante</param>
    /// <param name="defendingTypes">Lista de tipos del Pokémon defensor</param>
    /// <returns>Modificador acumulado (ver documentación arriba)</returns>
    public static int CalculateTypeModifier(PokemonType attackingType, List<PokemonType> defendingTypes)
    {
        int totalModifier = 1;
        int divisionCount = 0; // Contador para 0.5x (representado como -1)

        // Si el defensor no tiene tipos, modificador = 1 (sin afectación)
        if (defendingTypes.Count == 0)
        {
            return totalModifier;
        }

        foreach (var defendingType in defendingTypes)
        {
            // Obtener índices en la tabla
            // Todos los tipos del enum (0-9) corresponden directamente a filas/columnas
            int attackIndex = (int)attackingType;
            int defendIndex = (int)defendingType;

            // Buscar efectividad en la tabla
            int modifier = TypeEffectiveness[attackIndex, defendIndex];

            // Si es inmune (0), retornar 0 inmediatamente
            if (modifier == 0)
            {
                return 0;
            }
            // Si es resistencia (-1 representa 0.5x)
            else if (modifier == -1)
            {
                divisionCount++;
            }
            // Si es super efectivo o normal (2 o 1)
            else
            {
                totalModifier *= modifier;
            }
        }

        // Aplicar divisiones acumuladas
        // divisionCount=1 → dividir por 2 (retornar -1)
        // divisionCount=2 → dividir por 4 (retornar -2)
        if (divisionCount > 0)
        {
            // Si no hay multiplicadores, el resultado es "resistido"
            if (totalModifier == 1)
            {
                // Retornar -divisionCount para indicar cuántas divisiones por 2 hacer
                // -1 = dividir por 2
                // -2 = dividir por 4
                return -divisionCount;
            }
            else
            {
                // Si hay multiplicadores Y divisiones, aplicar ambos
                // Ejemplo: 2x (multiplicador) y 0.5x (división) = 1x final
                totalModifier = totalModifier / (1 << divisionCount);
            }
        }

        return totalModifier;
    }

    /// <summary>
    /// Calcula el daño total de un ataque usando las fórmulas del enunciado
    ///
    /// REFACTORIZACIÓN CRÍTICA: De float a int
    ///
    /// PROBLEMA ANTERIOR:
    /// Usar float causaba errores de precisión acumulativos:
    /// - Test esperaba: 16
    /// - Resultado float: 16.32 ❌
    ///
    /// SOLUCIÓN: Aritmética entera pura
    /// - División entera: 5/2 = 2 (no 2.5)
    /// - Orden de operaciones crucial: multiplicar ANTES de dividir
    /// - Modificador 0.5x = dividir por 2 (no multiplicar por 0.5f)
    ///
    /// FÓRMULAS DEL ENUNCIADO:
    /// Physical: DMG = ((2 * LV/5 + 2) * (PWR * ATK/DEF + 2)) / 50 * MOD
    /// Special:  DMG = ((2 * LV/5 + 2) * (PWR * SpATK/SpDEF + 2)) / 50 * MOD
    ///
    /// DESCOMPOSICIÓN PASO A PASO (para evitar errores de precedencia):
    /// 1. levelFactor = 2 * (LV / 5) + 2
    /// 2. atkDefRatio = ATK * 100 / DEF (multiplicar por 100 para mantener precisión)
    /// 3. powerFactor = PWR * atkDefRatio / 100 + 2
    /// 4. baseDamage = levelFactor * powerFactor / 50
    /// 5. finalDamage = baseDamage * MOD (o / 2 si MOD es 0.5x)
    /// </summary>
    /// <param name="attacker">Pokémon atacante</param>
    /// <param name="move">Movimiento usado</param>
    /// <param name="defender">Pokémon defensor</param>
    /// <returns>Daño final (entero, con aritmética entera)</returns>
    public static int CalculateDamage(Pokemon attacker, Move move, Pokemon defender)
    {
        // PASO 1: Calcular modificador de tipo
        int modifier = CalculateTypeModifier(move.Type, defender.Types);

        // Si es inmune (modifier = 0), no hay daño
        if (modifier == 0)
        {
            return 0;
        }

        // PASO 2: Calcular factor de nivel
        // levelFactor = 2 * (Level / 5) + 2
        // Ejemplo: Level=50 → 2 * (50/5) + 2 = 2 * 10 + 2 = 22
        int levelFactor = 2 * (attacker.Level / 5) + 2;

        // PASO 3: Calcular daño base según tipo de movimiento
        int baseDamage;

        if (move.MoveType == MoveType.Physical)
        {
            // ATAQUE FÍSICO: Usa ATK vs DEF
            //
            // FÓRMULA ORIGINAL: ((2 * LV/5 + 2) * (PWR * ATK/DEF + 2)) / 50
            //
            // PROBLEMA DETECTADO:
            // Los valores esperados del enunciado fueron calculados con la división ATK/DEF
            // hecha como FLOTANTE, no como entera. Para replicar exactamente los resultados
            // esperados, necesitamos hacer esa división específica como double, luego truncar al final.
            //
            // SOLUCIÓN HÍBRIDA:
            // - LV/5 → División entera (ya en levelFactor)
            // - ATK/DEF → División double para mantener decimales intermedios
            // - /50 → División double
            // - Truncar al final a int
            //
            // Esto replica el comportamiento original que generó los valores esperados.

            double atkDefRatio = (double)attacker.Atk / defender.Def;
            double powerFactor = move.Power * atkDefRatio + 2;
            baseDamage = (int)Math.Floor(levelFactor * powerFactor / 50);
        }
        else // MoveType.Special
        {
            // ATAQUE ESPECIAL: Usa SpATK vs SpDEF
            // Misma lógica que Physical

            double spAtkDefRatio = (double)attacker.SpAtk / defender.SpDef;
            double powerFactor = move.Power * spAtkDefRatio + 2;
            baseDamage = (int)Math.Floor(levelFactor * powerFactor / 50);
        }

        // PASO 4: Aplicar modificador de tipo
        int finalDamage;

        if (modifier > 0)
        {
            // Multiplicadores positivos (1x, 2x, 4x)
            finalDamage = baseDamage * modifier;
        }
        else
        {
            // Modificadores negativos representan divisiones por potencias de 2
            // -1 → dividir por 2^1 = 2 (0.5x)
            // -2 → dividir por 2^2 = 4 (0.25x)
            // Usamos bit shift: 1 << n = 2^n
            int divisionCount = -modifier;
            int divisor = 1 << divisionCount; // 2^divisionCount
            finalDamage = baseDamage / divisor;
        }

        // PASO 5: Daño mínimo de 1 (si no es inmune)
        // Según mecánicas de Pokémon, cualquier ataque que no sea inmune hace al menos 1 de daño
        return Math.Max(1, finalDamage);
    }
}
