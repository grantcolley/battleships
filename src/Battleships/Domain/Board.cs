using System;
using System.Collections.Generic;
using System.Linq;

namespace Battleships.Domain
{
    public class Board
    {
        private readonly Dictionary<Coordinate, Ship> _occupancy = new();
        public IReadOnlyCollection<Ship> Ships { get; }

        public Board(IEnumerable<Ship> ships)
        {
            Ships = ships.ToArray();
            foreach (var s in Ships)
            {
                foreach (var c in s.Cells)
                {
                    if (_occupancy.ContainsKey(c)) throw new InvalidOperationException("Overlap");
                    _occupancy[c] = s;
                }
            }
        }

        public bool TryGetShipAt(Coordinate c, out Ship? ship) => _occupancy.TryGetValue(c, out ship);

        public int TotalShipCells => _occupancy.Count;
    }
}
