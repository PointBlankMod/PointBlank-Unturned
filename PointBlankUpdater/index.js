const WebData = require('./API/WebData');

const URL_Info = "https://pastebin.com/raw/ZVcNXEVw";

console.log("Welcome to the PointBlank Updater!");
console.log("Created by AtiLion");
console.log("");
console.log("Connecting to pastebin....");

let infoData = WebData.DownloadText(URL_Info);

console.log("Parsing data...");

let jsonData = JSON.parse(infoData);
let pbVersion = jsonData.PointBlank_Version;
let pbLatest = jsonData.PointBlank_Latest;
