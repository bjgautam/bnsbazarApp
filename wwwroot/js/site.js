// Retrieve and store the Theme value from browser storage
let theme = localStorage.getItem("Theme");

// Apply theme on load
if (theme === "Secondary") {
    document.getElementById('themeStyle')
        .setAttribute("href", "/css/secondary-theme.css");

    document.body.classList.remove("light-theme");
    document.body.classList.add("dark-theme");
} else {
    document.getElementById('themeStyle')
        .setAttribute("href", "/css/primary-theme.css");

    document.body.classList.remove("dark-theme");
    document.body.classList.add("light-theme");
}

// Add listener once page loads
window.addEventListener('load', () => {
    document.getElementById('btnTheme')
        .addEventListener('click', () => switchTheme());
});

function switchTheme() {
    let currentTheme = localStorage.getItem("Theme");

    if (currentTheme === "Secondary") {
        // Switch to Primary (Light)
        localStorage.setItem("Theme", "Primary");
        document.getElementById('themeStyle')
            .setAttribute("href", "/css/primary-theme.css");

        document.body.classList.remove("dark-theme");
        document.body.classList.add("light-theme");
    }
    else {
        // Switch to Secondary (Dark)
        localStorage.setItem("Theme", "Secondary");
        document.getElementById('themeStyle')
            .setAttribute("href", "/css/secondary-theme.css");

        document.body.classList.remove("light-theme");
        document.body.classList.add("dark-theme");
    }
}