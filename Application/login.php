<?php
require_once 'init.php';
$_title = 'Inicio de sesión | Catedral Tijuana';

require_once 'inc/head.php';
?>
<style>
	/* Estilos adicionales para la página */
	body, html {
		height: 100%;
	}
	.container-full-height {
		min-height: 100%;
		display: flex;
		align-items: center;
		justify-content: center;
	}

</style>

<body>
    <div class="container-full-height">
		<div class="card text-dark bg-light mb-3 container-login">
			<div class="card-header text-white">
				<h4>Bienvenido al sistema de Catedral Tijuana</h4>
			</div>
			<div class="card-body">
				<form id="loginForm"  class="form-horizontal" role="form">
					<div class="row">
						<div class="col-12 text-center">
							<img src="assets/img/LogoParroquia.png" height="140">
						</div>
						<div class="col-12 form-group mt-2 upload-msg"></div>
						<div class="col-12 form-group mt-3">
							<label class="font-weight-bold" for="email">Correo:</label>
							<input type="text" id="email" name="email" class="form-control" required>
						</div>
						<div class="col-12 form-group mt-2">
							<label class="font-weight-bold" for="password">Contraseña:</label>
							<input type="password" id="password" name="password" class="form-control" required>
						</div>
						<div class="col-6 form-group mt-2">
							<div class="pretty p-icon p-smooth">
								<input type="checkbox" value="true" id="RememberMe" name="RememberMe">
								<div class="state p-success">
									<i class="icon mdi mdi-check"></i>
									<label class="font-weight-bold" for="RememberMe">Recordarme</label>
								</div>
							</div>
						</div>
						<div class="col-6 form-group mt-2">
							<a href="#" data-toggle="modal" data-target=".bs-example-modal-sm" class="float-right">¿Olvidó su contraseña?</a>
						</div>
						<div class="col-12 form-group mt-2">
							<input type="button" value="Iniciar sesión" class="btn btn-success w-100" onclick="autenticar();"/>
						</div>
					</div>
				</form>
			</div>
		</div>

		<div class="modal fade bs-example-modal-sm" tabindex="-1" role="dialog" aria-labelledby="mySmallModalLabel" aria-hidden="true">
			<div class="modal-dialog modal-md">
				<div class="modal-content">
					<div class="modal-header">
						<h4 class="modal-title">Recuperar contraseña</h4>
						<button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
					</div>
					<div class="modal-body">
						<div class="form-group">
							<label class="control-label">Ingrese su correo electrónico.</label>
							<input type="text" name="UserName" class="form-control" id="recovering">
						</div>
					</div>
					<div class="modal-footer">
						<h6 style="float: left;" id="message"></h6>
						<button type="button" id="btnRecoverPassword" class="btn btn-default">Recuperar</button>
					</div>
				</div>
			</div>
		</div>
	</div>
    
<?php require_once('inc/footer.php'); ?>
<script>
function autenticar(){
		
	var data = new FormData(document.getElementById("loginForm"));
	
	$.ajax({
		url: "api/access/access.php",        
		type: "POST",             
		data: data, 			  
		contentType: false,       
		cache: false,             
		processData:false,        
		success: function(data)   
		{	
			validateJS = JSON.parse(data);
			switch(validateJS.caso) {
				case 1:
                    window.location.href = "index.php";
					break;
				case 2:
					$(".upload-msg").html("<div class='alert alert-danger alert-dismissible' role='alert'>"+validateJS.mensaje+"</div> ");
					window.setTimeout(function() {
					$(".alert-dismissible").fadeTo(500, 0).slideUp(500, function(){
					$(this).remove();
					});	}, 5000);
							
					break;
					
			}

		}
	});
		
	}
</script>