function saveAndNext()
{
  document.getElementById("result").innerHTML = "Bara ett test";

    var pageUrl = "http://localhost:7071/api/SaveUserHealtRadarResult";
    
    fetch(pageUrl, { method: 'GET' })
    .then(response => response.json())
    .then(data => {
      document.getElementById("result").innerHTML = "Resultat: Ok, det gick bra!";
    });

    window.location=document.getElementById("next").value;
}