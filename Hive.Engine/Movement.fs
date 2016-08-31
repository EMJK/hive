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

    let spread (board: Board) (paths: FieldCoords list list) (canClimb: bool) =        
        let singleSpread (path: FieldCoords list) =
            let lastStep path = 
                let reversePath = List.rev path
                (path.Tail.Head, path.Head)
            let nextFields = 
                FieldCoords.neighbors (List.last path)
                |> List.filter (fun x -> not <| List.contains x path) // filtrujemy pola znajdujące się już na ścieżce
            let sourceField = List.last path
            let newPaths = 
                nextFields
                |> List.map (fun x -> path @ [x])
            let filteredNewPaths =
                newPaths
                |> List.map (fun x -> (x, lastStep x))
                |> List.filter (fun (path, lastStep) -> )

        let 

        let checkPathFreedomOfMovement path = true

        paths
        |> List.map singleSpread
        |> List.concat
        |> clean

    
