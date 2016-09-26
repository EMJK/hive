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

        static member offset src dst =
            { X = dst.X - src.X; Y = dst.Y - src.Y; Z = dst.Z - src.Z }

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
            let left idx = if idx = 0 then 5 else idx - 1
            let right idx = if idx = 5 then 0 else idx + 1

            let offset = FieldCoords.offset src dst
            let offsetIndex = Array.findIndex (fun x -> x = offset) FieldCoords.neighborMap
            let leftOffsetIndex = left offsetIndex
            let rightOffsetIndex = right offsetIndex
            let leftOffset = FieldCoords.neighborMap.[leftOffsetIndex]
            let rightOffset = FieldCoords.neighborMap.[rightOffsetIndex]
            let left = FieldCoords.add src leftOffset
            let right = FieldCoords.add src rightOffset
            (left, right)