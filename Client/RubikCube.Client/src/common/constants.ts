export const FACE_NAMES = ["Up", "Down", "Left", "Right", "Front", "Back"] as const;
export const FACE_POSITION_STYLES: Record<string, React.CSSProperties> = {
    Up: { gridArea: "up" },
    Down: { gridArea: "down" },
    Left: { gridArea: "left" },
    Right: { gridArea: "right" },
    Front: { gridArea: "front" },
    Back: { gridArea: "back" },
};
export const ROTATION_MOVES = ["F", "R'", "U", "B'", "L", "D'"];