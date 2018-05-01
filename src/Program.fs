module Bbh.Quiz.LuebeckCheck.Program

open Elmish
open Elmish.React
open Fable.PowerPack
open Fable.Core.JsInterop
open Fable.Import
open Fable.Helpers.React.Props
module R = Fable.Helpers.React

open Bbh.Quiz.LuebeckCheck.Library

importDefault "./styles.css" |> ignore


type Page =
    | Welcome
    | Quiz
    | Score

type Highlight =
    { Correct : Answer
      Selected : Answer }

type Msg =
    | Start
    | Answer of Answer
    | HighlightAnswersDone
    | NextQuestion
    | Reset
    | SetLanguage of Language

type Model =
    { Page : Page
      Language : Language
      Current : Question
      IsHighlighting : bool
      HighlightAnswers : Highlight option
      Answered : (Question * bool) list }

let init () =
    { Page = Welcome
      Language = German
      Current = { Text = ""; Answers = []; Info = "" }
      IsHighlighting = false
      HighlightAnswers = None
      Answered = [] }
    , Cmd.none

let cmdMsgAfter ms msg =
    Cmd.ofPromise
        Promise.sleep
        ms
        (fun _ -> msg)
        (fun _ -> msg)

let update msg model =
    match msg with
    | SetLanguage lang ->
        { model with Language = lang }, Cmd.none

    | Start ->
        { model with
            Page = Quiz
            Current = nextQuestion model.Language model.Answered }
        , Cmd.none

    | Answer _ when model.IsHighlighting ->
        model, Cmd.none

    | Answer answer ->
        let correctAnswer = getCorrectAnswer model.Language model.Current
        let isCorrect = correctAnswer = answer
        let answered = ( model.Current, isCorrect ) :: model.Answered
        { model with
            Answered = answered
            IsHighlighting = true
            HighlightAnswers = Some { Correct = correctAnswer; Selected = answer } }
        , cmdMsgAfter 600 HighlightAnswersDone

    | HighlightAnswersDone ->
        { model with HighlightAnswers = None }, cmdMsgAfter 200 NextQuestion

    | NextQuestion ->
        if isDone model.Answered
        then { model with IsHighlighting = false; Page = Score }
        else { model with IsHighlighting = false; Current = nextQuestion model.Language model.Answered }
        , Cmd.none

    | Reset -> init ()

let h1 text = R.h1 [] [ R.str text ]

let p text = R.p [] [ R.str text ]

let button dispatch msg text =
    R.button [ OnClick (fun _ -> dispatch msg) ] [ R.str text ]

let answerButton dispatch answer highlights =
    let highlightClass =
        highlights
        |> Option.map (fun h ->
            if h.Correct = answer then "correct"
            elif h.Selected = answer then "wrong"
            else "")
        |> Option.defaultValue ""
    let classes = "answer " + highlightClass

    R.button
        [ OnClick (fun _ -> dispatch (Answer answer))
          Class classes ]
        [ R.str answer ]

let languageButton dispatch lang =
    let (l, name) =
        match lang with
        | English -> German, "DE"
        | German -> English, "EN"
    R.button
        [ OnClick (fun _ -> dispatch (SetLanguage l))
          Class "language" ]
        [ R.str name ]


let page id elements = R.div [ sprintf "page-%s" id |> Id ] elements

let logo = R.div [ Id "logo" ] []

let welcomeView model dispatch i18n =
    page "welcome"
        [ logo
          R.h1 [ Id "title" ] [ R.str i18n.Title ]
          R.div
            [ Id "actions" ]
            [ languageButton dispatch model.Language
              button dispatch Start i18n.StartButton ] ]

// let quizView lang model dispatch =

//     let countStr =
//         let offset = if model.IsHighlighting then 0 else 1
//         lang.ProgressFormat (model.Answered.Length + offset)
//     let question = model.Current
//     let answers =
//         question.Answers
//         |> List.map (fun a -> answerButton dispatch a model.HighlightAnswers)

//     // let backArrow =
//     //     if model.Answered.IsEmpty
//     //     then R.div [ Id "back-arrow"; Class "disabled" ] []
//     //     else R.div [ Id "back-arrow"; OnClick (fun _ -> dispatch Back ) ] []

//     page "quiz" dispatch
//         [ logo
//           R.p [] [ R.str lang.Subtitle ]
//          (* backArrow *) ]
//         [ ]

//     List.append
//         [ R.div [ Id "header" ]
//             [ R.span [] [ R.str countStr ]
//               button dispatch Reset lang.ResetButton ]
//           R.h1 [ Id "question" ] [ R.str question.Text ] ]
//         answers
//     |> page "quiz" dispatch

// let scoreView lang model dispatch =
//     let correctCount =
//         model.Answered
//         |> List.filter (fun (_, isCorrect) -> isCorrect)
//         |> List.length
//     let result =
//         sprintf "Du hast %d/%d Fragen richtig beantwortet!"
//             correctCount questionCount

//     page "score" dispatch
//         [ h1 "Auswertung"
//           p result
//           button dispatch Reset "Zurück zum Start" ]

let view model dispatch =
    let i18n = getTranslation model.Language
    welcomeView model dispatch i18n

    // match model.Page with
    // | Welcome -> welcomeView lang dispatch
    // | Quiz -> quizView lang model dispatch
    // | Score -> scoreView lang model dispatch

Program.mkProgram init update view
|> Program.withReact "app"
|> Program.withConsoleTrace
|> Program.run

Browser.document.body.addEventListener_touchmove (fun e -> e.preventDefault ())
