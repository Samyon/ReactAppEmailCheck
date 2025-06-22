import React, { useState } from 'react';
import { useTranslation } from 'react-i18next';
import '../i18n';

function InputVerifCode({ onPrev }) {
    const [code, setCode] = useState('');
    const [error, setError] = useState('');
    const [serverResp, setServerResp] = useState("");
    const [serverRespDetails, setserverRespDetails] = useState("");

    const { t, i18n } = useTranslation();

    const handleChange = (e) => {
        const value = e.target.value;
        setCode(value);

        //Простая валидация code
        if (value === '') {
            setError(t('Ничего не ввели'));
        } else if (value.length > 10) {
            setError(t('Слишком большой код'));
        } else {
            setError('');
        }
    };

    const handleSubmit = async () => {
        try {
            const response = await fetch('/api/email/check_code', {
                method: 'POST',
                credentials: 'include',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ code })
            });


            const data = await response.json();
            console.log('Server response:', data);

            if (!response.ok) {
                setServerResp(t(data.error));
                setserverRespDetails(t(data.details));
                throw new Error('Network response was not ok');
            }
            localStorage.setItem("email", ""); //очищаем email в хранилище
            onPrev(false);

        } catch (error) {
            console.error('Error submitting code:', error);

        }
    };

    const prev = () => {
        onPrev(true);
    };

    return (
        <div style={{ padding: '1rem', fontFamily: 'sans-serif' }}>

            <h2>{t('Enter verification code')}</h2>
            <input
                type="text"
                value={code}
                onChange={handleChange}
                placeholder="code"
                style={{ padding: '0.5rem', fontSize: '1rem' }}
            />
            <button onClick={handleSubmit}>{t('submit')}</button>
            <div style={{ marginTop: '1rem' }}>
                {error ? (
                    <span style={{ color: 'red' }}>{error}</span>
                ) : (
                    code && <span>📧 {t('Введённый код')}: {code}</span>
                )}
            </div>

            <p className="red-text">{serverResp}</p>
            <p className="red-text">{serverRespDetails}</p>

            <div style={{ marginTop: '1rem' }}>
                <button onClick={prev} >
                    {t('Back')}
                </button>
            </div>
        </div>
    );
}

export default InputVerifCode;
