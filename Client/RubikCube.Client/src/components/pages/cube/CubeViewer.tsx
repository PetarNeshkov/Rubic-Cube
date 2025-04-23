import useCubeTiles from "../../../hooks/use-cube-tiles.ts";
import {CSSProperties, useCallback} from "react";
import {FaceName} from "../../../models/cube-tile.model.ts";
import styles from "./CubeViewer.module.css";


const facePositionStyles: Record<FaceName, CSSProperties> = {
    Up: { gridArea: "up" },
    Down: { gridArea: "down" },
    Left: { gridArea: "left" },
    Right: { gridArea: "right" },
    Front: { gridArea: "front" },
    Back: { gridArea: "back" },
};
const CubeViewer = () => {
    const {
        state: {tiles},
    } = useCubeTiles();

    const renderFace = useCallback((face: FaceName) => {
        const faceTiles = tiles.filter(t => t.face === face);
        return (
            <div
                style={{
                    display: "grid",
                    gridTemplateColumns: "repeat(3, 30px)",
                    gridTemplateRows: "repeat(3, 30px)",
                    gap: 2,
                    ...facePositionStyles[face],
                }}
            >
                {faceTiles.map(tile => (
                    <div
                        key={tile.id}
                        style={{
                            width: 30,
                            height: 30,
                            backgroundColor: tile.color.toLowerCase(),
                            border: "1px solid #333",
                        }}
                    />
                ))}
            </div>
        );
    }, [tiles]);

    return (
        <div className={styles.cubeGrid}>
            {(["Up", "Down", "Left", "Right", "Front", "Back"] as FaceName[])
                .map(face => renderFace(face))}
        </div>
    );
};

export default CubeViewer;