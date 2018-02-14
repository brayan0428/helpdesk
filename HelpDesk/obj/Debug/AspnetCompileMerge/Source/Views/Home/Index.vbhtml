@Code
    ViewData("Title") = "Index"
    Layout = "~/Views/Shared/_LayoutLogin.vbhtml"
End Code
<section id="wrapper" class="login-register login-sidebar" style="background-image:url(../../assets/images/background/login-register.jpg);" ng-controller="IniciarSesion">
    <div class="login-box card">
        <div class="card-block">
            <form class="form-horizontal form-material" id="loginform" action="https://wrappixel.com/demos/admin-templates/material-pro/horizontal/index.html">
                <a href="javascript:void(0)" class="text-center db"><img src="~/assets/images/logo.png" alt="Logo" /><br /></a>

                <div class="form-group m-t-40">
                    <div class="col-xs-12">
                        <input class="form-control" type="text" required="" placeholder="Usuario" id="Usuario" ng-model="usuario" value="bllanos">
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-xs-12">
                        <input class="form-control" type="password" required="" placeholder="Contraseña" id="Clave" ng-model="clave" value="Pa$$w0rd">
                    </div>
                </div>
                @*<div class="form-group">
                    <div class="col-md-12">
                        <div class="checkbox checkbox-primary pull-left p-t-0">
                            <input id="checkbox-signup" type="checkbox">
                            <label for="checkbox-signup"> Remember me </label>
                        </div>
                        <a href="javascript:void(0)" id="to-recover" class="text-dark pull-right"><i class="fa fa-lock m-r-5"></i> Forgot pwd?</a>
                    </div>
                </div>*@
                <div class="form-group text-center m-t-20">
                    <div class="col-xs-12">
                        <button class="btn btn-info btn-lg btn-block text-uppercase waves-effect waves-light" type="button" ng-click="ValidarSesion();">Iniciar Sesion</button>
                    </div>
                </div>
                @*<div class="row">
                    <div class="col-xs-12 col-sm-12 col-md-12 m-t-10 text-center">
                        <div class="social"><a href="javascript:void(0)" class="btn  btn-facebook" data-toggle="tooltip" title="Login with Facebook"> <i aria-hidden="true" class="fa fa-facebook"></i> </a> <a href="javascript:void(0)" class="btn btn-googleplus" data-toggle="tooltip" title="Login with Google"> <i aria-hidden="true" class="fa fa-google-plus"></i> </a> </div>
                    </div>
                </div>
                <div class="form-group m-b-0">
                    <div class="col-sm-12 text-center">
                        <p>Don't have an account? <a href="register2.html" class="text-primary m-l-5"><b>Sign Up</b></a></p>
                    </div>
                </div>*@
                <h6 style="text-align:center">@(Biblioteca.Variables.tFooter)</h6>
            </form>
            <form class="form-horizontal" id="recoverform" action="https://wrappixel.com/demos/admin-templates/material-pro/horizontal/index.html">
                <div class="form-group ">
                    <div class="col-xs-12">
                        <h3>Recover Password</h3>
                        <p class="text-muted">Enter your Email and instructions will be sent to you! </p>
                    </div>
                </div>
                <div class="form-group ">
                    <div class="col-xs-12">
                        <input class="form-control" type="text" required="" placeholder="Email">
                    </div>
                </div>
                <div class="form-group text-center m-t-20">
                    <div class="col-xs-12">
                        <button class="btn btn-primary btn-lg btn-block text-uppercase waves-effect waves-light" type="submit">Reset</button>
                    </div>
                </div>
            </form>
        </div>
    </div>
</section>


