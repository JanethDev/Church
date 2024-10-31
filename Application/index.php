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
    loadPage('inicio'); // Cargar 'inicio.php' en `#page-content` al inicio
});

function loadPage(page) {
    var url = 'views/' + page + '.php'; // Asegurarse de incluir `.php`
    
    $.ajax({
        url: url,
        type: 'GET',
        success: function(response) {
            $('#page-content').html(response);
            if (page !== 'inicio') {
                window.history.pushState({ page: page }, '', page);
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
