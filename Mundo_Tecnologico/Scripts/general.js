$(document).ready(function () {
    $('.popUpImage').click(function (event) {
        event.preventDefault();
        $(".modal img").attr("src", $(this).attr('href'));
        $(".modal").modal('show');
    });
});

$('.custom-file-input').on('change', function (event) {
    var inputFile = event.currentTarget;
    $(inputFile).parent()
        .find('.custom-file-label')
        .html(inputFile.files[0].name);
});

$(document).on('click', '.number-spinner button', function () {
    var btn = $(this),
        oldValue = btn.closest('.number-spinner').find('input').val().trim(),
        newVal = 0;

    if (btn.attr('data-dir') == 'up' && (btn.attr('data-dir') == 'up' && parseInt(btn.closest('.number-spinner').find('input').attr('max'), 10) > parseInt(btn.closest('.number-spinner').find('input').val(), 10))) {
        newVal = parseInt(oldValue) + 1;
    } else if (btn.attr('data-dir') == 'dwn') {
        if (oldValue > 1) {
            newVal = parseInt(oldValue) - 1;
        } else {
            newVal = 1;
        }
    } else {
        newVal = parseInt(oldValue);
        alert('Ha alcanzado el stock máximo')
    }
    btn.closest('.number-spinner').find('input').val(newVal);
});

$(document).on('click', '.editarCarrito', function () {
    var a = $(this),
        cantidad = a.closest('tr').find('input').val().trim(),
        codigo = a.closest('tr').find('#codigo').html();
    parent.location.href = "EditarCarr" + "?codigo=" + codigo + "&cantidad=" + cantidad;
});

$(function () {
    $('#dataTable').DataTable({
        "order": [[0, "asc"]],
        "lengthMenu": [[10, 20, 30, -1], [10, 20, 30, "All"]]
    });


    $('#dataTableVenta').DataTable({
        "order": [[1, "desc"]],
        "lengthMenu": [[10, 20, 30, -1], [10, 20, 30, "All"]]
    });
});

$(function () {
    $('#tablaDetalle').DataTable({
        "order": [[0, "asc"]],
        "lengthMenu": [[10, 20, 30, -1], [10, 20, 30, "All"]]
    });

});

$(function () {
    $('#formularioUsuario').validate({
        rules: {
            nombre: {
                required: true,
                minlength: 3,
                maxlength: 30
            },
            apellidos: {
                required: true,
                minlength: 5,
                maxlength: 30
            },
            documento: {
                required: true,
                digits: true,
                minlength: 8,
                maxlength: 8,
            },
            celular: {
                required: true,
                digits: true,
                minlength: 9,
                maxlength: 9,
            },
            correo: {
                required: true,
                email: true,
                minlength: 10,
                maxlength: 100
            },
            clave: {
                required: true,
                minlength: 5
            },
            nuevaClave: {
                required: true,
                minlength: 5,
            },
            confirmaClave: {
                required: true,
                minlength: 5,
                equalTo: "#nuevaClave"
            },
            estado: {
                required: true,
            },
            codigoTipoUsuario: {
                required: true,
            }
        },
        messages: {
            nombre: {
                required: "Este campo es obligatorio",
                minlength: "El nombre debe tener mínimo 3 caracteres",
                maxlength: "El nombre debe tener máximo 30 caracteres"
            },
            apellidos: {
                required: "Este campo es obligatorio",
                minlength: "El apellido debe tener mínimo 3 caracteres",
                maxlength: "El apellido debe tener máximo 30 caracteres"
            },
            documento: {
                required: "Este campo es obligatorio",
                minlength: "El documento debe tener 8 digitos",
                maxlength: "El documento debe tener 9 digitos",
                digits : "Deben ser digitos"
            },
            celular: {
                required: "Este campo es obligatorio",
                minlength: "El documento debe tener 9 digitos",
                maxlength: "El documento debe tener 9 digitos",
                digits: "Deben ser digitos"
            },
            correo: {
                email: "Ingrese un correo electrónico válido",
                required: "Este campo es obligatorio",
                minlength: "El correo debe tener mínimo 10 caracteres",
                maxlength: "El correo debe tener máximo 100 caracteres"
            },
            clave: {
                required: "Este campo es obligatorio",
                minlength: "La clave debe tener mínimo 5 caracteres",
            },
            nuevaClave: {
                required: "Este campo es obligatorio",
                minlength: "La clave debe tener mínimo 5 caracteres",
                
            },
            confirmaClave: {
                required: "Este campo es obligatorio",
                equalTo: "Las claves deben coincidir",
            },
            estado: {
                required: "Este campo es obligatorio"
            },
            codigoTipoUsuario: {
                required: "Este campo es obligatorio"
            },
        },
        errorElement: 'span',
        errorPlacement: function (error, element) {
            error.addClass('invalid-feedback');
            element.closest('.form-group').append(error);
        },
        highlight: function (element, errorClass, validClass) {
            $(element).addClass('is-invalid');
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).removeClass('is-invalid');
        }
    });
});

$(function () {
    $('#formularioCategoria').validate({
        rules: {
            nombre: {
                required: true,
                minlength: 4,
                maxlength: 30
            },
            estado: {
                required: true,
            }
        },
        messages: {
            nombre: {
                required: "Este campo es obligatorio",
                minlength: "El nombre debe tener mínimo 4 caracteres",
                maxlength: "El nombre debe tener máximo 30 caracteres"
            },
            estado: {
                required: "Este campo es obligatorio",
            },
        },
        errorElement: 'span',
        errorPlacement: function (error, element) {
            error.addClass('invalid-feedback');
            element.closest('.form-group').append(error);
        },
        highlight: function (element, errorClass, validClass) {
            $(element).addClass('is-invalid');
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).removeClass('is-invalid');
        }
    });
});

$(function () {
    $('#formularioVenta').validate({
        rules: {
            direccion: {
                required: true,
                minlength: 15,
                maxlength: 100
            },
            referencia: {
                required: true,
                minlength: 15,
                maxlength: 100
            },
            codigoDistrito: {
                required: true,
            }
        },
        messages: {
            direccion: {
                required: "Este campo es obligatorio",
                minlength: "El nombre debe tener mínimo 15 caracteres",
                maxlength: "El nombre debe tener máximo 100 caracteres"
            },
            referencia: {
                required: "Este campo es obligatorio",
                minlength: "El nombre debe tener mínimo 15 caracteres",
                maxlength: "El nombre debe tener máximo 100 caracteres"
            },
            codigoDistrito: {
                required: "Este campo es obligatorio",
            },
        },
        errorElement: 'span',
        errorPlacement: function (error, element) {
            error.addClass('invalid-feedback');
            element.closest('.form-group').append(error);
        },
        highlight: function (element, errorClass, validClass) {
            $(element).addClass('is-invalid');
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).removeClass('is-invalid');
        }
    });
});

$(function () {
    $('#formularioProducto').validate({
        rules: {
            nombre: {
                required: true,
                minlength: 10,
                maxlength: 100
            },
            descripcion: {
                required: true,
                minlength: 60,
                maxlength: 600
            },
            precio: {
                required: true,
                number: true,
                min: 1
            },
            stock: {
                required: true,
                number: true,
                min: 0
            },
            marca: {
                required: true,
            },
            categoria: {
                required: true,
            }

        },
        messages: {
            nombre: {
                required: "Este campo es obligatorio",
                minlength: "El nombre debe tener mínimo 10 caracteres",
                maxlength: "El nombre debe tener máximo 100 caracteres"
            },
            descripcion: {
                required: "Este campo es obligatorio",
                minlength: "El nombre debe tener mínimo 60 caracteres",
                maxlength: "El nombre debe tener máximo 600 caracteres"
            },
            precio: {
                required: "Este campo es obligatorio",
                min: "El precio debe ser mayor que 0.00",
                number: "Debe ingresar un número decimal"
            },
            stock: {
                required: "Este campo es obligatorio",
                min: "El stock debe ser mayor o igual que 0",
                number: "Debe ingresar un número entero"
            },
            marca: {
                required: "Este campo es obligatorio",
            },
            categoria: {
                required: "Este campo es obligatorio",
            },
        },
        errorElement: 'span',
        errorPlacement: function (error, element) {
            error.addClass('invalid-feedback');
            element.closest('.form-group').append(error);
        },
        highlight: function (element, errorClass, validClass) {
            $(element).addClass('is-invalid');
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).removeClass('is-invalid');
        }
    });
});

function ConfirmarDelete(e, codigo) {
    e.preventDefault();
    Swal.fire({
        title: '¿Seguro que deseas eliminar?',
        icon: 'warning',
        showCancelButton: true,
        cancelButtonText: "Cancelar",
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: '¡Sí, Elimínalo!',
    }).then((result) => {
        if (result.isConfirmed) {
            Swal.fire({
                position: 'center',
                icon: 'success',
                title: 'Se actualizo el estado a "Eliminado"',
                showConfirmButton: false,
                timer: 1500
            })
            setTimeout(function () { window.location.href = 'Eliminar/' + codigo; }, 1500);
        }
    })
}

function ConfirmarDeleteCarrito(e, codigo) {
    e.preventDefault();
    Swal.fire({
        title: '¿Seguro que deseas eliminar el producto del carrito?',
        icon: 'warning',
        showCancelButton: true,
        cancelButtonText: "Cancelar",
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: '¡Sí, Elimínalo!',
    }).then((result) => {
        if (result.isConfirmed) {
            Swal.fire({
                position: 'center',
                icon: 'success',
                title: 'Se eliminó el producto del carrito',
                showConfirmButton: false,
                timer: 1500
            })
            setTimeout(function () { window.location.href = 'Eliminar/' + codigo; }, 1500);
        }
    })
}

$(function () {
    $('#formularioProductoRegistro').validate({
        rules: {
            foto: {
                required: true
            },
            nombre: {
                required: true,
                minlength: 10,
                maxlength: 100
            },
            descripcion: {
                required: true,
                minlength: 60,
                maxlength: 600
            },
            precio: {
                required: true,
                number: true,
                min: 1
            },
            stock: {
                required: true,
                number: true,
                min: 0,
            },
            codigoMarca: {
                required: true,
            },
            codigoCategoria: {
                required: true,
            },
            estado: {
                required: true,
            }
        },
        messages: {
            foto: {
                required: "Este campo es obligatorio"
            },
            nombre: {
                required: "Este campo es obligatorio",
                minlength: "El nombre debe tener mínimo 10 caracteres",
                maxlength: "El nombre debe tener máximo 100 caracteres"
            },
            descripcion: {
                required: "Este campo es obligatorio",
                minlength: "El nombre debe tener mínimo 60 caracteres",
                maxlength: "El nombre debe tener máximo 600 caracteres"
            },
            precio: {
                required: "Este campo es obligatorio",
                min: "El precio debe ser mayor que 0.00",
                number: "Debe ingresar un número decimal"
            },
            stock: {
                required: "Este campo es obligatorio",
                min: "El stock debe ser mayor o igual que 0",
                number: "Debe ingresar un número entero"
            },
            codigoMarca: {
                required: "Este campo es obligatorio"
            },
            codigoCategoria: {
                required: "Este campo es obligatorio"
            },
            estado: {
                required: "Este campo es obligatorio"
            },
        },
        errorElement: 'span',
        errorPlacement: function (error, element) {
            error.addClass('invalid-feedback');
            element.closest('.form-group').append(error);
        },
        highlight: function (element, errorClass, validClass) {
            $(element).addClass('is-invalid');
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).removeClass('is-invalid');
        }
    });
});

$(function () {
    $('#formularioMarca').validate({
        rules: {
            nombre: {
                required: true,
                minlength: 2,
                maxlength: 30
            },
            estado: {
                required: true,
            }
        },
        messages: {
            nombre: {
                required: "Este campo es obligatorio",
                minlength: "El nombre debe tener mínimo 2 caracteres",
                maxlength: "El nombre debe tener máximo 30 caracteres"
            },
            estado: {
                required: "Este campo es obligatorio",
            },
        },
        errorElement: 'span',
        errorPlacement: function (error, element) {
            error.addClass('invalid-feedback');
            element.closest('.form-group').append(error);
        },
        highlight: function (element, errorClass, validClass) {
            $(element).addClass('is-invalid');
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).removeClass('is-invalid');
        }
    });
});

$(function () {
    $('#formularioLogin').validate({
        rules: {
            correo: {
                required: true,
                email: true
            },
            clave: {
                required: true,
                minlength: 5,
            }

        },
        messages: {
            correo: {
                required: "Este campo es obligatorio",
                email: "Debe ingresar un correo electrónico válido",
            },
            clave: {
                required: "Este campo es obligatorio",
                minlength: "La clave debe tener mínimo 5 caracteres",
            }
        },
        errorElement: 'span',
        errorPlacement: function (error, element) {
            error.addClass('invalid-feedback');
            element.closest('.form-group').append(error);
        },
        highlight: function (element, errorClass, validClass) {
            $(element).addClass('is-invalid');
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).removeClass('is-invalid');
        }
    });
});
