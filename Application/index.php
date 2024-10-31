<?php
require_once 'init.php';
require_once('auth/session.php');
require_once 'inc/head.php';
include_once('inc/navbar.php');

$currentPage = basename($_SERVER['PHP_SELF']);
?>
<div class="container">
    <div class="row">
        <div class="col-md-12" id="page-content">
            
        </div>
    </div>
</div>
<script>
$(document).ready(function() {
    // Verifica si hay una página en el historial y la carga; si no, carga 'inicio'
    const currentUrl = window.location.pathname.split('/').pop() || 'inicio';
    loadPage(currentUrl.replace('.php', '')); // Cargar la página actual o 'inicio'

    // Escuchar el evento popstate para manejar el botón de regreso
    window.addEventListener('popstate', function(event) {
        if (event.state && event.state.page) {
            loadPage(event.state.page, false); // Cargar sin actualizar el historial
        }
    });

    // Capturar clics en todos los enlaces <a>, incluso en los que se cargan dinámicamente
    $(document).on('click', 'a', function(event) {
        event.preventDefault(); // Evita el comportamiento predeterminado del enlace

        const href = $(this).attr('href');
        if (href === 'javascript:void(0);') {
            loadPage('solicitud');
        } else if (href) {
            loadPage(href); // Llamar a loadPage sin incluir .php en el historial
        }
    });
});

// Función para cargar el contenido dinámicamente
function loadPage(page, pushState = true) {
    var url = 'views/' + page + '.php'; // Ruta completa incluyendo la extensión internamente

    $.ajax({
        url: url,
        type: 'GET',
        success: function(response) {
            $('#page-content').html(response);
            if (pushState) {
                window.history.pushState({ page: page }, '', page); // Sin la extensión en la URL visible
            }
        },
        error: function(xhr, status, error) {
            console.error('Error al cargar la página:', error);
            $('#page-content').html('<p>Error al cargar el contenido. Inténtalo de nuevo.</p>');
        }
    });
}
</script>




<?php
require_once('inc/footer.php');
?>
