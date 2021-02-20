var pageUrl = "http://localhost:7071/api/GetAllHealthRadarResults";
var average_all = []
var all_answeres= [];
for (var i=0;i<16;i++) all_answeres.push([0,0,0,0,0]);

function DrawResults()
{
    DrawBackground();
    fetch(pageUrl, { method: 'GET'})
    .then(response => response.json())
    .then(data => {
      data.forEach(record => CalculateAverages(record));
      console.log(all_answeres);
      AddAveragePerSector();
      DrawRedDots();
      DrawBlackDots();
    });
}

function DrawResultsDummy()
{
  DrawBackground();
  var data = [];
  data.push({id:"", area:"HR", role:"team", answers:[ Math.ceil(Math.random() * 5),Math.ceil(Math.random() * 5),Math.ceil(Math.random() * 5),Math.ceil(Math.random() * 5),Math.ceil(Math.random() * 5),Math.ceil(Math.random() * 5),Math.ceil(Math.random() * 5),Math.ceil(Math.random() * 5),Math.ceil(Math.random() * 5),Math.ceil(Math.random() * 5),Math.ceil(Math.random() * 5),Math.ceil(Math.random() * 5),Math.ceil(Math.random() * 5),Math.ceil(Math.random() * 5),Math.ceil(Math.random() * 5),Math.ceil(Math.random() * 5)]});
  data.forEach(record => CalculateAverages(record));
  AddAveragePerSector();
  DrawRedDots();
}

function CalculateAverages(record)
{
  for (var i=0;i<16;i++)
  {
    all_answeres[i][record.answers[i]-1]+=1;
    var current = record.answers[i];
    if (current!=0)
    {
      if (!average_all[i] || average_all[i]==0) average_all[i] = current;
      else average_all[i] = (average_all[i]+current)/2;
    }
  }
}

function AddAveragePerSector()
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

}

function DrawBackground()
{
  var canvas = document.getElementById("thecanvas");
  var context = canvas.getContext("2d");
  var image = new Image();
  image.src = '/images/HealtRadarBackground.png';
  context.drawImage(image, 0, 0, 800,800);
}

function DrawRedDots()
{
  var scale = 57;
  var dx=400;
  var dy=400;
  var dAngle = 0.1*Math.PI
  var angle = dAngle/2-Math.PI/2;

  var canvas = document.getElementById("thecanvas");
  var context = canvas.getContext("2d");
  context.fillStyle = "#f00";
  context.lineWidth = "2";
  context.strokeStyle = "#f00"; 

  for (var i =0;i<average_all.length;i++)
  {
      var x1 = scale * average_all[i] * Math.cos(angle);
      var y1 = scale * average_all[i] * Math.sin(angle);
      var x2 = scale * average_all[(i+1)%average_all.length] * Math.cos(angle+dAngle);
      var y2 = scale * average_all[(i+1)%average_all.length] * Math.sin(angle+dAngle);

      context.beginPath();
      context.arc(x1+dx , y1+dy, 4, 0, Math.PI * 2, true);
      context.fill();

      context.beginPath();
      context.moveTo(x1+dx, y1+dy);
      context.lineTo(x2+dx, y2+dy);
      context.stroke();
      angle += dAngle;
  }
}

function DrawBlackDots()
{
  //TODO: Minor adjustments needed
  var scale = 57;
  var dx=400;
  var dy=400;
  var dAngle = 0.1*Math.PI;
  var angle = 0.01-Math.PI/2;

  var canvas = document.getElementById("thecanvas");
  var context = canvas.getContext("2d");
  context.fillStyle = "#000";

  for (var i=0;i<16;i++) 
  {
    for (var j=0;j<5;j++) 
    {
      var scale = 57*(j+1);
      var count = all_answeres[i][j]
      var dAngle2 = count*Math.PI/10;
      for (var k=0;k<count;k++)
      {
        var x1 = scale * Math.cos(angle+count*dAngle2);
        var y1 = scale * Math.sin(angle+count*dAngle2);
        context.beginPath();
        context.arc(x1+dx , y1+dy, 2, 0, Math.PI * 2, true);
        context.fill();
      }
    }
    angle += dAngle;
    if(i%4==1) angle += dAngle;
  }
}

DrawResults();
