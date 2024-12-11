// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
window.onscroll = function () {
    updateProgressBar();
};

function updateProgressBar() {
    const winScroll = document.body.scrollTop || document.documentElement.scrollTop;
    const height = document.documentElement.scrollHeight - document.documentElement.clientHeight;
    const scrolled = (winScroll / height) * 100;

    const progressBar = document.querySelector('.progress-bar');
    progressBar.style.width = scrolled + "%";
    progressBar.setAttribute('aria-valuenow', scrolled);

    const progressText = document.getElementById('progressText');
    progressText.textContent = Math.round(scrolled) + "%";
}