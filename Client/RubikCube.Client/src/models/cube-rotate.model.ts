import {CubeTile, FaceName} from "./cube-tile.model.ts";

export interface RotateCubeRequest {
    move: string;
    tiles: CubeTile[];
}

export const FACE_STRING_TO_ENUM_MAP: Record<FaceName, number> = {
    "Up": 0,
    "Down": 1,
    "Left": 2,
    "Right": 3,
    "Front": 4,
    "Back": 5
};