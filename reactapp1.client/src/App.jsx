import { useEffect, useState } from 'react';
import './App.css';
import { useTranslation } from 'react-i18next';
import './i18n';


import MultiStepForm from './components/MultiStepForm';




function App() {


    //Localosation
    const { t, i18n } = useTranslation();
    const changeLanguage = (lng) => {
        i18n.changeLanguage(lng);
    };






    return (
        <div>

            <div style={{ padding: '2rem', fontFamily: 'sans-serif' }}>

                <div style={{ marginTop: '1rem' }}>
                    <button onClick={() => changeLanguage('ru')}>🇷🇺 Русский</button>
                    <button onClick={() => changeLanguage('en')}>🇬🇧 English</button>
                </div>
            </div>

            <h2>--------------</h2>
            <h2>{t('email address confirmation')}</h2>
            <MultiStepForm />
            <h2>--------------</h2>


        </div>
    );

}

export default App;