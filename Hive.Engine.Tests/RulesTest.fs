namespace Hive.Engine.Tests
open Xunit
open Hive.Engine.Types
open Tools
open Hive.Engine
open BoardImpl
open FieldCoordsImpl

module RulesTest =
    [<Fact>]
    let freedomOfMovementRuleTest1() =
        let board = 
            makeBoard [
                makeField (makeCoords 0 0 0) [
                    makeBug Beetle PlayerColor.Black
                ];
                makeField (makeCoords 0 1 -1) [
                    makeBug Beetle PlayerColor.Black
                ];
                makeField (makeCoords 1 -1 0) [
                    makeBug Beetle PlayerColor.White
                ]
            ]
        Assert.True(Hive.Engine.Rules.freedomOfMovement  (makeCoords 0 0 0) (makeCoords 1 -1 0) board)
        Assert.True(Hive.Engine.Rules.freedomOfMovement  (makeCoords 0 0 0) (makeCoords 0 -1 1) board)
        Assert.True(Hive.Engine.Rules.freedomOfMovement  (makeCoords 0 0 0) (makeCoords -1 0 1) board)
        Assert.True(Hive.Engine.Rules.freedomOfMovement  (makeCoords 0 0 0) (makeCoords -1 1 0) board)
        Assert.True(Hive.Engine.Rules.freedomOfMovement  (makeCoords 0 0 0) (makeCoords 0 1 -1) board)
        Assert.False(Hive.Engine.Rules.freedomOfMovement (makeCoords 0 0 0) (makeCoords 1 0 -1) board)

    [<Fact>]
    let freedomOfMovementRuleTest2() =
        let src = makeCoords 1 0 -1
        let dst = makeCoords 2 -1 -1
        let (left, right) = FieldCoords.sidesOf src dst
        Assert.Equal((makeCoords 2 0 -2), left)
        Assert.Equal((makeCoords 1 -1 0), right)

    [<Fact>]
    let testCase1() =
        let board = 
            makeBoard [
                makeField (makeCoords -3 2 1) [makeBug SoldierAnt PlayerColor.Black];
                makeField (makeCoords -2 2 0) [makeBug Mosquito PlayerColor.Black];
                makeField (makeCoords -1 1 0) [makeBug QueenBee PlayerColor.Black];
                makeField (makeCoords 0 1 -1) [makeBug Spider PlayerColor.White];
                makeField (makeCoords 0 0 0)  [makeBug Beetle PlayerColor.White];
                makeField (makeCoords 1 -1 0) [makeBug SoldierAnt PlayerColor.White];
                makeField (makeCoords 0 -1 1) [makeBug Grasshopper PlayerColor.White];
            ]
        let allMoves = Engine.getPossibleMovesForPlayer board White
        let spiderCoords = makeCoords 0 1 -1
        let spiderMoves = Spider.movementGenerator spiderCoords board |> List.ofSeq
        Assert.Equal(2, Seq.length spiderMoves)


    [<Fact>]
    let oneHiveRuleTest() = 
        let board = 
            makeBoard [
                makeField (makeCoords 0 0 0) [
                    makeBug Beetle PlayerColor.Black
                ];
                makeField (makeCoords 0 1 -1) [
                    makeBug Beetle PlayerColor.Black
                ];
                makeField (makeCoords 1 -1 0) [
                    makeBug Beetle PlayerColor.White
                ]
            ]
        Assert.False(Hive.Engine.Rules.oneHive (makeCoords 0 0 0) (makeCoords 1 0 -1) board)
        Assert.False(Hive.Engine.Rules.oneHive (makeCoords 0 0 0) (makeCoords -1 1 0) board)
        Assert.False(Hive.Engine.Rules.oneHive (makeCoords 0 0 0) (makeCoords -1 0 1) board)
        Assert.True(Hive.Engine.Rules.oneHive (makeCoords 1 -1 0) (makeCoords 1 0 -1) board)
        Assert.True(Hive.Engine.Rules.oneHive (makeCoords 1 -1 0) (makeCoords 0 -1 1) board)
        Assert.True(Hive.Engine.Rules.oneHive (makeCoords 1 -1 0) (makeCoords 0 0 0) board)
        Assert.False(Hive.Engine.Rules.oneHive (makeCoords 1 -1 0) (makeCoords 1 -2 1) board)
        Assert.False(Hive.Engine.Rules.oneHive (makeCoords 0 1 -1) (makeCoords 0 2 -2) board)

    [<Fact>]
    let teamPlacementTest() = 
        let board = 
            makeBoard [
                makeField (makeCoords 0 0 0) [
                    makeBug Beetle PlayerColor.Black
                ];
                makeField (makeCoords 0 1 -1) [
                    makeBug Beetle PlayerColor.White
                ]
            ]
        Assert.False(Hive.Engine.Rules.teamPlacement PlayerColor.Black (makeCoords -1 -1 2) board)
        Assert.False(Hive.Engine.Rules.teamPlacement PlayerColor.White (makeCoords -1 -1 2) board)
        Assert.True(Hive.Engine.Rules.teamPlacement  PlayerColor.Black (makeCoords 0 -1 1) board)
        Assert.False(Hive.Engine.Rules.teamPlacement PlayerColor.White (makeCoords 0 -1 1) board)
        Assert.False(Hive.Engine.Rules.teamPlacement PlayerColor.Black (makeCoords 0 2 -2) board)
        Assert.True(Hive.Engine.Rules.teamPlacement  PlayerColor.White (makeCoords 0 2 -2) board)
        Assert.False(Hive.Engine.Rules.teamPlacement PlayerColor.Black (makeCoords 1 0 -1) board)
        Assert.False(Hive.Engine.Rules.teamPlacement PlayerColor.White (makeCoords 1 0 -1) board)