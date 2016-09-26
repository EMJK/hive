namespace Hive.Engine
open Types
open FieldCoordsImpl
open BoardImpl
open Movement

module Spider = 
    let movementGenerator (coords: FieldCoords) (board: Board) =
        let root = Tree.singleton coords
        let step1 = Movement.spreadTree board root StepType.ToGround
        ignore <| Tree.allPathsTopDown step1
        let step2 = Movement.spreadTree board step1 StepType.ToGround
        ignore (Tree.allPathsTopDown step2 |> List.ofSeq)
        let step3 = Movement.spreadTree board step2 StepType.ToGround
        ignore (Tree.allPathsTopDown step3 |> List.ofSeq)
        let allPaths = Tree.allPathsTopDown step3
        allPaths |> Seq.filter (fun x -> x.Length = 4)