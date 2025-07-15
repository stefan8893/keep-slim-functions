module KeepSlim.Aggregation.Tests

open Xunit
open System
open KeepSlim.Aggregation.Types

[<Fact>]
let ``My test`` () =

    let bodyData: BodyDataRecord =
        { RecordedAt = DateTime.Today
          Weight = 65
          MuscleMass = 45
          BodyFat = 13
          Water = 60 }

    Assert.True(true)
