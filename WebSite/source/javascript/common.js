
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

function DrawMenu()
{
  // TODO: Generate meny
}

const initalize = async () => {
  questionId= parseInt( getParameterByName('question'));
  const response = await fetch("/data/questions.json");
  questionData = await response.json();
  DrawMenu();
  if (questionId||questionId===0)  Load();

}

initalize();



