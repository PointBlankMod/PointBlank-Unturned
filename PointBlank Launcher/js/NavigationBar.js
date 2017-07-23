var currentSelect = "";

function openAppNav() {
  document.getElementById("AppNavigation").style.width = "250px";
}

function closeAppNav() {
  document.getElementById("AppNavigation").style.width = "0";
}

function loadPage(name) {
  var data = fs.readFileSync("pages/" + name + ".html");

  if(currentSelect != "") {
    document.getElementById(currentSelect).className = document.getElementById(currentSelect).className.replace(" active", "");
  }
  document.getElementById("Replacable").innerHTML = data.toString();
  document.getElementById(name).className += " active";
  currentSelect = name;
  closeAppNav();

  if(document.getElementById("StartupScript") != null) {
    eval(document.getElementById("StartupScript").innerHTML);
  }
}
