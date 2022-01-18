function DrawResults()
{
  average_all = []
  all_answeres= [];
  for (var i=0;i<16;i++) all_answeres.push([0,0,0,0,0]);
  DrawResultsFiltered();
}

function CreateTable()
{
  for (var i=0;i<16;i++) all_answeres.push([0,0,0,0,0]);
  console.log(all_answeres);
}

function DrawResultsFiltered()
{
  var items = []
    fetch(GetAllHealthRadarResultsUrl, { method: 'GET'})
    .then(response => response.json())
    .then(data => {
      data.forEach(record =>
        {
          items.push(record);
        } );
        ConfigDataTable(items);
    });
}

function ConfigDataTable(items)
{
  $('#table_id').DataTable({
    //"ajax": GetAllHealthRadarResultsUrl,
    data: items,
    "columns" : [
      { "data" : "id" },
      { "data" : "role" },
      { "data" : "area" },
      { "data" : "answers" },
      { "data" : "posted" },
  ]
  });

}

DrawResults();


