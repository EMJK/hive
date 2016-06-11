namespace Hive.Engine
open Types
open FieldCoordsImpl
open BoardImpl
open Rules
open Movement

open Beetle
open Grasshopper
open Ladybug
open Mosquito
open PillBug
open QueenBee
open SoldierAnt
open Spider

module Engine = 
    let private getGenerator (bug:Bug) : MovementGenerator =
        match bug.BugType with
        | Beetle -> Beetle.movementGenerator
        | Grasshopper -> Grasshopper.movementGenerator
        | Ladybug -> Ladybug.movementGenerator
        | Mosquito -> Mosquito.movementGenerator
        | PillBug -> PillBug.movementGenerator
        | QueenBee -> QueenBee.movementGenerator
        | SoldierAnt -> SoldierAnt.movementGenerator
        | Spider -> Spider.movementGenerator

    let oppositeColor color =
        match color with
        | PlayerColor.Black -> PlayerColor.White
        | PlayerColor.White -> PlayerColor.Black

    let getNewBugPlacementOptions (board: Board) (color: PlayerColor) =
        if Board.isEmpty board
        then [{X=0;Y=0;Z=0}]
        else
            let bugCoords = 
                Map.toList board.Map
                |> List.map (fun (c,s) -> (c, List.tryHead s))
                |> List.choose (fun (c,b) ->
                    match b with
                    | None -> None
                    | Some(x) -> Some(c,x))
                |> List.filter (fun (c,b) -> b.Color = color)
                |> List.map (fun (c,b) -> c)
            let emptyFields = 
                bugCoords
                |> List.map (fun x -> FieldCoords.neighbors x)
                |> List.concat
                |> List.distinct
                |> List.filter (fun x -> not <| Board.isPopulated x board)

            emptyFields
            |> List.filter (fun x -> 
                FieldCoords.neighbors x
                |> List.forall (fun x ->
                    match Board.topBugAt x board with
                    | None -> true
                    | Some(x) -> x.Color = color))

    let getPossibleMovesForPlayer (board: Board) (color: PlayerColor) =
        let allBugs =
            board.Map
            |> Map.toList
            |> List.filter (fun (c,s) -> not <| List.isEmpty s)
            |> List.map (fun (c,s) -> (c, List.head(s)))
        let playerBugs = allBugs |> List.filter (fun (c,b) -> b.Color = color)
        let enemyBugs = allBugs |> List.filter (fun (c,b) -> b.Color = oppositeColor color)
        
        let playerBugMoves = 
            playerBugs
            |> List.map (fun (c,b) ->
                let generator = getGenerator b
                let moves = generator c board
                moves)
            |> List.concat

        let playerBugMovesOverPillBug =
            playerBugs
            |> List.map (fun (c,_) -> Movement.movesOverPillBug c board color)
            |> List.concat

        let enemyBugMovesOverPillBug =
            enemyBugs
            |> List.map (fun (c,_) -> Movement.movesOverPillBug c board color)
            |> List.concat

        let allMoves = 
            playerBugMoves @ playerBugMovesOverPillBug @ enemyBugMovesOverPillBug
            |> List.distinctBy (fun x -> (List.head x, List.last x))
            |> List.groupBy (fun x -> List.head x)
            |> Set.ofList

        allMoves