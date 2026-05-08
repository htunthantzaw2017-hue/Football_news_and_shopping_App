// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
//
// Sidebar Functions
//

function openNav() {
    // Check screen size to determine sidebar behavior
    if (window.innerWidth < 768) {
        // Mobile behavior: show sidebar, content stays put (covers content)
        document.getElementById("mySidebar").style.width = "250px";
    } else {
        // Desktop behavior: show sidebar and push content
        document.getElementById("mySidebar").style.width = "250px";
        document.getElementById("main").style.marginLeft = "250px";
    }
}

function closeNav() {
    document.getElementById("mySidebar").style.width = "0";
    document.getElementById("main").style.marginLeft = "0";
}