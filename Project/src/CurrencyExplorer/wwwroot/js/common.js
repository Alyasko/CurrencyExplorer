$(document).ready(function () {
    // set handlers for explorer nav buttons


    var $menu = $("#main-menu");

    var offset = $menu.offset().top;
    var isFixed = true;

    var $wind = $(window);
    $wind.scroll(function () {
        if ($wind.scrollTop() > offset && isFixed) {
            $menu.addClass("main-menu-floating");
            isFixed = false;
        }
        else if ($wind.scrollTop() < offset && !isFixed) {
            $menu.removeClass("main-menu-floating");
            isFixed = true;
        }
    });
});