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

    const currentLang = i18n.language;

    return (
        <div>


            <div style={{ padding: '2rem', fontFamily: 'sans-serif', display: 'flex', justifyContent: 'flex-end', alignItems: 'flex-start' }}>
                <button
                    onClick={() => changeLanguage("en")}
                    className={currentLang === "en" ? "active-lang" : ""}
                >
                    English
                </button>
                <button
                    onClick={() => changeLanguage("ru")}
                    className={currentLang === "ru" ? "active-lang" : ""}
                >
                    Русский
                </button>
            </div>

            <div>
                <h2> {t('Welcome to our website!')}</h2>
                <div>
                    {t('We need to make sure that you have an email. To do this, you enter your email in the field below, click send, we will send you a confirmation code to this email, and you enter it in the code field that appears')}
                </div>
                <div>
                    {t('Important: Do not refresh the page after sending the email and before entering the code')}
                </div>
            </div>

            <MultiStepForm />

            

        </div>
    );

}

export default App;