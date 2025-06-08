import React, { useState } from 'react';

function EmailInput() {
    const [email, setEmail] = useState('');
    const [error, setError] = useState('');

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

    return (
        <div style={{ padding: '1rem', fontFamily: 'sans-serif' }}>
            <h2>Введите Email</h2>
            <input
                type="email"
                value={email}
                onChange={handleChange}
                placeholder="example@email.com"
                style={{ padding: '0.5rem', fontSize: '1rem' }}
            />
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
