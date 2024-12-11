$(document).ready(function () {
    $('#sidebarToggle').click(function () {
        const isMobile = window.innerWidth <= 768;

        $('#sidebar').toggleClass('show');

        if ($('#sidebar').hasClass('show')) {
            if (!isMobile) {
                $('body').css('margin-left', '200px');
            }
            $('#openIcon').hide();
            $('#closeIcon').show();
        } else {
            if (!isMobile) {
                $('body').css('margin-left', '0');
            }
            $('#openIcon').show();
            $('#closeIcon').hide();
        }
    });

    if (window.innerWidth > 768) {
        $('#sidebar').addClass('show');
        $('body').css('margin-left', '200px');
        $('#openIcon').hide();
        $('#closeIcon').show();
    } else {
        $('#sidebar').removeClass('show');
        $('body').css('margin-left', '0');
        $('#openIcon').show();
        $('#closeIcon').hide();
    }

    $('.sidebar-content a').click(function (e) {
        e.preventDefault();
        const target = $(this).data('target');

        $('.content-section').hide();

        $(target).fadeIn();
    });
});