namespace Hive.Engine
open Types
open FieldCoordsImpl
open BoardImpl

module Movement =
    let clean (movements: FieldCoords list list) =
        movements
        |> List.sortBy List.length
        |> List.distinctBy (fun x -> (List.head x, List.last x))       

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


    let spread (board: Board) (paths: FieldCoords list list) (canClimb: bool) =
        let firstAndLast path = (List.head path, List.last path)
        let lastStep path = List.rev path |> (fun x -> (x.Tail.Head, x.Head))

        let checkLastStepFreedomOfMovement board path =
            let (src, dst) = lastStep path
            let tmpBoard = Board.moveBug src dst board
            Rules.freedomOfMovement [src; dst] board

        let checkOneHiveRule board path =
            let (src, dst) = firstAndLast path
            Rules.oneHive src dst board

        let checkClimbing board path =
            match canClimb with
            | true -> true
            | false ->
                let (_, dst) = lastStep path
                not (Board.isPopulated dst board)
            
        let singleSpread (path: FieldCoords list) =
            let nextFields = 
                FieldCoords.neighbors (List.last path)
                |> List.filter (fun x -> not <| List.contains x path) // filtrujemy pola znajdujące się już na ścieżce
            let newPaths = 
                nextFields
                |> List.map (fun x -> path @ [x])

            newPaths
                |> List.filter (fun path -> checkOneHiveRule board path)
                |> List.filter (fun path -> checkLastStepFreedomOfMovement board path)
                |> List.filter (fun path -> checkClimbing board path)

        let spreadPaths = paths |> List.map singleSpread
        paths
        |> List.map singleSpread
        |> List.concat
        |> clean

    
