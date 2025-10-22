using System;

namespace ConsoleApp_Pokemon.Species;

public class Onix : Pokemon
{
    public Onix() : base(
        name: "Onix",
        types: new List<PokemonType> { PokemonType.Rock, PokemonType.Ground }
        )
        { }
}
