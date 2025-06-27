import { Base } from "../base/base.js";
import { showMain } from "/usecases/appFlow.js";
import { settings } from "/entities/settings.js";
import { Difficulties } from "/entities/constants.js";
import { Difficulties as DifficaultiesTexts } from "/entities/constants/texts.js";
import { Modes } from "/entities/constants.js";
import { Modes as ModesTexts } from "/entities/constants/texts.js";

const menuLogo = "/entities/asciiArts/optionsLogo.txt";

export class Options extends Base {
  init() {
    super.init();
    this.#logo();
    this.#applyOptions();
    this.#applySettings();
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

  #applyOptions() {
    document.getElementById("easy").value = Difficulties.Easy;
    document.getElementById("easyLabel").textContent = DifficaultiesTexts.Easy;
    document.getElementById("medium").value = Difficulties.Medium;
    document.getElementById("mediumLabel").textContent = DifficaultiesTexts.Medium;
    document.getElementById("hard").value = Difficulties.Hard;
    document.getElementById("hardLabel").textContent = DifficaultiesTexts.Hard;

    document.getElementById("normal").value = Modes.Normal;
    document.getElementById("normalLabel").textContent = ModesTexts.Normal;
    document.getElementById("speedrun").value = Modes.Speedrun;
    document.getElementById("speedrunLabel").textContent = ModesTexts.Speedrun;
  }

  #applySettings() {
    const difficultRadios = document.querySelectorAll('input[name="difficulty"]');
    difficultRadios.forEach(radio => {
      radio.checked = (radio.value === settings.difficulty);
    });

    const modeRadios = document.querySelectorAll('input[name="mode"]');
    modeRadios.forEach(radio => {
      radio.checked = (radio.value === settings.mode);
    });
  }

  hanldeKeyboardEvent(event) {
    if (event.key === "Escape" || event.key === "Enter") {
      showMain();
    }
    else if (event.key === "1" || event.key.toUpperCase() === "D") {
      const difficulties = [Difficulties.Easy, Difficulties.Medium, Difficulties.Hard];
      settings.difficulty = this.#cycleOptions(difficulties, settings.difficulty);
      this.#applySettings();
    }
    else if (event.key === "2" || event.key.toUpperCase() === "M") {
      const modes = [Modes.Normal, Modes.Speedrun];
      settings.mode = this.#cycleOptions(modes, settings.mode);
      this.#applySettings();
    }
  }

  #cycleOptions(options, currentValue) {
    const currentIndex = options.indexOf(currentValue);
    const nextIndex = (currentIndex + 1) % options.length;
    return options[nextIndex];
  }
}