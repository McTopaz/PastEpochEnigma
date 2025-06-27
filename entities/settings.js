import { Difficulties, Modes } from "/entities/constants.js";

export const settings = {
    difficulty: Difficulties.Easy,
    mode: Modes.Normal,

    get isNormalMode() {
        return this.mode === "normal";
    },
    get isSpeedrunMode() {
        return this.mode === "speedrun";
    },
}