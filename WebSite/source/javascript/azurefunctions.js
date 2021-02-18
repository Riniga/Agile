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

  console.log(userresult);
  localStorage.setItem('result', JSON.stringify(userresult) );
  saveToAzure(result)
  window.location=nextpage;
}

function answer_saveandnext(id, nextpage)
{
  var userresult = JSON.parse(localStorage.getItem('result')); 
  var answer = document.querySelector('input[name="answer"]:checked');
  if (answer)
  {
    userresult.answers[id-1] = answer.value;
    saveToAzure(result)
    window.location=nextpage;
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
    saveToAzure(result)
    console.log(userresult)
    //window.location=nextpage;
  }
  else
  {
    document.getElementById("result").innerHTML = "Du måste ange ett alternativ för att gå vidare!";
  }
}


 


function saveToAzure()
{
  var data = localStorage.getItem('result'); 
  
    var pageUrl = "http://localhost:7071/api/SaveUserHealthRadarResult";
    fetch(pageUrl, { method: 'POST', body: data})
    .then(response => response.json())
    .then(id => {
      var userresult = JSON.parse(data); 
      if (userresult.id === "") 
      {
        userresult.id = id;
        localStorage.setItem('result', JSON.stringify(userresult) );
      }
      document.getElementById("result").innerHTML = "Saved to Azure!";
      console.log("Saved to Azure")
      console.log(userresult);
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

