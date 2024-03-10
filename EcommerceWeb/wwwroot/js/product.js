
$(document).ready(function () {
    loadDataTable()
})
var dataTable;
function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        ajax: { url: '/admin/product/getall' },
        columns: [
            { data: 'title', "width": "30%" },
            { data: 'author', "width": "30%" },
            { data: 'category.name' },
            {
                data: 'id',
                render: function (data) {
                    return `
                    <div class='w-100 btn-group' role='group' >
                       <a href="/admin/product/upsert?id=${data}" class="btn btn-primary my-2 mx-4"> <i class="bi bi-pencil-square"></i> Edit</a>               
                       <a onClick="onDelete('/admin/product/delete/${data}', dataTable)" class="btn btn-danger my-2 "> <i class="bi bi-trash-fill"></i> Delete</a>         
                    </div>`
                }
            }
        ]
    });
}

