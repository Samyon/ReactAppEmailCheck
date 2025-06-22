import React, { useState } from 'react';
import { AnimatePresence, motion } from 'framer-motion';

import EmailInput from './EmailInput';
import InputVerifCode from './InputVerifCode';
import CodeVerified from './CodeVerified';

import { useTranslation } from 'react-i18next';
import '../i18n';

//const [userEmail, setUserEmail] = useState('');//Проброс вверх от компонента



function MultiStepForm() {




    // Компоненты шагов
    const StepOne = () => <div>    <EmailInput onEmailChange={handleEmailChange} /> </div>;
    const StepTwo = () => <div><InputVerifCode onPrev={goPrev} /></div>;
    const StepThree = () => <div><CodeVerified/></div>;

    const steps = [<StepOne />, <StepTwo />, <StepThree />];

    const { t, i18n } = useTranslation();

    const goPrev = (value) => {
        if (value == true)
            prev();
        else
            next();
    };

    //Проброс вверх от компонента
    const handleEmailChange = (value) => {
        //setUserEmail(value); // обновить состояние
        console.log('Новый email:', value); // дополнительный код
        // например, сделать API-запрос или валидацию
        next();
        //test();
    };

    const test = () => {
        console.log('test');
    };

    const [step, setStep] = useState(0);
    const [direction, setDirection] = useState(0); // -1 ← назад, 1 → вперёд

    const next = () => {
        if (step < steps.length - 1) {
            setDirection(1);
            setStep(step + 1);
        }
    };

    const prev = () => {
        if (step > 0) {
            setDirection(-1);
            setStep(step - 1);
        }
    };

    const variants = {
        enter: (dir) => ({
            x: dir > 0 ? 300 : -300,
            opacity: 0,
        }),
        center: {
            x: 0,
            opacity: 1,
        },
        exit: (dir) => ({
            x: dir > 0 ? -300 : 300,
            opacity: 0,
        }),
    };

    return (
        <div style={{ padding: '2rem', fontFamily: 'sans-serif' }}>
            {/*<h2>Шаг {step + 1}</h2>*/}

            <div style={{ overflow: 'hidden', position: 'relative' }}>
                <AnimatePresence custom={direction} mode="wait">
                    <motion.div
                        key={step}
                        custom={direction}
                        variants={variants}
                        initial="enter"
                        animate="center"
                        exit="exit"
                        transition={{ duration: 0.4 }}
                    //style={{ position: 'absolute', width: '100%' }}
                    >
                        {steps[step]}
                    </motion.div>
                </AnimatePresence>
            </div>

            <div  style={{ marginTop: '1rem', display: 'none' }}>
                <button onClick={prev} disabled={step === 0}>
                    {t('Back')}
                </button>
                <button onClick={next} disabled={step === steps.length - 1} style={{ marginLeft: '10px' }}>
                    {t('Next')}
                </button>
            </div>
        </div>
    );
}


export default MultiStepForm;
