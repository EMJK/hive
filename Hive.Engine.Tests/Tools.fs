namespace Hive.Engine.Tests
open Hive.Engine.Types

module Tools =
    let makeCoords x y z = { X = x; Y = y; Z = z}

    let makeBug bugType color = { BugType = bugType; Color = color }

    let makeField coords bugs = (coords, bugs)

    let makeBoard fields = { Map = Map.ofList fields }