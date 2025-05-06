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
      // Skapa ett container-element och sätt in HTML:en
      const container = document.createElement("div");
      container.innerHTML = html;

      // Lägg till i DOM
      document.body.prepend(container);
    })
    .catch(err => {
      console.error("Kunde inte ladda splash-screen:", err);
      startGame(); // Kör igång spelet ändå om splashen inte kan laddas
    });
}

// Gör startGame tillgänglig globalt så splash-knappen kan anropa den
window.startGame = () => {
  const splash = document.getElementById("splash-screen");
  if (splash) splash.remove();

  document.getElementById("game-container").style.display = "block";
  // Lägg till eventuell init-logik här
};