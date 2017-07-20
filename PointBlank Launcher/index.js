const electron = require("electron")
const {app, BrowserWindow} = electron

var MainWindow;

app.on("ready", () => {
  MainWindow = new BrowserWindow({
    frame: false,
    width: 1280,
    height: 720,
    icon: __dirname + "/images/PB_logo.ico"
  })

  MainWindow.loadURL(`file://${__dirname}/index.html`)
})
