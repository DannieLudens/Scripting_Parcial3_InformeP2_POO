using ConsoleApp_Pokemon;

namespace TestProject_Pokemon;

/// <summary>
/// TESTS DE VALIDACIÓN - Casos Borde (Edge Cases)
///
/// ¿QUÉ SON LOS EDGE CASES?
/// Son valores "extremos" o inusuales que pueden romper tu código:
/// - Valores negativos donde esperas positivos
/// - Valores mayores al máximo permitido
/// - Valores duplicados donde no deberían existir
/// - Listas vacías o con demasiados elementos
///
/// ¿POR QUÉ TESTEAR EDGE CASES?
/// 1. PREVENCIÓN: Detectan bugs antes que los usuarios
/// 2. DOCUMENTACIÓN: Muestran cómo debe comportarse el código en casos raros
/// 3. CONFIANZA: Si los edge cases funcionan, los casos normales también
/// 4. CALIFICACIÓN: El profesor espera ver que pensaste en estos casos
///
/// PRINCIPIO TDD: "Si no está testeado, no existe"
/// Tu código SÍ valida con Clamp(), pero sin tests, el profesor no lo puede verificar.
/// </summary>
[TestFixture]
public class ValidationTests
{
    #region Tests de Validación de Nivel (Level)

    /// <summary>
    /// TEST: Nivel negativo debe usar el valor por defecto (1)
    ///
    /// JUSTIFICACIÓN: En Pokémon, no existen niveles negativos.
    /// El constructor debe proteger contra datos inválidos.
    /// </summary>
    [TestCase(-1)]
    [TestCase(-10)]
    [TestCase(-99)]
    public void TestPokemon_NegativeLevel_UsesDefaultValue(int invalidLevel)
    {
        // Arrange & Act: Intentar crear Pokémon con nivel inválido
        var pokemon = new Pokemon(name: "TestPokemon", level: invalidLevel);

        // Assert: Debe usar el valor por defecto (1)
        Assert.That(pokemon.Level, Is.EqualTo(1),
            $"Un Pokémon con level={invalidLevel} debe tener level=1 por defecto");
    }

    /// <summary>
    /// TEST: Nivel cero debe usar el valor por defecto (1)
    ///
    /// JUSTIFICACIÓN: El nivel mínimo es 1, no 0.
    /// </summary>
    [Test]
    public void TestPokemon_ZeroLevel_UsesDefaultValue()
    {
        var pokemon = new Pokemon(level: 0);
        Assert.That(pokemon.Level, Is.EqualTo(1));
    }

    /// <summary>
    /// TEST: Nivel mayor a 99 debe truncarse a 99
    ///
    /// JUSTIFICACIÓN: El enunciado especifica que el nivel es 1-99.
    /// Tu código usa Clamp que limita al máximo (99).
    /// </summary>
    [TestCase(100)]
    [TestCase(150)]
    [TestCase(999)]
    public void TestPokemon_LevelAboveMaximum_ClampedToMaximum(int invalidLevel)
    {
        var pokemon = new Pokemon(level: invalidLevel);
        Assert.That(pokemon.Level, Is.EqualTo(99),
            $"Level {invalidLevel} debe truncarse a 99 (máximo permitido)");
    }

    /// <summary>
    /// TEST: Niveles válidos (1-99) deben respetarse
    ///
    /// JUSTIFICACIÓN: Además de probar casos inválidos, debemos verificar
    /// que los casos válidos NO se modifiquen.
    /// </summary>
    [TestCase(1)]
    [TestCase(50)]
    [TestCase(99)]
    public void TestPokemon_ValidLevel_IsPreserved(int validLevel)
    {
        var pokemon = new Pokemon(level: validLevel);
        Assert.That(pokemon.Level, Is.EqualTo(validLevel));
    }

    #endregion

    #region Tests de Validación de Estadísticas (ATK, DEF, SpATK, SpDEF)

    /// <summary>
    /// TEST: Estadísticas negativas deben usar el valor por defecto (10)
    ///
    /// JUSTIFICACIÓN: Las estadísticas no pueden ser negativas.
    /// Probar con todas las stats (ATK, DEF, SpATK, SpDEF).
    /// </summary>
    [Test]
    public void TestPokemon_NegativeAttack_UsesDefaultValue()
    {
        var pokemon = new Pokemon(atk: -50);
        Assert.That(pokemon.Atk, Is.EqualTo(10),
            "ATK negativo debe usar el valor por defecto (10)");
    }

    [Test]
    public void TestPokemon_NegativeDefense_UsesDefaultValue()
    {
        var pokemon = new Pokemon(def: -30);
        Assert.That(pokemon.Def, Is.EqualTo(10));
    }

    [Test]
    public void TestPokemon_NegativeSpecialAttack_UsesDefaultValue()
    {
        var pokemon = new Pokemon(spAtk: -100);
        Assert.That(pokemon.SpAtk, Is.EqualTo(10));
    }

    [Test]
    public void TestPokemon_NegativeSpecialDefense_UsesDefaultValue()
    {
        var pokemon = new Pokemon(spDef: -1);
        Assert.That(pokemon.SpDef, Is.EqualTo(10));
    }

    /// <summary>
    /// TEST: Estadísticas en cero deben usar el valor por defecto (10)
    ///
    /// JUSTIFICACIÓN: El enunciado dice "1-255", entonces 0 es inválido.
    /// </summary>
    [Test]
    public void TestPokemon_ZeroStats_UseDefaultValues()
    {
        var pokemon = new Pokemon(atk: 0, def: 0, spAtk: 0, spDef: 0);

        Assert.That(pokemon.Atk, Is.EqualTo(10));
        Assert.That(pokemon.Def, Is.EqualTo(10));
        Assert.That(pokemon.SpAtk, Is.EqualTo(10));
        Assert.That(pokemon.SpDef, Is.EqualTo(10));
    }

    /// <summary>
    /// TEST: Estadísticas mayores a 255 deben truncarse a 255
    ///
    /// JUSTIFICACIÓN: El enunciado especifica "1-255" como límite.
    /// </summary>
    [TestCase(256)]
    [TestCase(300)]
    [TestCase(999)]
    public void TestPokemon_StatsAboveMaximum_ClampedToMaximum(int invalidStat)
    {
        var pokemon = new Pokemon(atk: invalidStat, def: invalidStat,
                                   spAtk: invalidStat, spDef: invalidStat);

        Assert.That(pokemon.Atk, Is.EqualTo(255),
            $"ATK={invalidStat} debe truncarse a 255");
        Assert.That(pokemon.Def, Is.EqualTo(255));
        Assert.That(pokemon.SpAtk, Is.EqualTo(255));
        Assert.That(pokemon.SpDef, Is.EqualTo(255));
    }

    #endregion

    #region Tests de Validación de Tipos (Types)

    /// <summary>
    /// TEST: Tipos duplicados deben eliminarse
    ///
    /// JUSTIFICACIÓN: En Pokémon, no existe Fire/Fire o Electric/Electric.
    /// Cada especie tiene 1 o 2 tipos DIFERENTES.
    ///
    /// ERROR COMÚN: El profesor mencionó que casi ningún estudiante validó esto.
    /// Tu código actual PERMITE tipos duplicados, lo cual es incorrecto.
    ///
    /// SOLUCIÓN: Usar .Distinct() en el constructor para eliminar duplicados.
    /// </summary>
    [Test]
    public void TestPokemon_DuplicateTypes_RemovesDuplicates()
    {
        // Arrange: Intentar crear Pokémon con Fire/Fire
        var duplicateTypes = new List<PokemonType>
        {
            PokemonType.Fire,
            PokemonType.Fire
        };

        // Act: Crear Pokémon
        var pokemon = new Pokemon(name: "BuggyPokemon", types: duplicateTypes);

        // Assert: Debe tener solo 1 tipo (Fire)
        Assert.That(pokemon.Types.Count, Is.EqualTo(1),
            "Tipos duplicados deben eliminarse automáticamente");
        Assert.That(pokemon.Types[0], Is.EqualTo(PokemonType.Fire));
    }

    /// <summary>
    /// TEST: Más de 2 tipos deben truncarse a los primeros 2
    ///
    /// JUSTIFICACIÓN: El enunciado dice "Puede ser uno solo o una combinación de dos tipos".
    /// Si alguien intenta crear Fire/Water/Grass, debe quedarse solo con Fire/Water.
    /// </summary>
    [Test]
    public void TestPokemon_MoreThanTwoTypes_TruncatesToTwo()
    {
        // Arrange: Intentar crear Pokémon con 3 tipos
        var manyTypes = new List<PokemonType>
        {
            PokemonType.Fire,
            PokemonType.Water,
            PokemonType.Grass
        };

        // Act
        var pokemon = new Pokemon(name: "TripleType", types: manyTypes);

        // Assert: Debe tener solo 2 tipos
        Assert.That(pokemon.Types.Count, Is.EqualTo(2),
            "Un Pokémon no puede tener más de 2 tipos");
        Assert.That(pokemon.Types[0], Is.EqualTo(PokemonType.Fire));
        Assert.That(pokemon.Types[1], Is.EqualTo(PokemonType.Water));
    }

    /// <summary>
    /// TEST: Lista vacía de tipos debe ser permitida
    ///
    /// JUSTIFICACIÓN: El enunciado permite 0, 1 o 2 tipos.
    /// Algunos Pokémon pueden no tener tipo definido (para casos generales).
    /// </summary>
    [Test]
    public void TestPokemon_EmptyTypesList_IsAllowed()
    {
        var pokemon = new Pokemon(types: new List<PokemonType>());
        Assert.That(pokemon.Types.Count, Is.EqualTo(0),
            "Un Pokémon puede no tener tipos (caso genérico)");
    }

    /// <summary>
    /// TEST: Null en types debe convertirse a lista vacía
    ///
    /// JUSTIFICACIÓN: Prevenir NullReferenceException.
    /// Tu código ya lo hace con `types ?? new List<PokemonType>()`.
    /// </summary>
    [Test]
    public void TestPokemon_NullTypes_CreatesEmptyList()
    {
        var pokemon = new Pokemon(types: null);
        Assert.That(pokemon.Types, Is.Not.Null);
        Assert.That(pokemon.Types.Count, Is.EqualTo(0));
    }

    #endregion

    #region Tests de Validación de Movimientos (Moves)

    /// <summary>
    /// TEST: Mínimo 1 movimiento debe estar presente
    ///
    /// JUSTIFICACIÓN: El enunciado dice "Mínimo debe tener un movimiento".
    /// Si alguien pasa una lista vacía, tu código debe agregar un Move() por defecto.
    /// </summary>
    [Test]
    public void TestPokemon_EmptyMovesList_AddsDefaultMove()
    {
        var pokemon = new Pokemon(moves: new List<Move>());

        Assert.That(pokemon.Moves.Count, Is.GreaterThanOrEqualTo(1),
            "Un Pokémon debe tener al menos 1 movimiento");
    }

    /// <summary>
    /// TEST: Más de 4 movimientos deben truncarse a 4
    ///
    /// JUSTIFICACIÓN: El enunciado dice "máximo puede tener cuatro".
    /// </summary>
    [Test]
    public void TestPokemon_MoreThanFourMoves_TruncatesToFour()
    {
        // Arrange: Crear 6 movimientos
        var manyMoves = new List<Move>
        {
            new Move("Move1"),
            new Move("Move2"),
            new Move("Move3"),
            new Move("Move4"),
            new Move("Move5"),
            new Move("Move6")
        };

        // Act
        var pokemon = new Pokemon(moves: manyMoves);

        // Assert
        Assert.That(pokemon.Moves.Count, Is.EqualTo(4),
            "Un Pokémon no puede tener más de 4 movimientos");
    }

    #endregion

    #region Tests de Validación de Move (Clase Move)

    /// <summary>
    /// TEST: Power inválido debe usar el valor por defecto (100)
    /// </summary>
    [TestCase(-10)]
    [TestCase(0)]
    [TestCase(256)]
    [TestCase(999)]
    public void TestMove_InvalidPower_UsesDefaultValue(int invalidPower)
    {
        var move = new Move(power: invalidPower);
        Assert.That(move.Power, Is.EqualTo(100),
            $"Power inválido ({invalidPower}) debe usar el valor por defecto (100)");
    }

    /// <summary>
    /// TEST: Speed inválido debe usar el valor por defecto (1)
    ///
    /// JUSTIFICACIÓN: El enunciado dice "Speed: 1-5".
    /// </summary>
    [TestCase(-1)]
    [TestCase(0)]
    [TestCase(6)]
    [TestCase(10)]
    public void TestMove_InvalidSpeed_UsesDefaultValue(int invalidSpeed)
    {
        var move = new Move(speed: invalidSpeed);
        Assert.That(move.Speed, Is.EqualTo(1),
            $"Speed inválido ({invalidSpeed}) debe usar el valor por defecto (1)");
    }

    #endregion
}

/// <summary>
/// RESUMEN DE LO QUE APRENDISTE AL CREAR ESTOS TESTS:
///
/// 1. EDGE CASES: Siempre probar valores extremos (negativos, cero, máximos, duplicados)
///
/// 2. DOCUMENTACIÓN: Los tests sirven como documentación ejecutable del comportamiento esperado
///
/// 3. TDD: Estos tests fallarán inicialmente (RED), luego modificaremos el código para que pasen (GREEN)
///
/// 4. DEFENSIVA: El código debe protegerse contra datos inválidos, no asumir que siempre recibirá datos correctos
///
/// 5. REQUISITOS: El profesor espera ver que leíste TODO el enunciado y validaste TODAS las restricciones
///
/// PRÓXIMO PASO: Ejecutar estos tests y ver cuáles fallan, luego modificar el código de producción.
/// </summary>
