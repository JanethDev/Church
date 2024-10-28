<?php
require_once 'init.php';
require_once('auth/session.php');
require_once 'inc/head.php';
include_once('inc/navbar.php');

$currentPage = basename($_SERVER['PHP_SELF']);
?>
<div class="container">
    <div class="row justify-content-center">
        <!-- Tarjeta 1 -->
        <div class="col-md-3">
            <div class="card text-center">
                <img src="assets/img/Arquidiocesis_Color.png" class="card-img-top mx-auto" alt="Logo 1" style="width: 100px; height: 100px; margin-top: 10px;">
                <div class="card-body">
                    <h5 class="card-title">Parroquia 1</h5>
                    <p class="card-text">Este es un texto de ejemplo para la tarjeta 1.</p>
                </div>
            </div>
        </div>
        <!-- Tarjeta 2 -->
        <div class="col-md-3">
            <div class="card text-center">
                <img src="assets/img/LOGO_CATEDRAL_TIJUANA.png" class="card-img-top mx-auto" alt="Logo 2" style="width: 100px; height: 100px; margin-top: 10px;">
                <div class="card-body">
                    <h5 class="card-title">Parroquia 2</h5>
                    <p class="card-text">Este es un texto de ejemplo para la tarjeta 2.</p>
                </div>
            </div>
        </div>
    </div>
</div>
<script>
$(document).ready(function() {
    // Función para cargar el contenido de la página mediante AJAX
    function loadPage(page) {
        var url = 'views/' + page + '.php'; // Construimos la URL correcta
        $.ajax({
            url: url,
            type: 'GET',
            success: function(response) {
                $('#page-content').html(response);
            },
            error: function(xhr, status, error) {
                console.error('Error al cargar la página:', error);
            }
        });
    }

    // Cargar la página inicial al cargar la página por primera vez
    loadPage('<?php echo $currentPage; ?>');

    
});
</script>

<?php
require_once('inc/footer.php');
?>
