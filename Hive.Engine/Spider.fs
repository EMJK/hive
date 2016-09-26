namespace Hive.Engine
open Types
open FieldCoordsImpl
open BoardImpl
open Movement

module Spider = 
    let movementGenerator (coords: FieldCoords) (board: Board) =
        let root = Tree.singleton coords
        let step1 = Movement.spreadTree board root StepType.ToGround
        let step2 = Movement.spreadTree board step1 StepType.ToGround
        let step3 = Movement.spreadTree board step2 StepType.ToGround
        Tree.allPathsTopDown step3
        |> Seq.filter (fun x -> x.Length = 4)