var pageUrl = "http://localhost:7071/api/GetAllHealthRadarResults";

function GetResults()
{
    fetch(pageUrl, { method: 'GET'})
    .then(response => response.json())
    .then(data => {
      data.forEach(record => DrawDots(record));
    });
}

function DrawDots(record)
{
  var canvas = document.getElementById("thecanvas");
  var context = canvas.getContext("2d");
  var image = new Image();
  image.src = '/images/HealtRadarBackground.png';
  context.drawImage(image, 0, 0, 800,800);


  console.log(record);
  //TODO: Draw dots correct somehow....
  context.beginPath();
  context.arc(220, 220, 10, 0, Math.PI * 2, true);
  context.fillStyle = "#FF0000";
  context.fill();
  
  context.stroke();
}

GetResults();
