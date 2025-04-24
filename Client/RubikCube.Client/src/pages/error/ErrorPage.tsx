import {useCallback, useEffect } from "react";
import {isRouteErrorResponse, useNavigate, useRouteError } from "react-router-dom";
import NotFoundPage from "../not-found/NotFoundPage";
import {ErrorPageProps} from "../../models/error.model.ts";
import styles from "./ErrorPage.module.css";

const ErrorPage = ({ message = "Something went wrong."}: ErrorPageProps) => {
    const navigate = useNavigate();
    const error = useRouteError();

    useEffect(() => {
        document.title = "Error";

        const timeout = setTimeout(() => {
            navigate("/");
        }, 5000);

        return () => clearTimeout(timeout);
    }, [navigate]);

    const handleBackToHome = useCallback(() => {
        navigate('/');
    },[navigate]);
    
    if (isRouteErrorResponse(error) && error.status === 404) {
        return <NotFoundPage />;
    }

    const errorMessage =
        message ||
        (isRouteErrorResponse(error) && error.statusText) ||
        (error instanceof Error && error.message) ||
        "Something went wrong.";

    return (
        <div className={styles.container}>
            <h1 className={styles.heading}>Error</h1>
            <p className={styles.text}>{errorMessage}</p>
            <button className={styles.homeButton} onClick={handleBackToHome}>
                Back to Home
            </button>
        </div>
    );
};

export default ErrorPage;