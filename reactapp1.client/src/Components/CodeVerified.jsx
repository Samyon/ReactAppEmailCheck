import React, { useState } from 'react';
import { useTranslation } from 'react-i18next';
import '../i18n';

function CodeVerified({ }) {

    const { t, i18n } = useTranslation();

    return (
        <div style={{ padding: '1rem', fontFamily: 'sans-serif' }}>

            <h2>{t('Поздравляем, вы успешно подтвердили код для вашей почты!')}</h2>

        </div>
    );
}

export default CodeVerified;
