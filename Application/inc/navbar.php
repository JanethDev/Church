<div class="container-nav">

    <nav class="navbar navbar-expand-lg navbar-light bg-faded" style="border-bottom: 1px #d5e9ff solid;">
        <div style="margin-right:15px; color:#fff">

            <a  href="index" style="margin-right:15px; color:white">
                <img src="../assets/img/LogoParroquia.png" height="70" width="auto" />
            </a>

        </div>
        <div style="margin-right:15px; color:#fff">
           
            <button class="navbar-toggler" type="button" data-toggle="collapse" data-target=".navbar-collapse" aria-controls="navbarNavDropdown" aria-expanded="false" aria-label="Toggle navigation">

                <span class="navbar-toggler-icon"></span>

            </button>

            <div id="navbarNavDropdown" class="navbar-collapse collapse">

                <ul class="navbar-nav mr-auto">
                    <?php if($role = 'Administrador' || !$role = 'Facturacion' || !$role = 'Ventas' || !$role = 'Encargado'  ) {?>

                        <li style="margin-right:15px; color:white"><a <?php if ($current_page == 'solicitud') echo 'class="active"' ?> href="solicitud">Solicitud</a></li>
                        <li style="margin-right:15px; color:white"><a <?php if ($current_page == 'solicitudes') echo 'class="active"' ?> href="solicitudes">Solicitudes()</a></li>
                        <li style="margin-right:15px; color:white"><a <?php if ($current_page == 'pagos') echo 'class="active"' ?> href="pagos">Pagos</a></li>
                        <li class="dropdown csDropdown">
                            <a <?php if ($current_page == 'aviso_de_privacidad' ||$current_page == 'reglamento_columbario' ) echo 'class="active"' ?> href="#" data-toggle="dropdown" class="dropdown-toggle" style="margin-right:15px">Documentos<span class="caret"></span></a>
                            <ul class="dropdown-menu csWidth100" style="color:black">
                                <li style="margin-right:15px; margin-left:10px; color:white"><a href="aviso_de_privacidad">Aviso de privacidad</a></li>
                                <li style="margin-right:15px; margin-left:10px; color:white"><a href="reglamento_columbario">Reglamento del columbario</a></li>
                               
                            </ul>
                        </li>
                    <?php }?>
                
                        <li class="dropdown csDropdown">
                            <a <?php if (   $current_page == 'aniversario_luctuoso' ||
                                            $current_page == 'catedral' ||
                                            $current_page == 'ciudad' ||
                                            $current_page == 'clientes' ||
                                            $current_page == 'clientes_prospectos' ||
                                            $current_page == 'cryptType' ||
                                            $current_page == 'comisionistas' ||
                                            $current_page == 'cuota_de_mantenimiento' ||
                                            $current_page == 'descuentos' ||
                                            $current_page == 'descuento_contado' ||
                                            $current_page == 'estado_Civil' ||
                                            $current_page == 'estados' ||
                                            $current_page == 'horarios_misa' ||
                                            $current_page == 'impuesto_Federal' ||
                                            $current_page == 'intenciones' ||
                                            $current_page == 'notificaciones' ||
                                            $current_page == 'tipo_de_cambio' ||
                                            $current_page == 'usuarios'  ) echo 'class="active"' ?> href="#" data-toggle="dropdown" class="dropdown-toggle" style="margin-right:15px">Catálogos<span class="caret"></span></a>
                            <ul class="dropdown-menu csWidth100" style="color:black">
                            <?php if($role = 'Administrador' ) {?>
                                <li style="margin-right:15px; margin-left:10px;  color:white"><a href="aniversario_luctuoso">Aniversario luctuoso</a></li>
                                <li style="margin-right:15px; margin-left:10px;  color:white"><a href="catedral">Catedral</a></li>
                                <li style="margin-right:15px; margin-left:10px; color:white"><a href="ciudad">Ciudad</a></li>
                                <li style="margin-right:15px; margin-left:10px; color:white"><a href="clientes">Clientes</a></li>
                                <li style="margin-right:15px; margin-left:10px; color:white"><a href="clientes_prospectos">Clientes prospectos</a></li>
                                <li style="margin-right:15px; margin-left:10px; color:white"><a href="cryptType">Criptas</a></li>
                                <li style="margin-right:15px; margin-left:10px; color:white"><a href="comisionistas">Comisionistas</a></li>
                                <li style="margin-right:15px; margin-left:10px; color:white"><a href="cuota_de_mantenimiento">Cuota de mantenimiento</a></li>
                                <li style="margin-right:15px; margin-left:10px; color:white"><a href="descuentos">Descuentos</a></li>
                                <li style="margin-right:15px; margin-left:10px; color:white"><a href="descuento_contado">Descuento contado</a></li>
                                <li style="margin-right:15px; margin-left:10px; color:white"><a href="estado_Civil">Estado Civil</a></li>
                                <li style="margin-right:15px; margin-left:10px; color:white"><a href="estados">Estados</a></li>
                                <li style="margin-right:15px; margin-left:10px; color:white"><a href="horarios_misa">Horarios misa</a></li>
                                <li style="margin-right:15px; margin-left:10px; color:white"><a href="impuesto_Federal">Impuesto Federal</a></li>
                                <li style="margin-right:15px; margin-left:10px; color:white"><a href="intenciones">Intenciones</a></li>
                                <li style="margin-right:15px; margin-left:10px; color:white"><a href="notificaciones">Notificaciones</a></li>
                                <li style="margin-right:15px; margin-left:10px; color:white"><a href="tipo_de_cambio">Tipo de cambio</a></li>
                                <li style="margin-right:15px; margin-left:10px; color:white"><a href="usuarios">Usuarios</a></li>

                             <?php }elseif($role = 'Caja'){?>  
                                <li style="margin-right:15px; margin-left:10px; color:white"><a href="aniversario_luctuoso">Aniversario luctuoso</a></li>
                                <li style="margin-right:15px; margin-left:10px; color:white"><a href="intenciones">Intenciones</a></li>

                                <?php }elseif($role = 'Facturacion'){?>  
                                <li style="margin-right:15px; margin-left:10px; color:white"><a href="clientes">Clientes</a></li>
                                <li style="margin-right:15px; margin-left:10px; color:white"><a href="cuota_de_mantenimiento">Cuota de mantenimiento</a></li>

                                <?php }elseif($role = 'Ventas'){?>  
                                <li style="margin-right:15px; margin-left:10px; color:white"><a href="clientes">Clientes</a></li>
                                <li style="margin-right:15px; margin-left:10px; color:white"><a href="clientes_prospectos">Clientes prospectos</a></li>

                                <?php }elseif($role = 'Encargado'){?>  
                                <li style="margin-right:15px; margin-left:10px; color:white"><a href="aniversario_luctuoso">Aniversario luctuoso</a></li>
                                <li style="margin-right:15px; margin-left:10px; color:white"><a href="catedral">Catedral</a></li>
                                <li style="margin-right:15px; margin-left:10px; color:white"><a href="ciudad">Ciudad</a></li>
                                <li style="margin-right:15px; margin-left:10px; color:white"><a href="clientes">Clientes</a></li>
                                <li style="margin-right:15px; margin-left:10px; color:white"><a href="clientes_prospectos">Clientes prospectos</a></li>
                                <li style="margin-right:15px; margin-left:10px; color:white"><a href="cryptType">Criptas</a></li>
                                <li style="margin-right:15px; margin-left:10px; color:white"><a href="comisionistas">Comisionistas</a></li>
                                <li style="margin-right:15px; margin-left:10px; color:white"><a href="cuota_de_mantenimiento">Cuota de mantenimiento</a></li>
                                <li style="margin-right:15px; margin-left:10px; color:white"><a href="descuentos">Descuentos</a></li>
                                <li style="margin-right:15px; margin-left:10px; color:white"><a href="estado_Civil">Estado Civil</a></li>
                                <li style="margin-right:15px; margin-left:10px; color:white"><a href="estados">Estados</a></li>
                                <li style="margin-right:15px; margin-left:10px; color:white"><a href="impuesto_Federal">Impuesto Federal</a></li>
                                <li style="margin-right:15px; margin-left:10px; color:white"><a href="tipo_de_cambio">Tipo de cambio</a></li>
                                <li style="margin-right:15px; margin-left:10px; color:white"><a href="usuarios">Usuarios</a></li>

                                <?php }  ?>
                            </ul>
                        </li>
                        <li class="nav-item dropdown">
                            <a class="dropdown-toggle" href="#" id="navbarDropdown" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                Hola <?php echo $email; ?>!
                            </a>
                            <div class="dropdown-menu" aria-labelledby="navbarDropdown">
                                <a style="margin-left: 7%;" href="javascript:cerrarSesion();">Cerrar sesión</a>
                            </div>
                        </li>
                   
                </ul>
            </div>
        </div>
    </nav>
</div>