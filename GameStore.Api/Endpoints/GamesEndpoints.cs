using GameStore.Api.Data;
using GameStore.Api.Dtos;
using GameStore.Api.Entities;

namespace GameStore.Api;

public static class GamesEndpoints
{
    const string GetGameEndpointName = "GetGame";

    private static readonly List<GameDto> games =
    [
    new GameDto(1, "The Last of Us Part II", "Action-Adventure", 59.99m, new DateOnly(2020, 6, 19)),
    new GameDto(2, "Red Dead Redemption 2", "Action-Adventure", 49.99m, new DateOnly(2018, 10, 26)),
    new GameDto(3, "Cyberpunk 2077", "Action-RPG", 59.99m, new DateOnly(2020, 12, 10)),
    new GameDto(4, "The Witcher 3: Wild Hunt", "Action-RPG", 39.99m, new DateOnly(2015, 5, 19)),
    new GameDto(5, "Grand Theft Auto V", "Action-Adventure", 29.99m, new DateOnly(2013, 9, 17))
    ];

    public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app)
    {

        var group = app.MapGroup("games")
                        .WithParameterValidation();
        
        // GET /games
        group.MapGet("/", () => games);

        // GET /games/1
        group.MapGet("/{id}", (int id) =>
        {
            GameDto? game = games.Find(game => game.Id == id);

            return game is null ? Results.NotFound() : Results.Ok(game);
        })
            .WithName(GetGameEndpointName);

        // POST /games
        group.MapPost("/", (CreateGameDto newGame, GameStoreContext dbContext) =>
        {
            Game game = new()
            {
                Name = newGame.Name,
                Genre = dbContext.Genres.Find(newGame.GenreId),
                GenreId = newGame.GenreId,
                Price = newGame.Price,
                ReleaseDate = newGame.ReleaseDate
            };

            dbContext.Games.Add(game);
            dbContext.SaveChanges();

            GameDto gameDto = new(
                game.Id,
                game.Name,
                game.Genre!.Name,
                game.Price,
                game.ReleaseDate
            );

            return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, gameDto);
        });
        
        //PUT /games/1
        group.MapPut("/{id}", (int id, UpdateGameDto updatedgame) =>
        {
            var index = games.FindIndex(game => game.Id == id);

            if (index == -1)
            {
                return Results.NotFound();
            }

            games[index] = new GameDto(
                id,
                updatedgame.Name,
                updatedgame.Genre,
                updatedgame.Price,
                updatedgame.ReleaseDate
            );

            return Results.NoContent();
        });

        //DELETE /games/1
        group.MapDelete("/{id}", (int id) =>
        {
            games.RemoveAll(game => game.Id == id);

            return Results.NoContent();
        }
        );

        return group;

    }



}