import { Base } from "../base/base.js";
import { showMain } from "/usecases/appFlow.js";

const menuLogo = "/entities/asciiArts/optionsLogo.txt";

export class Options extends Base {
  init() {
    super.init();
    this.#logo();
  }

  #logo() {
    fetch(menuLogo)
      .then(res => res.text())
      .then(logo => {
        document.getElementById("asciiLogo").textContent = logo;
      })
      .catch(err => {
        console.error("Unable to load logo:", err);
      });
  }

  hanldeKeyboardEvent(event) {
    if (event.key === "Escape" || event.key === "Enter") {
      showMain();
    }
  }
}