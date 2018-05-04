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
    { IsCorrect : bool
      Correct : int
      Selected : int }

type Msg =
    | Start
    | Answer of int
    | NextQuestionClicked
    | Reset
    | SetLanguage of Language

type Model =
    { Page : Page
      Language : Language
      CurrentIndex : int
      AnswerOrder : int list
      IsHighlighting : bool
      Highlight : Highlight option
      Answered : (int * bool) list }

let initialModel =
    { Page = Welcome
      Language = German
      CurrentIndex = -1
      AnswerOrder = []
      IsHighlighting = false
      Highlight = None
      Answered = [] }

let init () =
    initialModel, Cmd.none

let cmdMsgAfter ms msg =
    Cmd.ofPromise
        Promise.sleep
        ms
        (fun _ -> msg)
        (fun _ -> msg)

let nextQuestion model =
    let nextIndex = nextQuestion model.Language model.Answered
    { model with
        CurrentIndex = nextIndex
        AnswerOrder = shuffleList [0..3] }

let update msg model =
    match msg with
    | SetLanguage lang ->
        { model with Language = lang }
        , Cmd.none

    | Start ->
        { nextQuestion model with Page = Quiz }
        , Cmd.none

    | Answer _ when model.IsHighlighting ->
        model, Cmd.none

    | Answer answerIndex ->
        let isCorrect = answerIndex = 0
        { model with
            Answered = ( model.CurrentIndex, isCorrect ) :: model.Answered
            IsHighlighting = true
            Highlight =
                { IsCorrect = isCorrect
                  Correct = 0
                  Selected = answerIndex } |> Some }
        , Cmd.none

    | NextQuestionClicked ->
        let model = { model with Highlight = None; IsHighlighting = false }

        if isDone model.Answered
        then { model with Page = Score }
        else nextQuestion model
        , Cmd.none

    | Reset ->
        { initialModel with Language = model.Language }, Cmd.none

let h1 text = R.h1 [] [ R.str text ]

let p text = R.p [] [ R.str text ]


let page id elements = R.div [ sprintf "page-%s" id |> Id ] elements

let logo = R.div [ Id "logo" ] []

let languageButton dispatch lang =
    [ English, "EN"
      German,  "DE" ]
    |> List.map (fun (l, name) ->
        let isActive = l = lang
        let classes =
            "language"
          + if isActive then " active" else ""
        R.button
            [ OnClick (fun _ -> dispatch (SetLanguage l))
              Class classes ]
            [ R.str name ] )
    |> R.div [ Id "languages" ]

let resetButton dispatch i18n =
    R.button
        [ Id "reset"
          Class "black"
          OnClick (fun _ -> dispatch Reset) ]
        [ R.str i18n.ResetButton ]

let bottomButton dispatch msg text =
    R.button
        [ OnClick (fun _ -> dispatch msg)
          Class "bottom black"]
        [ R.str text ]

let welcomeView model dispatch i18n =
    page "welcome"
        [ logo
          R.h1 [] [ R.str i18n.Title ]
          languageButton dispatch model.Language
          R.div
            [ Id "actions" ]
            [ bottomButton dispatch Start i18n.StartButton ] ]

let answerButton dispatch index text highlights =
    let highlightClass =
        highlights
        |> Option.map (fun h ->
            if h.Correct = index then
                if h.IsCorrect then "" else "hint "
              + "correct"
            elif h.Selected = index then "wrong"
            else "")
        |> Option.defaultValue ""
    let classes = "answer " + highlightClass

    R.button
        [ OnClick (fun _ -> dispatch (Answer index))
          Class classes ]
        [ R.str text ]

let contentPage id dispatch i18n lang elements =
    page id
        [ R.header []
            [ R.div [ Id "banner" ]
                [ logo; R.p [ Id "subtitle" ] [ R.str i18n.Subtitle ] ]
              R.div [ Id "buttons" ]
                [ languageButton dispatch lang
                  resetButton dispatch i18n ] ]
          R.main [] elements ]

let quizView model dispatch i18n =
    let progress =
        let offset = if model.IsHighlighting then 0 else 1
        i18n.ProgressFormat (model.Answered.Length + offset)
    let question = getQuestion model.Language model.CurrentIndex
    let answers =
        question.Answers
        |> List.mapi (fun i a ->
            i, answerButton dispatch i a model.Highlight)
        |> List.sortBy (fun (i, _) -> model.AnswerOrder.[i])
        |> List.map (fun (_, a) -> a)
        |> R.div [ Class "answers" ]

    let info, next =
        match model.Highlight with
        | None -> R.str "", R.str ""
        | Some highlight ->
            let cl, title =
                if highlight.IsCorrect
                then "correct", i18n.InfoTitleCorrect
                else "wrong", i18n.InfoTitleWrong
            let info =
                 R.div [ Id "info"; Class cl ]
                    [ R.h3 [] [ R.str title ]
                      R.p [] [ R.str question.Info] ]
            let next = bottomButton dispatch NextQuestionClicked i18n.NextButton
            info, next

    contentPage "quiz" dispatch i18n model.Language
        [ R.div [ Id "progress" ] [ R.str progress ]
          R.h2 [] [ R.str question.Text ]
          answers
          info
          next ]

let scoreView model dispatch i18n =
    let correctAnswerCount =
        model.Answered
        |> List.filter (fun (_, correct) -> correct)
        |> List.length
    contentPage "score" dispatch i18n model.Language
        [ R.div [ Id "progress" ]
            [ correctAnswerCount
              |> i18n.ScoreProgressFormat
              |> R.str ]
          R.h2 [] [ R.str i18n.ScoreTitle ]
          R.div [ Id "score-hero" ]
            [ R.h3 []
                [ R.em [] [ string correctAnswerCount |> R.str ]
                  R.str i18n.Points ]
              R.p [] [ i18n.AverageFormat 8.7 |> R.str] ]
          bottomButton dispatch Reset i18n.BackToStartButton ]

let view model dispatch =
    let i18n = getTranslation model.Language
    match model.Page with
    | Welcome -> welcomeView model dispatch i18n
    | Quiz -> quizView model dispatch i18n
    | Score -> scoreView model dispatch i18n

Program.mkProgram init update view
|> Program.withReact "app"
|> Program.withConsoleTrace
|> Program.run

Browser.document.body.addEventListener_touchmove (fun e -> e.preventDefault ())
