namespace Hive.Engine
open Types
open FieldCoordsImpl
open BoardImpl

module Movement =
    let clean (movements: MoveBug list) =
        movements
        |> List.sortBy List.length
        |> List.distinctBy (fun x -> (List.head x, List.tail x))

    let concat (movements: MoveBug list list) =
        List.reduce List.append movements

    let movesOverPillBug (coords: FieldCoords) (state: GameState) =
        let board = state.Board
        let movesToPillbugs = 
            FieldCoords.neighbors coords 
            |> List.filter (fun x ->
                match Board.topBugAt x board with
                | None -> false
                | Some bug -> bug.BugType = PillBug)
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
        movesToTargets |> concat |> clean


    let spread (board: Board) (pool: FieldCoords list list) (canClimb: bool) =
        let singleSpread (pool: FieldCoords list) =
            let nextFields = 
                FieldCoords.neighbors (List.head pool)
                |> List.filter (fun x -> not <| List.contains x pool)
                |> List.filter (fun x -> canClimb || (not <| Board.isPopulated x board))
                |> List.filter (fun x -> Rules.freedomOfMovement [(List.head pool); x] board)
            if List.isEmpty nextFields
            then []
            else nextFields |> List.map (fun x -> x :: pool)
        pool
        |> List.map (fun x -> singleSpread x)
        |> List.concat
        |> List.filter (fun x -> not <| List.isEmpty x)
        |> List.distinctBy (fun x -> List.head x)