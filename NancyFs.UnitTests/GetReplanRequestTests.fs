﻿namespace NancyFs.UnitTests

open Amazon.DynamoDBv2
open Nancy
open Nancy.Testing
open Nancy.TinyIoc
open NUnit.Framework
open NancyFs
open Foq
open FsUnit
open System.Collections.Generic

type Bootstrapper() =
    inherit DefaultNancyBootstrapper()

    override this.ConfigureApplicationContainer(container : TinyIoCContainer) =
        base.ConfigureApplicationContainer(container)

module tests =

    let r = new Model.GetItemResponse()
    r.Item <- (dict["ReplanRequestId", new Model.AttributeValue(s = "2"); "ReplanStatus", new Model.AttributeValue(s = "Test")] |> Dictionary)
            
    [<TestFixture>]
    type GetReplanRequestTests() =

        [<Test>]
        member test.``Successful retrieval returns a HTTP 200 response with the replan request`` () =
            let bootstrapper = new Bootstrapper()
            let browser = new Browser(fun c -> test.Setup c)

            let result = browser.Get("/v1/replanrequest/2")
            result.StatusCode |> should equal HttpStatusCode.OK
            let obj = result.Body.DeserializeJson<ReplanRequestModel>()
            obj.ReplanRequestId |> should equal "2"
            obj.ReplanStatus |> should equal "Test"

        member test.Setup(c : Nancy.Testing.ConfigurableBootstrapper.ConfigurableBootstrapperConfigurator) =         
            c.Module<Routes.Routes>() |> ignore
            
            let amazon =
                Mock<IAmazonDynamoDB>()
                    .Setup(fun a -> <@ a.GetItem(any()) @>).Returns(r)
                    .Create()
            c.Dependency<IAmazonDynamoDB>(amazon) |> ignore

    [<TestFixture>]
    type PostReplanRequestTests() =

        [<Test>]
        member test.``Successful post returns a HTTP 200 response`` () =
            let bootstrapper = new Bootstrapper()
            let browser = new Browser(fun c -> test.Setup c)

            let result = browser.Post("/v1/replanrequest")
            result.StatusCode |> should equal HttpStatusCode.OK

        member test.Setup(c : Nancy.Testing.ConfigurableBootstrapper.ConfigurableBootstrapperConfigurator) =         
            c.Module<Routes.Routes>() |> ignore

            
            let amazon =
                Mock<IAmazonDynamoDB>()
                    .Setup(fun a -> <@ a.PutItem(any()) @>).Returns(new Model.PutItemResponse(HttpStatusCode = System.Net.HttpStatusCode.OK))
                    .Create()
            c.Dependency<IAmazonDynamoDB>(amazon) |> ignore