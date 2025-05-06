import { GameTitle, GameVersion } from "../entities/constants.js";

export function initializeApp() {
  document.title = `${GameTitle} - V${GameVersion}`;

  const footer = document.getElementById("footer");
  if (footer) {
    footer.textContent = `Version: ${GameVersion}`;
  }
}