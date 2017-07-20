const electron = require('electron').remote;
const path = require('path');
const fs = require('fs');
const {app, BrowserWindow} = electron

var selectedServer = -1;
var servers = [];

class Storage {
  constructor(opts) {
    const userDataPath = app.getPath("userData");

    this.path = path.join(userDataPath, opts.configName + ".json");
    this.data = this.parseDataFile(filepath, opts.defaults);
  }

  get(key) {
    return this.data[key];
  }
  set(key, val) {
    this.data[key] = val;

    fs.writeFileSync(this.path, JSON.stringify(this.data));
  }

  static parseDataFile(filePath, defaults) {
    try {
      return JSON.parse(fs.readFileSync(filePath));
    } catch(error) {
      return defaults;
    }
  }
}
module.exports = Storage;

const config = new Storage({
  configName: "Servers",
  defaults: {
    ServerList: [],
    SteamCMD: ""
  }
});

function updateNavBar(show) {
  if(show) {
    document.getElementById("ServerManager").className = "";
  } else {
    document.getElementById("ServerManager").className = "hidden";
  }
}

function createTableEntry(name, online, installed, path) {
  var tr = document.createElement("tr");
  var tdName = document.createElement("td");
  var tdOnline = document.createElement("td");
  var tdInstalled = document.createElement("td");
  var tdPath = document.createElement("td");

  tdName.innerHTML = name;
  tdOnline.innerHTML = (online ? "Yes" : "No");
  tdInstalled.innerHTML = (installed ? "Yes" : "No");
  tdPath.innerHTML = path;

  tr.appendChild(tdName);
  tr.appendChild(tdOnline);
  tr.appendChild(tdInstalled);
  tr.appendChild(tdPath);

  document.getElementById("servers").appendChild(tr);
}

function updateServerList() {
  document.getElementById("servers").innerHTML = "";

  
}
