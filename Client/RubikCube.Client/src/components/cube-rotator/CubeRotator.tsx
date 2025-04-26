import {ROTATION_MOVES} from "../../common/constants.ts";
import styles from "./CubeRotator.module.css";

interface CubeRotatorProps {
    onRotate: (move: string) => void;
    onReset: () => void;
}

const CubeRotator = ({onRotate, onReset}: CubeRotatorProps) => {
    return (
        <div className={styles.container}>
            {ROTATION_MOVES.map((move) => (
                <button
                    key={move}
                    className={styles.button}
                    onClick={() => onRotate(move)}
                    aria-label={`Rotate ${move}`}
                >
                    {move}
                </button>
            ))}
            <div>
                <button
                    className={styles.resetButton}
                    onClick={onReset}
                    aria-label="Reset Cube"
                >
                    Reset
                </button>
            </div>
        </div>
    );
}

export default CubeRotator;
