namespace Hive.Engine
open Types
open FieldCoordsImpl
open BoardImpl
open Movement

module Spider = 
    let movementGenerator (coords: FieldCoords) (board: Board) =
        let pool = [[coords]]
        let step1 = Movement.spread board pool false
        let step2 = Movement.spread board step1 false
        let step3 = Movement.spread board step2 false
        step3