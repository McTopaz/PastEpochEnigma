import { GameTitle, GameVersion } from "./entities/constants.js";

window.addEventListener("DOMContentLoaded", () => {
  const footer = document.getElementById("footer");
  if (footer) {
    footer.textContent = `Version: ${GameVersion}`;
  }

  document.title = `${GameTitle} - V${GameVersion}`
});