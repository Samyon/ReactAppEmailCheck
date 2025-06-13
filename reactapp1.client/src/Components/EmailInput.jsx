import React, { useState } from 'react';
import { useTranslation } from 'react-i18next';
import '../i18n';

function EmailInput({ onEmailChange }) {
    const [email, setEmail] = useState('');
    const [error, setError] = useState('');

    const { t, i18n } = useTranslation();


    const handleChange = (e) => {
        const value = e.target.value;
        setEmail(value);

        // Простая валидация email
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        if (value === '') {
            setError('');
        } else if (!emailRegex.test(value)) {
            setError('Неверный формат email');
        } else {
            setError('');
        }
    };

    const handleSubmit = async () =>  {
        try {

            onEmailChange(email); // передаём значение вверх

            const payload = {
                email:  email
            };
            console.log(payload);

            const response = await fetch('/api/email', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(payload)
            });



            if (!response.ok) {
                throw new Error('Network response was not ok');
            }

            const data = await response.json();
            console.log('Server response:', data);
            alert(`${t('email')}: ${email}`);
        } catch (error) {
            console.error('Error submitting email:', error);
            //alert('Failed to submit email');
        }
    };


    return (
        <div style={{ padding: '1rem', fontFamily: 'sans-serif' }}>



            <h2>{t('Enter email')}</h2>
            <input
                type="email"
                value={email}
                onChange={handleChange}
                placeholder="example@email.com"
                style={{ padding: '0.5rem', fontSize: '1rem' }}
            />
            <button onClick={handleSubmit}>{t('submit')}</button>
            <div style={{ marginTop: '1rem' }}>
                {error ? (
                    <span style={{ color: 'red' }}>{error}</span>
                ) : (
                    email && <span>📧 Введённый email: {email}</span>
                )}
            </div>
        </div>
    );
}

export default EmailInput;
