# Proxima b 2.0
This is the second generation of [Proxima b](https://github.com/Tearth/Proxima-b) chess engine written in C#. The engine is divided into several projects:
  * **[Proxima.Core](https://github.com/Tearth/Proxima-b-2.0/tree/master/Proximab/Proxima.Core)** - the heart and soul of engine, contains all algorithms
  * **[GUI.App](https://github.com/Tearth/Proxima-b-2.0/tree/master/Proximab/GUI.App)** - development GUI
  * **[GUI.ContentDefinitions](https://github.com/Tearth/Proxima-b-2.0/tree/master/Proximab/GUI.ContentDefinitions)** - some content classes required by Monogame (Content Pipeline)
  * **[FICS.App](https://github.com/Tearth/Proxima-b-2.0/tree/master/Proximab/FICS.App)** - Free Internet Chess Server client
  * **[CECP.App](https://github.com/Tearth/Proxima-b-2.0/tree/master/Proximab/CECP.App)** - Chess Engine Communication Protocol client
  * **[OpeningBooksGenerator.App](https://github.com/Tearth/Proxima-b-2.0/tree/master/Proximab/OpeningBookGenerator.App)** - nothing more than in name, converts input file with text notation (e4, Rxg4) to more readable form for engine (e2e4, a7a8)
  * **[MagicKeysGenerator.App](https://github.com/Tearth/Proxima-b-2.0/tree/master/Proximab/MagicKeysGenerator.App)** - generates magic keys and saves them to output file (which is faster than doing this every time when chess engine is starting)
  * **[Helpers.Logger](https://github.com/Tearth/Proxima-b-2.0/tree/master/Proximab/Helpers.Loggers)** - contains loggers used in other projects
  * **[Helpers.ColorfulConsole](https://github.com/Tearth/Proxima-b-2.0/tree/master/Proximab/Helpers.ColorfulConsole)** - library that supports console with colorful content (eg. "$gHello $rWorld" is displayed as (Green)Hello and (Red)World)

# Statistics
**Source lines of code**: ~10000
**Comments**: ~5000
**XML**: ~350

# Used algorithms:

### Board representation
  * Bitboards
  * Zobrist hash
  
### Move generation
  * Pre-initialized move arrays for kind and knight
  * Magic bitboards for slide pieces

### AI
  * Negamax (based on a copy-make method)
  * Alpha-Beta
  * Quiescence Search
  * Transposition table
  * Negascout
  * History heuristic
  * Killer heuristic
  * Patterns detection
  * Opening book

### Evaluation
  * Score includes: material, castling, king safety, mobility, pawn structure (chain, isolated and doubled pawns), position
  * Static Exchange Evaluation