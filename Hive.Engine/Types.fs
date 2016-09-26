namespace Hive.Engine
module Types =
    type StepType = 
        | Any 
        | ToHive 
        | ToGround
        override x.ToString() = sprintf "%A" x



    type BugType = 
        | Beetle 
        | Grasshopper 
        | Ladybug 
        | Mosquito 
        | PillBug 
        | QueenBee 
        | SoldierAnt 
        | Spider
        override x.ToString() = sprintf "%A" x

    type PlayerColor = 
        | Black 
        | White
        override x.ToString() = sprintf "%A" x

    type Bug = 
        { BugType: BugType; Color: PlayerColor }
        override x.ToString() = sprintf "%A %A" x.Color x.BugType

    type FieldCoords = 
        { X:int; Y:int; Z:int }
        override x.ToString() = sprintf "%A %A %A" x.X x.Y x.Z

    type NewBug = 
        { Bug: Bug; Field: FieldCoords }
        override x.ToString() = sprintf "%A on %A" x.Bug x.Field

    type Action = 
        | New of NewBug 
        | Move of FieldCoords list
        override x.ToString() = sprintf "%A" x

    type Move = 
        { Player: PlayerColor; Action: Action }
        override x.ToString() = sprintf "%A" x

    type Board = 
        { Map: Map<FieldCoords, List<Bug>> }
        override x.ToString() = sprintf "%A" x.Map

    type GameState = 
        { Board: Board; Moves: Move list }
        override x.ToString() = sprintf "%A" x

    type Winner = 
        | Draw 
        | Player of PlayerColor
        override x.ToString() = sprintf "%A" x

    type MovementGenerator = FieldCoords -> Board -> FieldCoords list seq
