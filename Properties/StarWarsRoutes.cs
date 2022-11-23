using Microsoft.Extensions.Options;

public static class StarWarsRoutes
{
  public static void MapStarWarsRoutes(this IEndpointRouteBuilder routeBuilder)
  {
    routeBuilder.MapGet("/first", () => "Darth Vader")
      .WithName("GetFirstStarWarsCharacter");

    routeBuilder.MapGet("/second", (IOptionsSnapshot<Settings> settings) => 
        settings.Value.SecondCharacter)
      .WithName("GetSecondStarWarsCharacter");
  }
}