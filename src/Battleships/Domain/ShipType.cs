namespace Battleships.Domain
{
    public enum ShipType
    {
        Carrier,
        Battleship,
        Cruiser,
        Submarine,
        Destroyer
    }

    public static class ShipTypeExtensions
    {
        public static int Size(this ShipType t) => t switch
        {
            ShipType.Carrier => 5,
            ShipType.Battleship => 4,
            ShipType.Cruiser => 3,
            ShipType.Submarine => 3,
            ShipType.Destroyer => 2,
            _ => 0
        };
    }
}
