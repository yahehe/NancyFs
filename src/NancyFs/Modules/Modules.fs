[<AutoOpen>]
module Modules

open Amazon.DynamoDBv2

module Home = 
  let get() = View(Views.``Home.cshtml``, None)
  let post nameModel = View(Views.``Welcome.cshtml``, Some nameModel)

module About = 
  let get (name : string option) =
    View(Views.``About.cshtml``, name)

module StaticFile = 
  let get parameters = 
    match parameters?file with
    | Some file -> File file
    | None -> NotFound

module Redirect = 
  let get redirect = 
    match redirect with
    | Some true -> PermanentRedirect "/about"
    | _ -> View(Views.``Home.cshtml``, None)

module HealthCheck =
  let get() = OK

module Replanner =
  let get replanRequestId amazon = 
    match replanRequestId with
    | None -> NotFound
    | Some id -> Json(ReplanRequests.get id amazon)