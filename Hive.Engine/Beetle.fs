namespace Hive.Engine
open Types
open FieldCoordsImpl
open BoardImpl
open Movement

module Beetle = 
    let movementGenerator (coords: FieldCoords) (board: Board) =
        FieldCoords.neighbors coords
        |> List.map (fun x -> [coords;x])
        |> List.filter (fun x -> Rules.freedomOfMovement x board)
        
