using System;

namespace ConsoleApp_Pokemon.Species;

public class Wartortle : Pokemon
{
    public Wartortle() : base(
        name: "Wartortle",
        types: new List<PokemonType> { PokemonType.Water }
        )
        { }
}
