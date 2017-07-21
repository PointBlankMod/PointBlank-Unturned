const electron = require('electron').remote;
const path = require('path');
const fs = require('fs');
const {app, BrowserWindow} = electron

var selectedServer = -1;
var servers = {}

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
  let tr = document.createElement("tr");
  let tdName = document.createElement("td");
  let tdOnline = document.createElement("td");
  let tdInstalled = document.createElement("td");
  let tdPath = document.createElement("td");

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

  let server;
  for(server in config.get("ServerList")) {
    if(server in servers) {
      continue;
    }
    srv = {};

    srv["Name"] = path.dirname(server).split(path.sep).pop();
    srv["IsOnline"] = false;
    srv["IsInstalled"] = fs.existsSync(server + "/Modules/PointBlank");

    servers[server] = srv;
  }

  let entry;
  for(entry in servers) {
    if(!fs.existsSync(entry)) {
      continue;
    }
    createTableEntry(servers[entry]["Name"], servers[entry]["IsOnline"], servers[entry]["IsInstalled"], entry);
  }
}
