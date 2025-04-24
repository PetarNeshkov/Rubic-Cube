import { createBrowserRouter, RouterProvider } from 'react-router-dom';
import './App.css';
import CubeViewerPage from "./components/pages/cube/CubeViewerPage.tsx";
import ErrorPage from "./components/pages/error/ErrorPage.tsx";
import NotFoundPage from "./components/pages/not-found/NotFoundPage.tsx";

const router = createBrowserRouter([
    {
        path: "/",
        element: <CubeViewerPage />,
        errorElement: <ErrorPage />,
    },
    {
        path: "*",
        element: <NotFoundPage />,
    },
]);

const App = () => {
    return <RouterProvider router={router} />;
};

export default App;