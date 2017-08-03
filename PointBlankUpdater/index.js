const WebData = require('./API/WebData');
const Storage = require('./API/Storage');

const URL_Info = "https://pastebin.com/raw/ZVcNXEVw";

const Config = new Storage({
  configName: "PointBlankUpdater_Unturned",
  defaults: {
    "PBVersion": "0",
    "APIVersion": "0",
    "LauncherVersion": "0"
  }
});

console.log("Welcome to the PointBlank Updater!");
console.log("Created by AtiLion");
console.log("");

if(Config.get("LauncherVersion") != "0") {
  console.log("Checking folder...");
  
}

console.log("Connecting to pastebin....");
let infoData = WebData.DownloadText(URL_Info);

console.log("Parsing data...");
let jsonData = JSON.parse(infoData);
let pbVersion = jsonData.PointBlank_Version;
let pbLatest = jsonData.PointBlank_Latest;
let unturnedVersion = jsonData.Games.Unturned.API_Version;
let unturnedLatest = jsonData.Games.Unturned.API_Latest;
let launcherVersion = jsonData.Games.Unturned.Launcher_Version;
let launcherLatest = jsonData.Games.Unturned.Launcher_Latest;

if(Config.get("LauncherVersion") == "0") {
  console.log("Latest PointBlank version: " + pbVersion);
  console.log("Current PointBlank version: " + Config.get("PBVersion"));
  console.log("Latest API version: " + unturnedVersion);
  console.log("Current API version" + Config.get("APIVersion"));
}
console.log("Latest launcher version: " + launcherVersion);
console.log("Current launcher version: " + Config.get("LauncherVersion"));
Config.update();

console.log("");
if(Config.get("LauncherVersion") == "0") {
  if(pbVersion != Config.get("PBVersion") && pbLatest != "")
    console.log("PointBlank update available!");
  if(unturnedVersion != Config.get("APIVersion") && unturnedLatest != "")
    console.log("API update available!");
}
if(launcherVersion != Config.get("LauncherVersion") && launcherLatest != "")
  console.log("Launcher update available!");
