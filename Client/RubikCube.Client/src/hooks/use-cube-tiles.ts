import useHttp from "../common/use-http.ts";
import {GET_CUBE} from "../common/utils/urls.ts";
import {useCallback, useEffect, useState} from "react";
import {CubeTile, FaceName} from "../models/cube-tile.model.ts";

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


  const getInitialCube = useCallback(async () => {
    if(tiles.length !== 0) {
      return;
    }

    try {
      const data = await useHttp.get<CubeTile[]>(GET_CUBE);
      
      const normalizedTiles = data.map(tile => ({
        ...tile,
        face: FACE_ENUM_MAP[Number(tile.face)]
      }));
      
      setTiles(normalizedTiles);
    } catch (error) {
      const errorMessage = error instanceof Error ? error.message : "An unknown error occurred";

      console.error(errorMessage);
    }
    
  },[tiles.length]);
  
  useEffect(() => {
    getInitialCube();
  }, [getInitialCube]);
  
  return {
    state: {
      tiles
    },
    actions: {
      getInitialCube,
    }
  };
};

export default useCubeTiles;