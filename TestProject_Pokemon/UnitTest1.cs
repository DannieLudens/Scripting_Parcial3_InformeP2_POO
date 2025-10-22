using ConsoleApp_Pokemon;
using ConsoleApp_Pokemon.Species;

namespace TestProject_Pokemon;


[TestFixture]
public class CombatTests
{
    [TestFixture]
    public class TestPokemonAndMove
    {
        [Test]
        public void TestPokemon_DefaultValues()
        {
            Pokemon pikachu = new Pokemon();

            Assert.That(pikachu.Name, Is.EqualTo(""));  // por defecto vacío
            Assert.That(pikachu.Level, Is.EqualTo(1));  // mínimo 1
            Assert.That(pikachu.Atk, Is.EqualTo(10));
            Assert.That(pikachu.Def, Is.EqualTo(10));
            Assert.That(pikachu.SpAtk, Is.EqualTo(10));
            Assert.That(pikachu.SpDef, Is.EqualTo(10));
            Assert.That(pikachu.Types.Count, Is.EqualTo(0)); // sin valor por defecto
            Assert.That(pikachu.Moves.Count, Is.EqualTo(1));
        } // testear no solo el defecto sino también los límites u otros casos aleatorios de creacion

        [Test]
        public void TestPokemon_CustomValues()
        {
            Pokemon charmander = new Pokemon(
                name: "Charmander",
                level: 5,
                atk: 52,
                def: 43,
                spAtk: 60,
                spDef: 50,
                types: new List<PokemonType> { PokemonType.Fire }
            );

            Assert.That(charmander.Name, Is.EqualTo("Charmander"));
            Assert.That(charmander.Level, Is.EqualTo(5));
            Assert.That(charmander.Atk, Is.EqualTo(52));
            Assert.That(charmander.Def, Is.EqualTo(43));
            Assert.That(charmander.SpAtk, Is.EqualTo(60));
            Assert.That(charmander.SpDef, Is.EqualTo(50));
            Assert.That(charmander.Types.Contains(PokemonType.Fire));
        }

        [Test]
        public void TestMove_DefaultValues()
        {
            Move tackle = new Move();

            Assert.That(tackle.Name, Is.EqualTo(""));
            Assert.That(tackle.Power, Is.EqualTo(100));
            Assert.That(tackle.Speed, Is.EqualTo(1));
            Assert.That(tackle.Type, Is.EqualTo(PokemonType.Rock));
            Assert.That(tackle.MoveType, Is.EqualTo(MoveType.Physical));
        }

        [Test]
        public void TestMove_CustomValues()
        {
            Move flamethrower = new Move(
                name: "Flamethrower",
                power: 90,
                speed: 2,
                type: PokemonType.Fire,
                moveType: MoveType.Special
            );

            Assert.That(flamethrower.Name, Is.EqualTo("Flamethrower"));
            Assert.That(flamethrower.Power, Is.EqualTo(90));
            Assert.That(flamethrower.Speed, Is.EqualTo(2));
            Assert.That(flamethrower.Type, Is.EqualTo(PokemonType.Fire));
            Assert.That(flamethrower.MoveType, Is.EqualTo(MoveType.Special));
        }
    }

    /// <summary>
    /// Tests de modificadores de tipo
    ///
    /// CAMBIO IMPORTANTE: float → int
    /// - Valores negativos representan divisiones: -1 = 0.5x (dividir por 2)
    /// - Valores positivos son multiplicadores: 2 = 2x, 4 = 4x
    /// - 0 = inmune (sin daño)
    /// </summary>
    [TestFixture]
    public class TypeModifierTests
    {
        // NOTA: expectedMod cambió de float a int
        // -1 representa 0.5x (no muy efectivo)
        // 2 representa 2x (super efectivo)
        [TestCase(PokemonType.Fire, PokemonType.Water, -1)]      // Fire vs Water = No muy efectivo (0.5x)
        [TestCase(PokemonType.Water, PokemonType.Fire, 2)]       // Water vs Fire = Super efectivo (2x)
        [TestCase(PokemonType.Electric, PokemonType.Ground, 0)]  // Electric vs Ground = Inmune (0x)
        [TestCase(PokemonType.Ghost, PokemonType.Ghost, 2)]      // Ghost vs Ghost = Super efectivo (2x)
        [TestCase(PokemonType.Rock, PokemonType.Fire, 2)]        // Rock vs Fire = Super efectivo (2x)
        [TestCase(PokemonType.Fire, PokemonType.Rock, -1)]       // Fire vs Rock = No muy efectivo (0.5x)
        public void TestSingleTypeModifiers(PokemonType attackType, PokemonType defendType, int expectedMod)
        {
            var defendingTypes = new List<PokemonType> { defendType };
            int result = CombatCalculator.CalculateTypeModifier(attackType, defendingTypes);
            Assert.That(result, Is.EqualTo(expectedMod));
        }

        [Test]
        public void TestDualTypeModifiers_OnixExamples()
        {
            // Onix es Rock/Ground
            var onix = new Onix();

            // Water vs Rock/Ground = 2 × 2 = 4 (cuádruple daño)
            int waterMod = CombatCalculator.CalculateTypeModifier(PokemonType.Water, onix.Types);
            Assert.That(waterMod, Is.EqualTo(4),
                "Water es super efectivo contra Rock (2x) y Ground (2x) = 4x total");

            // Electric vs Rock/Ground = 1 × 0 = 0 (inmune)
            int electricMod = CombatCalculator.CalculateTypeModifier(PokemonType.Electric, onix.Types);
            Assert.That(electricMod, Is.EqualTo(0),
                "Electric es inmune contra Ground, por lo tanto el ataque no hace daño");
        }

        [Test]
        public void TestDualTypeModifiers_GengarExamples()
        {
            // Gengar es Ghost/Poison
            var gengar = new Gengar();

            // Psychic vs Ghost/Poison = 2 × 1 = 2
            int psychicMod = CombatCalculator.CalculateTypeModifier(PokemonType.Psychic, gengar.Types);
            Assert.That(psychicMod, Is.EqualTo(2),
                "Psychic es super efectivo contra Poison (2x), neutral contra Ghost (1x) = 2x total");
        }
    }

    [TestFixture]
    public class DamageCalculationTests
    {
        // 40 casos en líneas individuales - mucho más compacto
        [TestCase(1, 1, 1, 1, 1, 0, 0, true, TestName = "Case01_Damage_0_Special")]
        [TestCase(2, 1, 1, 1, 1, 1, 1, false, TestName = "Case02_Damage_1_Physical")]
        [TestCase(3, 5, 50, 100, 50, 2, 16, true, TestName = "Case03_Damage_16_Special")]
        [TestCase(4, 5, 50, 100, 50, 1, 5, false, TestName = "Case04_Damage_5_Physical")]
        [TestCase(5, 10, 20, 30, 15, 1, 5, true, TestName = "Case05_Damage_5_Special")]
        [TestCase(6, 12, 40, 60, 80, 2, 9, false, TestName = "Case06_Damage_9_Physical")]
        [TestCase(7, 25, 80, 120, 60, 1, 40, true, TestName = "Case07_Damage_40_Special")]
        [TestCase(8, 30, 100, 50, 100, 4, 58, false, TestName = "Case08_Damage_58_Physical")]
        [TestCase(9, 40, 150, 200, 150, 1, 37, true, TestName = "Case09_Damage_37_Special")]
        [TestCase(10, 50, 128, 200, 100, 1, 58, false, TestName = "Case10_Damage_58_Physical")]
        [TestCase(11, 50, 128, 200, 100, 4, 455, true, TestName = "Case11_Damage_455_Special")]
        [TestCase(12, 60, 200, 250, 200, 1, 132, false, TestName = "Case12_Damage_132_Physical")]
        [TestCase(13, 70, 180, 200, 100, 2, 435, true, TestName = "Case13_Damage_435_Special")]
        [TestCase(14, 80, 90, 45, 90, 1, 33, false, TestName = "Case14_Damage_33_Physical")]
        [TestCase(15, 90, 255, 200, 50, 2, 1554, true, TestName = "Case15_Damage_1554_Special")]
        [TestCase(16, 99, 255, 255, 1, 2, 108206, false, TestName = "Case16_Damage_108206_Physical")]
        [TestCase(17, 99, 255, 255, 255, 4, 856, true, TestName = "Case17_Damage_856_Special")]
        [TestCase(18, 99, 255, 255, 255, 0, 0, false, TestName = "Case18_Damage_0_Physical")]
        [TestCase(19, 99, 255, 1, 255, 1, 2, true, TestName = "Case19_Damage_2_Special")]
        [TestCase(20, 45, 60, 10, 200, 1, 2, false, TestName = "Case20_Damage_2_Physical")]
        [TestCase(21, 20, 30, 5, 250, 1, 1, true, TestName = "Case21_Damage_1_Special")]
        [TestCase(22, 2, 10, 1, 255, 1, 1, false, TestName = "Case22_Damage_1_Physical")]
        [TestCase(23, 3, 5, 2, 3, 1, 1, true, TestName = "Case23_Damage_1_Special")]
        [TestCase(24, 15, 200, 255, 255, 1, 33, false, TestName = "Case24_Damage_33_Physical")]
        [TestCase(25, 16, 200, 255, 254, 1, 34, true, TestName = "Case25_Damage_34_Special")]
        [TestCase(26, 17, 200, 255, 128, 1, 36, false, TestName = "Case26_Damage_36_Physical")]
        [TestCase(27, 33, 77, 77, 77, 1, 25, true, TestName = "Case27_Damage_25_Special")]
        [TestCase(28, 48, 33, 99, 11, 4, 508, false, TestName = "Case28_Damage_508_Physical")]
        [TestCase(29, 55, 44, 88, 22, 1, 44, true, TestName = "Case29_Damage_44_Special")]
        [TestCase(30, 66, 11, 11, 11, 1, 8, false, TestName = "Case30_Damage_8_Physical")]
        [TestCase(31, 77, 123, 200, 100, 2, 326, true, TestName = "Case31_Damage_326_Special")]
        [TestCase(32, 88, 200, 100, 50, 4, 1197, false, TestName = "Case32_Damage_1197_Physical")]
        [TestCase(33, 10, 200, 200, 200, 0, 0, true, TestName = "Case33_Damage_0_Special")]
        [TestCase(34, 50, 255, 100, 50, 0, 0, false, TestName = "Case34_Damage_0_Physical")]
        [TestCase(35, 75, 180, 255, 180, 0, 0, true, TestName = "Case35_Damage_0_Special")]
        [TestCase(36, 99, 255, 255, 1, 0, 0, false, TestName = "Case36_Damage_0_Physical")]
        [TestCase(37, 25, 60, 40, 20, 0, 0, true, TestName = "Case37_Damage_0_Special")]
        [TestCase(38, 60, 100, 255, 128, 1, 40, false, TestName = "Case38_Damage_40_Physical")]
        [TestCase(39, 80, 90, 45, 90, 1, 17, true, TestName = "Case39_Damage_17_Special")]
        [TestCase(40, 99, 200, 150, 150, 1, 84, false, TestName = "Case40_Damage_84_Physical")]
        /// <summary>
        /// Test parametrizado de cálculo de daño
        ///
        /// CAMBIO IMPORTANTE: modifier ahora es int (era float)
        /// - 0 = Inmune (0x)
        /// - 1 = Normal (1x)
        /// - 2 = Super efectivo (2x)
        /// - 4 = Cuádruple (4x)
        ///
        /// Este cambio refleja la refactorización de aritmética entera en CombatCalculator
        /// </summary>
        public void TestDamageCalculation(int caseNum, int level, int power, int attackStat, int defenseStat, int modifier, int expected, bool isSpecial)
        {
            // Obtener tipos según el modificador
            var (attackType, defenseTypes) = GetTypesForModifier(modifier);

            // Crear atacante (usar atacanteStat en la posición correcta)
            Pokemon attacker;
            Pokemon defender;
            if (isSpecial)
            {
                attacker = new Pokemon($"TestAttacker{caseNum}", level, 10, 10, attackStat, 10, new List<PokemonType> { attackType });
                defender = new Pokemon($"TestDefender{caseNum}", 1, 10, 10, 10, defenseStat, defenseTypes);
            }
            else
            {
                attacker = new Pokemon($"TestAttacker{caseNum}", level, attackStat, 10, 10, 10, new List<PokemonType> { attackType });
                defender = new Pokemon($"TestDefender{caseNum}", 1, 10, defenseStat, 10, 10, defenseTypes);
            }

            // Crear movimiento
            var moveType = isSpecial ? MoveType.Special : MoveType.Physical;
            var move = new Move($"TestMove{caseNum}", power, 1, attackType, moveType);

            // Ejecutar y verificar (ahora retorna int, no float)
            int damage = CombatCalculator.CalculateDamage(attacker, move, defender);
            Assert.That(damage, Is.EqualTo(expected), $"Case {caseNum} failed");
        }

        /// <summary>
        /// Método helper para obtener tipos según el modificador
        ///
        /// CAMBIO: modifier ahora es int
        /// CORRECCIÓN: Se eliminó PokemonType.Normal (no está en el enunciado)
        /// - Para modificador 1x (normal) usamos Rock vs Rock (efecto neutral según tabla)
        /// </summary>
        private static (PokemonType attackType, List<PokemonType> defenseTypes) GetTypesForModifier(int modifier)
        {
            return modifier switch
            {
                0 => (PokemonType.Electric, new List<PokemonType> { PokemonType.Ground }),        // Inmune
                1 => (PokemonType.Rock, new List<PokemonType> { PokemonType.Rock }),              // Neutral (1x)
                2 => (PokemonType.Water, new List<PokemonType> { PokemonType.Fire }),            // Super efectivo
                4 => (PokemonType.Water, new List<PokemonType> { PokemonType.Rock, PokemonType.Ground }), // Cuádruple
                _ => (PokemonType.Rock, new List<PokemonType> { PokemonType.Rock })
            };
        }
    }
}
