namespace Hive.Engine

type TreeNode<'T> = 
    | Root of 'T
    | Node of 'T * TreeNode<'T>

type Tree<'T> = {
    root: TreeNode<'T>;
    tips: TreeNode<'T> seq;
}

module Tree = 
    //reverse
    let private assemblePath node =
        let rec followPath node =
            match node with
            | Root(x) -> [x]
            | Node(x, n) -> x :: followPath n
        followPath node
    
    //reverse
    let private withIntermediatePaths path =
        let rec followPath path allPaths =
            match path with
            | [] -> allPaths
            | _ :: t -> followPath t (path :: allPaths)
        followPath path []

    let nodeCount tree =
        let rec getParentNodes node =
            match node with
            | Root(_) -> [node]
            | Node(_, p) -> node :: (getParentNodes p)
                
        let allNodes = 
            tree.tips
            |> Seq.map getParentNodes
            |> Seq.concat
            |> Seq.distinct
            
        Seq.length allNodes

    let singleton item =
        let item = Root(item)
        { root = item; tips = [item] }

    let allPaths tree =
        tree.tips
        |> Seq.map assemblePath
        |> Seq.map withIntermediatePaths
        |> Seq.concat
        |> Seq.filter (fun x -> x.Length > 1)

    let allPathsTopDown tree =
        allPaths tree
        |> Seq.map List.rev

    let getPath node =
        assemblePath node

    let expandTips tree (expander: TreeNode<'T> -> 'T list) =
        let newTips = 
            tree.tips 
            |> Seq.map (fun x -> (x, expander x))
            |> Seq.map (fun (t, c) ->
                match c with
                | [] -> [t]
                | ls -> ls |> List.map (fun x -> Node(x, t)))
            |> Seq.concat
        { root = tree.root; tips = newTips }

    let distinctTips tree = { 
        root = tree.root;
        tips = 
            tree.tips
            |> Seq.distinctBy(fun x ->
                match x with
                | Root(v) -> v
                | Node(v, p) -> v)
    }

    let maxPathLength tree =
        tree.tips
        |> Seq.map assemblePath
        |> Seq.map List.length
        |> Seq.max
