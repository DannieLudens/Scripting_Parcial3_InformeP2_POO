# Alternativa con Switch Expression para Tabla de Tipos

## Contexto

El profesor sugirió en sus comentarios que la tabla bidimensional de efectividad de tipos podría implementarse con `if` anidados y `switch` en lugar de una matriz. Este documento explora esa alternativa **sin reemplazar el código actual**, solo como referencia educativa.

## Implementación Actual (Matriz Bidimensional)

```csharp
private static readonly int[,] TypeEffectiveness = new int[,]
{
    //             Rock  Ground Water Electric Fire  Grass Ghost Poison Psychic Bug
    /* Rock     */ { 1,   -1,   1,    1,      2,    -1,   1,    1,     1,     2 },
    /* Ground   */ { 2,    1,   1,    2,      2,    -1,   1,    2,     1,    -1 },
    // ... más filas
};

// Uso:
int modifier = TypeEffectiveness[attackIndex, defendIndex];
```

### ✅ **Ventajas de la Matriz**:
1. **Compacta y visual**: Toda la tabla de tipos cabe en pantalla
2. **O(1) lookup**: Acceso instantáneo por índices
3. **Fácil de modificar**: Cambiar un valor es directo
4. **Menos código**: No hay lógica condicional repetida
5. **Fácil de probar**: Puedes iterar sobre toda la tabla programáticamente

### ❌ **Desventajas de la Matriz**:
1. **Menos legible para principiantes**: Requiere entender índices y matrices 2D
2. **Magic numbers**: Los índices no son autodescriptivos
3. **Difícil de debuggear**: Si hay un error, no es obvio dónde
4. **No permite lógica compleja**: Solo almacena valores, no puede ejecutar código condicional

---

## Alternativa con Switch Expression

### Implementación Propuesta

```csharp
/// <summary>
/// Calcula el modificador de efectividad de tipo usando switch expressions
/// </summary>
private static int GetTypeEffectiveness(PokemonType attacking, PokemonType defending)
{
    // Switch anidado: primero por tipo atacante, luego por tipo defensor
    return attacking switch
    {
        PokemonType.Rock => defending switch
        {
            PokemonType.Rock => 1,
            PokemonType.Ground => -1,  // No muy efectivo (0.5x)
            PokemonType.Water => 1,
            PokemonType.Electric => 1,
            PokemonType.Fire => 2,      // Super efectivo (2x)
            PokemonType.Grass => -1,
            PokemonType.Ghost => 1,
            PokemonType.Poison => 1,
            PokemonType.Psychic => 1,
            PokemonType.Bug => 2,
            _ => 1  // Caso por defecto (Normal u otros)
        },

        PokemonType.Ground => defending switch
        {
            PokemonType.Rock => 2,
            PokemonType.Ground => 1,
            PokemonType.Water => 1,
            PokemonType.Electric => 2,
            PokemonType.Fire => 2,
            PokemonType.Grass => -1,
            PokemonType.Ghost => 1,
            PokemonType.Poison => 2,
            PokemonType.Psychic => 1,
            PokemonType.Bug => -1,
            _ => 1
        },

        PokemonType.Water => defending switch
        {
            PokemonType.Rock => 2,
            PokemonType.Ground => 2,
            PokemonType.Water => -1,
            PokemonType.Electric => 1,
            PokemonType.Fire => 2,
            PokemonType.Grass => -1,
            PokemonType.Ghost => 1,
            PokemonType.Poison => 1,
            PokemonType.Psychic => 1,
            PokemonType.Bug => 1,
            _ => 1
        },

        PokemonType.Electric => defending switch
        {
            PokemonType.Rock => 1,
            PokemonType.Ground => 0,    // Inmune (0x)
            PokemonType.Water => 2,
            PokemonType.Electric => -1,
            PokemonType.Fire => 1,
            PokemonType.Grass => -1,
            PokemonType.Ghost => 1,
            PokemonType.Poison => 1,
            PokemonType.Psychic => 1,
            PokemonType.Bug => 1,
            _ => 1
        },

        PokemonType.Fire => defending switch
        {
            PokemonType.Rock => -1,
            PokemonType.Ground => 1,
            PokemonType.Water => -1,
            PokemonType.Electric => 1,
            PokemonType.Fire => -1,
            PokemonType.Grass => 2,
            PokemonType.Ghost => 1,
            PokemonType.Poison => 1,
            PokemonType.Psychic => 1,
            PokemonType.Bug => 2,
            _ => 1
        },

        PokemonType.Grass => defending switch
        {
            PokemonType.Rock => 2,
            PokemonType.Ground => 2,
            PokemonType.Water => 2,
            PokemonType.Electric => 1,
            PokemonType.Fire => -1,
            PokemonType.Grass => -1,
            PokemonType.Ghost => 1,
            PokemonType.Poison => -1,
            PokemonType.Psychic => 1,
            PokemonType.Bug => -1,
            _ => 1
        },

        PokemonType.Ghost => defending switch
        {
            PokemonType.Rock => 1,
            PokemonType.Ground => 1,
            PokemonType.Water => 1,
            PokemonType.Electric => 1,
            PokemonType.Fire => 1,
            PokemonType.Grass => 1,
            PokemonType.Ghost => 2,
            PokemonType.Poison => 1,
            PokemonType.Psychic => 2,
            PokemonType.Bug => 1,
            _ => 1
        },

        PokemonType.Poison => defending switch
        {
            PokemonType.Rock => -1,
            PokemonType.Ground => -1,
            PokemonType.Water => 1,
            PokemonType.Electric => 1,
            PokemonType.Fire => 1,
            PokemonType.Grass => 2,
            PokemonType.Ghost => -1,
            PokemonType.Poison => -1,
            PokemonType.Psychic => 1,
            PokemonType.Bug => 1,
            _ => 1
        },

        PokemonType.Psychic => defending switch
        {
            PokemonType.Rock => 1,
            PokemonType.Ground => 1,
            PokemonType.Water => 1,
            PokemonType.Electric => 1,
            PokemonType.Fire => 1,
            PokemonType.Grass => 1,
            PokemonType.Ghost => 1,
            PokemonType.Poison => 2,
            PokemonType.Psychic => -1,
            PokemonType.Bug => -1,
            _ => 1
        },

        PokemonType.Bug => defending switch
        {
            PokemonType.Rock => 1,
            PokemonType.Ground => 1,
            PokemonType.Water => 1,
            PokemonType.Electric => 1,
            PokemonType.Fire => -1,
            PokemonType.Grass => 2,
            PokemonType.Ghost => 1,
            PokemonType.Poison => 1,
            PokemonType.Psychic => 2,
            PokemonType.Bug => 1,
            _ => 1
        },

        // Caso por defecto: tipo Normal o cualquier otro
        _ => 1
    };
}

// Uso en CalculateTypeModifier:
int modifier = GetTypeEffectiveness(attackingType, defendingType);
```

### ✅ **Ventajas del Switch**:
1. **Muy legible**: Cada caso está claramente etiquetado con nombres de tipos
2. **Autodocumentado**: No necesitas comentarios para entender qué hace
3. **Fácil de debuggear**: Puedes poner breakpoints en casos específicos
4. **Extensible**: Puedes agregar lógica compleja en cada caso si fuera necesario
5. **Type-safe**: El compilador verifica que uses los enums correctamente

### ❌ **Desventajas del Switch**:
1. **Mucho más código**: ~100 líneas vs ~10 líneas de matriz
2. **Difícil de visualizar**: No puedes ver toda la tabla de un vistazo
3. **Repetitivo**: Muchos casos devuelven el mismo valor (1)
4. **Más difícil de mantener**: Cambiar la lógica general requiere tocar muchos lugares
5. **Posibles errores de tipeo**: Es fácil equivocarse al copiar/pegar tantos casos

---

## Comparación de Rendimiento

```
Matriz:     O(1) - Acceso directo por índice
Switch:     O(1) - El compilador optimiza a jump table
```

**Conclusión**: Ambos tienen el mismo rendimiento en tiempo de ejecución. La diferencia está en legibilidad y mantenibilidad.

---

## Cuándo Usar Cada Uno

### **Usa Matriz cuando:**
- Tienes muchos valores tabulados (como tabla de tipos)
- Los valores son numéricos simples
- Quieres ver toda la tabla de un vistazo
- El código es para programadores experimentados

### **Usa Switch cuando:**
- Tienes pocos casos (<20)
- La lógica de cada caso es diferente y compleja
- Necesitas máxima legibilidad para principiantes
- Cada caso requiere nombres descriptivos

---

## Opinión y Recomendación

**Para este proyecto específico (tabla de efectividad de Pokémon), la MATRIZ es superior porque:**

1. Es la aproximación usada en los juegos Pokémon reales
2. La tabla tiene 100 valores (10x10) - demasiados para switch legible
3. Los valores son simples (1, 2, -1, 0)
4. Es más fácil de probar exhaustivamente
5. Es más compacta y profesional

**El switch sería mejor si:**
- Tuviéramos <10 tipos en total
- Cada combinación tuviera lógica especial
- El proyecto fuera para enseñar a principiantes absolutos

---

## Reflexión Educativa

El comentario del profesor de usar switch es **válido para enseñanza**. Si estás aprendiendo, escribir cada caso explícitamente te ayuda a entender las interacciones entre tipos.

Sin embargo, en **código de producción**, la matriz es la elección correcta por las razones mencionadas arriba.

**Ambas soluciones son correctas**. La elección depende del contexto: ¿priorizar aprendizaje o eficiencia?

En tu caso, dado que ya implementaste y entiendes la matriz, has demostrado que comprendes ambas aproximaciones. Esto es lo importante: saber cuándo usar cada herramienta.

---

## Pregunta para el Profesor

En tu próxima sesión con el profesor, puedes preguntar:

> "Profesor, entiendo que sugirió usar switch en vez de matriz para la tabla de tipos. Implementé la matriz porque tiene estas ventajas: [listar ventajas]. Sin embargo, entiendo que el switch sería más explícito para aprendizaje. ¿Podría explicar en qué contextos específicos prefiere switch sobre matriz, y si hay algún aspecto que no estoy considerando?"

Esta pregunta muestra que:
1. Consideraste su feedback seriamente
2. Investigaste ambas opciones
3. Tomaste una decisión informada
4. Estás abierto a aprender más

¡Eso es pensamiento crítico de calidad!
