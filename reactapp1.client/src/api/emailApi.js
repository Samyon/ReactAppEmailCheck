export async function sendEmailToServer(email) {
    const response = await fetch('/api/email', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({ email }),
    });

    if (!response.ok) {
        throw new Error('Ошибка отправки email');
    }

    return await response.json();
}
