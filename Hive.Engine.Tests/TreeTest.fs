namespace Hive.Engine.Tests
open Xunit
open Hive.Engine.Types
open Tools
open Hive.Engine

module TreeTest =
    [<Fact>]
    let treeExpansion() =
        let mutable counter = 0
        let next() = 
            counter <- counter + 1
            counter

        let tree = Tree.singleton counter

        let expand tree = Tree.expandTips tree (fun x -> [next(); next();])
        let newTree = tree |> expand |> expand
        let allPaths = Tree.allPaths newTree
        ()
