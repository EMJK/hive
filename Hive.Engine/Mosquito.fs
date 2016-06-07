namespace Hive.Engine
open Types
open FieldCoordsImpl
open BoardImpl
open Movement

module Mosquito = 
    let movementGenerator (coords: FieldCoords) (board: Board) =
        if Board.stackHeight coords board > 1
        then Beetle.movementGenerator coords board
        else
            let neighbors = FieldCoords.neighbors coords
            let neighborBugs = 
                neighbors
                |> List.map (fun x -> Board.topBugAt x board)
                |> List.choose (fun x -> x)
            let availableMovesets = 
                neighborBugs
                |> List.map (fun x ->
                    match x.BugType with
                    | BugType.Beetle -> Some(Beetle.movementGenerator)
                    | BugType.Grasshopper -> Some(Grasshopper.movementGenerator)
                    | BugType.Ladybug -> Some(Ladybug.movementGenerator)
                    | BugType.PillBug -> Some(PillBug.movementGenerator)
                    | BugType.QueenBee -> Some(QueenBee.movementGenerator)
                    | BugType.SoldierAnt -> Some(SoldierAnt.movementGenerator)
                    | BugType.Spider -> Some(Spider.movementGenerator)
                    | _ -> None)
                |> List.choose (fun x -> x)
                |> List.map (fun x -> x coords board)
            availableMovesets
            |> List.concat
            |> Movement.clean
