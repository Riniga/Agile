$(document).ready(function () {
    $('#personstable_1').DataTable(
        {
            ajax: { url: 'http://localhost:7071/api/GetEmployees', dataSrc: "" },
            columns: [
                { data: 'displayName' },
                { data: 'uniqueName' },
                { data: 'id' }
            ],
            columnDefs: [
                //{
                //    targets: 0,
                //    render: function (data, type, row, meta) {
                //        var ret = row.firstname + ' ' + row.lastname;
                //        return ret;
                //    }
                //},
                {
                    targets: 1,
                    data: 'email',
                    render: function (data, type, row, meta) {
                        return '<a href="mailto:' + data + ' ">' + data + '</a>';
                    }
                },
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
//id":1,"email":"rickard@nisses-gagner.se","firstname":"Rickard","lastname":"Nisses-Gagnér","notes":"","inDevops":false,"inTeaams":false}