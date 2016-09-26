namespace Hive.Engine
open Types
open FieldCoordsImpl
open BoardImpl

module Movement =
    let movesOverPillBug (coords: FieldCoords) (board: Board) (player: PlayerColor) =
        let neighborFriendlyPillBugCoords =
            let isNearPillBug coords =
                let neighbors = 
                    FieldCoords.neighbors coords
                    |> List.map (fun x -> Board.topBugAt x board)
                    |> List.choose id
                    |> List.map (fun x -> x.BugType)
                neighbors |> List.contains BugType.PillBug
            FieldCoords.neighbors coords
            |> List.map (fun x -> (x, Board.stackAt x board))
            |> List.map (fun (c, s) ->
                match s with
                | None -> None
                | Some(x) -> Some(c, x))
            |> List.choose id
            |> List.filter (fun (_, s) -> s.Length = 1)
            |> List.map (fun (c, s) -> (c, s.Head))
            |> List.filter (fun (c, b) -> b.Color = player)
            |> List.map (fun (c, b) -> (c, b.BugType))
            |> List.filter (fun (c, b) ->
                match b with
                | PillBug -> true
                | Mosquito -> isNearPillBug c
                | _ -> false)
            |> List.map fst

        let movesToPillbugs = 
            neighborFriendlyPillBugCoords
            |> List.map (fun x -> (coords, x))
            |> List.filter (fun (a, b) -> Rules.freedomOfMovement a b board)
            |> List.filter (fun (a, b) -> Rules.oneHive a b board)

        let movesToTargets =
            let emptyNeighbors coords =
                FieldCoords.neighbors coords
                |> List.filter (fun x -> not (Board.isPopulated x board))
            movesToPillbugs
            |> List.map (fun (a, b) -> (a, b, emptyNeighbors b |> List.except [a]))
            |> List.map (fun (a, b, l) -> l |> List.map (fun x -> [a; b; x]))
            |> List.concat

        movesToTargets

    let spreadTree (board: Board) (tree: Tree<FieldCoords>) (stepType: StepType) =
        let root = tree.root
        let expandNode node =
            let path = Tree.getPath node
            let tmpBoard = Board.moveBug (List.last path) path.Head board
            let nextFields = 
                FieldCoords.neighbors path.Head 
                |> List.except path.Tail
                |> List.filter (fun x ->
                    match stepType with
                    | Any -> true
                    | ToGround -> not (Board.isPopulated x tmpBoard)
                    | ToHive -> Board.isPopulated x tmpBoard)
                |> List.filter (fun x ->                    
                    let freedomOfMovement = Rules.freedomOfMovement path.Head x tmpBoard
                    let oneHive = Rules.oneHive path.Head x tmpBoard
                    let canMove = freedomOfMovement && oneHive
                    canMove)
            nextFields
        Tree.expandTips tree expandNode |> Tree.distinctTips