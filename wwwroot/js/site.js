document.addEventListener('DOMContentLoaded', function () {
    const body = document.body;

    document.getElementById('sunIcon').addEventListener('click', function () {
        setTheme('light');
    });

    document.getElementById('moonIcon').addEventListener('click', function () {
        setTheme('dark');
    });

    const savedTheme = localStorage.getItem('theme');
    if (savedTheme) {
        setTheme(savedTheme);
    }
});

function setTheme(theme) {
    const body = document.body;

    if (theme === 'light') {
        body.classList.remove('dark-mode');
        localStorage.setItem('theme', 'light');
    } else if (theme === 'dark') {
        body.classList.add('dark-mode');
        localStorage.setItem('theme', 'dark');
    }
}

/* okno z informacjami */
function openExplanation() {
    var modal = document.getElementById("infoModal");
    modal.style.display = "block";
}

function closeModal() {
    var modal = document.getElementById("infoModal");
    modal.style.display = "none";
}

/* listy rozwijane */
document.addEventListener('click', function (event) {
    if (!event.target.matches('.dropdown')) {
        var dropdowns = document.querySelectorAll('.dropdown');
        dropdowns.forEach(function (dropdown) {
            dropdown.size = 1;
        });
    }
});

document.querySelectorAll('.dropdown').forEach((dropdown) => {
    dropdown.addEventListener('focus', function () {
        this.size = Math.min(this.length, 8);
    });
    dropdown.addEventListener('blur', function () {
        this.size = this.length < 8 ? this.length : 1;
    });
    dropdown.addEventListener('change', function () {
        this.size = 1;
        this.blur();
    });
});


