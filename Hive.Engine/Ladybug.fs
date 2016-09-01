namespace Hive.Engine
open Types
open FieldCoordsImpl
open BoardImpl
open Movement

module Ladybug = 
    let movementGenerator (coords: FieldCoords) (board: Board) =
        let pool = [[coords]]
        let step1 = Movement.spread board pool true
        let step2 = Movement.spread board step1 true
        let step3 = Movement.spread board step2 false
        step3
        |> List.filter (
            fun path ->
                match path with
                | [_; a; b; _] -> List.forall (fun coords -> Board.isPopulated coords board) [a; b]
                | _ -> false)