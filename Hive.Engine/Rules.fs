namespace Hive.Engine
open Types
open FieldCoordsImpl
open BoardImpl

module Rules = 

    let oneHive (src: FieldCoords) (dst: FieldCoords) (board: Board) =
        let (bug, step1) = Board.pickBug src board
        let step2 = Board.placeBug bug dst step1
        let canPick = Board.hasExactlyOneGroup step1
        let canPlace = Board.hasExactlyOneGroup step2
        let result = canPick && canPlace
        result
                    

    let freedomOfMovement (src: FieldCoords) (dst: FieldCoords) (board: Board) =
        let maxHeight = max ((Board.stackHeight src board) - 1) (Board.stackHeight dst board)
        let left, right = FieldCoords.sidesOf src dst
        let leftHeight = Board.stackHeight left board
        let rightHeight = Board.stackHeight right board
        leftHeight <= maxHeight || rightHeight <= maxHeight

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
            