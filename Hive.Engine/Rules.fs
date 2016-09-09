namespace Hive.Engine
open Types
open FieldCoordsImpl
open BoardImpl

module Rules = 

    let oneHive (src: FieldCoords) (dst: FieldCoords) (board: Board) =
        let getGroupAt (coords:FieldCoords) (board:Board) =
            let rec expandGroup (group: FieldCoords list) (board: Board) =
                let newFields =
                    group
                    |> List.map FieldCoords.neighbors
                    |> List.concat
                    |> List.except group
                    |> List.distinct
                    |> List.filter (fun x -> Board.isPopulated x board)
                match newFields with
                | [] -> group
                | fields -> 
                    let newGroup =
                        group
                        |> List.append fields
                        |> List.distinct
                    expandGroup newGroup board
            
            expandGroup [coords] board

        let isSingleGroup (group: FieldCoords list) (board: Board) =
            List.length group = Board.populatedFieldCount board

        let isSingleGroupAt coords board =
            let group = getGroupAt coords board
            isSingleGroup group board

        let isOneHive (board: Board) =
            let bugCoords = 
                board.Map
                |> Map.toSeq
                |> Seq.filter (fun (_, stack) -> not (List.isEmpty stack))
                |> Seq.map fst
                |> Seq.tryHead
            match bugCoords with
            | Some coords -> isSingleGroupAt coords board
            | None -> true

        let (movedBug, stateAfterPickup) = (Board.pickBug src board)
        let stateAfterPlacement = Board.placeBug movedBug dst stateAfterPickup

        let canPick = isOneHive stateAfterPickup

        let canPlace = isOneHive stateAfterPlacement

        let result = canPick && canPlace
        result
                    

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

    let teamPlacement (color: PlayerColor) (coords: FieldCoords) (board: Board) =
        if Board.isEmpty board 
        then true
        elif Board.isPopulated coords board
        then false
        elif (Board.totalBugCount board) = 1
        then 
            board.Map
            |> Map.toSeq
            |> Seq.head
            |> fst
            |> FieldCoords.neighbors
            |> List.exists (fun x -> x = coords)
        else
            let neighbors = 
                FieldCoords.neighbors coords
                |> List.map (fun x -> Board.topBugAt x board)
                |> List.choose id
            (not <| List.isEmpty neighbors) && neighbors |> List.forall (fun x -> x.Color = color)

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
            