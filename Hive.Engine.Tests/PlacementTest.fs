namespace Hive.Engine.Tests
open Xunit
open Hive.Engine.Types
open Tools
open Hive.Engine

module PlacementTest =
    let orderCoords coords = coords |> List.sortBy (fun x -> (x.X, x.Y, x.Z))
    let listEquals list1 list2 =
        List.forall2 (fun x y -> x = y) list1 list2

    [<Fact>]
    let newBugPlacementTestWithPopulatedBoard() =
        let board = 
            makeBoard [
                makeField (makeCoords 0 0 0) [
                    makeBug Beetle PlayerColor.Black
                ];
                makeField (makeCoords 0 1 -1) [
                    makeBug Beetle PlayerColor.White
                ]
            ]
        let options = (Engine.getNewBugPlacementOptions board PlayerColor.Black) |> orderCoords
        let desiredOptions = [(makeCoords 1 -1 0); (makeCoords 0 -1 1); (makeCoords -1 0 1)] |> orderCoords
        Assert.True(listEquals options desiredOptions)

    [<Fact>]
    let newBugPlacementTestWithEmptyBoard() =
        let board = makeBoard []
        let options = Engine.getNewBugPlacementOptions board PlayerColor.Black
        Assert.True(listEquals options [makeCoords 0 0 0])

    [<Fact>]
    let newBugPlacementTestWithOneBugOnBoard() =
        let board = 
            makeBoard [
                makeField (makeCoords 0 0 0) [
                    makeBug Beetle PlayerColor.Black
                ]
            ]
        let options = (Engine.getNewBugPlacementOptions board PlayerColor.White) |> orderCoords
        let desiredOptions = 
            [
                makeCoords 1 -1 0
                makeCoords 0 -1 1
                makeCoords -1 0 1
                makeCoords -1 1 0
                makeCoords 0 1 -1
                makeCoords 1 0 -1
            ] |> orderCoords
        Assert.True(listEquals options desiredOptions)

