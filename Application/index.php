<?php
require_once 'init.php';
require_once 'auth/session.php';
require_once 'inc/head.php';
include_once 'inc/navbar.php';
?>

<div class="container">
    <div class="row">
        <div class="col-md-12" id="page-content">
            <!-- Aquí se cargará el contenido dinámico -->
        </div>
    </div>
</div>

<script>
$(document).ready(function () {
    const currentUrl = window.location.pathname.split('/').pop() || 'inicio';
    loadPage(currentUrl.replace('.php', '')); // Carga la página actual o "inicio"

    // Escucha el evento popstate para manejar el botón de regresar
    window.addEventListener('popstate', function (event) {
        if (event.state && event.state.page) {
            loadPage(event.state.page, false);
        }
    });

    // Intercepta clics en enlaces para cargar contenido dinámicamente
    $(document).on('click', 'a:not([data-toggle="tab"]):not([data-section])', function (event) {
        const href = $(this).attr('href');

        // Ignorar enlaces que no requieran carga de página dinámica
        if (
            !href || 
            href === '#' || 
            $(this).closest('.dropdown-menu').length > 0 // Ignorar dropdowns
        ) {
            return;
        }

        event.preventDefault();
        loadPage(href);
    });

    // Función para cargar una página
    function loadPage(page, pushState = true) {
        let url = 'views/' + page + '.php';

        $.ajax({
            url: url,
            type: 'GET',
            success: function (response) {
                $('#page-content').html(response);
                if (pushState) {
                    window.history.pushState({ page: page }, '', page);
                }
            },
            error: function (xhr, status, error) {
                console.error('Error al cargar la página:', error);
                $('#page-content').html('<p>Error al cargar el contenido. Inténtalo de nuevo.</p>');
            }
        });
    }
});

</script>

<?php
require_once('inc/footer.php');
?>
