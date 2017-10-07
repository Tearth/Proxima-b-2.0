namespace Core.Commons
{
    public class Move
    {
        public Position From { get; set; }
        public Position To { get; set; }

        public Move() : this(new Position(1, 1), new Position(1, 1))
        {

        }

        public Move(Position from, Position to)
        {
            From = from;
            To = to;
        }
    }
}
