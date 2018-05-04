module Bbh.Quiz.LuebeckCheck.Library

open System


type Answer = string

type Question =
    { Text : string
      Answers : Answer list
      Info : string }

type LanguageData =
    { Title : string
      Subtitle : string
      StartButton : string
      ResetButton : string
      InfoTitleCorrect : string
      InfoTitleWrong : string
      NextButton : string
      ScoreButton : string
      BackToStartButton : string
      ProgressFormat : int -> string
      ScoreProgressFormat : int -> string
      ScoreTitle : string
      Points : string
      AverageFormat : float -> string
      Questions : Question list }

type Language =
    | English
    | German

let private germanQuestions =
    [ { Text = "Wie viele Figuren stehen auf der Puppenbrücke?"
        Answers = [ "8"; "143"; "22"; "0" ]
        Info = "Neben Eintracht und Friede steht dort auch der Gott Merkur, der allen Fremden den blanken Podex zukehrt." }
      { Text = "Wie viele Kinder hatte Thomas Mann?"
        Answers = [ "6"; "2"; "Keine"; "13" ]
        Info = "Fünf seiner sechs Kinder haben zu ihren Lebzeiten selbst Literatur veröffentlicht." }
      { Text = "Welches Tier findet man in der Marienkirche?"
        Answers = [ "Maus"; "Marienkäfer"; "Einhorn"; "Alpaka" ]
        Info = "Wer die Maus berührt, kehrt irgendwann nach Lübeck zurück - so der Volksglaube." }
      { Text = "Haben Sie aufgepasst? Über der Eingangstür des Buddenbrookhauses steht:"
        Answers = [ "Anno: Dominus providebit: 1758"; "Anno: Domini providebant: 1679"; "Anna: Domina providebit: 1850"; "Annus: Dominus providebimus: 2000" ]
        Info = "Die Inschrift bedeutet „Im Jahre: Der Herr wird vorsorgen: 1758“" }
      { Text = "Wie viel Zeit verbringen die Besucher durchschnittlich im Buddenbrookhaus?"
        Answers = [ "1,5 Stunden"; "30 Minuten"; "50 Minuten"; "4 Stunden" ]
        Info = "Neben den Ausstellungen finden auch regelmäßig Lesungen und andere interessante Veranstaltungen im Buddenbrookhaus statt. Schauen Sie doch mal vorbei!" }
      { Text = "Wie groß ist Lübeck in etwa in Quadratkilometern?"
        Answers = [ "215 Quadratkilometer"; "105 Quadratkilometer"; "875 Quadratkilometer"; "48 Quadratkilometer" ]
        Info = "Zum Stadtgebiet gehören 10 Stadtteile; darunter auch Travemünde, das Thomas Mann in Buddenbrooks verewigt hat." }
      { Text = "Von wem stammt die Formulierung „Herzensheimat des Nordens“?"
        Answers = [ "Thomas Mann"; "Marten Krienke, aus dem Projekt Literatur als Ereignis"; "Günter Grass"; "Bernd Saxe, Ex-Bürgermeister von Lübeck" ]
        Info = "Thomas Mann verwendete die Formulierung in seiner Rede zur Nobelpreisverleihung 1929." }
      { Text = "Wie alt ist das Lübecker Rathaus?"
        Answers = [ "710 Jahre"; "908 Jahre"; "62 Jahre"; "1015 Jahre" ]
        Info = "Der Bau des Rathauses begann im Jahr 1230 und wurde 1308 fertiggestellt. Seitdem wurde es immer wieder umgebaut und erweitert, wodurch das Gebäude viele verschiedene Baustile zeigt." }
      { Text = "Aus wie vielen Gebäuden bestehen die Salzspeicher?"
        Answers = [ "6"; "12"; "3"; "112" ]
        Info = "Erbaut zwischen 1579 und 1745, dienten die 6 Gebäude der Lagerung von Salz." }
      { Text = "Welche Aussage über Lübeck stammt von Heinrich Mann?"
        Answers = [ "Es habe einen „Millionengestank“ an sich"; "Es sei ein „Weltwinkel“"; "Es sei eine „Handelszentrale“"; "Es sei „die Stadt mit den dümmsten Menschen“" ]
        Info = "Heinrich Mann beschreibt den „Millionengestank“ in seinem Text Fantasieen über meine Vaterstadt L. Beide Brüder Mann äußerten sich wiederholt auch negativ über ihre Heimatstadt Lübeck." }
      { Text = "Thomas Mann sagt über Lübeck aus, es sei…"
        Answers = [ "„durchaus normal.“"; "„wenig abwechslungsreich.“"; "„ein prunkvoller Fleck.“"; "„einzigartig.“" ]
        Info = "In seinem Essay Vom Beruf des deutschen Schriftstellers in unserer Zeit. Ansprache an den Bruder von 1931 wendet sich Thomas an seinen Bruder Heinrich und reflektiert ihre ,doppelte Herkunft'." }
      { Text = "„Das Meer ist keine Landschaft, es ist das Erlebnis der Ewigkeit.“ Von wem stammt dieses Zitat?"
        Answers = [ "Thomas Mann"; "Heinrich Mann"; "Golo Mann"; "Leonie Mann" ]
        Info = "1926 hielt Thomas Mann anlässlich der 700-Jahrfeier der Reichsfreiheit Lübecks im Lübecker Stadttheater die Rede Lübeck als geistige Lebensform." }
      { Text = "Wer sagte über Lübeck aus es sei „das Juwel vom Nordsee“?"
        Answers = [ "Ein Kunde bei Holiday Check"; "Thomas Mann"; "Erika Mann"; "Die Bild-Zeitung" ]
        Info = "Neben seiner wunderschönen Altstadt, seinem reichhaltigen Kulturangebot und seiner Nähe zur Ostsee (!) ist Lübeck vor allem für sein Marzipan weltbekannt." }
      { Text = "Wer behauptet, Lübeck sei die „Wohlfühlhauptstadt“?"
        Answers = [ "Die Stadt Lübeck"; "Heinrich Mann"; "Günter Grass"; "Dner, ein Youtuber aus Lübeck" ]
        Info = "Und sie hat Recht! Überzeugen Sie sich selbst!" }
      { Text = "Für welchen Film dienten die Lübecker Salzspeicher als Drehort?"
        Answers = [ "Nosferatu - Eine Symphonie des Grauens"; "Das A-Team"; "Inglorious Basterds"; "Berlin – Tag & Nacht" ]
        Info = "Max Schreck mietet im Film von 1922 als blutsaugender Graf Orlok eines der alten Gebäude. " } ]

let private englishQuestions =
    [ { Text = "How many statues are there on the \"Puppenbrücke\"?"
        Answers = [ "8"; "143"; "22"; "0" ]
        Info = "Next to  \"Agreement\" and \"Peace\", the god Mercury is also there, turning his bare posterior towards all visitors." }
      { Text = "How many children did Thomas Mann have?"
        Answers = [ "6"; "2"; "None"; "13" ]
        Info = "Five of his six children were also published authors. " }
      { Text = "Which animal can be found in the Marienkirche?"
        Answers = [ "Mouse"; "Ladybug"; "Unicorn"; "Alpaca" ]
        Info = "According to popular belief, a person who touches the mouse will return to Lübeck one day." }
      //{ Text = "Have you been paying attention? Which of the following is written above the entrance of the Buddenbrookhaus: "
      { Text = "Which of the following is written above the entrance of the Buddenbrookhaus: "
        Answers = [ "Anno: Dominus providebit: 1758"; "Anno: Domini providebant: 1679"; "Anna: Domina providebit: 1850"; "Annus: Dominus providebimus: 2000" ]
        Info = "The inscription reads \"In the year of: the Lord will provide: 1758\" " }
      { Text = "How much time do visitors spend on average at the Buddenbrookhaus?"
        Answers = [ "1.5 hours"; "30 minutes"; "50 minutes"; "4 hours" ]
        Info = "In addition to our exhibitions, we regularly organize readings and other interesting events. Why don't you come in and check it out?" }
      { Text = "How big is Lübeck approximately? (in square kilometers)"
        Answers = [ "215 square kilometers"; "105 square kilometers"; "875 square kilometers"; "48 square kilometers" ]
        Info = "The city consists of ten districts; one of them is Travemünde, immortalized in Thomas Mann's \"Buddenbrooks\"." }
      { Text = "Who coined the phrase \"Home of the Heart\"?"
        Answers = [ "Thomas Mann"; "Marten Krienke, from 'Literature as an Event'"; "Günter Grass"; "Bernd Saxe, Ex-mayor of Lübeck" ]
        Info = "Thomas Mann used that phrase in his speech at the Nobel Prize Award ceremony of 1929." }
      { Text = "How old is the townhall of Lübeck?"
        Answers = [ "710 years"; "908 years"; "62 years"; "1015 years" ]
        Info = "Construction of the townhall began in 1230 and was completed in 1308. Since then, it has often been remodelled and expanded leading to a building that features many different architectural styles." }
      { Text = "How many buildings are part of the salt silos?"
        Answers = [ "6"; "12"; "3"; "112" ]
        Info = "The six buildings, constructed between 1579 and 1745, were used for the storage of salt." }
      { Text = "Which of the following phrases was said by Heinrich Mann?"
        Answers = [ "There is a \"money stench\" in Lübeck"; "It is a \"world corner\""; "It is a \"trade centre\""; "\"It is the city with the dumbest people\"" ]
        Info = "In his writing \"Imaginations about my Hometown L.\", Heinrich Mann describes the \"money stench\" of the city. Both brothers repeatedly said negative things about their hometown of Lübeck." }
      { Text = "Thomas Mann said about Lübeck that it…"
        Answers = [ "is \"quite normal.\""; "\"lacks in diversity.\""; "is \"a magnificent spot.\""; "is \"one-of-a-kind.\"" ]
        Info = "In his essay 'On the Career of the German Writer of our Time. Address to my Brother' of 1931, Thomas Mann turns to his brother Heinrich and reflects on their \"double origin\"." }
      { Text = "\"The ocean is not a landscape, it is the experience of eternity.\" Who said that?"
        Answers = [ "Thomas Mann"; "Heinrich Mann"; "Golo Mann"; "Leonie Mann" ]
        Info = "In 1926 in the Lübeckan city theatre, Thomas Mann gave the speech 'Lübeck as a spiritual way of life' on the occasion of Lübeck's 700 years  celebration of imperial freedom." }
      { Text = "Who called Lübeck \"the jewel of the North Sea\"?"
        Answers = [ "A client at Holiday Check"; "Thomas Mann"; "Erika Mann"; "The \"Bild\" newspaper" ]
        Info = "In addition to its beautiful old city centre, the rich cultural heritage and its proximity to the Baltic (!) Sea, Lübeck is especially famous for its Marzipan." }
      { Text = "Who claimed that Lübeck is the \"feel-well capital\" of Germany?"
        Answers = [ "The city of Lübeck"; "Heinrich Mann"; "Günter Grass"; "Dner, a YouTuber from Lübeck" ]
        Info = "And she is right! See for yourself!" }
      { Text = "Which of the following movies features the Lübeckan salt silos?"
        Answers = [ "Nosferatu - A Symphony of Horror"; "The A-Team"; "Inglourious Basterds"; "Berlin – Day & Night" ]
        Info = "In the movie of 1922, Max Schreck, or the blood-sucking Lord Orlok, rents one of the old buildings." } ]

let questionCount =
    if englishQuestions.Length = germanQuestions.Length
    then englishQuestions.Length
    else Exception "question data invalid" |> raise

let getTranslation lang =
    match lang with
    | English ->
        { Title = sprintf "%d questions about the exhibition" questionCount
          Subtitle = "The quiz"
          StartButton = "Start quiz"
          ResetButton = "Reset"
          InfoTitleCorrect = "Correct!"
          InfoTitleWrong = "Wrong!"
          NextButton = "Next question"
          ScoreButton = "See score"
          BackToStartButton = "Back to start"
          ProgressFormat = fun n -> sprintf "%02d of %02d" n questionCount
          ScoreProgressFormat = fun n -> sprintf "%02d of %02d correct" n questionCount
          ScoreTitle = "You made it!"
          Points = "points"
          AverageFormat = sprintf "The average score of previous players is %.1f points."
          Questions = englishQuestions }
    | German ->
        { Title = sprintf "%d Fragen zur Ausstellung" questionCount
          Subtitle = "Das Quiz"
          StartButton = "Quiz starten"
          ResetButton = "Reset"
          InfoTitleCorrect = "Richtig!"
          //InfoTitleWrong = "Falsch!"
          InfoTitleWrong = "Leider falsch."
          NextButton = "Nächste Frage"
          ScoreButton = "Zur Auswertung"
          BackToStartButton = "Zurück zum Start"
          ProgressFormat = fun n -> sprintf "%02d von %02d" n questionCount
          ScoreProgressFormat = fun n -> sprintf "%02d von %02d richtige" n questionCount
          ScoreTitle = "Geschafft!"
          Points = "Punkte"
          AverageFormat =
            fun n ->
                "Der Durchschnitt der anderen Spieler liegt bei "
              + (sprintf "%.1f" n).Replace ('.', ',')
              + " Punkten."
          Questions = germanQuestions }

let getQuestions lang =
    match lang with
    | German -> germanQuestions
    | English -> englishQuestions

let shuffleList list =
    let rand = Random ()
    list |> List.sortBy (fun _ -> rand.Next())

let getQuestion lang index =
    getQuestions lang
    |> List.item index

let getShuffledQuestion lang index =
    getQuestion lang index
    |> (fun q -> { q with Answers = shuffleList q.Answers })

let nextQuestion lang (answered : (int * bool) list) =
    getQuestions lang
    |> List.mapi (fun i _ -> i)
    |> List.filter (fun i ->
        answered
        |> List.exists (fun (a, _) -> a = i)
        |> not)
    |> (fun qs ->
        let rand = Random ()
        qs.[rand.Next qs.Length])

let getCorrectAnswer lang index =
    getQuestion lang index
    |> (fun q -> q.Answers |> List.head)

let isDone (answered : (int * bool) list) =
    questionCount = answered.Length

let getScore (answered : (int * bool) list) =
    answered |> List.filter (fun (_, c) -> c) |> List.length


// Json + localstorage

open Fable.Import
open Fable.Core.JsInterop

let private scoresKey = "scores"

let loadScores () =
    let saved = Browser.localStorage.getItem scoresKey
    if isNull saved then []
    else ofJson<int list> (saved :?> string)

let saveScores scores =
    Browser.localStorage.setItem (scoresKey, toJson scores)
