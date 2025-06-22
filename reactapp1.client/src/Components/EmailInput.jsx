import React, { useState, useEffect } from 'react';
import { useTranslation } from 'react-i18next';
import '../i18n';


function EmailInput({ onEmailChange }) {
    const [email, setEmail] = useState('');
    const [error, setError] = useState('');
    const [storeEmail, setStoreEmail] = useState('');
    const [serverResp, setServerResp] = useState("");
    const [serverRespDetails, setserverRespDetails] = useState("");

    useEffect(() => {
        const em = localStorage.getItem("email");
        if (em) setStoreEmail(em);
        return () => {
        };
    }, []);

    const { t, i18n } = useTranslation();

    // Простая валидация email
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

    const handleChange = (e) => {
        const value = e.target.value;
        setEmail(value);
        if (value === '') {
            setError('');
        } else if (!emailRegex.test(value)) {
            setError(t('Неверный формат email'));
        } else {
            setError('');
        }
    };

    const isValidEmail = (email) => {
        return emailRegex.test(email);
    };

    const handleSubmit = async () => {
        try {

            const payload = {
                email: email
            };
            console.log(payload);

            const response = await fetch('/api/email/recive_email', {
                method: 'POST',
                credentials: 'include',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(payload)
            });

            const data = await response.json();
            console.log('Server response:', data);


            if (!response.ok) {
                setServerResp(t(data.error));
                setserverRespDetails(t(data.details));
                throw new Error('Network response was not ok');
            }

            onEmailChange(email); // передаём значение вверх
            localStorage.setItem("email", email); // сохраняем email в хранилище
        } catch (error) {
            console.error('Error submitting email:', error);
        }
    };

    const next = async () => {
        onEmailChange(email); // передаём значение вверх
    };

    return (
        <div style={{ padding: '1rem', fontFamily: 'sans-serif' }}>

            <h2>{t('Enter email')}</h2>
            <input name="emailInput"
                type="email"
                value={email}
                onChange={handleChange}
                placeholder="example@email.com"
                style={{ padding: '0.5rem', fontSize: '1rem' }}
            />
            <button disabled={!isValidEmail(email)} onClick={handleSubmit}>{t('submit')}</button>
            <div style={{ marginTop: '1rem' }}>
                {error ? (
                    <span style={{ color: 'red' }}>{error}</span>
                ) : (
                    email && <span>📧 {t('Введённый email')}: {email}</span>
                )}
            </div>
            <div>
                {storeEmail && t('Вы уже вводили E-mail:')} <i>{storeEmail}</i>{storeEmail && t(', и на него уже был отправлен код подтверждения. Если вы введёте Email, и отправите его вновь, то на него снова будет отправлен новый код. если вы не хотите этого, просто нажмите кнопку ')}
                <button disabled={!storeEmail} style={{ marginLeft: '10px' }} onClick={next}>
                    {t('Next')}
                </button>
            </div>
            <div>
                <p className="red-text">{serverResp}</p>
                <p className="red-text">{serverRespDetails}</p>
            </div>
        </div>
    );
}

export default EmailInput;
