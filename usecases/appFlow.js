import { SplashDuration } from "../entities/constants.js";

export function runAppFlow() {
    const shouldShowSplash  = true;

    if (shouldShowSplash ) {
        showSplash();
    } else {
        showMainGame();
    }
}

function showSplash() {
    fetch("/view/splash/splash.html")
        .then(res => res.text())
        .then(html => {
            const parser = new DOMParser();
            const doc = parser.parseFromString(html, "text/html");
            const container = document.createElement("div");

            container.innerHTML = doc.body.innerHTML;
            document.body.prepend(container);

            doc.querySelectorAll("script").forEach(oldScript => {
                const newScript = document.createElement("script");
                if (oldScript.src) {
                    newScript.src = oldScript.src;
                    newScript.type = oldScript.type || "text/javascript";
                } else {
                    newScript.textContent = oldScript.textContent;
                }
                document.body.appendChild(newScript);
            });

            setTimeout(() => {
                container.remove();
                showMainGame();
            }, SplashDuration);
        })
        .catch(error => {
            console.error("Kunde inte ladda splash-vyn:", error);
            showMainGame();
        });
}

function showMainGame() {
    fetch("view/main/main.html")
        .then(res => res.text())
        .then(html => {
            const mainContainer = document.createElement("div");
            mainContainer.innerHTML = html;
            document.body.appendChild(mainContainer);
        })
        .catch(error => {
            console.error("Kunde inte ladda huvudvyn:", error);
        });
}
