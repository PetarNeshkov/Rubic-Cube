import {Fragment, useCallback, useEffect} from "react";
import styles from "./CubeViewerPage.module.css";
import ErrorPage from "../error/ErrorPage.tsx";
import useCubeTiles from "../../hooks/use-cube-tiles.ts";
import {FaceName} from "../../models/cube-tile.model.ts";
import {FACE_NAMES, FACE_POSITION_STYLES} from "../../common/constants.ts";
import SpinningLoader from "../../components/spinning-loader/SpinningLoader.tsx";
import CubeRotator from "../../components/cube-rotator/CubeRotator.tsx";

const CubeViewerPage = () => {
    const {
        state: {tiles, isLoading, error},
        actions: {rotateFace}
    } = useCubeTiles();

    useEffect(() => {
        document.title = "Rubik Cube";
    }, []);

    const renderFace = useCallback((face: FaceName) => {
        const faceTiles = tiles.filter(t => t.face === face);
        return (
            <div className={`${styles.faceGrid}`}
                 style={{...FACE_POSITION_STYLES[face]}}
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

    if (isLoading) {
        return (<SpinningLoader/>)
    }

    if (error) {
        return <ErrorPage message={error}/>;
    }

    return (
        <>
            <div className={styles.cubeGrid}>
                {FACE_NAMES
                    .map(face => (
                        <Fragment key={face}>{renderFace(face)}</Fragment>))}
            </div>
            <CubeRotator onRotate={rotateFace}/>
        </>

    );
};
export default CubeViewerPage;