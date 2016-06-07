namespace Hive.Engine
open Types
open FieldCoordsImpl
open BoardImpl

module Rules = 

    let oneHive (src: FieldCoords) (dst: FieldCoords) (board:Board) =
        let getGroupAt (coords:FieldCoords) (board:Board) =
            let rec expandGroup (found:FieldCoords list) (next:FieldCoords list) (board:Board) =
                let newFields =
                    next
                    |> List.map FieldCoords.neighbors
                    |> List.concat
                    |> List.except next
                    |> List.except found
                    |> List.distinct
                    |> List.filter (fun x -> Board.isPopulated x board)
                if List.length newFields = 0
                then found
                else expandGroup (List.append found newFields) newFields board
            expandGroup [] [coords] board

        let isSingleGroup (group: FieldCoords list) (board: Board) =
            List.length group = Board.populatedFieldCount board

        let canPick () =
            let possibleNextState = snd (Board.pickBug src board)
            let startCoords = 
                FieldCoords.neighbors src
                |> List.filter (fun x -> Board.isPopulated x possibleNextState)
                |> List.tryHead
            match startCoords with
            | None -> false
            | Some(src) ->
                let possibleGroup = getGroupAt src possibleNextState
                isSingleGroup possibleGroup possibleNextState

        let canMove () =
            if Board.isPopulated src board && not (Board.isPopulated dst board)
            then
                let populatedNeighbors (coords: FieldCoords)=
                    FieldCoords.neighbors coords
                    |> List.filter (fun x -> Board.isPopulated x board)
                let srcNeighbors = populatedNeighbors src
                let dstNeighbors = populatedNeighbors dst
                let commonNeighbors = Set.intersect (Set.ofList srcNeighbors) (Set.ofList dstNeighbors)
                Set.count commonNeighbors > 0
            else false

        canPick() && canMove()
                    

    let freedomOfMovement (sequence: FieldCoords list) (board:Board) =
        let canMove src dst board =
            let maxHeight = max (Board.stackHeight src board) (Board.stackHeight dst board)
            let left, right = FieldCoords.sidesOf src dst
            let leftHeight = Board.stackHeight left board
            let rightHeight = Board.stackHeight right board
            leftHeight <= maxHeight || rightHeight <= maxHeight

        let newState = Board.removeBug (List.head sequence) board
        sequence
        |> List.windowed 2
        |> List.forall (fun list -> 
            match list with
            | [src;dst] -> canMove src dst newState
            | _ -> false)

    let teamPlacement (bug: Bug) (coords: FieldCoords) (board: Board) =
        if Board.isEmpty board 
        then true
        else
            let neighbors = FieldCoords.neighbors coords |> List.map (fun x -> Board.topBugAt x board)
            let noEnemies =
                neighbors
                |> List.forall (fun x ->
                    match x with
                    | None -> true
                    | Some(neighbor) -> not (neighbor.Color = bug.Color))
            let atLeastOneFriend =
                neighbors
                |> List.filter (fun x -> 
                    match x with
                    | None -> false
                    | Some neighbor -> neighbor.Color = bug.Color)
                |> List.isEmpty 
                |> not
            noEnemies && atLeastOneFriend

    let getWinner (board: Board) =
        let findQueenBee color = 
            let maybeBug =
                board.Map
                |> Map.toSeq
                |> Seq.tryFind  (fun (coords, stack) ->
                    match List.tryFind (fun x -> x.BugType = QueenBee) stack with
                    | None -> false
                    | Some(bug) -> bug.Color = color)
            match maybeBug with
            | None -> None
            | Some(coords, bug) -> Some(coords)

        let isSurrounded coords =
            FieldCoords.neighbors coords
            |> List.forall (fun x -> Board.isPopulated x board)
                        
        let playerLost color =
            match findQueenBee color with
                | None -> false
                | Some(coords) -> isSurrounded coords

        let whiteLost = playerLost White
        let blackLost = playerLost Black

        if (whiteLost && blackLost) then Some Draw
        else if (whiteLost) then Some (Player Black)
        else if (blackLost) then Some (Player White)
        else None
            