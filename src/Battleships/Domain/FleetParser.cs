using System;
using System.Collections.Generic;
using System.Linq;

namespace Battleships.Domain
{
    public class FleetParseResult
    {
        public Board? Board { get; }
        public string? Error { get; }

        public bool IsValid => Board != null && Error == null;

        public FleetParseResult(Board board)
        {
            Board = board;
        }

        public FleetParseResult(string error)
        {
            Error = error;
        }
    }

    public static class FleetParser
    {
        private static readonly Dictionary<string, ShipType> _nameMap = Enum.GetNames(typeof(ShipType))
            .ToDictionary(n => n, n => Enum.Parse<ShipType>(n), StringComparer.OrdinalIgnoreCase);

        public static FleetParseResult ParseLines(IEnumerable<string> lines)
        {
            var entries = new List<(ShipType type, List<Coordinate> coords)>();
            var seenNames = new List<string>();

            foreach (var raw in lines.Select(l => l?.Trim()).Where(l => !string.IsNullOrEmpty(l)))
            {
                var parts = raw!.Split(',').Select(p => p.Trim()).Where(p => p.Length > 0).ToArray();
                if (parts.Length < 2) return new FleetParseResult("Invalid coordinate");
                var name = parts[0];
                if (!_nameMap.TryGetValue(name, out var type)) return new FleetParseResult("Ships not recognised");
                seenNames.Add(name);
                var coords = new List<Coordinate>();
                foreach (var cs in parts.Skip(1))
                {
                    if (!Coordinate.TryParse(cs, out var coord)) return new FleetParseResult("Invalid coordinate");
                    coords.Add(coord);
                }
                entries.Add((type, coords));
            }

            // Check presence and duplicates
            var required = Enum.GetValues<ShipType>().ToHashSet();
            var provided = entries.Select(e => e.type).ToList();
            if (provided.Count != required.Count || provided.ToHashSet().Count != required.Count)
            {
                return new FleetParseResult("Ships not recognised");
            }

            // Validate each ship size and placement
            var occupied = new HashSet<Coordinate>();
            var ships = new List<Ship>();

            foreach (var (type, coords) in entries)
            {
                if (coords.Count != type.Size()) return new FleetParseResult("Incorrect ship size");

                // all coords within board already checked by Coordinate.TryParse

                // check straight line
                bool sameX = coords.All(c => c.X == coords[0].X);
                bool sameY = coords.All(c => c.Y == coords[0].Y);
                if (!sameX && !sameY) return new FleetParseResult("Ships cannot sit diagonally on the board");

                // check contiguous
                if (sameX)
                {
                    var ys = coords.Select(c => c.Y).OrderBy(y => y).ToArray();
                    for (int i = 1; i < ys.Length; i++) if (ys[i] != ys[i - 1] + 1) return new FleetParseResult("Ships cannot sit diagonally on the board");
                }
                else // sameY
                {
                    var xs = coords.Select(c => c.X).OrderBy(x => x).ToArray();
                    for (int i = 1; i < xs.Length; i++) if (xs[i] != xs[i - 1] + 1) return new FleetParseResult("Ships cannot sit diagonally on the board");
                }

                // check overlap
                foreach (var c in coords)
                {
                    if (occupied.Contains(c)) return new FleetParseResult("Ships cannot overlap");
                }

                foreach (var c in coords) occupied.Add(c);
                ships.Add(new Ship(type, coords));
            }

            try
            {
                var board = new Board(ships);
                return new FleetParseResult(board);
            }
            catch
            {
                return new FleetParseResult("Ships cannot overlap");
            }
        }
    }
}
