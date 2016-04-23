namespace Hive.Engine
open Types
open FieldCoordsImpl
open BoardImpl
open Rules
open Movement

open Beetle
open Grasshopper
open Ladybug
open Mosquito
open PillBug
open QueenBee
open SoldierAnt
open Spider

module MovementChecker = 
    let private getGenerator (bug:Bug) =
        match bug.BugType with
        | Beetle -> Beetle.movementGenerator
        | Grasshopper -> Grasshopper.movementGenerator
        | Ladybug -> Ladybug.movementGenerator
        | Mosquito -> Mosquito.movementGenerator
        | PillBug -> PillBug.movementGenerator
        | QueenBee -> QueenBee.movementGenerator
        | SoldierAnt -> SoldierAnt.movementGenerator
        | Spider -> Spider.movementGenerator