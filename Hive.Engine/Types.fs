﻿namespace Hive.Engine
module Types =
    type StepType = Jump | ToHive | ToGround

    type BugType = Beetle | Grasshopper | Ladybug | Mosquito | PillBug | QueenBee | SoldierAnt | Spider

    type PlayerColor = Black | White

    type Bug = { BugType: BugType; Color: PlayerColor }

    type FieldCoords = { X:int; Y:int; Z:int }

    type NewBug = { Bug: Bug; Field: FieldCoords }

    type Action = New of NewBug | Move of FieldCoords list

    type Move = { Player: PlayerColor; Action: Action }

    type Board = { Map: Map<FieldCoords, List<Bug>> }        

    type GameState = { Board: Board; Moves: Move list }

    type Winner = Draw | Player of PlayerColor

    type MovementGenerator = FieldCoords -> Board -> FieldCoords list list
