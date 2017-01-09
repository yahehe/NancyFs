module Bootstrapper

open Amazon
open Amazon.DynamoDBv2
open Amazon.Util
open Amazon.Runtime
open FSharp.AWS.DynamoDB
open Nancy
open Nancy.TinyIoc
open Nancy.Bootstrapper

let client : IAmazonDynamoDB= 
    let creds =
        try AWSCredentials.FromEnvironmentVariables()
        with _ -> AWSCredentials.FromCredentialsStore("FNPServiceUser")
    let region = RegionEndpoint.EUWest1
    new AmazonDynamoDBClient(creds, region) :> IAmazonDynamoDB

type Bootstrapper() = 
  inherit DefaultNancyBootstrapper()
  
  override this.ApplicationStartup(container : TinyIoCContainer, pipelines : IPipelines) =
    container.Register<IAmazonDynamoDB>(client) |> ignore
    base.ApplicationStartup(container, pipelines)