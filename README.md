BBH Lübeck Check
================

This is quiz, built with students of the
[GGS St. Jürgen](http://www.ggs-stjuergen.de/home.html) school and the
[Buddenbrookhaus](https://buddenbrookhaus.de) museum in Lübeck, Germany, for
the exhibition ["Herzensheimat"](https://buddenbrookhaus.de/herzensheimat).

Development
-----------

The project is basically a website built using [F#](http://fsharp.org),
[Fable](http://fable.io), [Elmish](https://fable-elmish.github.io) and
[React](https://reactjs.org). Don't ask why, at least I didn't put Electron on
that list as well.

### Requirements

* [dotnet SDK](https://www.microsoft.com/net/download/core) 2.0 or higher
* [node.js](https://nodejs.org) 6.11 or higher
* [yarn](https://yarnpkg.com) JS package manager
* [mono](http://www.mono-project.com/) on macOS/Linux to run
  [paket](https://fsprojects.github.io/Paket/)

### Build + Run

> In the commands below, yarn is the tool of choice. If you want to use npm,
just replace `yarn` by `npm` in the commands.


- run `yarn` (installs dependencies)
- run `yarn start` (starts Fable daemon and [Webpack](https://webpack.js.org/) dev
  server)
- In your browser, open: http://localhost:8080/ .
  Any modification you do to the F# code will be reflected in the web page after
  saving.

For production:

- run `yarn build` and you'll get your frontend files ready for deployment in
  the `build` directory.

Kiosk mode on Windows 10 Home
--------------------------

Since Microsoft only offers a real kiosk mode on Windows >= Professional we had
to go a little different way. Killing explorer.exe to disable buttons and
gestures and using Chrome's kiosk mode does the job quite well. Our autostart
file is `start.bat`.
