var $menu;
var $settings;
var $wind;

var defSettingsBaseTop;
var defSettingsMenuTopDiff;
var defSettingsWidth;

// is added to defSettingsWidth to hide the shadow
var defSettingsHideValue = 20;

var isSettingsShown = true;
var isScrolledUnderMenu = false;

$(document).ready(function () {

    $menu = $("#main-menu");
    $settings = $("#settings");
    $wind = $(window);

    // set handlers for explorer nav buttons

    $("#main-menu li").click(function (e) {
        MenuItemsClick($(this).children("a").first().attr("data-name"));
        e.preventDefault();
    });

    $(".lang").click(function() {
        $(".lang").removeClass("lang-selected");
        $(this).toggleClass("lang-selected");
    });

    $("#main-menu ul li").hover(function() {
        $(this).toggleClass("menu-item-hover");
    });

    $("#settings-close").click(function (e) {
        $settings.css({
            left: -defSettingsWidth - defSettingsHideValue + "px"
        });
        e.preventDefault();
    });

    // menu and settings movements

    var menuOffset = $menu.offset().top;
    var settingsOffset = $settings.offset().top;
    defSettingsBaseTop = settingsOffset;
    var blocksTopDiff = settingsOffset - menuOffset;
    defSettingsMenuTopDiff = blocksTopDiff;

    defSettingsWidth = $settings.width();

    $settings.css({
        left: -defSettingsWidth - defSettingsHideValue + "px"
    });

    isSettingsShown = false;

    //$settings.css({
    //    height: $wind.height() - settingsOffset + "px"
    //});

    var isFixed = true;

    $wind.scroll(function () {
        if ($wind.scrollTop() > menuOffset && isFixed) {
            $menu.addClass("main-menu-floating");

            if (isSettingsShown) {
                $settings.css({
                    position: "fixed"
                });
            }
            isScrolledUnderMenu = true;
            isFixed = false;
        }
        else if ($wind.scrollTop() < menuOffset && !isFixed) {
            $menu.removeClass("main-menu-floating");

            if (isSettingsShown) {
                $settings.css({
                    position: "absolute"
                });
            }
            isScrolledUnderMenu = false;
            isFixed = true;
        }
    });


    // REMOVE
    MenuItemsClick("settings");
});


function MenuItemsClick(name) {
    switch (name) {
        case "settings":
        {
            var pos = "absolute";
            var offsetTop = defSettingsMenuTopDiff;

            if (isScrolledUnderMenu) {
                pos = "fixed";
                offsetTop = defSettingsMenuTopDiff;
            }

            $("#settings").css({
                left: "0px",
                position: pos,
                top: offsetTop + "px"
            });

            isSettingsShown = true;
            break;
        }
        case "explore":
        {
            $wind.scrollTop(0, 200);
            break;
        }
    }
}