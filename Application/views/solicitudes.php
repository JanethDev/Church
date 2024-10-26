<?php
$_title = 'Solicitudes | Catedral Tijuana';
?>
<!-- jQuery Library -->
<script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>

<!-- DataTables CSS and JS -->
<link rel="stylesheet" href="https://cdn.datatables.net/1.11.5/css/jquery.dataTables.min.css">
<script src="https://cdn.datatables.net/1.11.5/js/jquery.dataTables.min.js"></script>

<!-- Bootstrap DataTables CSS and JS -->
<link rel="stylesheet" href="https://cdn.datatables.net/1.11.5/css/dataTables.bootstrap5.min.css">
<script src="https://cdn.datatables.net/1.11.5/js/dataTables.bootstrap5.min.js"></script>

<!-- DataTables Responsive CSS and JS -->
<link rel="stylesheet" href="https://cdn.datatables.net/responsive/2.2.9/css/responsive.bootstrap5.min.css">
<script src="https://cdn.datatables.net/responsive/2.2.9/js/dataTables.responsive.min.js"></script>
<script src="https://cdn.datatables.net/responsive/2.2.9/js/responsive.bootstrap5.min.js"></script>
<table id="testTable" class="table">
    <thead>
        <tr>
            <th>Name</th>
            <th>Position</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td>Test 1</td>
            <td>Test 2</td>
        </tr>
    </tbody>
</table>

<script>

$(document).ready(function() {
    console.log("Initializing DataTable test");

    try {
        $('#testTable').DataTable();
        console.log("DataTable initialized successfully");
    } catch (e) {
        console.error("Error initializing DataTable", e);
    }
});


</script>
