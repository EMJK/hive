namespace Hive.Engine
open Types
open FieldCoordsImpl
open BoardImpl

module Movement =
    let clean (movements: FieldCoords list list) =
        movements
        |> List.sortBy List.length
        |> List.distinctBy (fun x -> List.last x)        

    let movesOverPillBug (coords: FieldCoords) (board: Board) (pillBugColor: PlayerColor) =
        let movesToPillbugs = 
            FieldCoords.neighbors coords 
            |> List.filter (fun x ->
                match Board.topBugAt x board with
                | None -> false
                | Some bug -> bug.BugType = PillBug && bug.Color = pillBugColor)
            |> List.map (fun x -> [coords;x])
        let movesToTargets = 
            movesToPillbugs
            |> List.map (fun move -> 
                let pillBug = List.last move
                let emptyNeighbors =
                    FieldCoords.neighbors pillBug
                    |> List.filter (fun x -> not (Board.isPopulated x board))
                emptyNeighbors
                |> List.map (fun x -> move @ [x]))
        let result = clean (List.concat movesToTargets)
        result
        |> List.filter (fun x ->
            match x with
            | [src;middle;_] -> Rules.oneHive src middle board
            | _ -> false)

    let spread (board: Board) (pool: FieldCoords list list) (canClimb: bool) =
        let twoLastItems lst = List.rev lst |> (fun x -> x.Tail.Head, x.Head)
        let singleSpread (pool: FieldCoords list) =
            let nextFields = 
                FieldCoords.neighbors (List.last pool)
                |> List.filter (fun x -> not <| List.contains x pool)
                |> List.filter (fun x -> canClimb || (not <| Board.isPopulated x board))
                |> List.filter (fun x -> Rules.freedomOfMovement [(List.last pool); x] board)
            if List.isEmpty nextFields
            then []
            else nextFields |> List.map (fun x -> pool @ [x])
        pool
        |> List.map (fun x -> singleSpread x)
        |> List.concat
        |> List.filter (fun x -> not <| List.isEmpty x)
        |> List.filter (fun x ->
            let src,dst = twoLastItems x
            Rules.oneHive src dst board)
        |> clean

    
