import useCubeTiles from "../../../hooks/use-cube-tiles.ts";
import {CSSProperties, Fragment, useCallback, useEffect} from "react";
import {FaceName} from "../../../models/cube-tile.model.ts";
import styles from "./CubeViewer.module.css";
import SpinningLoader from "../../spinning-loader/SpinningLoader.tsx";


const facePositionStyles: Record<FaceName, CSSProperties> = {
    Up: {gridArea: "up"},
    Down: {gridArea: "down"},
    Left: {gridArea: "left"},
    Right: {gridArea: "right"},
    Front: {gridArea: "front"},
    Back: {gridArea: "back"},
};
const CubeViewer = () => {
    const {
        state: {tiles},
    } = useCubeTiles();

    useEffect(() => {
        document.title = "Rubik Cube";
    }, []);

    const renderFace = useCallback((face: FaceName) => {
        const faceTiles = tiles.filter(t => t.face === face);
        return (
            <div className={`${styles.faceGrid}`} 
                 style={{ ...facePositionStyles[face] }}
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

    if (tiles.length == 0) {
        return (<SpinningLoader/>)
    }

    return (
        <div className={styles.cubeGrid}>
            {(["Up", "Down", "Left", "Right", "Front", "Back"] as FaceName[])
                .map(face =>(
                    <Fragment key={face}>{renderFace(face)}</Fragment>))}
        </div>
    );
};

export default CubeViewer;