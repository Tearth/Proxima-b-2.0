using Core.Commons.Colors;

namespace Core.Boards
{
    public class OccupancyContainer
    {
        public ulong FriendlyOccupancy { get; set; }
        public ulong EnemyOccupancy { get; set; }
        public ulong Occupancy { get; set; }

        public OccupancyContainer()
        {

        }

        public OccupancyContainer(Color color, ulong[] occupancy)
        {
            FriendlyOccupancy = occupancy[(int)color];
            EnemyOccupancy = occupancy[(int)ColorOperations.Invert(color)];

            Occupancy = FriendlyOccupancy | EnemyOccupancy;
        }
    }
}
