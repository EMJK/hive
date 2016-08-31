namespace Hive.Engine.Tests
open Xunit
open Hive.Engine.Types
open Tools

module RulesTest =
    [<Fact>]
    let freedomOfMovementRuleTest() =
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
        Assert.True(Hive.Engine.Rules.freedomOfMovement [(makeCoords 0 0 0); (makeCoords 1 -1 0)] board)
        Assert.True(Hive.Engine.Rules.freedomOfMovement [(makeCoords 0 0 0); (makeCoords 0 -1 1)] board)
        Assert.True(Hive.Engine.Rules.freedomOfMovement [(makeCoords 0 0 0); (makeCoords -1 0 1)] board)
        Assert.True(Hive.Engine.Rules.freedomOfMovement [(makeCoords 0 0 0); (makeCoords -1 1 0)] board)
        Assert.True(Hive.Engine.Rules.freedomOfMovement [(makeCoords 0 0 0); (makeCoords 0 1 -1)] board)
        Assert.False(Hive.Engine.Rules.freedomOfMovement [(makeCoords 0 0 0); (makeCoords 1 0 -1)] board)

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
        Assert.False(Hive.Engine.Rules.teamPlacement (makeBug Beetle PlayerColor.Black) (makeCoords -1 -1 2) board)
        Assert.False(Hive.Engine.Rules.teamPlacement (makeBug Beetle PlayerColor.White) (makeCoords -1 -1 2) board)
        Assert.True(Hive.Engine.Rules.teamPlacement (makeBug Beetle PlayerColor.Black) (makeCoords 0 -1 1) board)
        Assert.False(Hive.Engine.Rules.teamPlacement (makeBug Beetle PlayerColor.White) (makeCoords 0 -1 1) board)
        Assert.False(Hive.Engine.Rules.teamPlacement (makeBug Beetle PlayerColor.Black) (makeCoords 0 2 -2) board)
        Assert.True(Hive.Engine.Rules.teamPlacement (makeBug Beetle PlayerColor.White) (makeCoords 0 2 -2) board)
        Assert.False(Hive.Engine.Rules.teamPlacement (makeBug Beetle PlayerColor.Black) (makeCoords 1 0 -1) board)
        Assert.False(Hive.Engine.Rules.teamPlacement (makeBug Beetle PlayerColor.White) (makeCoords 1 0 -1) board)