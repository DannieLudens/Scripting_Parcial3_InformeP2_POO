using System;

namespace ConsoleApp_Pokemon.Species;

public class Gengar : Pokemon
{
    public Gengar() : base(
        name: "Gengar",
        types: new List<PokemonType> { PokemonType.Ghost, PokemonType.Poison }
        )
        { }
}
