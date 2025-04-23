export type FaceName = "Up" | "Down" | "Left" | "Right" | "Front" | "Back";
export interface CubeTile {
    id: number;
    face: FaceName;
    row: number;
    column: number;
    color: string;
}