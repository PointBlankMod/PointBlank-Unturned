const {app, BrowserWindow} = require("electron").remote

function exit()
{
  let win = BrowserWindow.getFocusedWindow();

  win.close();
}

function maximize()
{
  let win = BrowserWindow.getFocusedWindow();

  if(win.isMaximized()) {
    win.unmaximize();
  } else {
    win.maximize();
  }
}

function minimize()
{
  let win = BrowserWindow.getFocusedWindow();

  win.minimize();
}
