module Bbh.Quiz.LuebeckCheck

open System
open Elmish
open Elmish.React
open Fable.PowerPack
open Fable.Core.JsInterop
open Fable.Helpers.React.Props
module R = Fable.Helpers.React

importDefault "./styles.css" |> ignore

type Answer = string

type Question =
    { Text : string
      Answers : Answer list }

let questions =
    [ { Text = "Wie viele Puppen stehen auf der Puppenbrücke?"
        Answers =
            [ "8"; "12"; "4"; "0" ] }
      { Text = "Wie viele Kinder hatte Thomas Mann?"
        Answers =
            [ "6"; "8"; "3"; "10" ] }
      { Text = "In welcher Kirche sieht man steinerne Maus?"
        Answers =
            [ "Marienkirche"; "Dom"; "Aegidienkirche"; "Petrikirche" ] }
      { Text = "Wann wurde das Buddenbrookhaus erbaut?"
        Answers =
            [ "1758"; "2000"; "1679"; "1850" ] }
      { Text = "Wie viel Zeit verbringen die Besucher durchschnittlich im Buddenbrookhaus?"
        Answers =
            [ "1,5 Stunden"; "4 Stunden"; "50 Minuten"; "30 Minuten" ] }
      { Text = "Wie groß ist Lübeck?"
        Answers =
            [ "215 Quadratkilometer"; "199 Quadratkilometer"; "425 Quadratkilometer"; "300 Quadratkilometer" ] }
      { Text = "Von wem stammt das Zitat „Lübeck ist meine Herzensheimat.“"
        Answers =
            [ "Thomas Mann"; "Günter Grass"; "Bernd Saxe"; "Marten Krienke, aus dem Projekt Literatur als Ereignis" ] }
      { Text = "Wie alt ist das Lübecker Rathaus?"
        Answers =
            [ "778 Jahre"; "908 Jahre"; "617 Jahre"; "834 Jahre" ] }
      { Text = "Aus wie vielen Gebäuden bestehen die Salzspeicher?"
        Answers =
            [ "6"; "4"; "3"; "5" ] }
      { Text = "Welche dieser Aussagen stammt von Heinrich Mann?"
        Answers =
            [ "Lübeck hat wahrhaftig einen Millionengestank an sich"; "Lübeck als Weltwinkel"; "Lübeck ist eine Handelszentrale"; "Lübeck, die Stadt am Meer" ] }
      { Text = "Thomas Mann sagt über Lübeck: „Es sei…"
        Answers =
            [ "…durchaus normal“"; "…einzigartig“"; "…ein prunkvoller Fleck“"; "…normal“" ] }
      { Text = "„Das Meer ist keine Landschaft, es ist das Erlebnis der Ewigkeit.“ Von wem stammt dieses Zitat?"
        Answers =
            [ "Thomas Mann"; "Heinrich Mann"; "Golo Mann"; "Leonie Mann" ] }
      { Text = "Wer sagt über Lübeck aus es sei „das Juwel der Nordsee.“?"
        Answers =
            [ "Ein Kunde bei Holiday Check"; "Thomas Mann"; "Erika Mann"; "Die Bild" ] }
      { Text = "Wer behauptet Lübeck sei die Wohlfühlhauptstadt?"
        Answers =
            [ "Lübeck Marketing"; "Heinrich Mann"; "Günter Grass"; "DNER, ein Youtuber aus Lübeck" ] } ]

type Page =
    | Welcome
    | Quiz
    | Score

type Highlight =
    { Correct : Answer
      Selected : Answer }

type Model =
    { Page : Page
      Current : Question
      IsHighlighting : bool
      HighlightAnswers : Highlight option
      Answered : (Question * bool) list }

type Msg =
    | Start
    | Answer of Answer // index
    | HighlightAnswersDone
    | NextQuestion
    | Reset

let init () =
    { Page = Welcome
      Current = { Text = ""; Answers = [] }
      IsHighlighting = false
      HighlightAnswers = None
      Answered = [] }
    , Cmd.none

let shuffleList list =
    let rand = Random ()
    list |> List.sortWith (fun _ _ -> 1 - (rand.Next 2))

let nextQuestion (answered : (Question * bool) list) =
    let rand = Random ()
    questions
    |> List.filter (fun q ->
        answered
        |> List.exists (fun (a, _) ->
            a.Text = q.Text)
        |> not)
    |> (fun qs ->
        let question = qs.[rand.Next qs.Length]
        { question with Answers = shuffleList question.Answers })

let cmdMsgAfter ms msg =
    Cmd.ofPromise
        Promise.sleep
        ms
        (fun _ -> msg)
        (fun _ -> msg)

let update msg model =
    match msg with
    | Start ->
        { model with
            Page = Quiz
            Current = nextQuestion model.Answered }
        , Cmd.none

    | Answer _ when model.IsHighlighting ->
        model, Cmd.none

    | Answer answer ->
        let correctAnswer =
            questions
            |> List.find (fun q -> q.Text = model.Current.Text)
            |> (fun q -> q.Answers |> List.head)
        let isCorrect = correctAnswer = answer
        let answered =
            ( model.Current, isCorrect )
            :: model.Answered
        { model with
            Answered = answered
            IsHighlighting = true
            HighlightAnswers = Some { Correct = correctAnswer; Selected = answer } }
        , cmdMsgAfter 600 HighlightAnswersDone

    | HighlightAnswersDone ->
        { model with HighlightAnswers = None }, cmdMsgAfter 200 NextQuestion

    | NextQuestion ->
        if questions.Length = model.Answered.Length
        then { model with IsHighlighting = false; Page = Score }
        else { model with IsHighlighting = false; Current = nextQuestion model.Answered }
        , Cmd.none

    | Reset -> init ()

let page id elements = R.div [ sprintf "page-%s" id |> Id ] elements
let h1 text = R.h1 [] [ R.str text ]
let p text = R.p [] [ R.str text ]
let button dispatch msg text = R.button [ OnClick (fun _ -> dispatch msg) ] [ R.str text ]
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

let welcomeView dispatch =
    page "welcome"
        [ h1 "Lübeck-Check"

          p "Denkst du, du weißt alles über Lübeck?"
          p <| "In diesem Quiz kannst du nicht nur dein Wissen über "
             + "Lübeck testen, sondern es zeitgleich verbessern."
          p <| "Ob Zitate von bekannten Lübeckern oder knifflige "
             + "Fragen über unsere Hansestadt, in diesen Quiz "
             + "erwarten dich verschiedenste Herausforderungen."
          p "Beantwortest du alle Fragen richtig?"

          button dispatch Start "Start" ]

let quizView model dispatch =
    let countStr =
        let offset = if model.IsHighlighting then 0 else 1
        sprintf "Frage %d/%d" (model.Answered.Length + offset) questions.Length
    let question = model.Current
    let answers =
        question.Answers
        |> List.map (fun a -> answerButton dispatch a model.HighlightAnswers)
    List.append
        [ R.div [ Id "header" ]
            [ R.span [] [ R.str countStr ]
              button dispatch Reset "Zurück zum Start" ]
          R.h1 [ Id "question" ] [ R.str question.Text ] ]
        answers
    |> page "quiz"

let scoreView model dispatch =
    let correctCount =
        model.Answered
        |> List.filter (fun (_, isCorrect) -> isCorrect)
        |> List.length
    let result =
        sprintf "Du hast %d/%d Fragen richtig beantwortet!"
            correctCount questions.Length

    page "score"
        [ h1 "Auswertung"
          p result
          button dispatch Reset "Zurück zum Start" ]

let view model dispatch =
    match model.Page with
    | Welcome -> welcomeView dispatch
    | Quiz -> quizView model dispatch
    | Score -> scoreView model dispatch

//Program.mkSimple init update view
Program.mkProgram init update view
|> Program.withReact "app"
|> Program.withConsoleTrace
|> Program.run