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
