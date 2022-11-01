$(document).ready(function () {
    $('#personstable_1').DataTable(
        {
            ajax: { url: 'http://localhost:7071/api/GetRoles', dataSrc: "" },
            columns: [
                { data: 'name' },
                { data: 'id' }
            ],
            columnDefs: [
                {
                    targets: 1,
                    data: 'id',
                    orderable: false,
                    render: function (data, type, row, meta) {
                        return '<a href="/teams/role.html?id=' + data + '">Open</a>';
                    }
                }
            ]
        }
    );
});