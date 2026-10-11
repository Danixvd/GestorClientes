(() => {

    // Maneja el borrado de clientes: abre el modal de confirmación y elimina por AJAX
    const Cliente = {
        idEliminar: 0,
        modalEliminar: null,

        init() {
            this.modalEliminar = new bootstrap.Modal('#modalEliminarCliente');
            this.registrarEventos();
        },

        registrarEventos() {
            // Botón "Eliminar" de cada fila: guarda el id y abre el modal de confirmación
            $(document).on('click', '.btn-eliminar-cliente', (e) => {
                const boton = $(e.currentTarget);
                this.idEliminar = boton.data('id');
                $('#nombreClienteEliminar').text(boton.data('nombre'));
                $('#mensajeEliminarCliente').addClass('d-none').text('');
                $('#btnConfirmarEliminar').prop('disabled', false);
                this.modalEliminar.show();
            });

            // Botón "Eliminar" del modal: el usuario ya confirmó
            $('#btnConfirmarEliminar').on('click', () => this.eliminarCliente());
        },

        eliminarCliente() {
            $('#btnConfirmarEliminar').prop('disabled', true);

            $.ajax({
                url: '/Cliente/EliminarCliente',
                type: 'POST',
                data: { id: this.idEliminar }
            })
                .done((respuesta) => {
                    if (!respuesta.esCorrecto) {
                        this.mostrarError(respuesta.mensaje);
                        return;
                    }

                    // Se quita la fila de la tabla y se muestra el mensaje en la página
                    $(`#fila-cliente-${this.idEliminar}`).remove();
                    this.modalEliminar.hide();
                    $('#mensajeCliente')
                        .removeClass('d-none alert-danger')
                        .addClass('alert-success')
                        .text(respuesta.mensaje);
                })
                .fail(() => {
                    this.mostrarError('Ocurrió un error al eliminar el cliente. Intente de nuevo.');
                });
        },

        mostrarError(mensaje) {
            $('#mensajeEliminarCliente')
                .removeClass('d-none alert-success')
                .addClass('alert-danger')
                .text(mensaje);
            $('#btnConfirmarEliminar').prop('disabled', false);
        }
    };

    $(document).ready(() => Cliente.init());

})();
