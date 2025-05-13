import { GameVersion, Createdby } from "/entities/constants.js";

export class Base {
  constructor() {
  }

  init() {
    document.getElementById("version").textContent = `Version: ${GameVersion}`;
    document.getElementById("createdBy").textContent = `Created by: ${Createdby}`;
  }
}

