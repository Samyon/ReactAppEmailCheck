import React, { useState } from 'react';
import { useTranslation } from 'react-i18next';
import '../i18n';

function InputVerifCode() {
    const [code, setCode] = useState('');
    const [error, setError] = useState('');

    const { t, i18n } = useTranslation();


    const handleChange = (e) => {
        const value = e.target.value;
        setCode(value);

         //Простая валидация code
        if (value === '') {
            setError('Ничего не ввели');
        } else if (value.length > 10) {
            setError('Слишком большой код');
        } else {
            setError('');
        }
    };

    const handleSubmit = async () => {
        try {
            const response = await fetch('/api/email/check_code', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ code })
            });

            if (!response.ok) {
                throw new Error('Network response was not ok');
            }

            const data = await response.json();
            console.log('Server response:', data);
            alert(`${t('code')}: ${code}`);
        } catch (error) {
            console.error('Error submitting code:', error);
            alert('Failed to submit code');
        }
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
                        code && <span>📧 Введённый код: {code}</span>
                )}
            </div>
        </div>
    );
}

export default InputVerifCode;
