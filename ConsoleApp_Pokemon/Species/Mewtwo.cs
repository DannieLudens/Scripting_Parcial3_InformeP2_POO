using System;

namespace ConsoleApp_Pokemon.Species;

public class Mewtwo : Pokemon
{
    public Mewtwo() : base(
        name: "Mewtwo",
        types: new List<PokemonType> { PokemonType.Psychic }
        )
        { }
}
