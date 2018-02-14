<!DOCTYPE html>
<html lang="en">


<!-- Mirrored from wrappixel.com/demos/admin-templates/material-pro/horizontal/pages-login-2.html by HTTrack Website Copier/3.x [XR&CO'2014], Wed, 05 Jul 2017 22:08:27 GMT -->
<head>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <!-- Tell the browser to be responsive to screen width -->
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <meta name="description" content="">
    <meta name="author" content="">
    <!-- Favicon icon -->
    <link rel="icon" type="image/png" sizes="16x16" href="~/assets/images/favicon.png">
    <title>HelpDesk - Cajacopi</title>
    <!-- Bootstrap Core CSS -->
    <link href="~/assets/plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet">
    <!-- Custom CSS -->
    <link href="~/Content/css/style.css" rel="stylesheet">
    <!-- You can change the theme colors from here -->
    <link href="~/Content/css/colors/blue.css" id="theme" rel="stylesheet">
    <link href="~/Content/css/toastr.min.css" rel="stylesheet" />
    <!-- HTML5 Shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
    <!--[if lt IE 9]>
        <script src="https://oss.maxcdn.com/libs/html5shiv/3.7.0/html5shiv.js"></script>
        <script src="https://oss.maxcdn.com/libs/respond.js/1.4.2/respond.min.js"></script>
    <![endif]-->
    <script src="~/AngularJS/angular.js"></script>
    <script src="~/AngularJS/angular-route.js"></script>
    <script src="~/AngularJS/HomeController.js"></script>
</head>

<body ng-app="HelpDesk">
    <!-- ============================================================== -->
    <!-- Preloader - style you can find in spinners.css -->
    <!-- ============================================================== -->
    <div class="preloader">
        <svg class="circular" viewBox="25 25 50 50">
            <circle class="path" cx="50" cy="50" r="20" fill="none" stroke-width="2" stroke-miterlimit="10" />
        </svg>
    </div>
    <!-- ============================================================== -->
    <!-- Main wrapper - style you can find in pages.scss -->
    <!-- ============================================================== -->
    @RenderBody
    <!-- ============================================================== -->
    <!-- End Wrapper -->
    <!-- ============================================================== -->
    <!-- ============================================================== -->
    <!-- All Jquery -->
    <!-- ============================================================== -->
    <script src="~/assets/plugins/jquery/jquery.min.js"></script>
    <!-- Bootstrap tether Core JavaScript -->
    <script src="~/assets/plugins/bootstrap/js/tether.min.js"></script>
    <script src="~/assets/plugins/bootstrap/js/bootstrap.min.js"></script>
    <!-- slimscrollbar scrollbar JavaScript -->
    <script src="~/Content/js/jquery.slimscroll.js"></script>
    <!--Wave Effects -->
    <script src="~/Content/js/waves.js"></script>
    <!--Menu sidebar -->
    <script src="~/Content/js/sidebarmenu.js"></script>
    <!--stickey kit -->
    <script src="~/assets/plugins/sticky-kit-master/dist/sticky-kit.min.js"></script>
    <script src="~/assets/plugins/sparkline/jquery.sparkline.min.js"></script>
    <!--Custom JavaScript -->
    <script src="~/Content/js/custom.min.js"></script>
    <script src="~/Content/js/toastr.min.js"></script>
    <script src="~/Content/js/ui-toastr.min.js"></script>
    <!-- ============================================================== -->
    <!-- Style switcher -->
    <!-- ============================================================== -->
    <script src="~/assets/plugins/styleswitcher/jQuery.style.switcher.js"></script>
    <script>
        function setValor(id,valor) {
            $("#" + id).val(valor);
        }

        function getValor(id) {
            return $("#" + id).val();
        }

        function NotificacionPopup(shortCutFunction, titulo, mensaje) {
            //shortCutFunction: success, error
            var i = -1,
                toastCount = 0,
                $toastlast
            var msg = mensaje;
            var title = titulo;
            var $showDuration = 1000;
            var $hideDuration = 1000;
            var $timeOut = 5000;
            var $extendedTimeOut = 1000;
            var $showEasing = 'swing';
            var $hideEasing = 'linear';
            var $showMethod = 'fadeIn';
            var $hideMethod = 'fadeOut';
            var toastIndex = toastCount++;
            toastr.options = {
                closeButton: $('#closeButton').prop('checked'),
                debug: $('#debugInfo').prop('checked'),
                positionClass: $('#positionGroup input:checked').val() || 'toast-top-right',
                onclick: null
            };
            toastr.options.showDuration = $showDuration;
            toastr.options.hideDuration = $hideDuration;
            toastr.options.timeOut = $timeOut;
            toastr.options.extendedTimeOut = $extendedTimeOut;
            toastr.options.showEasing = $showEasing;
            toastr.options.hideEasing = $hideEasing;
            toastr.options.showMethod = $showMethod;
            toastr.options.hideMethod = $hideMethod;
            var $toast = toastr[shortCutFunction](msg, title); // Wire up an event handler to a button in the toast, if it exists
            $toastlast = $toast;
            if ($toast.find('#okBtn').length) {
                $toast.delegate('#okBtn', 'click', function () {
                    alert('you clicked me. i was toast #' + toastIndex + '. goodbye!');
                    $toast.remove();
                });
            }
            if ($toast.find('#surpriseBtn').length) {
                $toast.delegate('#surpriseBtn', 'click', function () {
                    alert('Surprise! you clicked me. i was toast #' + toastIndex + '. You could perform an action here.');
                });
            }
        };
    </script>
</body>
</html>
