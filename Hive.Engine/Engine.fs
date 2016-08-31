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
                |> List.map (fun (c, b) -> (c, List.tryHead b))
                |> List.filter (fun (c, b) -> b.IsSome)
                |> List.map fst

            let emptyNeighborCoords =  
                bugCoords
                |> List.map FieldCoords.neighbors
                |> List.concat
                |> List.distinct
                |> List.filter (fun x -> not (Board.isPopulated x board))


            emptyNeighborCoords
                |> List.filter (fun x -> Rules.teamPlacement color x board)

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

    let hasQueen stack color =
        stack
        |> List.tryFind(fun bug -> bug.BugType = BugType.QueenBee && bug.Color = color)
        |> Option.isSome
    
    let hasPlayerLost color state =
        let board = state.Board
        let queenCoords = 
            Map.toSeq board.Map
                |> Seq.filter (fun (_, bugs) -> hasQueen bugs PlayerColor.White)
                |> Seq.map fst
                |> Seq.tryHead
        match queenCoords with
        | None -> false
        | Some(coords) -> 
            FieldCoords.neighbors coords
            |> Seq.forall (fun x -> Board.isPopulated x board)

    let findWinner state =
        let whiteLost = hasPlayerLost PlayerColor.White state
        let blackLost = hasPlayerLost PlayerColor.Black state
        if whiteLost && blackLost then Some(Draw)
        elif whiteLost then Some(Player(PlayerColor.Black))
        elif blackLost then Some(Player(PlayerColor.White))
        else None        

    let newGame () = { 
        Board = { Map = Map.empty };
        Moves = [];
    }

    let applyAction state action playerColor = 
        let newBoard = Board.applyAction action state.Board
        let moves = state.Moves @ [{ Player = playerColor; Action = action }]
        { Board = newBoard; Moves = moves }
        