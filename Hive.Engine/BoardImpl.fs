namespace Hive.Engine
open Types
open FieldCoordsImpl

module BoardImpl =
    type Board with
        static member isPopulated coords board =
            board.Map
            |> Map.tryFind coords
            |> (fun x -> 
                match x with
                | Some(list) -> List.length list > 0
                | _ -> false)

        static member populatedFieldCount board =
            board.Map
            |> Map.toSeq
            |> Seq.filter (fun (k,v) -> List.length v > 0)
            |> Seq.length


        static member totalBugCount board =
            board.Map
            |> Map.toSeq
            |> Seq.map snd
            |> Seq.concat
            |> Seq.length

        static member stackAt coords board =
            match Map.tryFind coords board.Map with
            | None -> None
            | Some([]) -> None
            | stack -> stack

        static member topBugAt coords board =
            match Board.stackAt coords board with
            | None -> None
            | Some(stack) -> Some(List.head stack)

        static member getBugAtLevel level coords board = 
            match Board.stackAt coords board with
            | None -> None
            | Some(stack) -> 
                stack
                |> List.rev
                |> List.skip level
                |> List.tryHead

        static member stackHeight coords board =
            match Board.stackAt coords board with
            | None -> 0
            | Some(stack) -> List.length stack

        static member pickBug coords board =
            let stack = Board.stackAt coords board
            match stack with
            | None -> failwith (sprintf "Cannot pick a bug from empty field %O" coords)
            | Some(stack) ->
                let bug = List.head stack
                let newMap = 
                    match stack with
                    | [bug] -> Map.remove coords board.Map
                    | _ -> Map.add coords (List.tail stack) board.Map
                (bug, { Map = newMap })

        static member placeBug bug coords board =
            let newStack = 
                match Board.stackAt coords board with
                | None -> [bug]
                | Some(stack) -> bug :: stack
            let newMap = Map.add coords newStack board.Map
            { Map = newMap }

        static member moveBug src dst board =
            let (bug, tmpBoard) = Board.pickBug src board
            Board.placeBug bug dst tmpBoard

        static member removeBug coords board =
            snd (Board.pickBug coords board)

        static member isEmpty board =
            board.Map
            |> Map.toSeq
            |> Seq.forall (fun (_, stack) -> List.length stack = 0)

        static member applyAction action board =
            match action with
            | New { Bug = bug; Field = coords } ->
                Board.placeBug bug coords board
            | Move move -> 
                let (bug, tmpBoard) = Board.pickBug move.Head board
                Board.placeBug bug (List.last move) tmpBoard

        static member hasExactlyOneGroup board =
            let getGroupAt (coords:FieldCoords) =
                let rec expandGroup (group: FieldCoords list) =
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
                        expandGroup newGroup            
                expandGroup [coords]

            if Board.isEmpty board 
            then false
            else    
                let coords = 
                    board.Map
                    |> Map.toSeq
                    |> Seq.filter (fun (_, s) -> s.Length > 0)
                    |> Seq.head
                    |> fst
                let group = getGroupAt coords
                group.Length = Board.populatedFieldCount board




                