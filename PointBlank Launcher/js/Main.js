const electron = require('electron').remote;
const path = require("path");
const Storage = require('./js/Storage.js');
const fs = require("fs");
const dialog = electron.dialog;
const {app, BrowserWindow} = require("electron").remote

var selectedServer = "-1";
var servers = {}

const config = new Storage({
  configName: "Servers",
  defaults: {
    ServerList: [],
    SteamCMD: ""
  }
});

function createTableEntry(name, online, installed, path, id) {
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
  tr.setAttribute("id", id);
  tr.setAttribute("onclick", "runClickServer('" + id + "')");

  document.getElementById("servers").appendChild(tr);
}
function updateServerList() {
  document.getElementById("servers").innerHTML = "";

  let sIndex;
  servers = {}
  for(sIndex in config.get("ServerList")) {
    server = config.get("ServerList")[sIndex];
    if(server in servers) {
      continue;
    }
    srv = {};

    srv["Name"] = path.basename(server);
    srv["IsOnline"] = false;
    srv["IsInstalled"] = fs.existsSync(server + "/Modules/PointBlank");

    servers[server] = srv;
  }

  let entry;
  let id = 0;
  for(entry in servers) {
    if(!fs.existsSync(entry)) {
      continue;
    }
    createTableEntry(servers[entry]["Name"], servers[entry]["IsOnline"], servers[entry]["IsInstalled"], entry, id);
    id++;
  }
  runClickServer(selectedServer);
}
function runClickServer(id) {
  if(id == "-1") {
    return;
  }
  if(selectedServer != "-1") {
    document.getElementById(selectedServer).className = "";
  }
  document.getElementById(id).className = "selected";
  selectedServer = id;
  updateNavBar(true);
}

function addServer() {
  let win = BrowserWindow.getFocusedWindow();
  let path = dialog.showOpenDialog(win, {
    properties: ["openDirectory"]
  });

  if(path == undefined) {
    return;
  }
  if(!checkServer(path)) {
    alert("Please select a valid server path!", "PointBlank Launcher");
    return;
  }

  let cf = config.get("ServerList");
  cf.push(path[0]);
  config.update();
  updateServerList();
}
function removeServer() {
  if(selectedServer == "-1") {
    return;
  }
  updateNavBar(false);
  let cf = config.get("ServerList");
  let index = parseInt(selectedServer);
  selectedServer = "-1";

  cf.splice(index, 1);
  config.update();
  updateServerList();
}
function checkServer(path) {
  return (fs.existsSync(path + "/Unturned.exe") && fs.existsSync(path + "/Modules"));
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
    document.getElementById("NavButton").className = "navbtn";
  } else {
    document.getElementById("NavButton").className = "hidden";
  }
}

function loadSite() {
  if(config.get("SteamCMD") != "" && !checkSteamCMD(config.get("SteamCMD"))) {
    config.set("SteamCMD", "");
    alert("The currently saved SteamCMD path is invalid! Please reselect it!", "PointBlank Launcher");
  }

  allowNavBar();
  loadMain();
}
function loadMain() {
  if(config.get("SteamCMD") != "") {
    loadPage("Servers");
  }
}

function saveMainConfig() {
  saveSteamCMD();
}

function openSteamCMDDialog() {
  let win = BrowserWindow.getFocusedWindow();
  let path = dialog.showOpenDialog(win, {
    properties: ["openDirectory"]
  });

  if(path == undefined) {
    return;
  }
  if(!checkSteamCMD(path)) {
    alert("Please select a valid SteamCMD path!", "PointBlank Launcher");
    return;
  }

  document.getElementById("path_steamcmd").value = path;
}
function checkSteamCMD(path) {
  return (fs.existsSync(path + "/steamcmd.exe") && fs.existsSync(path + "/steam.dll"));
}
function saveSteamCMD() {
  if(!checkSteamCMD(document.getElementById("path_steamcmd").value)) {
    alert("Please select a valid SteamCMD path!", "PointBlank Launcher");
    return;
  }

  config.set("SteamCMD", document.getElementById("path_steamcmd").value);
  loadSite();
}
