const fs = require("fs");

function openAppNav() {
  document.getElementById("AppNavigation").style.width = "250px";
}

function closeAppNav() {
  document.getElementById("AppNavigation").style.width = "0";
}

function loadPage(name) {
  var data = fs.readFileSync("pages/" + name + ".html");

  document.getElementById("Replacable").innerHTML = data.toString();
  closeAppNav();
}
