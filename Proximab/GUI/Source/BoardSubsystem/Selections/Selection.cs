using Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.Source.BoardSubsystem.Selections
{
    internal class Selection
    {
        public Position Position { get; set; }
        public SelectionType Type { get; set; }

        public Selection(Position position, SelectionType type)
        {
            Position = position;
            Type = type;
        }
    }
}
