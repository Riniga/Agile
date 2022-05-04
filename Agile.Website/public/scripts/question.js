var hr = [0,3, 5,6,7,8 ,9,10,12,14];


function Load()
{
  var areaId = Math.floor(questionId/4);
  var currentQuestion = questionId%4;
  document.getElementById("area").innerHTML = questionData.areas[areaId].name;
  document.getElementById("name").innerHTML = questionData.areas[areaId].questions[currentQuestion].name;
  document.getElementById("description").innerHTML = questionData.areas[areaId].questions[currentQuestion].description;
  document.getElementById("question").innerHTML = questionData.areas[areaId].questions[currentQuestion].question;
  document.getElementById("image").src = questionData.areas[areaId].questions[currentQuestion].image;
  document.getElementById("sit").innerHTML = questionData.areas[areaId].questions[currentQuestion].sit;
  document.getElementById("crawl").innerHTML = questionData.areas[areaId].questions[currentQuestion].crawl;
  document.getElementById("walk").innerHTML = questionData.areas[areaId].questions[currentQuestion].walk;
  document.getElementById("run").innerHTML = questionData.areas[areaId].questions[currentQuestion].run;
  document.getElementById("fly").innerHTML = questionData.areas[areaId].questions[currentQuestion].fly;
}

function answer_saveandnext()
{
  var userresult = JSON.parse(localStorage.getItem('result')); 
  var answer = document.querySelector('input[name="answer"]:checked');
  if (answer)
  {
    userresult.answers[questionId] = answer.value;
    if (questionId==15) SaveAndRedirect(userresult, "result.html");
    else 
    {
      //TODO: Enable selective set of questions
      var currentIndex = hr.indexOf(questionId);
      SaveAndRedirect(userresult, "question.html?question="+ (questionId+1));
    }
  }
  else
  {
    document.getElementById("result").innerHTML = "Du måste ange ett alternativ för att gå vidare!";
  }
}

function SaveAndRedirect(userresult, page)
{
  localStorage.setItem('result', JSON.stringify(userresult) );
  document.getElementById("spinner").removeAttribute('hidden');
  fetch(SaveUserHealthRadarResultUrl, { method: 'POST', body: JSON.stringify(userresult)})
    .then(function(response){return response.json();} )
    .then(id => {
      if (userresult.id === "")
      {
        userresult.id = id;
        localStorage.setItem('result', JSON.stringify(userresult) );
      } 
      document.getElementById("result").innerHTML = "Saved!";
      document.getElementById("spinner").setAttribute('hidden', '');
      window.location=page;
    });
}

function enableNext()
{
  document.getElementById("saveandnext").disabled =false;
}
