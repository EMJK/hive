namespace Hive.Engine
open Types

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

        static member removeBug coords board =
            snd (Board.pickBug coords board)

        static member isEmpty board =
            board.Map
            |> Map.toSeq
            |> Seq.forall (fun (_, stack) -> List.length stack > 0)

                