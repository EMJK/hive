namespace Hive.Engine.Tests
open Xunit
open Hive.Engine.Types
open Tools
open Hive.Engine

module MovementTest =

    [<Fact>]
    let spreadTest() =
        let board = 
            makeBoard [
                makeField (makeCoords 0 0 0) [
                    makeBug Beetle PlayerColor.Black
                ];
                makeField (makeCoords 0 1 -1) [
                    makeBug Beetle PlayerColor.White
                ]
            ]

        let moves = Movement.spread board [[makeCoords 0 0 0]] true
        ()