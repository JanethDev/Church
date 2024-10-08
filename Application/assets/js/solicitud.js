$(document).ready(function() {
    // Manejar el clic en los botones de carga de secciones
    $('a[data-section]').on('click', function(e) {
        e.preventDefault();
        var section = $(this).data('section');
        if (section === 'A') {
            loadSectionA();
        } else if (section === 'B') {
            loadSectionB();
        } else if (section === 'C') {
            loadSectionC();
        }
    });

    // Funciones para cargar secciones en la pestaña de Áreas
    function loadSectionA() {
        fetch('/views/crypts/cryptnichosA.php')
            .then(response => {
                if (!response.ok) {
                    throw new Error('Contenido no disponible');
                }
                return response.text();
            })
            .then(html => {
                $('#areaContent').html(html);
                $('#nav-area-tab').tab('show');
                history.pushState(null, '', ''); 
            })
            .catch(error => {
                console.error('Error al cargar la sección A:', error);
                $('#areaContent').html('<p>Error al cargar la sección A.</p>');
                Swal.fire({
                    icon: 'error',
                    title: 'Contenido no disponible',
                    text: 'El contenido que intentas acceder no está disponible en este momento.',
                    }).then((result) => {
                    // Al hacer clic en "OK", recargar la página
                    if (result.isConfirmed) {
                        location.reload(); // Recargar la página
                    }
                });
               
            });
    }

    function loadSectionB() {
        fetch('/views/crypts/cryptnichosB.php')
            .then(response => {
                if (!response.ok) {
                    throw new Error('Contenido no disponible');
                }
                return response.text();
            })
            .then(html => {
                $('#areaContent').html(html);
                $('#nav-area-tab').tab('show');
                history.pushState(null, '', ''); 
            })
            .catch(error => {
                console.error('Error al cargar la sección B:', error);
                $('#areaContent').html('<p>Error al cargar la sección B.</p>');
                Swal.fire({
                    icon: 'error',
                    title: 'Contenido no disponible',
                    text: 'El contenido que intentas acceder no está disponible en este momento.',
                });
            });
    }

    function loadSectionC() {
        fetch('/views/crypts/cryptnichosC.php')
            .then(response => {
                if (!response.ok) {
                    throw new Error('Contenido no disponible');
                }
                return response.text();
            })
            .then(html => {
                $('#areaContent').html(html);
                $('#nav-area-tab').tab('show');
                history.pushState(null, '', ''); 
            })
            .catch(error => {
                console.error('Error al cargar la sección C:', error);
                $('#areaContent').html('<p>Error al cargar la sección C.</p>');
                Swal.fire({
                    icon: 'error',
                    title: 'Contenido no disponible',
                    text: 'El contenido que intentas acceder no está disponible en este momento.',
                });
            });
    }

    // Manejar clic en las áreas del mapa para cargar el contenido en criptasContent
    $(document).on('click', 'area[data-section="A"]', function(e) {
        e.preventDefault();
        var href = $(this).attr('href');
        loadCriptaContent(href);
    });

    function loadCriptaContent(href) {
        $.ajax({
            url: '../../api/crypts/byzone.php',
            type: 'POST',
            data: { href: href },
            dataType: 'json',
            success: function(data) {
                if (data.error) {
                    Swal.fire({
                        icon: 'error',
                        title: 'Error en la solicitud',
                        text: data.error,
                    });
                } else {
                    // Envía los datos recibidos a cryptsections.php
                    $.ajax({
                        url: '/views/crypts/cryptsections.php',
                        type: 'POST',
                        data: { response: JSON.stringify(data) },
                        success: function(sectionData) {
                            $('#criptasContent').html(sectionData);
                            $('#nav-criptas-tab').tab('show');
                            history.pushState(null, '', ''); 
                        },
                        error: function() {
                            Swal.fire({
                                icon: 'error',
                                title: 'Contenido no disponible',
                                text: 'El contenido que intentas acceder no está disponible en este momento.',
                            });
                        }
                    });
                }
            },
            error: function() {
                Swal.fire({
                    icon: 'error',
                    title: 'Contenido no disponible',
                    text: 'El contenido que intentas acceder no está disponible en este momento.',
                });
            }
        });
    }

    // Manejar clic en el botón de forma de pago (btnPaymentMethod)
    $(document).on('click', '#btnPaymentMethod', function() {
        actualizarDatosSeleccionados();
    });

    $('a[data-toggle="tab"]').on('shown.bs.tab', function(e) {
        var target = $(e.target).attr("href"); // Obtener el tab objetivo
        if (target === '#nav-forma-pago') {
            actualizarDatosSeleccionados(); // Llamar a la función de actualización al cambiar al tab de forma de pago
        }
    });

    // Función para enviar los datos seleccionados
    function actualizarDatosSeleccionados() {
        var datos = [];

        // Recorrer todas las posiciones seleccionadas
        $('#tablecryptsection .disponible.selected').each(function() {
            var id = $(this).data('id');
            var fullPosition = $(this).data('full-position');
            var zone = $(this).data('zone');
            var position = $(this).data('position-name');
            var isShared = $(this).data('is-shared');
            var placesShared = $(this).data('places-shared');
            var price = $(this).data('price');
            var priceShared = $(this).data('price-shared');
            var statusId = $(this).data('status-id');
            var level = $(this).data('level');
            var status = $(this).data('status');
            var total = $('#precio').text(); // Precio mostrado
            var aisle = $(this).data('aisle');

            // Crear el objeto de datos
            var positionData = {
                id: id,
                full_position: fullPosition,
                zone: zone,
                position: position,
                is_shared: isShared,
                places_shared: placesShared,
                price: price,
                price_shared: priceShared,
                status_id: statusId,
                status: status,
                total: total,
                aisle: aisle,
                level: level
            };

            datos.push(positionData);
        });

        if (datos.length > 0) {
            // Enviar datos al tab de forma de pago vía AJAX
            $.ajax({
                url: '/views/crypts/cryptrequest.php',
                type: 'POST',
                data: { positions: JSON.stringify(datos) }, // Enviar datos como JSON
                success: function(response) {
                    $('#nav-forma-pago').html(response); // Cargar el contenido
                    $('#nav-forma-pago-tab').tab('show'); // Mostrar el tab de forma de pago
                },
                error: function() {
                    Swal.fire({
                        icon: 'error',
                        title: 'Error al enviar los datos',
                        text: 'Hubo un problema al enviar los datos seleccionados.',
                    });
                }
            });
        } else {
            Swal.fire({
                icon: 'warning',
                title: 'Selecciona posiciones',
                text: 'Debes seleccionar al menos una posición antes de continuar.',
            });
        }
    }

    
    $('#nav-tab a').on('click', function(e) {
        e.preventDefault();
        $(this).tab('show');
    });

    

});
