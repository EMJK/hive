namespace Hive.Engine
open Types
open FieldCoordsImpl
open BoardImpl
open Movement

module SoldierAnt = 
    let movementGenerator (coords: FieldCoords) (board: Board) =
        let totalSteps (moves: FieldCoords list list) =
            moves
            |> List.map List.length
            |> Seq.sum

        let rec spread (moves: FieldCoords list list) =
            let newMoves = Movement.spread board moves false
            let allMoves = 
                newMoves @ moves
                |> Movement.clean

            if moves.Length = allMoves.Length
            then allMoves
            else 
                spread newMoves @ allMoves
                |> Movement.clean
        spread [[coords]]