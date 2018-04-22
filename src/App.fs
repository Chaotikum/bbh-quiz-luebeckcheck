module Bbh.Quiz.LuebeckCheck

open Fable.Core.JsInterop
open Elmish
open Elmish.React
module R = Fable.Helpers.React

importDefault "./styles.css" |> ignore

type Model = int

type Msg =
    | Foo

let init () : Model =
    0

let update msg model =
    match msg with
    | _ -> model

let view model _dispatch =
    R.span [] [ string model |> R.str ]

Program.mkSimple init update view
|> Program.withReact "app"
|> Program.withConsoleTrace
|> Program.run