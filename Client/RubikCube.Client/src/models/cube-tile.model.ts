export type FaceName = "Up" | "Down" | "Left" | "Right" | "Front" | "Back";
export interface CubeTile {
    id: number;
    face: FaceName;
    row: number;
    column: number;
    color: string;
}
export const FACE_ENUM_MAP: Record<number, FaceName> = {
    0: "Up",
    1: "Down",
    2: "Left",
    3: "Right",
    4: "Front",
    5: "Back"
};