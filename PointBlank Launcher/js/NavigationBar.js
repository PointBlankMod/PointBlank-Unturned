const fs = require("fs");

function openAppNav(id) {
  document.getElementById(id).style.width = "250px";
}

function closeAppNav(id) {
  document.getElementById(id).style.width = "0";
}

function loadPage(name) {
  var data = fs.readFileSync("pages/" + name + ".html");

  document.getElementById("Replacable").innerHTML = data.toString();
}
