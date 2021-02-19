var pageUrl = "http://localhost:7071/api/GetAllHealthRadarResults";
var average_all = []
var average_area = []

function GetResults()
{
    DrawBackground();
    fetch(pageUrl, { method: 'GET'})
    .then(response => response.json())
    .then(data => {
      data.forEach(record => DrawBlackDots(record));
      DrawRedDots();
    });
}

function DrawBackground(record)
{
  var canvas = document.getElementById("thecanvas");
  var context = canvas.getContext("2d");
  var image = new Image();
  image.src = '/images/HealtRadarBackground.png';
  context.drawImage(image, 0, 0, 800,800);
}

function DrawRedDots()
{
  var average_area = []
  average_area[0] = (average_all[0]+average_all[1]+average_all[2]+average_all[3])/4;
  average_area[1] = (average_all[4]+average_all[5]+average_all[6]+average_all[7])/4;
  average_area[2] = (average_all[8]+average_all[9]+average_all[10]+average_all[11])/4;
  average_area[3] = (average_all[12]+average_all[13]+average_all[14]+average_all[15])/4;

  average_all.splice(4,0,average_area[0]);
  average_all.splice(9,0,average_area[1]);
  average_all.splice(14,0,average_area[2]);
  average_all.splice(19,0,average_area[3]);
  
  var options = {
    series: [{
    name: 'Series 1',
    data: average_all
  }],
    chart: {
    height: 350,
    type: 'radar',
  },
  title: {
    text: ''
  },
  xaxis: {
    categories: ['a', 'b', 'c', 'd', 'e', 'f','a', 'b', 'c', 'd', 'e', 'f','a', 'b', 'c', 'd', 'e', 'f','g','h']
  }
  };
  var chartDiv =  document.querySelector("#chart");
  

  var chart = new ApexCharts(chartDiv, options);
  chart.render();


  console.log(average_area);
}

function DrawBlackDots(record)
{
  var canvas = document.getElementById("thecanvas");
  var context = canvas.getContext("2d");

  for (var i=0;i<16;i++)
  {
    var current = record.answers[i];
    if (current!=0)
    {
      if (!average_all[i] || average_all[i]==0) average_all[i] = current;
      else average_all[i] = (average_all[i]+current)/2;
    }
  }
  for (var i=0;i<16;i++)
  {
    context.beginPath();
    context.arc(Math.floor(Math.random() * 400)+200 , Math.floor(Math.random() * 400)+200, 2, 0, Math.PI * 2, true);
    context.fillStyle = "#000";
    context.fill();
  }
}

GetResults();
