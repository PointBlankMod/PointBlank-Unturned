const electron = require('electron').remote;
const path = require("path");
const Storage = require('./js/Storage.js');
const fs = require("fs");

var selectedServer = -1;
var servers = {}

const config = new Storage({
  configName: "Servers",
  defaults: {
    ServerList: [],
    SteamCMD: ""
  }
});

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

function updateNavBar(show) {
  if(show) {
    document.getElementById("ServerManager").className = "";
  } else {
    document.getElementById("ServerManager").className = "hidden";
  }
}

function allowNavBar() {
  if(config.get("SteamCMD") != "") {
    document.getElementById("NavButton").className = "";
  } else {
    document.getElementById("NavButton").className = "hidden";
  }
}
