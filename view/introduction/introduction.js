import { Base } from "../base/base.js";
import { showMain } from "/usecases/appFlow.js";
import { EvilPlaceholder, EvilName } from "/entities/constants.js";

const menuLogo = "/entities/asciiArts/introductionLogo.txt";
const text = "/entities/texts/introduction.txt"

export class Introduction extends Base {
  init() {
    super.init();
    this.#logo();
    this.#text();
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

  #text() {
    fetch(text)
      .then(res => res.text())
      .then(text => {
        text = text.replace(/\n/g, "<br>")
          .replace(EvilPlaceholder, EvilName);
        document.getElementById("text").innerHTML = text;
      })
      .catch(err => {
        console.error("Unable to load text:", err);
      });
  }

  hanldeKeyboardEvent(event) {
    if (event.key === "Escape" || event.key === "Enter") {
      showMain();
    }
  }
}