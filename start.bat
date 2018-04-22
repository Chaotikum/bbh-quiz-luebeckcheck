'''' kill explorer (to suppress button inputs and gestures)
taskkill /f /im explorer.exe

'''' start and kill chrome (no force), to have a "graceful" shutdown
'''' this deals with a possible the session restoration dialog
START /B "foo" "C:\Program Files\Google\Chrome\Application\chrome.exe" --kiosk file:///C:/Users/gast/Desktop/quiz/index.html
timeout /t 5
taskkill /im chrome.exe
timeout /t 5

'''' start chrome again in foreground
"C:\Program Files\Google\Chrome\Application\chrome.exe" --kiosk file:///C:/Users/gast/Desktop/quiz/index.html
