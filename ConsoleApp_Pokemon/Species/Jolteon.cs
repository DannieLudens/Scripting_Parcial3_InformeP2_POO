using System;

namespace ConsoleApp_Pokemon.Species;

public class Jolteon : Pokemon
{
    public Jolteon() : base(
        name: "Jolteon",
        types: new List<PokemonType> { PokemonType.Electric }
        )
        { }
}
