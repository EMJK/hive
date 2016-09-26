namespace Hive.Engine
open Types
open FieldCoordsImpl
open BoardImpl
open Movement

module Grasshopper = 
    let findJumpTarget coords offset board =    
        let rec jumpToOffset coords =
            let nextField = FieldCoords.add coords offset
            if Board.isPopulated nextField board
            then jumpToOffset nextField
            else nextField
        let target = jumpToOffset coords
        if (FieldCoords.neighbors coords) |> List.contains target
        then None
        else Some target

    let movementGenerator (coords: FieldCoords) (board: Board) =
        let targets = 
            FieldCoords.neighborOffsets
            |> List.map (fun x -> findJumpTarget coords x board)
            |> List.choose id
        targets
        |> List.map (fun target -> [coords; target])
        |> Seq.ofList