var dataTable;
$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    datata = $('#tblData').DataTable({
        "ajax": {
            "url": "/Admin/Product/GetAll"
        },
        "columns": [
            { "data": "code", "width": "10%" },
            { "data": "title", "width": "30%" },            
            { "data": "price", "width": "10%" },
            { "data": "cost", "width": "10%" },
            { "data": "perce", "width": "10%" },
            { "data": "name", "width": "10%" },
            
            {
                "data": "id",
                "render": function (data) {
                    return `
                            <div class="text-center">
                                <a href="/Admin/Product/Upsert/${data}" class="btn btn-success text-white" style="cursor:pointer"><i class="fas fa-edit"></i></a>
                                <a onclick=Delete("/Admin/Product/Delete/${data}") class="btn btn-danger text-white" style="cursor:pointer"><i class="fas fa-trash-alt"></i></a>
                            </div>                    
                            `;
                }, "width": "20%"
            }
        ]
    });
}

function Delete(url) {
    swal({
        title: "Are you sure you want to Delete?",
        text: "You will not be able to restore the data!",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: "DELETE",
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);                        
                        $('#tblData').DataTable().ajax.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}