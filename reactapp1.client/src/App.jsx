import { useEffect, useState } from 'react';
import './App.css';
import { useTranslation } from 'react-i18next';
import './i18n';


import MultiStepForm from './components/MultiStepForm';




function App() {
    const [forecasts, setForecasts] = useState();
    const [dbvalue, setDbvalue] = useState();
    const [text, setText] = useState(''); // состояние для ввода

    const { t, i18n } = useTranslation();
    const changeLanguage = (lng) => {
        i18n.changeLanguage(lng);
    };

    console.log('Текущее значение dbvalue:', dbvalue); // ← смотри в консоли браузера
    console.log('Текущее значение forecasts:', forecasts); // ← смотри в консоли браузера


    useEffect(() => {
        populateWeatherData();
    }, []);

    useEffect(() => {
        getDbData();
    }, []);

    const handleChange = (event) => {
        setText(event.target.value); // обновляем состояние при изменении
    };

    const contents = forecasts === undefined
        ? <p><em>Loading....</em></p>
        : <table className="table table-striped" aria-labelledby="tableLabel">
            <thead>
                <tr>
                    <th>Date</th>
                    <th>Temp. (C)</th>
                    <th>Temp. (F)</th>
                    <th>Summary</th>
                </tr>
            </thead>
            <tbody>
                {forecasts.map(forecast =>
                    <tr key={forecast.date}>
                        <td>{forecast.date}</td>
                        <td>{forecast.temperatureC}</td>
                        <td>{forecast.temperatureF}</td>
                        <td>{forecast.summary}</td>
                    </tr>
                )}
            </tbody>
        </table>;

    const contentsDb = dbvalue === undefined
        ? <p><em>Loading... Loading from DB1.</em></p>
        : { dbvalue };

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

            <div style={{ padding: '1rem', fontFamily: 'sans-serif' }}>



                <h2>Введите текст:</h2>
                <input
                    type="text"
                    value={text}
                    onChange={handleChange}
                    placeholder="Напиши что-нибудь..."
                ></input>

                <p>🔹 Вы ввели: <strong>{text}</strong></p>

                Отладка
                <pre>DEBUG: {JSON.stringify({ text }, null, 2)}</pre>
            </div>

            <div>
                <h1>Регистрация</h1>
                <p>Введите адрес электронной почты</p>

                <button>Подтвердить</button>
                {contents}
            </div>

            <div>
                <h1 id="tableLabel">Weather forecast</h1>
                <p>This component demonstrates fetching data from the server.</p>
                {contents}
            </div>

            <div>
                <h1>Строка с БД</h1>
                <p>Это демонстрация соединения с БД</p>
                {dbvalue}
            </div>
        </div>
    );

    async function populateWeatherData() {
        const response = await fetch('api/weatherforecast');
        if (response.ok) {
            const data = await response.json();
            setForecasts(data);
        }
    }

    async function getDbData() {
        const response = await fetch('api/db');
        if (response.ok) {
            const data = await response.text();
            setDbvalue(data);
        }
    }


}

export default App;