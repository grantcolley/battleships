using System;
using System.IO;
using System.Text;
using Battleships.Domain;

string exeDir = AppContext.BaseDirectory ?? Environment.CurrentDirectory;
var csvPath = Path.Combine(exeDir, "Battleships.csv");

if (!File.Exists(csvPath))
{
    Console.WriteLine("Ships not recognised");
    return;
}

var lines = File.ReadAllLines(csvPath);
var parse = FleetParser.ParseLines(lines);
if (!parse.IsValid)
{
    Console.WriteLine(parse.Error);
    return;
}

Console.WriteLine("Battleships Begin");
var game = new Game(parse.Board!);

while (true)
{
    var input = ReadLineWithEscape();
    if (input is null) return; // Escape pressed
    input = input.Trim();
    if (input.Length == 0) continue;

    if (!Coordinate.TryParse(input, out var coord))
    {
        Console.WriteLine("Invalid coordinate");
        continue;
    }

    var result = game.ProcessShot(coord);
    switch (result)
    {
        case ShotResult.AlreadyShot:
            Console.WriteLine("Cannot enter same coordinate twice");
            break;
        case ShotResult.Miss:
            Console.WriteLine("Miss");
            break;
        case ShotResult.Hit:
            Console.WriteLine("Hit");
            break;
        case ShotResult.Sunk:
            Console.WriteLine("Sunk");
            if (game.IsGameOver)
            {
                Console.WriteLine("Game Over");
                return;
            }
            break;
        default:
            break;
    }
}

static string? ReadLineWithEscape()
{
    var sb = new StringBuilder();
    while (true)
    {
        var key = Console.ReadKey(intercept: true);
        if (key.Key == ConsoleKey.Escape) return null;
        if (key.Key == ConsoleKey.Enter)
        {
            Console.WriteLine();
            return sb.ToString();
        }
        if (key.Key == ConsoleKey.Backspace)
        {
            if (sb.Length > 0)
            {
                sb.Length -= 1;
                Console.Write("\b \b");
            }
            continue;
        }
        // ignore other control keys
        if (char.IsControl(key.KeyChar)) continue;
        sb.Append(key.KeyChar);
        Console.Write(key.KeyChar);
    }
}
