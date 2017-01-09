[<AutoOpen>]
module ReplanRequests

open FSharp.AWS.DynamoDB
open Amazon
open Amazon.DynamoDBv2
open Amazon.Util
open Amazon.Runtime
open System.Collections.Generic


let inline get id (client:IAmazonDynamoDB) =
    let req = new Model.GetItemRequest("dev-dap-replanner-requests", dict["ReplanRequestId", new Model.AttributeValue(s = id)] |> Dictionary)
    let resp = client.GetItem(req).Item
    let m = {ReplanRequestId = resp.["ReplanRequestId"].S; ReplanStatus = resp.["ReplanStatus"].S}
    m

let inline post (request:ReplanRequestModel) (client:IAmazonDynamoDB) =
    let req = new Model.PutItemRequest("dev-dap-replanner-requests", 
                                            dict[
                                                "ReplanRequestId", new Model.AttributeValue(s = System.Guid.NewGuid().ToString());
                                                "ReplanStatus", new Model.AttributeValue(s = "New")
                                            ] |> Dictionary)
    client.PutItem(req).HttpStatusCode