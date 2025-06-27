import { Difficulties } from "/entities/constants.js";

export const settings = {
    difficulty: Difficulties.Easy,
    mode: "normal",

    get isNormalMode() {
        return this.mode === "normal";
    },
    get isSpeedrunMode() {
        return this.mode === "speedrun";
    },
}