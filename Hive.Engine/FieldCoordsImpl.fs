namespace Hive.Engine
open Types

module FieldCoordsImpl =
    type FieldCoords with
        static member private round num max = ((num % max) + max) % max
        static member private neighborMap = [|
            { X = 1;  Y = 0;  Z = -1 };
            { X = 1;  Y = -1; Z = 0  };
            { X = 0;  Y = -1; Z = 1  };
            { X = -1; Y = 0;  Z = 1  };
            { X = -1; Y = 1;  Z = 0  };
            { X = 0;  Y = 1;  Z = -1 }|]

        static member diff src dst =
            { X = src.X - dst.X; Y = src.Y - dst.Y; Z = src.Z - dst.Z }

        static member neighbors coords = [
            { X = coords.X + 1; Y = coords.Y + 0; Z = coords.Z - 1 };
            { X = coords.X + 1; Y = coords.Y - 1; Z = coords.Z + 0 };
            { X = coords.X + 0; Y = coords.Y + 1; Z = coords.Z - 1 };
            { X = coords.X + 0; Y = coords.Y - 1; Z = coords.Z + 1 };
            { X = coords.X - 1; Y = coords.Y + 0; Z = coords.Z + 1 };
            { X = coords.X - 1; Y = coords.Y + 1; Z = coords.Z + 0 };]

        static member add a b =
            { X = a.X + b.X; Y = a.Y + b.Y; Z = a.Z + b.Z }

        static member neighborOffsets = List.ofArray FieldCoords.neighborMap

        static member sidesOf src dst =
            let targetDiff = FieldCoords.diff src dst 
            let targetIndex = Array.findIndex (fun x -> x = dst) FieldCoords.neighborMap
            let leftIndex = FieldCoords.round (targetIndex - 1) FieldCoords.neighborMap.Length
            let rightIndex = FieldCoords.round (targetIndex + 1) FieldCoords.neighborMap.Length
            (FieldCoords.neighborMap.[leftIndex], FieldCoords.neighborMap.[rightIndex])