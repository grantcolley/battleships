using System;
using System.Collections.Generic;
using System.Linq;

namespace Battleships.Domain
{
    public enum ShotResult
    {
        Miss,
        Hit,
        Sunk,
        GameOver,
        AlreadyShot
    }

    public class Game
    {
        private readonly Board _board;
        private readonly HashSet<Coordinate> _shots = new();
        private int _remainingShipCells;

        public Game(Board board)
        {
            _board = board ?? throw new ArgumentNullException(nameof(board));
            _remainingShipCells = board.TotalShipCells;
        }

        public ShotResult ProcessShot(Coordinate coord)
        {
            if (!_shots.Add(coord)) return ShotResult.AlreadyShot;

            if (!_board.TryGetShipAt(coord, out var ship) || ship == null)
            {
                return ShotResult.Miss;
            }

            var added = ship.RegisterHit(coord);
            if (!added)
            {
                // already counted in hits but we deduplicated earlier
            }

            _remainingShipCells = Math.Max(0, _remainingShipCells - 1);

            if (ship.IsSunk)
            {
                if (_remainingShipCells == 0) return ShotResult.Sunk; // console will print Sunk then Game Over
                return ShotResult.Sunk;
            }

            return ShotResult.Hit;
        }

        public bool IsGameOver => _remainingShipCells == 0;
    }
}
