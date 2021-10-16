import swal from '@sweetalert/with-react';

export const alertService = {
    success,
    error,
    confirm
}

function success(title, message) {
    swal(title, message, "success");
}

function error(message) {
    swal("Error!", message, "error");
}

function confirm(title, message) {
    return swal({
        title: title,
        text: message,
        icon: "warning",
        dangerMode: true,
        buttons: ["Cancel", true],
    });
}