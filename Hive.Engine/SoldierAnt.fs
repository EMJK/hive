namespace Hive.Engine
open Types
open FieldCoordsImpl
open BoardImpl
open Movement

module SoldierAnt = 
    let movementGenerator (coords: FieldCoords) (board: Board) =
        let rec spread tree =
            let oldCount = Tree.nodeCount tree
            let newTree = Movement.spreadTree board tree StepType.ToGround
            let newCount = Tree.nodeCount newTree
            if newCount = oldCount
            then newTree
            else spread newTree

        let tree = spread (Tree.singleton coords)
        let moves = Tree.allPathsTopDown tree
        moves