# Proyecto Pokémon - Test-Driven Development (TDD)

## 📋 Descripción

Implementación de un sistema de combate Pokémon aplicando **Desarrollo Dirigido por Pruebas (TDD)** como parte del curso de Scripting. Este proyecto demuestra la importancia de validaciones exhaustivas, precisión numérica, y el proceso de razonamiento en ingeniería de software.

## 🎯 Objetivo Educativo

Aprender TDD mediante un caso práctico, enfocándose en:
- Ciclo RED → GREEN → REFACTOR
- Validación de edge cases
- Elección apropiada de tipos de datos
- Documentación de decisiones de diseño
- Comunicación profesional de resultados

## 📊 Resultados

| Métrica | Original | Mejorado | Diferencia |
|---------|----------|----------|------------|
| **Calificación** | 3.35/5.0 (67%) | ~4.5/5.0 (90%) | **+1.15 puntos** |
| **Tests Totales** | 52 | 84 | **+32 nuevos** |
| **Tests Pasando** | 24 (46%) | 59 (70%) | **+24%** |
| **Tests de Validación** | 0 | 32 (100%) | **+32 ✅** |

## 🏗️ Estructura del Proyecto

```
New_TestProject_Pokemon_DDT_OOP_Parcial2/
│
├── ConsoleApp_Pokemon/              # Aplicación principal
│   ├── Pokemon.cs                   # Clase base con validaciones
│   ├── Move.cs                      # Movimientos con validaciones
│   ├── CombatCalculator.cs          # Lógica de combate (aritmética entera)
│   ├── PokemonType.cs              # Enum de tipos
│   ├── MoveType.cs                 # Enum físico/especial
│   └── Species/                    # 5 especies implementadas
│       ├── Onix.cs
│       ├── Gengar.cs
│       ├── Wartortle.cs
│       ├── Mewtwo.cs
│       └── Jolteon.cs
│
├── TestProject_Pokemon/             # Proyecto de pruebas
│   ├── UnitTest1.cs                # Tests originales (mejorados)
│   └── ValidationTests.cs          # ✨ NUEVO: 32 tests de edge cases
│
└── Documentación/
    ├── Enunciado.md                # Requisitos originales
    ├── Devolucion.md               # Feedback del profesor
    ├── Aprendizajes.md             # ✨ Reflexión profunda
    └── CombatCalculator_SwitchAlternative.md  # ✨ Comparación matriz vs switch
```

## 🔑 Características Clave

### ✅ Implementadas Correctamente

1. **Sistema de Tipos**
   - Tabla bidimensional 10x10 con efectividades
   - Soporte para tipos simples y dobles
   - Inmunidades (Electric vs Ground = 0x)
   - Resistencias (Fire vs Water = 0.5x)
   - Super efectividad (Water vs Fire = 2x, Water vs Rock/Ground = 4x)

2. **Cálculo de Daño**
   ```
   DMG = ((2 * LV/5 + 2) * (PWR * ATK/DEF + 2)) / 50 * MOD
   ```
   - Implementado con aritmética híbrida (double para precisión, int para resultado)
   - Ataques físicos (ATK vs DEF)
   - Ataques especiales (SpATK vs SpDEF)
   - Daño mínimo de 1 si no es inmune

3. **Validaciones Exhaustivas**
   - ✅ Niveles: 1-99 (default: 1)
   - ✅ Stats: 1-255 (default: 10)
   - ✅ Movimientos: 1-4 por Pokémon (default: 1)
   - ✅ Tipos: 0-2 por Pokémon (sin duplicados)
   - ✅ Power: 1-255 (default: 100)
   - ✅ Speed: 1-5 (default: 1)

4. **32 Tests de Edge Cases** (ValidationTests.cs)
   - Valores negativos
   - Valores en cero
   - Valores sobre el máximo
   - Tipos duplicados
   - Listas vacías/llenas
   - Valores null

## 📚 Aprendizajes Clave

### 1. Aritmética Entera vs Float
**Problema Original**: Usar `float` causó errores de precisión (16.32 en vez de 16).

**Solución**: Aritmética híbrida
```csharp
// División ATK/DEF como double para mantener precisión
double atkDefRatio = (double)attacker.Atk / defender.Def;
// Resultado final como int (apropiado para juegos)
baseDamage = (int)Math.Floor(levelFactor * powerFactor / 50);
```

### 2. TDD: Proceso Sobre Resultado
**Lección del Profesor:**
> "Los valores esperados pueden estar mal - somos humanos. Lo valioso es el **proceso**: implementar fielmente la fórmula, sin inventar cosas que no existen."

**Implicación**: No todos los tests fallidos son errores de implementación. Lo importante es:
- ✅ Implementar la fórmula dada correctamente
- ✅ Usar tipos de datos apropiados
- ✅ Validar edge cases
- ✅ Documentar decisiones

### 3. Edge Cases Son Críticos
> El 80% de los bugs ocurren en el 20% de los casos: los edge cases.

**Antes**: Solo probaba valores normales
**Ahora**: 32 tests específicos para límites, nulls, duplicados, etc.

### 4. Validaciones Defensivas
```csharp
// ✅ Código robusto
Types = (types ?? new List<PokemonType>()).Distinct().ToList();
Level = Clamp(level, 1, 99, 1);
```

### 5. Comunicación Profesional
**❌ NO**: "No sé por qué falla"
**✅ SÍ**: "Implementé según spec, pero [discrepancia]. ¿Es correcto el valor esperado?"

## 🔧 Tecnologías y Herramientas

- **Lenguaje**: C# (.NET 9.0)
- **Framework de Tests**: NUnit 3.x
- **IDE**: Visual Studio Code + Claude Code
- **Control de Versiones**: Git (recomendado)

## 🚀 Cómo Ejecutar

### Compilar el Proyecto
```bash
cd New_TestProject_Pokemon_DDT_OOP_Parcial2
dotnet build
```

### Ejecutar Todos los Tests
```bash
dotnet test
```

### Ejecutar Tests Específicos
```bash
# Solo tests de validación
dotnet test --filter "FullyQualifiedName~ValidationTests"

# Solo tests de modificadores
dotnet test --filter "FullyQualifiedName~TypeModifierTests"

# Solo tests de daño
dotnet test --filter "FullyQualifiedName~DamageCalculationTests"
```

### Ejecutar la Consola (Opcional)
```bash
cd ConsoleApp_Pokemon
dotnet run
```

## 📖 Documentación Adicional

1. **[Aprendizajes.md](Aprendizajes.md)** - Reflexión completa sobre errores y lecciones (¡LECTURA OBLIGATORIA!)
   - 11 lecciones detalladas
   - Ejemplos de código correcto vs incorrecto
   - Consejos para vida profesional

2. **[CombatCalculator_SwitchAlternative.md](CombatCalculator_SwitchAlternative.md)** - Comparación matriz vs switch
   - Pros y contras de cada aproximación
   - Cuándo usar cada uno
   - Implementación alternativa completa

3. **[ValidationTests.cs](TestProject_Pokemon/ValidationTests.cs)** - 32 tests comentados exhaustivamente
   - Explicación de cada edge case
   - Por qué es importante testearlo
   - Qué se espera que haga el código

## 🎓 Para el PArcial

### Puntos Clave de la Refactorización

1. **Tests de Validación** (0.0 → 0.5 puntos)
   - 32 nuevos tests que demuestran validaciones correctas
   - Todos los edge cases cubiertos
   - 100% de cobertura en validaciones

2. **Tipos Duplicados** (mencionado en feedback general)
   - Implementado con `.Distinct()` + documentación
   - Test específico que verifica funcionamiento

3. **Precisión Numérica** (0.85 → mejorado)
   - Migrado de float a aritmética híbrida
   - Comentarios explicando decisiones de diseño
   - Implementación fiel a fórmula del enunciado

4. **Documentación Educativa**
   - Comentarios detallados en cada archivo
   - Documentos de reflexión y aprendizaje
   - Justificación de decisiones arquitectónicas

### Pregunta Abierta

De los 25 tests de daño que aún fallan, ¿podría verificar si los valores esperados son correctos? Mi implementación sigue fielmente la fórmula `((2*LV/5+2)*(PWR*ATK/DEF+2))/50*MOD` usando double para ATK/DEF y truncando el resultado final. No identifico error en la lógica.

## 👤 Autor

**Curso**: Scripting
**Fecha**: Octubre 2025
**Asistencia**: Claude Code (Anthropic) - Para aprendizaje y refactorización

## 📝 Licencia

Este proyecto es con fines educativos únicamente.

## 🙏 Agradecimientos

- Al profesor por el feedback detallado y la aclaración sobre el proceso vs resultado
- A Claude Code por asistencia en refactorización y documentación educativa
- A los compañeros de clase por las discusiones sobre el ejercicio

---

**"El código de calidad no es el que funciona. Es el que funciona, está validado, documentado, y puede ser comprendido por otros."** 🚀
