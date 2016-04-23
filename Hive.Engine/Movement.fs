namespace Hive.Engine
open Types
open FieldCoordsImpl
open BoardImpl

module Movement =
    let concat (movements: MoveBug list list) =
        movements
        |> List.reduce List.append
        |> List.sortBy List.length
        |> List.distinctBy (fun x -> (List.head x, List.tail x))



    