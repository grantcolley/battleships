using System.Collections.Generic;
using System.Linq;

namespace Battleships.Domain
{
    public class Ship
    {
        public ShipType Type { get; }
        public IReadOnlyCollection<Coordinate> Cells { get; }
        private readonly HashSet<Coordinate> _hits = new();

        public Ship(ShipType type, IEnumerable<Coordinate> cells)
        {
            Type = type;
            Cells = cells.ToArray();
        }

        public bool Occupies(Coordinate c) => Cells.Contains(c);

        public bool IsSunk => _hits.Count >= Cells.Count;

        public bool RegisterHit(Coordinate c)
        {
            if (!Occupies(c)) return false;
            return _hits.Add(c);
        }

        public int Hits => _hits.Count;
    }
}
