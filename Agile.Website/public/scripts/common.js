
var questionId = -1
var areaId=-1;
var questionData;

function getParameterByName(name, url = window.location.href) {
  name = name.replace(/[\[\]]/g, '\\$&');
  var regex = new RegExp('[?&]' + name + '(=([^&#]*)|&|#|$)'),
      results = regex.exec(url);
  if (!results) return null;
  if (!results[2]) return '';
  return decodeURIComponent(results[2].replace(/\+/g, ' '));
}

function UpdateContent()
{
  var userresult = JSON.parse(localStorage.getItem('result')); 
  var footer = document.getElementById("sessioninfo");

  if (userresult)
  {
    footer.innerHTML = userresult.id + ", " + userresult.area + ", " + userresult.role;
  }
  else
  {
    footer.innerHTML = "No user";
  }
}



const initalize = async () => {
  questionId= parseInt( getParameterByName('question'));
  const response = await fetch("/data/questions.json");
  questionData = await response.json();
  UpdateContent();
  if (questionId||questionId===0)  Load();

}

initalize();



