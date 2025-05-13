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
  const splashCssHref = "view/splash/splash.css";

  const existingLink = document.querySelector(`link[href="${splashCssHref}"]`);
  if (!existingLink) {
    const link = document.createElement("link");
    link.rel = "stylesheet";
    link.href = splashCssHref;
    document.head.appendChild(link);
  }


  fetch("view/base/base.html")
    .then(res => res.text())
    .then(baseHtml => {
      document.body.innerHTML = baseHtml;

      return fetch("view/splash/splash.html");
    })
    .then(res => res.text())
    .then(splashHtml => {
      const content = document.getElementById("content");
      content.innerHTML = splashHtml;

      // Init view if callback exists.
      import("../view/splash/splash.js").then(module => {
        if (typeof module.initSplashView === "function") {
          module.initSplashView();
        }
      });

      setTimeout(() => {
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
