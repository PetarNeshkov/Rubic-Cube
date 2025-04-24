import {useCallback, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import image from '../../assets/page-not-found.png'
import styles from './NotFoundPage.module.css';

const NotFoundPage = () => {
    const navigate = useNavigate();

    useEffect(() => {
        document.title = "Page not Found";

        const timeout = setTimeout(() => {
            navigate("/");
        }, 5000);

        return () => clearTimeout(timeout);
    }, [navigate]);

    const handleBackToHome = useCallback(() => {
        navigate('/');
    },[navigate]);
    
    return (
        <div className={styles.container}>
            <h1 className={styles.heading}>404 - Page Not Found</h1>
            <p className={styles.text}>Oops! The page you're looking for doesn't exist.</p>
            <img className={styles.image} src={image} alt="Page Not Found" />
            <button className={styles.homeButton} onClick={handleBackToHome}>
                Back to Home
            </button>
        </div>
    );
};
export default NotFoundPage;
