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
        var url;
        if (page.includes('views/')) {
            url = page + '.php';
        } else {
            url = 'views/' + page + '.php';
        }
        $.ajax({
            url: url,
            type: 'GET',
            success: function(response) {
                $('#page-content').html(response);
            }
        });
    }

    // Cargar la página inicial al cargar la página por primera vez
    loadPage('<?php echo $currentPage; ?>');

    // Manejar el evento de clic en el navbar para cargar la página correspondiente
    $('.navbar-nav a').click(function(e) {
        e.preventDefault();
        var page = $(this).attr('href');
        loadPage(page);
    });
});
</script>

<?php
require_once('inc/footer.php');
?>
