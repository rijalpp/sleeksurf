jQuery.noConflict();
var $j = jQuery;
var openTab = false;
$j(document).ready(function () {

    if (openTab) {
        $j("fieldset#signin_menu").toggle();
        $j(".signin").toggleClass("menu-open");
    }

    $j(".signin").click(function (e) {
        e.preventDefault();
        $j("fieldset#signin_menu").toggle();
        $j(".signin").toggleClass("menu-open");
    });

    $j("fieldset#signin_menu").mouseup(function () {
        return false
    });
    $j(document).mouseup(function (e) {
        if ($j(e.target).parent("a.signin").length == 0) {
            $j(".signin").removeClass("menu-open");
            $j("fieldset#signin_menu").hide();
        }
    });
});