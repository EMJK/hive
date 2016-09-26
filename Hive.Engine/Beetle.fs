namespace Hive.Engine
open Types
open FieldCoordsImpl
open BoardImpl
open Movement

module Beetle = 
    let movementGenerator (coords: FieldCoords) (board: Board) =
        let root = Tree.singleton coords
        let step1 = Movement.spreadTree board root StepType.Any
        Tree.allPathsTopDown step1
        
