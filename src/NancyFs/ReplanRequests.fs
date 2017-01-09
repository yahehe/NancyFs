[<AutoOpen>]
module ReplanRequests

open FSharp.AWS.DynamoDB
open Amazon
open Amazon.DynamoDBv2
open Amazon.Util
open Amazon.Runtime
open System.Collections.Generic

[<CLIMutable>]
type ReplanRequest =
    {
        [<HashKey>]
        ReplanRequestId : string;
        ReplanStatus : string;
    }

let deps (client : IAmazonDynamoDB) =
    fun id ->
        let req = new Model.GetItemRequest("dev-dap-replanner-requests", dict["ReplanRequestId", new Model.AttributeValue(s = id)] |> Dictionary)
        let resp = client.GetItem(req).Item
        {ReplanRequestId = resp.["ReplanRequestId"].S; ReplanStatus = resp.["ReplanStatus"].S;}

let get id (client:IAmazonDynamoDB) =
    let req = new Model.GetItemRequest("dev-dap-replanner-requests", dict["ReplanRequestId", new Model.AttributeValue(s = id)] |> Dictionary)
    let resp = client.GetItem(req).Item
    {ReplanRequestId = resp.["ReplanRequestId"].S; ReplanStatus = resp.["ReplanStatus"].S;}