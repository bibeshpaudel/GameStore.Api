using GameStore.Api.Entities;

const string getGameEndpointName = "GetGame";
List<Game> gameList =
[
    new Game()
    {
        Id = 1,
        Name = "GTA",
        Genre = "Adventure",
        Price = 62.0M,
        ReleaseDate = DateTime.Now,
        ImageUri = "https://placehold.co/100"
    },

    new Game()
    {
        Id = 2,
        Name = "Forza",
        Genre = "Racing",
        Price = 70.0M,
        ReleaseDate = DateTime.Now,
        ImageUri = "https://placehold.co/100"
    },

    new Game()
    {
        Id = 3,
        Name = "Valorant",
        Genre = "Shooter",
        Price = 80.0M,
        ReleaseDate = DateTime.Now,
        ImageUri = "https://placehold.co/100"
    }

];

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
var routeGroup = app.MapGroup("/game");
routeGroup.MapGet("/", () => gameList);

routeGroup.MapGet("/{id:int}", (int id) =>
{
    Game? game = gameList.Find(x => x.Id == id);
    
    return game is null ? Results.NotFound() : Results.Ok(game);
}).WithName(getGameEndpointName);

routeGroup.MapPost("/", (Game game) =>
{
    game.Id = gameList.Max(x => x.Id) + 1;
    gameList.Add(game);

    return Results.CreatedAtRoute(getGameEndpointName, new { id = game.Id }, game);
});

routeGroup.MapPut("/{id:int}", (int id, Game updatedGame) =>
{
    Game? existingGame = gameList.Find(x => x.Id == id);
    if (existingGame is null)
    {
        return Results.NotFound();
    }

    existingGame.Name = updatedGame.Name;
    existingGame.Genre = updatedGame.Genre;
    existingGame.Price = updatedGame.Price;
    existingGame.ReleaseDate = updatedGame.ReleaseDate;
    existingGame.ImageUri = updatedGame.ImageUri;

    return Results.NoContent();
});

routeGroup.MapDelete("/{id:int}", (int id) =>
{
    Game? game = gameList.Find(x => x.Id == id);

    if (game is null)
    {
        return Results.NotFound();
    }

    gameList.Remove(game);
    return Results.NoContent();
});

app.Run();
