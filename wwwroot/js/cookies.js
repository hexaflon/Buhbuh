document.addEventListener("DOMContentLoaded", function () {
    const banner = document.getElementById("cookieConsentBanner");
    const acceptBtn = document.getElementById("acceptCookies");
    const rejectBtn = document.getElementById("rejectCookies");

    // Sprawdzenie, czy użytkownik już zaakceptował/odrzucił cookies
    if (getCookie("cookieConsent")) {
        banner.style.display = "none";
    }

    // Obsługa akceptacji
    acceptBtn.addEventListener("click", function () {
        setCookie("cookieConsent", "accepted", 365);
        banner.style.display = "none";
    });

    // Obsługa odrzucenia
    rejectBtn.addEventListener("click", function () {
        setCookie("cookieConsent", "rejected", 365);
        banner.style.display = "none";
    });

    // Funkcja zapisu ciasteczka
    function setCookie(name, value, days) {
        let expires = "";
        if (days) {
            let date = new Date();
            date.setTime(date.getTime() + days * 24 * 60 * 60 * 1000);
            expires = "; expires=" + date.toUTCString();
        }
        document.cookie = name + "=" + value + "; path=/" + expires;
    }

    // Funkcja odczytu ciasteczka
    function getCookie(name) {
        let nameEQ = name + "=";
        let ca = document.cookie.split(";");
        for (let i = 0; i < ca.length; i++) {
            let c = ca[i];
            while (c.charAt(0) === " ") c = c.substring(1, c.length);
            if (c.indexOf(nameEQ) === 0) return c.substring(nameEQ.length, c.length);
        }
        return null;
    }
});
