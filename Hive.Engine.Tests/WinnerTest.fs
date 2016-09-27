namespace Hive.Engine.Tests
open Xunit
open Hive.Engine.Types
open Tools
open Hive.Engine
open BoardImpl
open FieldCoordsImpl

module WinnerTest =
    [<Fact>]
    let testWinner1() =
        let aroundCenter = 
            FieldCoords.neighbors (makeCoords 0 0 0)
            |> List.map (fun x -> makeField x [makeBug Beetle White])
        let center = makeField (makeCoords 0 0 0) [makeBug QueenBee Black];
        let board = makeBoard (center :: aroundCenter)
        let state = { Board = board; Moves = [] }
        let winner = Engine.findWinner state
        let isWhite = 
            match winner with
            | Some(w) ->
                match w with
                | Player(x) -> x = White
                | _ -> false
            | _ -> false
        Assert.True(isWhite)


    [<Fact>]
    let testWinner2() =
        let aroundCenter = 
            FieldCoords.neighbors (makeCoords 0 0 0)
            |> List.map (fun x -> makeField x [makeBug Beetle White])
        let center = makeField (makeCoords 0 0 0) [makeBug QueenBee White];
        let board = makeBoard (center :: aroundCenter)
        let state = { Board = board; Moves = [] }
        let winner = Engine.findWinner state
        let isBlack = 
            match winner with
            | Some(w) ->
                match w with
                | Player(x) -> x = Black
                | _ -> false
            | _ -> false
        Assert.True(isBlack)

    [<Fact>]
    let testWinner3() =
        let aroundCenter = 
            FieldCoords.neighbors (makeCoords 0 0 0)
            |> List.append (FieldCoords.neighbors (makeCoords 1 -1 0))
            |> List.distinct
            |> List.except [makeCoords 0 0 0; makeCoords 1 -1 0]
            |> List.map (fun x -> makeField x [makeBug Beetle White])

        let center1 = makeField (makeCoords 0 0 0) [makeBug QueenBee Black];
        let center2 = makeField (makeCoords 1 -1 0) [makeBug QueenBee White];
        let board = makeBoard (center1 :: center2 :: aroundCenter)
        let state = { Board = board; Moves = [] }
        let winner = Engine.findWinner state
        let isNone = 
            match winner with
            | Some(Draw) -> true
            | _ -> false

        Assert.True(isNone)
