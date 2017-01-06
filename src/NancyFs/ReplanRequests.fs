[<AutoOpen>]
module ReplanRequests

open FSharp.AWS.DynamoDB
open Amazon
open Amazon.DynamoDBv2
open Amazon.Util
open Amazon.Runtime
open System.Collections.Generic

type ReplanRequest =
    {
        [<HashKey>]
        ReplanRequestId : string;
        ReplanStatus : string;
    }

let client : IAmazonDynamoDB= 
    let creds =
        try AWSCredentials.FromEnvironmentVariables()
        with _ -> AWSCredentials.FromCredentialsStore("FNPServiceUser")
    let region = RegionEndpoint.EUWest1
    new AmazonDynamoDBClient(creds, region) :> IAmazonDynamoDB

let get id =
    let req = new Model.GetItemRequest("dev-dap-replanner-requests", dict["ReplanRequestId", new Model.AttributeValue(s = id)] |> Dictionary)
    let resp = client.GetItem(req).Item
    {ReplanRequestId = resp.["ReplanRequestId"].S; ReplanStatus = resp.["ReplanStatus"].S;}