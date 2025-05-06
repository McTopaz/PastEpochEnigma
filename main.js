import { initializeApp } from "./usecases/initApp.js";
import { runAppFlow } from "./usecases/appFlow.js";

window.addEventListener("DOMContentLoaded", () => {
  initializeApp();
  runAppFlow();
});