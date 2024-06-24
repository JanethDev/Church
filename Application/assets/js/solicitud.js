$(document).ready(function() {
    // Manejar el clic en los enlaces <a> con atributo data-section
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
    
    
    $('#nav-tab a').on('click', function(e) {
        e.preventDefault();
        $(this).tab('show');
    });
});