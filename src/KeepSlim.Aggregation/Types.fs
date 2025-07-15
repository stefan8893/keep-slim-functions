namespace KeepSlim.Aggregation.Types

open System

type BodyDataRecord =
    { RecordedAt: DateTime
      Weight: float
      MuscleMass: float
      BodyFat: float
      Water: float }


type BoundaryRecords =
    { First: BodyDataRecord
      FirstN: BodyDataRecord list
      Last: BodyDataRecord
      LastN: BodyDataRecord list }

type TimeRange = { start: DateTime; ``end``: DateTime }

type Interval =
    | WeeklyExact
    | MonthlyExact

[<Measure>]
type kg

[<Measure>]
type percent

type BodyDataChange<[<Measure>] 'a> =
    { Interval: Interval
      ChangeInInterval: float<'a>
      TimeRange: TimeRange }
