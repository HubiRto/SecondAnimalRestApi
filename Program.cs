using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using SecoundAnimalRestApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

Animal MapToAnimal(MySqlDataReader reader)
{
    return new Animal
    {
        Id = reader.GetInt32(0),
        Name = reader.GetString(1),
        Description = reader.GetString(2),
        Category = reader.GetString(3),
        Area = reader.GetString(4),
    };
}

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

app.MapGet("/api/animals", async ([FromQuery] string orderBy = "Name") =>
{
    var query = $"SELECT * FROM Animal ORDER BY {orderBy} ASC";
    var animals = new List<Animal>();

    await using (var connection = new MySqlConnection(connectionString))
    {
        var command = new MySqlCommand(query, connection);
        await connection.OpenAsync();
        await using (var reader = await command.ExecuteReaderAsync())
        {
            while (await reader.ReadAsync())
            {
                animals.Add(MapToAnimal(reader));
            }
        }
    }

    return Results.Ok(animals);
});

app.MapPost("/api/animals", async ([FromBody] Animal animal) =>
{
    const string query = "INSERT INTO Animal (Name, Description, Category, Area) VALUES (@Name, @Description, @Category, @Area)";

    await using (var connection = new MySqlConnection(connectionString))
    {
        var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@Name", animal.Name);
        command.Parameters.AddWithValue("@Description", animal.Description);
        command.Parameters.AddWithValue("@Category", animal.Category);
        command.Parameters.AddWithValue("@Area", animal.Area);

        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();
    }

    return Results.StatusCode(201);
});

app.MapPut("/api/animals/{idAnimal}", async (int idAnimal, [FromBody] Animal animal) =>
{
    if (!await CheckAnimalExists(idAnimal))
    {
        return Results.NotFound($"No animal with ID {idAnimal} found.");
    }
    
    const string query = "UPDATE Animal SET " +
                         "Name = @Name, " +
                         "Description = @Description, " +
                         "Category = @Category, " +
                         "Area = @Area " +
                         "WHERE IdAnimal = @IdAnimal";

    await using (var connection = new MySqlConnection(connectionString))
    {
        var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@IdAnimal", idAnimal);
        command.Parameters.AddWithValue("@Name", animal.Name);
        command.Parameters.AddWithValue("@Description", animal.Description);
        command.Parameters.AddWithValue("@Category", animal.Category);
        command.Parameters.AddWithValue("@Area", animal.Area);

        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();
    }

    return Results.Ok();
});

app.MapDelete("/api/animals/{idAnimal}", async (int idAnimal) =>
{
    if (!await CheckAnimalExists(idAnimal))
    {
        return Results.NotFound($"No animal with ID {idAnimal} found.");
    }
    
    const string query = "DELETE FROM Animal WHERE IdAnimal = @IdAnimal";

    await using (var connection = new MySqlConnection(connectionString))
    {
        var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@IdAnimal", idAnimal);

        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();
    }

    return Results.Ok();
});

app.Run();
return;

async Task<bool> CheckAnimalExists(int idAnimal)
{
    const string query = "SELECT COUNT(1) FROM Animal WHERE IdAnimal = @IdAnimal";
    await using var connection = new MySqlConnection(connectionString);
    var cmd = new MySqlCommand(query, connection);
    cmd.Parameters.AddWithValue("@IdAnimal", idAnimal);
        
    await connection.OpenAsync();
    var result = Convert.ToInt32(await cmd.ExecuteScalarAsync());
    return result > 0;
}

