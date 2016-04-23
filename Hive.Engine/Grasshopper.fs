namespace Hive.Engine
open Types
open FieldCoordsImpl
open BoardImpl
open Movement

module Grasshopper = 
    let findJumpTarget coords offset board =    
        let rec jumpToOffset sequence =
            let nextField = FieldCoords.add (List.head sequence) offset
            if Board.isPopulated nextField board
            then jumpToOffset (nextField :: sequence)
            else if List.length sequence = 2
            then None
            else Some sequence
        jumpToOffset [coords]

    let movementGenerator (coords: FieldCoords) (board: Board) =
        FieldCoords.neighborOffsets
        |> List.map (fun x -> findJumpTarget coords x board)
        |> List.choose (fun x -> x)