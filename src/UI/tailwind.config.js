module.exports = {
    purge: ['./pages/**/*.{js,ts,jsx,tsx}', './components/**/*.{js,ts,jsx,tsx}'],
    theme: {
        themeVariants: [],
    },
    variants: {
    },
    plugins: [
        require('@tailwindcss/forms')({
            strategy: 'class',
        })
    ],
}