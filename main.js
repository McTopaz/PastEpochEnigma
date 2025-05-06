import { GameTitle, GameVersion } from "./entities/constants.js";

const SHOW_SPLASH = true;

window.addEventListener("DOMContentLoaded", () => {
  document.title = `${GameTitle} - V${GameVersion}`;
  document.getElementById("footer").textContent = `Version: ${GameVersion}`;

  if (SHOW_SPLASH) {
    loadSplashScreen();
  } else {
    startGame();
  }
});

function loadSplashScreen() {
  fetch("view/splash/splash.html")
    .then(response => response.text())
    .then(html => {
      const container = document.createElement("div");
      container.innerHTML = html;
      document.body.prepend(container);
    })
    .catch(err => {
      console.error("Kunde inte ladda splash-screen:", err);
      startGame();
    });
}

window.startGame = () => {
  const splash = document.getElementById("splash-screen");
  if (splash) splash.remove();

  loadMainScreen();
};

function loadMainScreen() {
  fetch("view/main/main.html")
    .then(response => response.text())
    .then(html => {
      const mainContainer = document.createElement("div");
      mainContainer.innerHTML = html;
      document.body.appendChild(mainContainer);
    })
    .catch(err => {
      console.error("Kunde inte ladda huvudvyn:", err);
    });
}
