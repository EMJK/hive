namespace Hive.Engine
open Types
open FieldCoordsImpl
open BoardImpl
open Movement

module PillBug = 
    let movementGenerator (coords: FieldCoords) (board: Board) =
        let pool = [[coords]]
        let step1 = Movement.spread board pool false
        step1