import { BrowserRouter, Route, Routes } from 'react-router-dom';
import './App.css';
import CubeViewer from "./components/pages/cube/CubeViewer.tsx";
import NotFound from './components/pages/not-found/NotFoundPage.tsx';

const App = () => {
    return (
        <BrowserRouter>
            <Routes>
                <Route path="/" element={<CubeViewer/>}/>
                <Route path="*" element={<NotFound />} />
            </Routes>
        </BrowserRouter>
    );
}

export default App;