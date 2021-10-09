showInPopup = (url, title) => {
    $.ajax({
        type: "GET",
        url: url,
        success: function (res) {
            $("#form-modal .modal-title").html(title);
            $("#form-modal .modal-body").html(res);
            $("#form-modal").modal('show');
        }
    })
}

verifyVIN = url => {
    var VIN = document.getElementById("VIN").value;
    url = url + "?VIN=" + VIN
    $.ajax({
        type: "POST",
        url: url,
        success: function (res) {
            if (res != "true" && res != "false") {
                $("#VIN").addClass("is-invalid");
                $("#VIN").removeClass("is-valid");
                $("#validationVIN").text(res)
                $("#AddOrEditSubmit").prop('disabled', true);
            } else {
                $("#validationVIN").text("")
                $("#AddOrEditSubmit").prop('disabled', false);
                $("#VIN").addClass("is-valid");
                $("#VIN").removeClass("is-invalid");
            }
        },
        error: function (err) {
            console.log(err)
        }
    })
}

jQueryAjaxPost = form => {
    try {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (res) {
                if (res.isValid) {
                    $('#view-all').html(res.html)
                    $('#form-modal .modal-body').html('');
                    $('#form-modal .modal-title').html('');
                    $('#form-modal').modal('hide');
                    $("#unusedImages").text(res.count.result)
                }
                else
                    $('#form-modal .modal-body').html(res.html);
            },
            error: function (err) {
                console.log(err)
            }
        })
        //to prevent default form submit event
        return false;
    } catch (ex) {
        console.log(ex)
    }
}

jQueryAjaxDelete = form => {
    if (confirm('Are you sure to delete this record ?')) {
        try {
            $.ajax({
                type: 'POST',
                url: form.action,
                data: new FormData(form),
                contentType: false,
                processData: false,
                success: function (res) {
                    $('#view-all').html(res.html);
                    console.log(new FormData(form));
                },
                error: function (err) {
                    console.log(err)
                }
            })
        } catch (ex) {
            console.log(ex)
        }
    }
    //prevent default form submit event
    return false;
}

jQueryAjaxDeleteAll = () => {
    if (confirm('Are you sure to delete all records ?')) {
        try {
            $.ajax({
                type: 'GET',
                url: '/Dashboard/DeleteAll',
                success: function (res) {
                    $('#view-all').html(res.html);
                },
                error: function (err) {
                    console.log(err)
                }
            })
        } catch (ex) {
            console.log(ex)
        }
    }
    //prevent default form submit event
    return false;
}

function ResizeCardFee() {
    var height = $(".small-box.bg-success").css("height")
    $(".card.bg-warning").css("height", height);
}

$(document).ready(function () {
    ResizeCardFee()
})

$(window).on('resize', function () {
    ResizeCardFee()
});

const Toast = Swal.mixin({
    toast: true,
    position: 'top-end',
    showConfirmButton: false,
    timer: 3000
});

function FireToast(title) {
    Toast.fire({
        icon: 'success',
        title: title
    })
};