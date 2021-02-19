const saveUrl = "http://localhost:7071/api/SaveUserHealthRadarResult";

function page1_saveandnext(nextpage)
{
  var areaselect = document.getElementById("area");
  var roleselect = document.getElementById("role");
  var userresult = JSON.parse(localStorage.getItem('result')); 
  
  if ((!userresult))
  {
      userresult = {id:"", area:areaselect.options[areaselect.selectedIndex].value, role:roleselect.options[roleselect.selectedIndex].value, answers:["0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0"]};
  }
  else
  {
    userresult.area = areaselect.options[areaselect.selectedIndex].value;
    userresult.role = roleselect.options[roleselect.selectedIndex].value;
  }
  SaveAndRedirect(userresult, nextpage);
}

function answer_saveandnext(id, nextpage)
{
  var userresult = JSON.parse(localStorage.getItem('result')); 
  var answer = document.querySelector('input[name="answer"]:checked');
  
  if (answer)
  {
    userresult.answers[id-1] = answer.value;
    SaveAndRedirect(userresult, nextpage);
  }
  else
  {
    document.getElementById("result").innerHTML = "Du måste ange ett alternativ för att gå vidare!";
  }
}

function answer_saveandsubmit(id)
{
  var userresult = JSON.parse(localStorage.getItem('result')); 
  var answer = document.querySelector('input[name="answer"]:checked');
  if (answer)
  {
    userresult.answers[id-1] = answer.value;
    console.log(answer.value);
    console.log(userresult);
    SaveAndRedirect(userresult, "result.html")
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
  fetch(saveUrl, { method: 'POST', body: JSON.stringify(userresult)})
    .then(response => response.json())
    .then(id => {
      if (userresult.id === "") userresult.id = id;
      
      document.getElementById("result").innerHTML = "Saved!";
      window.location=page;
      document.getElementById("spinner").setAttribute('hidden', '');
      
    });
}

function enableNext()
{
  document.getElementById("saveandnext").disabled =false;
}

radioboxes = document.getElementsByName("answer")
for(i in radioboxes)
{
  if (radioboxes[i].value )  radioboxes[i].addEventListener('click',enableNext);
} 

