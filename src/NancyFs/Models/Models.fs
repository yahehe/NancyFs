[<AutoOpen>]
module Models

open FSharp.AWS.DynamoDB

[<CLIMutable>]
type NameModel =
  {Name : string}
  

[<CLIMutable>]
type ReplanRequestModel =
    {
        [<HashKey>]
        ReplanRequestId : string;
        ReplanStatus : string;
    }
