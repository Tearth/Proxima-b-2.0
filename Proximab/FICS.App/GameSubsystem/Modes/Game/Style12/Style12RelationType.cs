using System.Diagnostics.CodeAnalysis;

namespace FICS.App.GameSubsystem.Modes.Game.Style12
{
    /// <summary>
    /// Represents available types of Style12 relations.
    /// </summary>
    [SuppressMessage("ReSharper", "MissingXmlDoc")]
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
