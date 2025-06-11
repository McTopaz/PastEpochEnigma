import { Base } from "../base/base.js";

export class Options extends Base {
  init() {
    super.init();
    // Initialize options view here
  }

  hanldeKeyboardEvent(event) {
    // Handle keyboard events for options view here
    // Example: Close options on Escape
    if (event.key === "Escape") {
      // Implement navigation or close logic
    }
  }
}