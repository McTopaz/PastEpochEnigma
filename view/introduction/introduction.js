import { Base } from "../base/base.js";

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
        document.getElementById("text").innerHTML = text.replace(/\n/g, "<br>");
      })
      .catch(err => {
        console.error("Unable to load text:", err);
      });
  }
}