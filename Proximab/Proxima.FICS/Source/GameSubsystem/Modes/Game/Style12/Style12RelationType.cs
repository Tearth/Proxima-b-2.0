using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proxima.FICS.Source.GameSubsystem.Modes.Game.Style12
{
    public enum Style12RelationType
    {
        IsolatedPosition = -3,
        ObservingGameBeingExamined = -2,
        EnemyMove = -1,
        ObservingGameBeingPlayed = 0,
        EngineMove = 1,
        ExaminerOfGame = 2
    }
}
