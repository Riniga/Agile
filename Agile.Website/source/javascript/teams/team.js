$(document).ready(function () {

    id = parseInt(getParameterByName('id'));
    fetch("http://localhost:7071/api/GetTeam?teamId=" + id, { method: 'GET' })
        .then(response => response.json())
        .then(data => {
            document.getElementById("teamName").textContent = data.name;
        });
    $('#personstable_1').DataTable(
        {
            ajax: { url: 'http://localhost:7071/api/GetEmployeesInTeam?teamId='+id, dataSrc: "" },
            columns: [
                { data: 'name' },
                { data: 'role' },
                { data: 'id' }
            ]
            ,
            columnDefs: [
                {
                    targets: 2,
                    data: 'id',
                    orderable: false,
                    render: function (data, type, row, meta) {
                        return '<a href="/teams/employee.html?id=' + data + '">Open</a>';
                    }
                }
            ]
        }
    );
});