namespace Gem_Hunters
{
    public class Cell
    {
        public OccupantType Occupant { get; set; }

        public Cell(OccupantType occupant)
        {
            Occupant = occupant;
        }
    }
}