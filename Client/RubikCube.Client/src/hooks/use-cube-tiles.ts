import {GET_CUBE, ROTATE_CUBE} from "../common/utils/urls.ts";
import {useCallback, useEffect, useState} from "react";
import {CubeTile, FaceName} from "../models/cube-tile.model.ts";
import useHttp from "./use-http.ts";

const FACE_ENUM_MAP: Record<number, FaceName> = {
  0: "Up",
  1: "Down",
  2: "Left",
  3: "Right",
  4: "Front",
  5: "Back"
};

const useCubeTiles = () => {
  const [tiles, setTiles] = useState<CubeTile[]>([]);
  const [isLoading, setIsLoading] = useState<boolean>(false);
  const [error, setError] = useState<string | null>(null);

  const getInitialCube = useCallback(async () => {
    if(tiles.length !== 0) {
      return;
    }

    try {
      setIsLoading(true);
      const data = await useHttp.get<CubeTile[]>(GET_CUBE);
      
      const normalizedTiles = data.map(tile => ({
        ...tile,
        face: FACE_ENUM_MAP[Number(tile.face)]
      }));
      
      setTiles(normalizedTiles)
      setError(null);
    } catch (error) {
      const errorMessage = error instanceof Error ? error.message : "An unknown error occurred";

      setError(errorMessage);
    } finally {
      setIsLoading(false);
    }
    
  },[tiles.length]);

  const rotateFace = useCallback(async (move: string) => {
    try {
      const rotated = await useHttp.post<CubeTile[]>(ROTATE_CUBE, { move });
      const normalized = rotated.map(tile => ({
        ...tile,
        face: FACE_ENUM_MAP[Number(tile.face)],
      }));
      
      setTiles(normalized);
      setError(null);
    } catch (error) {
      const errorMessage = error instanceof Error ? error.message : "An unknown error occurred";

      setError(errorMessage);
    } finally {
      setIsLoading(false);
    }
  }, []);
  
  useEffect(() => {
    getInitialCube();
  }, [getInitialCube]);
  
  return {
    state: {
      tiles,
      isLoading,
      error,
    },
    actions: {
      rotateFace,
    }
  };
};

export default useCubeTiles;