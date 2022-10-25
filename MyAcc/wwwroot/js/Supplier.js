var dataTable;
$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    datata = $('#tblData').DataTable({
        "ajax": {
            "url": "/Supplier/GetAll"
        },
        "columns": [
            { "data": "name", "width": "10%" },
            { "data": "email", "width": "10%" },
            { "data": "phone", "width": "10%" },
            { "data": "address1", "width": "10%" },
            { "data": "address2", "width": "10%" },
            { "data": "town", "width": "10%" },
            { "data": "state", "width": "10%" },
            { "data": "postcode", "width": "10%" },
            {
                "data": "Id",
                "render": function (data) {
                    return `
                            <div class="text-center">
                                <a href="/Supplier/Upsert/${data}" class="btn btn-success text-white" style="cursor:pointer"><i class="fas fa-edit"></i></a>
                                <a onclick=Delete("/Supplier/Delete/${data}") class="btn btn-danger text-white" style="cursor:pointer"><i class="fas fa-trash-alt"></i></a>
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