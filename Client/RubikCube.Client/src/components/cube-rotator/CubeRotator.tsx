import {ROTATION_MOVES} from "../../common/constants.ts";
import styles from "./CubeRotator.module.css";

interface CubeRotatorProps {
    onRotate: (move: string) => void;
}

const CubeRotator = ({onRotate}: CubeRotatorProps) => {
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
        </div>
    );
}

export default CubeRotator;
