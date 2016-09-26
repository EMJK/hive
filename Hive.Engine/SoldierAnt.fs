namespace Hive.Engine
open Types
open FieldCoordsImpl
open BoardImpl
open Movement

module SoldierAnt = 
    let movementGenerator (coords: FieldCoords) (board: Board) =
        let rec spread tree =
            let oldLength = Tree.maxPathLength tree
            let newTree = Movement.spreadTree board tree StepType.ToGround
            let newLength = Tree.maxPathLength newTree
            if newLength = oldLength
            then newTree
            else spread newTree

        let tree = spread (Tree.singleton coords)
        let moves = Tree.allPathsTopDown tree
        moves