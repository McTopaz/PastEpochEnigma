export const settings = {
    difficulty: "easy",
    mode: "normal",

    get isNormalMode() {
        return this.mode === "normal";
    },
    get isSpeedrunMode() {
        return this.mode === "speedrun";
    },
}