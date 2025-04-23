import { BrowserRouter, Route, Routes } from 'react-router-dom';
import './App.css';
import CubeViewer from "./components/pages/cube/CubeViewer.tsx";

const App = () => {
    return (
        <BrowserRouter>
            <Routes>
                <Route path="/" element={<CubeViewer/>}/>
            </Routes>
        </BrowserRouter>
    );
}

export default App;