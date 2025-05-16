import { ViewParts } from "/entities/viewParts.js";

const repoName = "PastEpochEnigma";
export const BASE_PATH = window.location.pathname.includes(`/${repoName}/`)
  ? `/${repoName}/`
  : "/";

export const ViewPaths = {
  base: new ViewParts("base", BASE_PATH),
  splash: new ViewParts("splash", BASE_PATH),
  main: new ViewParts("main", BASE_PATH),
  introduction: new ViewParts("introduction", BASE_PATH),
};

export const AsciiPaths = {
  horizontalLogo: `${BASE_PATH}entities/asciiArts/horizontalSplashLogo.txt`,
  verticalLogo: `${BASE_PATH}entities/asciiArts/verticalSplashLogo.txt`,
  menuLogo: `${BASE_PATH}entities/asciiArts/menuLogo.txt`
};