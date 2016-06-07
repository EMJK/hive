namespace Hive.Engine
open Types
open FieldCoordsImpl
open BoardImpl
open Movement

module SoldierAnt = 
    let movementGenerator (coords: FieldCoords) (board: Board) =
        let rec spread (moves: FieldCoords list list) =
            let newMoves = Movement.spread board moves false
            if moves.Length = newMoves.Length
            then newMoves
            else spread newMoves
        spread [[coords]]