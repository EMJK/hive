namespace Hive.Engine
open Types
open FieldCoordsImpl
open BoardImpl
open Movement

module PillBug = 
    let movementGenerator (coords: FieldCoords) (board: Board) =
        let root = Tree.singleton coords
        let step1 = Movement.spreadTree board root StepType.ToGround
        Tree.allPathsTopDown step1