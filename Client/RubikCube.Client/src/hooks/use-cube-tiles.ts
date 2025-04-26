import {GET_CUBE, ROTATE_CUBE} from "../common/utils/urls.ts";
import {useCallback, useEffect, useState} from "react";
import {CubeTile, FACE_ENUM_MAP} from "../models/cube-tile.model.ts";
import useHttp from "./use-http.ts";
import {FACE_STRING_TO_ENUM_MAP, RotateCubeRequest} from "../models/cube-rotate.model.ts";

const useCubeTiles = () => {
  const [tiles, setTiles] = useState<CubeTile[]>([]);
  const [isLoading, setIsLoading] = useState<boolean>(false);
  const [error, setError] = useState<string | null>(null);

  const getInitialCubeData = useCallback(async () => {
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
  }, [])
  
  const getInitialCube = useCallback(async () => {
    if(tiles.length !== 0) {
      return;
    }

    await getInitialCubeData();
    
  },[getInitialCubeData, tiles.length]);

  const rotateFace = useCallback(async (move: string) => {
    try {
      const mappedTiles = tiles.map(tile => ({
        ...tile,
        face: FACE_STRING_TO_ENUM_MAP[tile.face] 
      })) as [];
      
      const requestModel : RotateCubeRequest = {
         move, 
         tiles: mappedTiles,
      }
      
      const rotated = await useHttp.post<CubeTile[]>(ROTATE_CUBE, requestModel);
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
  }, [tiles]);
  
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
      getInitialCubeData
    }
  };
};

export default useCubeTiles;