<?php
$_title = 'Solicitud | Catedral Tijuana';
?>
<form id="frmCryptRequest" method="post" action="<?php echo htmlspecialchars($_SERVER["PHP_SELF"]); ?>">

    <nav>
        <div class="nav nav-tabs" id="nav-tab" role="tablist">
            <a class="nav-item nav-link active" id="nav-zona-tab" data-toggle="tab" href="#nav-zona" role="tab" aria-controls="nav-zona" aria-selected="true">Zonas</a>
            <a class="nav-item nav-link" id="nav-area-tab" data-toggle="tab" href="#nav-area" role="tab" aria-controls="nav-area" aria-selected="false">Areas</a>
            <a class="nav-item nav-link" id="nav-criptas-tab" data-toggle="tab" href="#nav-criptas" role="tab" aria-controls="nav-criptas" aria-selected="false">Criptas</a>
            <a class="nav-item nav-link" id="nav-forma-pago-tab" data-toggle="tab" href="#nav-forma-pago" role="tab" aria-controls="nav-forma-pago" aria-selected="false">Forma de pago</a>
        </div>
    </nav>
    <div class="tab-content">
        <div class="tab-pane fade show active" id="nav-zona" role="tabpanel" aria-labelledby="nav-zona-tab">
            <?php include_once('crypts/cryptmaps.php'); ?>
        </div>
        <div class="tab-pane fade" id="nav-area" role="tabpanel" aria-labelledby="nav-area-tab">
            <div id="areaContent"></div>
        </div>
        <div class="tab-pane fade" id="nav-criptas" role="tabpanel" aria-labelledby="nav-criptas-tab">
            <?php include_once('crypts/cryptsections.php'); ?>
        </div>
        <div class="tab-pane fade" id="nav-forma-pago" role="tabpanel" aria-labelledby="nav-forma-pago-tab">
            <!-- Contenido de la pestaña Forma de pago -->
        </div>
    </div>
</form>
<script>
    $(document).ready(function() {
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
    });

    function loadSectionA() {
        fetch('/views/crypts/cryptnichosA.php')
            .then(response => response.text())
            .then(html => {
                $('#areaContent').html(html); 
                $('#nav-area-tab').tab('show'); 
                history.pushState(null, '', ''); 
            })
            .catch(error => {
                console.error('Error al cargar la sección B:', error);
                $('#areaContent').html('<p>Error al cargar la sección B.</p>');
            });
    }

    function loadSectionB() {
        fetch('crypts/cryptnichos.php')
            .then(response => response.text())
            .then(html => {
                $('#areaContent').html(html); 
                $('#nav-area-tab').tab('show'); 
                history.pushState(null, '', ''); 
            })
            .catch(error => {
                console.error('Error al cargar la sección B:', error);
                $('#areaContent').html('<p>Error al cargar la sección B.</p>');
            });
    }

    function loadSectionC() {
        fetch('crypts/cryptnichos.php')
            .then(response => response.text())
            .then(html => {
                $('#areaContent').html(html); 
                $('#nav-area-tab').tab('show'); 
                history.pushState(null, '', ''); 
            })
            .catch(error => {
                console.error('Error al cargar la sección C:', error);
                $('#areaContent').html('<p>Error al cargar la sección C.</p>');
            });
    }
</script>