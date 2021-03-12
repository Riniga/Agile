
function isItIE() {
  user_agent = navigator.userAgent;
  var is_it_ie = user_agent.indexOf("MSIE ") > -1 || user_agent.indexOf("Trident/") > -1;
  return is_it_ie; 
}


if (isItIE()){
  alert ("Denna site fungerar INTE överhuvudtaget i Internet explorer, använd Google Chrome eller Microsoft Edge istället.");
}